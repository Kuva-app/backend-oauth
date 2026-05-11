using Kuva.Auth.Business.Interfaces;
using Kuva.Auth.Business.Models;
using Kuva.Auth.Entities.Constants;
using Kuva.Auth.Entities.Dtos.Requests;
using Kuva.Auth.Entities.Dtos.Responses;
using Kuva.Auth.Entities.Entities;
using Kuva.Auth.Entities.Enums;
using Kuva.Auth.Entities.ValueObjects;
using Kuva.Auth.Repository.Interfaces;

namespace Kuva.Auth.Business.Services;

public sealed class AuthService(
    IUserRepository userRepository,
    IRoleRepository roleRepository,
    IPasswordHashProvider passwordHashProvider,
    IPasswordPolicyService passwordPolicyService,
    IJwtTokenService jwtTokenService,
    IRefreshTokenService refreshTokenService,
    IAuthAuditService auditService,
    IUnitOfWork unitOfWork,
    IClock clock) : IAuthService
{
    public async Task<AuthTokenResponse> RegisterConsumerAsync(RegisterConsumerRequest request, RequestContext context, CancellationToken cancellationToken)
    {
        var email = EmailAddress.Create(request.Email);
        if (await userRepository.ExistsByNormalizedEmailAsync(email.NormalizedValue, cancellationToken))
        {
            await auditService.RecordAsync(null, AuthAuditEventType.UserRegistered, AuthAuditResult.Failed, context, "{\"reason\":\"duplicate_email\"}", cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            throw AuthException.Conflict("E-mail já cadastrado.");
        }

        var policy = passwordPolicyService.Validate(request.Password);
        if (!policy.IsValid)
        {
            await auditService.RecordAsync(null, AuthAuditEventType.UserRegistered, AuthAuditResult.Failed, context, "{\"reason\":\"weak_password\"}", cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            throw AuthException.Unprocessable("weak_password", "A senha não atende à política definida.", policy.Violations);
        }

        var role = await roleRepository.GetByNameAsync(RoleNames.Consumer, cancellationToken)
            ?? throw AuthException.Unprocessable("missing_role", "Role CONSUMER não encontrada.");

        var now = clock.UtcNow;
        var user = new User
        {
            Email = email.Value,
            NormalizedEmail = email.NormalizedValue,
            Name = request.Name,
            Phone = request.Phone,
            Status = UserStatus.Active,
            CreatedAt = now,
            Credential = new UserCredential
            {
                PasswordHash = passwordHashProvider.HashPassword(request.Password),
                PasswordAlgorithm = passwordHashProvider.Algorithm,
                PasswordUpdatedAt = now,
                CreatedAt = now
            },
            UserRoles = new List<UserRole> { new() { RoleId = role.Id, CreatedAt = now } },
            TermsAcceptances = new List<TermsAcceptance>
            {
                new TermsAcceptance
                {
                    TermsVersion = request.AcceptedTermsVersion,
                    PrivacyPolicyVersion = request.AcceptedPrivacyPolicyVersion,
                    AcceptedAt = now,
                    IpAddress = context.IpAddress,
                    UserAgent = context.UserAgent
                }
            }
        };

        if (request.PhotoProcessingConsent)
        {
            user.UserConsents.Add(new UserConsent
            {
                ConsentType = ConsentType.PhotoProcessing,
                Version = request.AcceptedPrivacyPolicyVersion,
                AcceptedAt = now,
                IpAddress = context.IpAddress,
                UserAgent = context.UserAgent
            });
        }

        await userRepository.AddAsync(user, cancellationToken);
        await auditService.RecordAsync(user.Id, AuthAuditEventType.UserRegistered, AuthAuditResult.Succeeded, context, null, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        var loaded = await userRepository.GetByIdAsync(user.Id, cancellationToken) ?? user;
        return await CreateSessionAsync(loaded, context, cancellationToken);
    }

    public async Task<AuthTokenResponse> LoginAsync(LoginRequest request, RequestContext context, CancellationToken cancellationToken)
    {
        var normalizedEmail = EmailAddress.Normalize(request.Email);
        var user = await userRepository.GetByNormalizedEmailWithAuthGraphAsync(normalizedEmail, cancellationToken);
        if (user?.Credential is null || !passwordHashProvider.VerifyPassword(user.Credential.PasswordHash, request.Password))
        {
            await auditService.RecordAsync(user?.Id, AuthAuditEventType.LoginFailed, AuthAuditResult.Failed, context, null, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            throw AuthException.Unauthorized();
        }

        if (user.Status != UserStatus.Active)
        {
            var eventType = user.Status == UserStatus.Blocked ? AuthAuditEventType.UserBlocked : AuthAuditEventType.LoginFailed;
            await auditService.RecordAsync(user.Id, eventType, AuthAuditResult.Failed, context, null, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            throw AuthException.Unauthorized();
        }

        var model = AuthUserMapper.ToAuthenticatedUser(user);
        if (model.Roles.Any(role => RoleNames.MerchantRoles.Contains(role)) && model.StoreId is null)
        {
            await auditService.RecordAsync(user.Id, AuthAuditEventType.LoginFailed, AuthAuditResult.Failed, context, "{\"reason\":\"missing_active_store\"}", cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            throw AuthException.Forbidden("Operador sem vínculo ativo com loja.");
        }

        user.LastLoginAt = clock.UtcNow;
        await auditService.RecordAsync(user.Id, AuthAuditEventType.LoginSucceeded, AuthAuditResult.Succeeded, context, null, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return await CreateSessionAsync(user, context, cancellationToken);
    }

    public Task LogoutAsync(LogoutRequest request, RequestContext context, CancellationToken cancellationToken) =>
        refreshTokenService.RevokeAsync(request.RefreshToken, context, cancellationToken);

    public async Task<CurrentUserResponse> GetCurrentUserAsync(Guid userId, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(userId, cancellationToken) ?? throw AuthException.Unauthorized();
        if (user.Status != UserStatus.Active)
        {
            throw AuthException.Unauthorized();
        }

        var model = AuthUserMapper.ToAuthenticatedUser(user);
        return new CurrentUserResponse(model.Id, model.Email, model.Name, model.Roles, model.Permissions, model.StoreId);
    }

    private async Task<AuthTokenResponse> CreateSessionAsync(User user, RequestContext context, CancellationToken cancellationToken)
    {
        var model = AuthUserMapper.ToAuthenticatedUser(user);
        var jwt = jwtTokenService.CreateAccessToken(model);
        var refreshToken = await refreshTokenService.IssueAsync(model, context, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return new AuthTokenResponse(jwt.AccessToken, jwt.ExpiresAt, refreshToken.Token, refreshToken.ExpiresAt,
            new AuthenticatedUserResponse(model.Id, model.Email, model.Name, model.Roles, model.Permissions, model.StoreId));
    }
}

using Kuva.Auth.Business.Interfaces;
using Kuva.Auth.Business.Models;
using Kuva.Auth.Entities.Constants;
using Kuva.Auth.Entities.Dtos.Requests;
using Kuva.Auth.Entities.Dtos.Responses;
using Kuva.Auth.Entities.Entities;
using Kuva.Auth.Entities.Enums;
using Kuva.Auth.Repository.Interfaces;
using Microsoft.Extensions.Options;

namespace Kuva.Auth.Business.Services;

public sealed class RefreshTokenService(
    IRefreshTokenRepository refreshTokenRepository,
    ITokenHashProvider tokenHashProvider,
    IJwtTokenService jwtTokenService,
    IAuthAuditService auditService,
    IUnitOfWork unitOfWork,
    IClock clock,
    IOptions<RefreshTokenOptions> options) : IRefreshTokenService
{
    private readonly RefreshTokenOptions _options = options.Value;

    public async Task<RefreshTokenResult> IssueAsync(AuthenticatedUserModel user, RequestContext context, CancellationToken cancellationToken)
    {
        var token = tokenHashProvider.GenerateToken(_options.TokenBytes);
        var expiresAt = GetExpiration(user);
        var entity = new RefreshToken
        {
            UserId = user.Id,
            TokenHash = tokenHashProvider.HashToken(token),
            ExpiresAt = expiresAt,
            IpAddress = context.IpAddress,
            UserAgent = context.UserAgent,
            CreatedAt = clock.UtcNow
        };

        await refreshTokenRepository.AddAsync(entity, cancellationToken);
        return new RefreshTokenResult(token, expiresAt, entity.Id);
    }

    public async Task<AuthTokenResponse> RefreshAsync(RefreshTokenRequest request, RequestContext context, CancellationToken cancellationToken)
    {
        var existing = await refreshTokenRepository.GetByHashAsync(tokenHashProvider.HashToken(request.RefreshToken), cancellationToken);
        if (existing is null || existing.User is null || !existing.IsActive(clock.UtcNow) || existing.User.Status != UserStatus.Active)
        {
            await auditService.RecordAsync(existing?.UserId, AuthAuditEventType.RefreshTokenFailed, AuthAuditResult.Failed, context, null, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            throw AuthException.Unauthorized();
        }

        var userModel = AuthUserMapper.ToAuthenticatedUser(existing.User);
        ValidateMerchantStore(userModel);
        var jwt = jwtTokenService.CreateAccessToken(userModel);
        var replacement = await IssueAsync(userModel, context, cancellationToken);
        existing.RevokedAt = clock.UtcNow;
        existing.ReplacedByTokenId = replacement.EntityId;

        await auditService.RecordAsync(existing.UserId, AuthAuditEventType.RefreshTokenSucceeded, AuthAuditResult.Succeeded, context, null, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new AuthTokenResponse(jwt.AccessToken, jwt.ExpiresAt, replacement.Token, replacement.ExpiresAt, ToResponse(userModel));
    }

    public async Task RevokeAsync(string refreshToken, RequestContext context, CancellationToken cancellationToken)
    {
        var entity = await refreshTokenRepository.GetByHashAsync(tokenHashProvider.HashToken(refreshToken), cancellationToken);
        if (entity is not null && entity.RevokedAt is null)
        {
            entity.RevokedAt = clock.UtcNow;
            await auditService.RecordAsync(entity.UserId, AuthAuditEventType.LogoutSucceeded, AuthAuditResult.Succeeded, context, null, cancellationToken);
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task RevokeAllActiveAsync(Guid userId, RequestContext context, CancellationToken cancellationToken)
    {
        var tokens = await refreshTokenRepository.GetActiveByUserIdAsync(userId, clock.UtcNow, cancellationToken);
        foreach (var token in tokens)
        {
            token.RevokedAt = clock.UtcNow;
        }

        await auditService.RecordAsync(userId, AuthAuditEventType.RefreshTokenRevoked, AuthAuditResult.Succeeded, context, null, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    private DateTimeOffset GetExpiration(AuthenticatedUserModel user)
    {
        var isMerchant = user.Roles.Any(role => role == RoleNames.StoreOwner || role == RoleNames.StoreOperator);
        return isMerchant ? clock.UtcNow.AddHours(_options.MerchantExpirationHours) : clock.UtcNow.AddDays(_options.ConsumerExpirationDays);
    }

    private static AuthenticatedUserResponse ToResponse(AuthenticatedUserModel user) =>
        new(user.Id, user.Email, user.Name, user.Roles, user.Permissions, user.StoreId);

    private static void ValidateMerchantStore(AuthenticatedUserModel user)
    {
        if (user.Roles.Any(role => RoleNames.MerchantRoles.Contains(role)) && user.StoreId is null)
        {
            throw AuthException.Forbidden("Operador sem vínculo ativo com loja.");
        }
    }
}

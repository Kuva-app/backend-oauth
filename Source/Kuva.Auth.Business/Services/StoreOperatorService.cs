using Kuva.Auth.Business.Interfaces;
using Kuva.Auth.Business.Models;
using Kuva.Auth.Entities.Constants;
using Kuva.Auth.Entities.Dtos.Internal;
using Kuva.Auth.Entities.Entities;
using Kuva.Auth.Entities.Enums;
using Kuva.Auth.Entities.ValueObjects;
using Kuva.Auth.Repository.Interfaces;

namespace Kuva.Auth.Business.Services;

public sealed class StoreOperatorService(
    IUserRepository userRepository,
    IRoleRepository roleRepository,
    IStoreOperatorRepository storeOperatorRepository,
    IPasswordHashProvider passwordHashProvider,
    IPasswordPolicyService passwordPolicyService,
    IAuthAuditService auditService,
    IUnitOfWork unitOfWork,
    IClock clock) : IStoreOperatorService
{
    public async Task<StoreOperatorResponse> CreateAsync(Guid storeId, CreateStoreOperatorRequest request, RequestContext context, CancellationToken cancellationToken)
    {
        if (request.Role is not (RoleNames.StoreOperator or RoleNames.StoreOwner))
        {
            throw AuthException.Unprocessable("invalid_operator_role", "Role de operador inválida.");
        }

        var policy = passwordPolicyService.Validate(request.TemporaryPassword);
        if (!policy.IsValid)
        {
            throw AuthException.Unprocessable("weak_password", "A senha temporária não atende à política definida.", policy.Violations);
        }

        var email = EmailAddress.Create(request.Email);
        var user = await userRepository.GetByNormalizedEmailWithAuthGraphAsync(email.NormalizedValue, cancellationToken);
        var role = await roleRepository.GetByNameAsync(request.Role, cancellationToken)
            ?? throw AuthException.Unprocessable("missing_role", "Role de operador não encontrada.");

        var now = clock.UtcNow;
        if (user is null)
        {
            user = new User
            {
                Email = email.Value,
                NormalizedEmail = email.NormalizedValue,
                Name = request.Name,
                Phone = request.Phone,
                Status = UserStatus.Active,
                CreatedAt = now,
                Credential = new UserCredential
                {
                    PasswordHash = passwordHashProvider.HashPassword(request.TemporaryPassword),
                    PasswordAlgorithm = passwordHashProvider.Algorithm,
                    PasswordUpdatedAt = now,
                    CreatedAt = now
                },
                UserRoles = new List<UserRole> { new() { RoleId = role.Id, CreatedAt = now } }
            };

            await userRepository.AddAsync(user, cancellationToken);
        }
        else if (await storeOperatorRepository.ExistsAsync(storeId, user.Id, cancellationToken))
        {
            throw AuthException.Conflict("Operador já vinculado a esta loja.");
        }
        else if (!user.UserRoles.Any(x => x.RoleId == role.Id))
        {
            user.UserRoles.Add(new UserRole { RoleId = role.Id, CreatedAt = now });
        }

        var storeOperator = new StoreOperator
        {
            StoreId = storeId,
            UserId = user.Id,
            Role = request.Role,
            Status = StoreOperatorStatus.Active,
            CreatedAt = now
        };

        await storeOperatorRepository.AddAsync(storeOperator, cancellationToken);
        await auditService.RecordAsync(user.Id, AuthAuditEventType.StoreOperatorCreated, AuthAuditResult.Succeeded, context, null, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return ToResponse(storeOperator, user);
    }

    public async Task<IReadOnlyCollection<StoreOperatorResponse>> ListByStoreAsync(Guid storeId, CancellationToken cancellationToken)
    {
        var operators = await storeOperatorRepository.ListByStoreAsync(storeId, cancellationToken);
        return operators.Select(x => ToResponse(x, x.User!)).ToArray();
    }

    public async Task UpdateStatusAsync(Guid operatorId, StoreOperatorStatus status, RequestContext context, CancellationToken cancellationToken)
    {
        await storeOperatorRepository.UpdateStatusAsync(operatorId, status, cancellationToken);
        await auditService.RecordAsync(null, status == StoreOperatorStatus.Blocked ? AuthAuditEventType.StoreOperatorBlocked : AuthAuditEventType.StoreOperatorCreated, AuthAuditResult.Succeeded, context, null, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    private static StoreOperatorResponse ToResponse(StoreOperator storeOperator, User user) =>
        new(storeOperator.Id, storeOperator.StoreId, user.Id, user.Email, user.Name, storeOperator.Role, storeOperator.Status);
}

using Kuva.Auth.Entities.Entities;
using Kuva.Auth.Repository.Context;
using Kuva.Auth.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Kuva.Auth.Repository.Repositories;

public sealed class UserRepository(AuthDbContext dbContext) : IUserRepository
{
    public Task AddAsync(User user, CancellationToken cancellationToken) => dbContext.Users.AddAsync(user, cancellationToken).AsTask();

    public Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken) =>
        dbContext.Users
            .Include(x => x.UserRoles).ThenInclude(x => x.Role).ThenInclude(x => x!.RolePermissions).ThenInclude(x => x.Permission)
            .Include(x => x.StoreOperators)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public Task<User?> GetByNormalizedEmailAsync(string normalizedEmail, CancellationToken cancellationToken) =>
        dbContext.Users.FirstOrDefaultAsync(x => x.NormalizedEmail == normalizedEmail, cancellationToken);

    public Task<User?> GetByNormalizedEmailWithAuthGraphAsync(string normalizedEmail, CancellationToken cancellationToken) =>
        dbContext.Users
            .Include(x => x.Credential)
            .Include(x => x.UserRoles).ThenInclude(x => x.Role).ThenInclude(x => x!.RolePermissions).ThenInclude(x => x.Permission)
            .Include(x => x.StoreOperators)
            .FirstOrDefaultAsync(x => x.NormalizedEmail == normalizedEmail, cancellationToken);

    public Task<bool> ExistsByNormalizedEmailAsync(string normalizedEmail, CancellationToken cancellationToken) =>
        dbContext.Users.AnyAsync(x => x.NormalizedEmail == normalizedEmail, cancellationToken);
}

using Kuva.Auth.Entities.Entities;
using Kuva.Auth.Repository.Context;
using Kuva.Auth.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Kuva.Auth.Repository.Repositories;

public sealed class RefreshTokenRepository(AuthDbContext dbContext) : IRefreshTokenRepository
{
    public Task AddAsync(RefreshToken refreshToken, CancellationToken cancellationToken) =>
        dbContext.RefreshTokens.AddAsync(refreshToken, cancellationToken).AsTask();

    public Task<RefreshToken?> GetByHashAsync(string tokenHash, CancellationToken cancellationToken) =>
        dbContext.RefreshTokens
            .Include(x => x.User).ThenInclude(x => x!.UserRoles).ThenInclude(x => x.Role).ThenInclude(x => x!.RolePermissions).ThenInclude(x => x.Permission)
            .Include(x => x.User).ThenInclude(x => x!.StoreOperators)
            .FirstOrDefaultAsync(x => x.TokenHash == tokenHash, cancellationToken);

    public async Task<IReadOnlyCollection<RefreshToken>> GetActiveByUserIdAsync(Guid userId, DateTimeOffset now, CancellationToken cancellationToken) =>
        await dbContext.RefreshTokens
            .Where(x => x.UserId == userId && x.RevokedAt == null && x.ExpiresAt > now)
            .ToArrayAsync(cancellationToken);
}

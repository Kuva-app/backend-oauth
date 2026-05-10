using Kuva.Auth.Entities.Entities;

namespace Kuva.Auth.Repository.Interfaces;

public interface IRefreshTokenRepository
{
    Task AddAsync(RefreshToken refreshToken, CancellationToken cancellationToken);
    Task<RefreshToken?> GetByHashAsync(string tokenHash, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<RefreshToken>> GetActiveByUserIdAsync(Guid userId, DateTimeOffset now, CancellationToken cancellationToken);
}

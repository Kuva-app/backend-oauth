using Kuva.Auth.Entities.Entities;

namespace Kuva.Auth.Repository.Interfaces;

public interface IUserRepository
{
    Task AddAsync(User user, CancellationToken cancellationToken);
    Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<User?> GetByNormalizedEmailAsync(string normalizedEmail, CancellationToken cancellationToken);
    Task<User?> GetByNormalizedEmailWithAuthGraphAsync(string normalizedEmail, CancellationToken cancellationToken);
    Task<bool> ExistsByNormalizedEmailAsync(string normalizedEmail, CancellationToken cancellationToken);
}

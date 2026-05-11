using Kuva.Auth.Entities.Entities;

namespace Kuva.Auth.Repository.Interfaces;

public interface IRoleRepository
{
    Task<Role?> GetByNameAsync(string name, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<string>> GetPermissionsForRolesAsync(IEnumerable<string> roleNames, CancellationToken cancellationToken);
}

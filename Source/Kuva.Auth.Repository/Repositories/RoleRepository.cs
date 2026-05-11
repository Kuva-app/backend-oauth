using Kuva.Auth.Entities.Entities;
using Kuva.Auth.Repository.Context;
using Kuva.Auth.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Kuva.Auth.Repository.Repositories;

public sealed class RoleRepository(AuthDbContext dbContext) : IRoleRepository
{
    public Task<Role?> GetByNameAsync(string name, CancellationToken cancellationToken) =>
        dbContext.Roles.FirstOrDefaultAsync(x => x.NormalizedName == name.ToUpperInvariant() && x.Active, cancellationToken);

    public async Task<IReadOnlyCollection<string>> GetPermissionsForRolesAsync(IEnumerable<string> roleNames, CancellationToken cancellationToken)
    {
        var normalized = roleNames.Select(x => x.ToUpperInvariant()).ToArray();
        return await dbContext.RolePermissions
            .Where(x => x.Role != null && normalized.Contains(x.Role.NormalizedName) && x.Role.Active && x.Permission != null && x.Permission.Active)
            .Select(x => x.Permission!.Name)
            .Distinct()
            .OrderBy(x => x)
            .ToArrayAsync(cancellationToken);
    }
}

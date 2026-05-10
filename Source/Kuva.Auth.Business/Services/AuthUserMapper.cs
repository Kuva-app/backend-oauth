using Kuva.Auth.Business.Models;
using Kuva.Auth.Entities.Constants;
using Kuva.Auth.Entities.Entities;
using Kuva.Auth.Entities.Enums;

namespace Kuva.Auth.Business.Services;

internal static class AuthUserMapper
{
    public static AuthenticatedUserModel ToAuthenticatedUser(User user)
    {
        var roles = user.UserRoles
            .Where(x => x.Role is { Active: true })
            .Select(x => x.Role!.Name)
            .Distinct()
            .OrderBy(x => x)
            .ToArray();

        var permissions = user.UserRoles
            .Where(x => x.Role is { Active: true })
            .SelectMany(x => x.Role!.RolePermissions)
            .Where(x => x.Permission is { Active: true })
            .Select(x => x.Permission!.Name)
            .Distinct()
            .OrderBy(x => x)
            .ToArray();

        Guid? storeId = null;
        if (roles.Any(role => RoleNames.MerchantRoles.Contains(role)))
        {
            storeId = user.StoreOperators.FirstOrDefault(x => x.Status == StoreOperatorStatus.Active)?.StoreId;
        }

        return new AuthenticatedUserModel(user.Id, user.Email, user.Name, roles, permissions, storeId);
    }
}

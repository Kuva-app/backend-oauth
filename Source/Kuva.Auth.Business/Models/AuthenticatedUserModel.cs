namespace Kuva.Auth.Business.Models;

public sealed record AuthenticatedUserModel(
    Guid Id,
    string Email,
    string? Name,
    IReadOnlyCollection<string> Roles,
    IReadOnlyCollection<string> Permissions,
    Guid? StoreId);

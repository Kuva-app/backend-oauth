namespace Kuva.Auth.Entities.Dtos.Responses;

public sealed record CurrentUserResponse(
    Guid Id,
    string Email,
    string? Name,
    IReadOnlyCollection<string> Roles,
    IReadOnlyCollection<string> Permissions,
    Guid? StoreId);

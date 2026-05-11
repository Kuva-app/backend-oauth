using Kuva.Auth.Entities.Enums;

namespace Kuva.Auth.Entities.Dtos.Internal;

public sealed record StoreOperatorResponse(
    Guid Id,
    Guid StoreId,
    Guid UserId,
    string Email,
    string? Name,
    string Role,
    StoreOperatorStatus Status);

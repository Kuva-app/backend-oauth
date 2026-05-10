using System.ComponentModel.DataAnnotations;

namespace Kuva.Auth.Entities.Dtos.Internal;

public sealed record CreateStoreOperatorRequest(
    [property: Required, MaxLength(160)] string Name,
    [property: Required, EmailAddress, MaxLength(255)] string Email,
    [property: MaxLength(32)] string? Phone,
    [property: Required] string TemporaryPassword,
    [property: Required, MaxLength(64)] string Role);

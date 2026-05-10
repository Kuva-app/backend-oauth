using System.ComponentModel.DataAnnotations;

namespace Kuva.Auth.Entities.Dtos.Requests;

public sealed record LoginRequest(
    [property: Required, EmailAddress, MaxLength(255)] string Email,
    [property: Required] string Password);

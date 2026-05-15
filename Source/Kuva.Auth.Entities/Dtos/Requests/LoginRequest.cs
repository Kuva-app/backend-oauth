using System.ComponentModel.DataAnnotations;

namespace Kuva.Auth.Entities.Dtos.Requests;

public sealed record LoginRequest(
    [param: Required, EmailAddress, MaxLength(255)] string Email,
    [param: Required] string Password);

using System.ComponentModel.DataAnnotations;

namespace Kuva.Auth.Entities.Dtos.Requests;

public sealed record ForgotPasswordRequest([param: Required, EmailAddress] string Email);

using System.ComponentModel.DataAnnotations;

namespace Kuva.Auth.Entities.Dtos.Requests;

public sealed record ForgotPasswordRequest([property: Required, EmailAddress] string Email);

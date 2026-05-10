using System.ComponentModel.DataAnnotations;

namespace Kuva.Auth.Entities.Dtos.Requests;

public sealed record ResetPasswordRequest([property: Required] string ResetToken, [property: Required] string NewPassword);

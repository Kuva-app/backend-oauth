using System.ComponentModel.DataAnnotations;

namespace Kuva.Auth.Entities.Dtos.Requests;

public sealed record ResetPasswordRequest([param: Required] string ResetToken, [param: Required] string NewPassword);

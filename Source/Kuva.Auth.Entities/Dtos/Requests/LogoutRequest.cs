using System.ComponentModel.DataAnnotations;

namespace Kuva.Auth.Entities.Dtos.Requests;

public sealed record LogoutRequest([param: Required] string RefreshToken);

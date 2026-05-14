using System.ComponentModel.DataAnnotations;

namespace Kuva.Auth.Entities.Dtos.Requests;

public sealed record RefreshTokenRequest([param: Required] string RefreshToken);

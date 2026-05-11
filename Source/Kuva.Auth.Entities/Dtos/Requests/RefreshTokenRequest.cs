using System.ComponentModel.DataAnnotations;

namespace Kuva.Auth.Entities.Dtos.Requests;

public sealed record RefreshTokenRequest([property: Required] string RefreshToken);

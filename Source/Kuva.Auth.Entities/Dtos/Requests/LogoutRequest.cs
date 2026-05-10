using System.ComponentModel.DataAnnotations;

namespace Kuva.Auth.Entities.Dtos.Requests;

public sealed record LogoutRequest([property: Required] string RefreshToken);

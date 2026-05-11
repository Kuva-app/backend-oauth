namespace Kuva.Auth.Business.Models;

public sealed record JwtTokenResult(string AccessToken, DateTimeOffset ExpiresAt);

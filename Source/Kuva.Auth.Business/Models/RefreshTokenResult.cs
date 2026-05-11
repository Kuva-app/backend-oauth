namespace Kuva.Auth.Business.Models;

public sealed record RefreshTokenResult(string Token, DateTimeOffset ExpiresAt, Guid EntityId);

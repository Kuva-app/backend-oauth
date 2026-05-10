namespace Kuva.Auth.Business.Models;

public sealed class JwtOptions
{
    public string Issuer { get; set; } = "https://api.kuva.com.br/auth";
    public string Audience { get; set; } = "kuva-api";
    public int AccessTokenMinutes { get; set; } = 15;
    public string KeyId { get; set; } = "kuva-auth-local";
    public string? PrivateKeyPem { get; set; }
    public string PrivateKeySecretName { get; set; } = "Jwt--PrivateKeyPem";
}

public sealed class RefreshTokenOptions
{
    public int ConsumerExpirationDays { get; set; } = 30;
    public int MerchantExpirationHours { get; set; } = 12;
    public int TokenBytes { get; set; } = 64;
}

public sealed class PasswordPolicyOptions
{
    public int MinimumLength { get; set; } = 10;
    public bool RequireUppercase { get; set; } = true;
    public bool RequireLowercase { get; set; } = true;
    public bool RequireDigit { get; set; } = true;
    public bool RequireSpecialCharacter { get; set; } = true;
}

namespace Kuva.Auth.Service.Options;

public sealed class CorsOptions
{
    public string[] AllowedOrigins { get; set; } = [];
}

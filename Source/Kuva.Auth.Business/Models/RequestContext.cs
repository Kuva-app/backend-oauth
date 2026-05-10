namespace Kuva.Auth.Business.Models;

public sealed record RequestContext(string? IpAddress, string? UserAgent, string? CorrelationId)
{
    public static RequestContext Empty { get; } = new(null, null, null);
}

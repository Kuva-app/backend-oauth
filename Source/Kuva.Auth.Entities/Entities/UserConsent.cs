using Kuva.Auth.Entities.Enums;

namespace Kuva.Auth.Entities.Entities;

public sealed class UserConsent
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public ConsentType ConsentType { get; set; }
    public string Version { get; set; } = string.Empty;
    public DateTimeOffset AcceptedAt { get; set; } = DateTimeOffset.UtcNow;
    public string? IpAddress { get; set; }
    public string? UserAgent { get; set; }
    public User? User { get; set; }
}

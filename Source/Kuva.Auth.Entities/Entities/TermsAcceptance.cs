namespace Kuva.Auth.Entities.Entities;

public sealed class TermsAcceptance
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public string TermsVersion { get; set; } = string.Empty;
    public string PrivacyPolicyVersion { get; set; } = string.Empty;
    public DateTimeOffset AcceptedAt { get; set; } = DateTimeOffset.UtcNow;
    public string? IpAddress { get; set; }
    public string? UserAgent { get; set; }
    public User? User { get; set; }
}

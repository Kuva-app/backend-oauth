using Kuva.Auth.Entities.Enums;

namespace Kuva.Auth.Entities.Entities;

public sealed class AuthAuditLog
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid? UserId { get; set; }
    public AuthAuditEventType EventType { get; set; }
    public AuthAuditResult Result { get; set; }
    public string? IpAddress { get; set; }
    public string? UserAgent { get; set; }
    public string? CorrelationId { get; set; }
    public string? MetadataJson { get; set; }
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public User? User { get; set; }
}

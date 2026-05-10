using Kuva.Auth.Business.Interfaces;
using Kuva.Auth.Business.Models;
using Kuva.Auth.Entities.Entities;
using Kuva.Auth.Entities.Enums;
using Kuva.Auth.Repository.Interfaces;

namespace Kuva.Auth.Business.Services;

public sealed class AuthAuditService(IAuthAuditRepository auditRepository) : IAuthAuditService
{
    public Task RecordAsync(Guid? userId, AuthAuditEventType eventType, AuthAuditResult result, RequestContext context, string? metadataJson, CancellationToken cancellationToken)
    {
        return auditRepository.AddAsync(new AuthAuditLog
        {
            UserId = userId,
            EventType = eventType,
            Result = result,
            IpAddress = context.IpAddress,
            UserAgent = context.UserAgent,
            CorrelationId = context.CorrelationId,
            MetadataJson = metadataJson,
            CreatedAt = DateTimeOffset.UtcNow
        }, cancellationToken);
    }
}

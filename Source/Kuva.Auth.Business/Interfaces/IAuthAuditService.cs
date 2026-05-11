using Kuva.Auth.Business.Models;
using Kuva.Auth.Entities.Enums;

namespace Kuva.Auth.Business.Interfaces;

public interface IAuthAuditService
{
    Task RecordAsync(Guid? userId, AuthAuditEventType eventType, AuthAuditResult result, RequestContext context, string? metadataJson, CancellationToken cancellationToken);
}

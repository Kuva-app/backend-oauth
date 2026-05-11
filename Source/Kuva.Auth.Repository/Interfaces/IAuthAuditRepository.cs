using Kuva.Auth.Entities.Entities;

namespace Kuva.Auth.Repository.Interfaces;

public interface IAuthAuditRepository
{
    Task AddAsync(AuthAuditLog auditLog, CancellationToken cancellationToken);
}

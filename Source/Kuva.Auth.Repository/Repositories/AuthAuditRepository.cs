using Kuva.Auth.Entities.Entities;
using Kuva.Auth.Repository.Context;
using Kuva.Auth.Repository.Interfaces;

namespace Kuva.Auth.Repository.Repositories;

public sealed class AuthAuditRepository(AuthDbContext dbContext) : IAuthAuditRepository
{
    public Task AddAsync(AuthAuditLog auditLog, CancellationToken cancellationToken) =>
        dbContext.AuthAuditLogs.AddAsync(auditLog, cancellationToken).AsTask();
}

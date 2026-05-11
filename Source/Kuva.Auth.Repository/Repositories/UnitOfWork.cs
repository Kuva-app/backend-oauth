using Kuva.Auth.Repository.Context;
using Kuva.Auth.Repository.Interfaces;

namespace Kuva.Auth.Repository.Repositories;

public sealed class UnitOfWork(AuthDbContext dbContext) : IUnitOfWork
{
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken) => dbContext.SaveChangesAsync(cancellationToken);
}

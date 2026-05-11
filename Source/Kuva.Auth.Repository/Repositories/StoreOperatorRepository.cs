using Kuva.Auth.Entities.Entities;
using Kuva.Auth.Entities.Enums;
using Kuva.Auth.Repository.Context;
using Kuva.Auth.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Kuva.Auth.Repository.Repositories;

public sealed class StoreOperatorRepository(AuthDbContext dbContext) : IStoreOperatorRepository
{
    public Task AddAsync(StoreOperator storeOperator, CancellationToken cancellationToken) =>
        dbContext.StoreOperators.AddAsync(storeOperator, cancellationToken).AsTask();

    public Task<StoreOperator?> GetActiveByUserIdAsync(Guid userId, CancellationToken cancellationToken) =>
        dbContext.StoreOperators.Include(x => x.User).FirstOrDefaultAsync(x => x.UserId == userId && x.Status == StoreOperatorStatus.Active, cancellationToken);

    public Task<StoreOperator?> GetByIdAsync(Guid id, CancellationToken cancellationToken) =>
        dbContext.StoreOperators.Include(x => x.User).FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public Task<StoreOperator?> GetByStoreAndUserAsync(Guid storeId, Guid userId, CancellationToken cancellationToken) =>
        dbContext.StoreOperators.Include(x => x.User).FirstOrDefaultAsync(x => x.StoreId == storeId && x.UserId == userId, cancellationToken);

    public async Task<IReadOnlyCollection<StoreOperator>> ListByStoreAsync(Guid storeId, CancellationToken cancellationToken) =>
        await dbContext.StoreOperators.Include(x => x.User).Where(x => x.StoreId == storeId).OrderBy(x => x.User!.Email).ToArrayAsync(cancellationToken);

    public Task<bool> ExistsAsync(Guid storeId, Guid userId, CancellationToken cancellationToken) =>
        dbContext.StoreOperators.AnyAsync(x => x.StoreId == storeId && x.UserId == userId, cancellationToken);

    public async Task UpdateStatusAsync(Guid operatorId, StoreOperatorStatus status, CancellationToken cancellationToken)
    {
        var storeOperator = await dbContext.StoreOperators.FirstOrDefaultAsync(x => x.Id == operatorId, cancellationToken);
        if (storeOperator is null)
        {
            return;
        }

        storeOperator.Status = status;
        storeOperator.UpdatedAt = DateTimeOffset.UtcNow;
    }
}

using Kuva.Auth.Entities.Entities;
using Kuva.Auth.Entities.Enums;

namespace Kuva.Auth.Repository.Interfaces;

public interface IStoreOperatorRepository
{
    Task AddAsync(StoreOperator storeOperator, CancellationToken cancellationToken);
    Task<StoreOperator?> GetActiveByUserIdAsync(Guid userId, CancellationToken cancellationToken);
    Task<StoreOperator?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<StoreOperator?> GetByStoreAndUserAsync(Guid storeId, Guid userId, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<StoreOperator>> ListByStoreAsync(Guid storeId, CancellationToken cancellationToken);
    Task<bool> ExistsAsync(Guid storeId, Guid userId, CancellationToken cancellationToken);
    Task UpdateStatusAsync(Guid operatorId, StoreOperatorStatus status, CancellationToken cancellationToken);
}

using Kuva.Auth.Business.Models;
using Kuva.Auth.Entities.Dtos.Internal;
using Kuva.Auth.Entities.Enums;

namespace Kuva.Auth.Business.Interfaces;

public interface IStoreOperatorService
{
    Task<StoreOperatorResponse> CreateAsync(Guid storeId, CreateStoreOperatorRequest request, RequestContext context, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<StoreOperatorResponse>> ListByStoreAsync(Guid storeId, CancellationToken cancellationToken);
    Task UpdateStatusAsync(Guid operatorId, StoreOperatorStatus status, RequestContext context, CancellationToken cancellationToken);
}

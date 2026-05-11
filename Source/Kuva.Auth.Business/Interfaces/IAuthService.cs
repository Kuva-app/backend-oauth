using Kuva.Auth.Business.Models;
using Kuva.Auth.Entities.Dtos.Requests;
using Kuva.Auth.Entities.Dtos.Responses;

namespace Kuva.Auth.Business.Interfaces;

public interface IAuthService
{
    Task<AuthTokenResponse> RegisterConsumerAsync(RegisterConsumerRequest request, RequestContext context, CancellationToken cancellationToken);
    Task<AuthTokenResponse> LoginAsync(LoginRequest request, RequestContext context, CancellationToken cancellationToken);
    Task LogoutAsync(LogoutRequest request, RequestContext context, CancellationToken cancellationToken);
    Task<CurrentUserResponse> GetCurrentUserAsync(Guid userId, CancellationToken cancellationToken);
}

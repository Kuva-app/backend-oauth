using Kuva.Auth.Business.Models;
using Kuva.Auth.Entities.Dtos.Requests;
using Kuva.Auth.Entities.Dtos.Responses;

namespace Kuva.Auth.Business.Interfaces;

public interface IRefreshTokenService
{
    Task<RefreshTokenResult> IssueAsync(AuthenticatedUserModel user, RequestContext context, CancellationToken cancellationToken);
    Task<AuthTokenResponse> RefreshAsync(RefreshTokenRequest request, RequestContext context, CancellationToken cancellationToken);
    Task RevokeAsync(string refreshToken, RequestContext context, CancellationToken cancellationToken);
    Task RevokeAllActiveAsync(Guid userId, RequestContext context, CancellationToken cancellationToken);
}

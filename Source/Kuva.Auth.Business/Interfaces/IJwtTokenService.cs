using Kuva.Auth.Business.Models;

namespace Kuva.Auth.Business.Interfaces;

public interface IJwtTokenService
{
    JwtTokenResult CreateAccessToken(AuthenticatedUserModel user);
}

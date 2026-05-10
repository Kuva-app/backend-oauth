using Kuva.Auth.Business.Models;

namespace Kuva.Auth.Business.Interfaces;

public interface IJwtKeyProvider
{
    JwtSigningKeyModel GetSigningKey();
}

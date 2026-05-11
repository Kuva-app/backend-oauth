using Kuva.Auth.Entities.Dtos.Responses;

namespace Kuva.Auth.Business.Interfaces;

public interface IJwksProvider
{
    JwksResponse GetJwks();
}

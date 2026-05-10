using Kuva.Auth.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Kuva.Auth.Service.Controllers;

[ApiController]
[Route("api/v1/auth/jwks")]
public sealed class JwksController(IJwksProvider jwksProvider) : ControllerBase
{
    [HttpGet]
    public IActionResult Get() => Ok(jwksProvider.GetJwks());
}

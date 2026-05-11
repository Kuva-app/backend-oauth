using Kuva.Auth.Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kuva.Auth.Service.Controllers;

[ApiController]
[Route("api/v1/auth/me")]
public sealed class MeController(IAuthService authService) : ControllerBase
{
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        var response = await authService.GetCurrentUserAsync(User.GetUserId(), cancellationToken);
        return Ok(response);
    }
}

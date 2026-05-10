using Microsoft.AspNetCore.Mvc;

namespace Kuva.Auth.Service.Controllers;

[ApiController]
[Route("api/v1/auth/health")]
public sealed class HealthController : ControllerBase
{
    [HttpGet]
    public IActionResult Get() => Ok(new { status = "Healthy" });
}

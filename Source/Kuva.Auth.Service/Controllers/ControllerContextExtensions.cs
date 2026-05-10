using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Kuva.Auth.Business.Models;
using Kuva.Auth.Service.Middlewares;

namespace Kuva.Auth.Service.Controllers;

internal static class ControllerContextExtensions
{
    public static RequestContext ToRequestContext(this HttpContext context) => new(
        context.Connection.RemoteIpAddress?.ToString(),
        context.Request.Headers.UserAgent.ToString(),
        context.Items.TryGetValue(CorrelationIdMiddleware.HeaderName, out var correlationId) ? correlationId?.ToString() : null);

    public static Guid GetUserId(this ClaimsPrincipal principal)
    {
        var value = principal.FindFirstValue(JwtRegisteredClaimNames.Sub) ?? principal.FindFirstValue("sub");
        return Guid.TryParse(value, out var userId) ? userId : throw new UnauthorizedAccessException();
    }
}

using Kuva.Auth.Business.Interfaces;
using Kuva.Auth.Entities.Dtos.Requests;
using Kuva.Auth.Service.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kuva.Auth.Service.Controllers;

[ApiController]
[Route("api/v1/auth")]
public sealed class AuthController(IAuthService authService, IRefreshTokenService refreshTokenService) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterConsumerRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var response = await authService.RegisterConsumerAsync(request, HttpContext.ToRequestContext(), cancellationToken);
            MetricsExtensions.RegisterSuccessTotal.Inc();
            MetricsExtensions.TokenIssuedTotal.WithLabels(string.Join(",", response.User.Roles)).Inc();
            return CreatedAtAction(nameof(MeController.Get), "Me", null, response);
        }
        catch
        {
            MetricsExtensions.RegisterFailedTotal.WithLabels("business_or_validation").Inc();
            throw;
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var response = await authService.LoginAsync(request, HttpContext.ToRequestContext(), cancellationToken);
            MetricsExtensions.LoginSuccessTotal.WithLabels(string.Join(",", response.User.Roles)).Inc();
            MetricsExtensions.TokenIssuedTotal.WithLabels(string.Join(",", response.User.Roles)).Inc();
            if (response.User.StoreId is not null)
            {
                MetricsExtensions.StoreOperatorLoginTotal.Inc();
            }

            return Ok(response);
        }
        catch
        {
            MetricsExtensions.LoginFailedTotal.WithLabels("invalid_credentials_or_business").Inc();
            throw;
        }
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> Refresh(RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var response = await refreshTokenService.RefreshAsync(request, HttpContext.ToRequestContext(), cancellationToken);
            MetricsExtensions.RefreshSuccessTotal.Inc();
            MetricsExtensions.TokenIssuedTotal.WithLabels(string.Join(",", response.User.Roles)).Inc();
            MetricsExtensions.TokenRevokedTotal.Inc();
            return Ok(response);
        }
        catch
        {
            MetricsExtensions.RefreshFailedTotal.WithLabels("invalid_refresh_token").Inc();
            throw;
        }
    }

    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout(LogoutRequest request, CancellationToken cancellationToken)
    {
        await authService.LogoutAsync(request, HttpContext.ToRequestContext(), cancellationToken);
        MetricsExtensions.LogoutTotal.Inc();
        MetricsExtensions.TokenRevokedTotal.Inc();
        return NoContent();
    }
}

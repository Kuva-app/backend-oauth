using FluentAssertions;
using Kuva.Auth.Business.Interfaces;
using Kuva.Auth.Business.Models;
using Kuva.Auth.Entities.Dtos.Requests;
using Kuva.Auth.Entities.Dtos.Responses;
using Kuva.Auth.Service.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Kuva.Auth.Tests.Service;

public sealed class AuthControllerTests
{
    [Test]
    public async Task Register_ShouldReturnCreated()
    {
        var auth = new Mock<IAuthService>();
        var refresh = new Mock<IRefreshTokenService>();
        auth.Setup(x => x.RegisterConsumerAsync(It.IsAny<RegisterConsumerRequest>(), It.IsAny<RequestContext>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Response());
        var controller = new AuthController(auth.Object, refresh.Object) { ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() } };

        var result = await controller.Register(new RegisterConsumerRequest("Cliente", "cliente@email.com", null, "SenhaSegura@123", "1.0", "1.0", true), CancellationToken.None);

        result.Should().BeOfType<CreatedAtActionResult>();
    }

    [Test]
    public async Task Login_ShouldReturnOk()
    {
        var auth = new Mock<IAuthService>();
        var refresh = new Mock<IRefreshTokenService>();
        auth.Setup(x => x.LoginAsync(It.IsAny<LoginRequest>(), It.IsAny<RequestContext>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Response());
        var controller = new AuthController(auth.Object, refresh.Object) { ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() } };

        var result = await controller.Login(new LoginRequest("cliente@email.com", "SenhaSegura@123"), CancellationToken.None);

        result.Should().BeOfType<OkObjectResult>();
    }

    [Test]
    public async Task Logout_ShouldReturnNoContent()
    {
        var auth = new Mock<IAuthService>();
        var refresh = new Mock<IRefreshTokenService>();
        var controller = new AuthController(auth.Object, refresh.Object) { ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() } };

        var result = await controller.Logout(new LogoutRequest("refresh"), CancellationToken.None);

        result.Should().BeOfType<NoContentResult>();
    }

    private static AuthTokenResponse Response() =>
        new("access", DateTimeOffset.UtcNow.AddMinutes(15), "refresh", DateTimeOffset.UtcNow.AddDays(30),
            new AuthenticatedUserResponse(Guid.NewGuid(), "cliente@email.com", "Cliente", ["CONSUMER"], ["orders.create"], null));
}

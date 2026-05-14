using System.Security.Claims;
using Kuva.Auth.Business.Interfaces;
using Kuva.Auth.Entities.Dtos.Responses;
using Kuva.Auth.Service.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Kuva.Auth.Tests.Service;

public sealed class MeControllerTests
{
    [Test]
    public async Task Get_ShouldReturnCurrentUser()
    {
        var userId = Guid.NewGuid();
        var auth = new Mock<IAuthService>();
        auth.Setup(x => x.GetCurrentUserAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new CurrentUserResponse(userId, "me@test.com", "Me", ["CONSUMER"], ["profile.read_own"], null));
        var context = new DefaultHttpContext
        {
            User = new ClaimsPrincipal(new ClaimsIdentity([new Claim("sub", userId.ToString())]))
        };
        var controller = new MeController(auth.Object) { ControllerContext = new ControllerContext { HttpContext = context } };

        var result = await controller.Get(CancellationToken.None);

        Assert.That(result, Is.TypeOf<OkObjectResult>());
    }
}

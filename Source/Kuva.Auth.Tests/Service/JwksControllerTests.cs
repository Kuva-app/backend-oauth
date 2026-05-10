using FluentAssertions;
using Kuva.Auth.Business.Interfaces;
using Kuva.Auth.Entities.Dtos.Responses;
using Kuva.Auth.Service.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Kuva.Auth.Tests.Service;

public sealed class JwksControllerTests
{
    [Test]
    public void Get_ShouldReturnKeys()
    {
        var provider = new Mock<IJwksProvider>();
        provider.Setup(x => x.GetJwks()).Returns(new JwksResponse([new JwksKeyResponse("RSA", "sig", "kid", "RS256", "n", "e")]));
        var controller = new JwksController(provider.Object);

        var result = controller.Get();

        result.Should().BeOfType<OkObjectResult>();
    }
}

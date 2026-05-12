using System.IdentityModel.Tokens.Jwt;
using FluentAssertions;
using Kuva.Auth.Business.Interfaces;
using Kuva.Auth.Business.Models;
using Kuva.Auth.Entities.Constants;
using Microsoft.Extensions.DependencyInjection;

namespace Kuva.Auth.Tests.Business;

public sealed class JwtTokenProviderTests : TestBase
{
    [Test]
    public void CreateAccessToken_ShouldIncludeExpectedClaimsAndKid()
    {
        using var provider = CreateServices();
        var jwtService = provider.GetRequiredService<IJwtTokenService>();
        var user = new AuthenticatedUserModel(
            Guid.NewGuid(),
            "operador@loja.com",
            "Operador",
            [RoleNames.StoreOperator],
            [PermissionNames.MerchantOrdersRead, PermissionNames.CatalogView, PermissionNames.PriceEdit],
            Guid.NewGuid());

        var token = jwtService.CreateAccessToken(user);
        var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token.AccessToken);

        jwt.Claims.Should().Contain(c => c.Type == "sub" && c.Value == user.Id.ToString());
        jwt.Claims.Should().Contain(c => c.Type == AuthClaimTypes.Roles && c.Value == RoleNames.StoreOperator);
        jwt.Claims.Should().Contain(c => c.Type == AuthClaimTypes.Permissions && c.Value == PermissionNames.MerchantOrdersRead);
        jwt.Claims.Should().Contain(c => c.Type == AuthClaimTypes.Permissions && c.Value == PermissionNames.CatalogView);
        jwt.Claims.Should().Contain(c => c.Type == AuthClaimTypes.Permissions && c.Value == PermissionNames.PriceEdit);
        jwt.Claims.Should().Contain(c => c.Type == AuthClaimTypes.StoreId && c.Value == user.StoreId.ToString());
        jwt.Header.Kid.Should().Be("test-key");
        token.ExpiresAt.Should().BeAfter(DateTimeOffset.UtcNow);
    }
}

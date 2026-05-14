using System.IdentityModel.Tokens.Jwt;
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

        Assert.That(jwt.Claims, Has.Some.Matches<System.Security.Claims.Claim>(c => c.Type == "sub" && c.Value == user.Id.ToString()));
        Assert.That(jwt.Claims, Has.Some.Matches<System.Security.Claims.Claim>(c => c.Type == AuthClaimTypes.Roles && c.Value == RoleNames.StoreOperator));
        Assert.That(jwt.Claims, Has.Some.Matches<System.Security.Claims.Claim>(c => c.Type == AuthClaimTypes.Permissions && c.Value == PermissionNames.MerchantOrdersRead));
        Assert.That(jwt.Claims, Has.Some.Matches<System.Security.Claims.Claim>(c => c.Type == AuthClaimTypes.Permissions && c.Value == PermissionNames.CatalogView));
        Assert.That(jwt.Claims, Has.Some.Matches<System.Security.Claims.Claim>(c => c.Type == AuthClaimTypes.Permissions && c.Value == PermissionNames.PriceEdit));
        Assert.That(jwt.Claims, Has.Some.Matches<System.Security.Claims.Claim>(c => c.Type == AuthClaimTypes.StoreId && c.Value == user.StoreId.ToString()));
        Assert.That(jwt.Header.Kid, Is.EqualTo("test-key"));
        Assert.That(token.ExpiresAt, Is.GreaterThan(DateTimeOffset.UtcNow));
    }
}

using Kuva.Auth.Business.Interfaces;
using Kuva.Auth.Business.Models;
using Kuva.Auth.Entities.Dtos.Requests;
using Kuva.Auth.Repository.Context;
using Microsoft.Extensions.DependencyInjection;

namespace Kuva.Auth.Tests.Business;

public sealed class RefreshTokenServiceTests : TestBase
{
    [Test]
    public async Task RefreshAsync_ShouldRotateValidToken()
    {
        using var provider = CreateServices();
        var auth = provider.GetRequiredService<IAuthService>();
        var refresh = provider.GetRequiredService<IRefreshTokenService>();
        var session = await auth.RegisterConsumerAsync(Request(), RequestContext.Empty, CancellationToken.None);

        var rotated = await refresh.RefreshAsync(new RefreshTokenRequest(session.RefreshToken), RequestContext.Empty, CancellationToken.None);

        Assert.That(rotated.RefreshToken, Is.Not.EqualTo(session.RefreshToken));
        var db = provider.GetRequiredService<AuthDbContext>();
        Assert.That(db.RefreshTokens.Count(), Is.EqualTo(2));
        Assert.That(db.RefreshTokens.Single(x => x.ReplacedByTokenId != null).RevokedAt, Is.Not.Null);
    }

    [Test]
    public async Task RefreshAsync_ShouldRejectRevokedToken()
    {
        using var provider = CreateServices();
        var auth = provider.GetRequiredService<IAuthService>();
        var refresh = provider.GetRequiredService<IRefreshTokenService>();
        var session = await auth.RegisterConsumerAsync(Request(), RequestContext.Empty, CancellationToken.None);
        await refresh.RevokeAsync(session.RefreshToken, RequestContext.Empty, CancellationToken.None);

        var ex = Assert.ThrowsAsync<AuthException>(async () => await refresh.RefreshAsync(new RefreshTokenRequest(session.RefreshToken), RequestContext.Empty, CancellationToken.None));
        Assert.That(ex!.StatusCode, Is.EqualTo(401));
    }

    [Test]
    public async Task RevokeAllActiveAsync_ShouldRevokeUserTokens()
    {
        using var provider = CreateServices();
        var auth = provider.GetRequiredService<IAuthService>();
        var refresh = provider.GetRequiredService<IRefreshTokenService>();
        var session = await auth.RegisterConsumerAsync(Request(), RequestContext.Empty, CancellationToken.None);

        await refresh.RevokeAllActiveAsync(session.User.Id, RequestContext.Empty, CancellationToken.None);

        Assert.That(provider.GetRequiredService<AuthDbContext>().RefreshTokens, Is.All.Matches<Entities.Entities.RefreshToken>(x => x.RevokedAt != null));
    }

    private static Kuva.Auth.Entities.Dtos.Requests.RegisterConsumerRequest Request() =>
        new("Cliente Kuva", "cliente@email.com", null, "SenhaSegura@123", "1.0", "1.0", false);
}

using Kuva.Auth.Business.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Kuva.Auth.Tests.Business;

public sealed class TokenHashProviderTests : TestBase
{
    [Test]
    public void GenerateAndHash_ShouldNeverExposePlainToken()
    {
        using var provider = CreateServices();
        var tokenProvider = provider.GetRequiredService<ITokenHashProvider>();
        var token = tokenProvider.GenerateToken(32);
        var hash = tokenProvider.HashToken(token);
        Assert.That(string.IsNullOrWhiteSpace(token), Is.False);
        Assert.That(hash, Is.Not.EqualTo(token));
        Assert.That(tokenProvider.HashToken(token), Is.EqualTo(hash));
    }
}

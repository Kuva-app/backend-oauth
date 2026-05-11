using FluentAssertions;
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
        token.Should().NotBeNullOrWhiteSpace();
        hash.Should().NotBe(token);
        tokenProvider.HashToken(token).Should().Be(hash);
    }
}

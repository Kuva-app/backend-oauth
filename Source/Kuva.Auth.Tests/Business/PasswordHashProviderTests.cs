using FluentAssertions;
using Kuva.Auth.Business.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Kuva.Auth.Tests.Business;

public sealed class PasswordHashProviderTests : TestBase
{
    [Test]
    public void HashPassword_ShouldHashAndValidatePassword()
    {
        using var provider = CreateServices();
        var hashProvider = provider.GetRequiredService<IPasswordHashProvider>();
        var hash = hashProvider.HashPassword("SenhaSegura@123");
        hash.Should().NotBe("SenhaSegura@123");
        hashProvider.VerifyPassword(hash, "SenhaSegura@123").Should().BeTrue();
        hashProvider.VerifyPassword(hash, "SenhaErrada@123").Should().BeFalse();
        hashProvider.NeedsRehash(hash).Should().BeFalse();
    }
}

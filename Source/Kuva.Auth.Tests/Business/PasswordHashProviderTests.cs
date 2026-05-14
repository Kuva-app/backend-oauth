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
        Assert.That(hash, Is.Not.EqualTo("SenhaSegura@123"));
        Assert.That(hashProvider.VerifyPassword(hash, "SenhaSegura@123"), Is.True);
        Assert.That(hashProvider.VerifyPassword(hash, "SenhaErrada@123"), Is.False);
        Assert.That(hashProvider.NeedsRehash(hash), Is.False);
    }
}

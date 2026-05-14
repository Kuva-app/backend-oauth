using Kuva.Auth.Business.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Kuva.Auth.Tests.Business;

public sealed class PasswordPolicyServiceTests : TestBase
{
    [Test]
    public void Validate_ShouldAcceptStrongPassword()
    {
        using var provider = CreateServices();
        var result = provider.GetRequiredService<IPasswordPolicyService>().Validate("SenhaSegura@123");
        Assert.That(result.IsValid, Is.True);
    }

    [TestCase("Curta@1", "pelo menos")]
    [TestCase("senhasegura@123", "maiúscula")]
    [TestCase("SENHASEGURA@123", "minúscula")]
    [TestCase("SenhaSegura@", "número")]
    [TestCase("SenhaSegura123", "especial")]
    public void Validate_ShouldReturnViolations(string password, string expectedViolation)
    {
        using var provider = CreateServices();
        var result = provider.GetRequiredService<IPasswordPolicyService>().Validate(password);
        Assert.That(result.IsValid, Is.False);
        Assert.That(result.Violations, Has.Some.Matches<string>(v => v.Contains(expectedViolation, StringComparison.OrdinalIgnoreCase)));
    }
}

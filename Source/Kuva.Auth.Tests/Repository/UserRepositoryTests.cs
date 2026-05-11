using FluentAssertions;
using Kuva.Auth.Entities.Entities;
using Kuva.Auth.Entities.ValueObjects;
using Kuva.Auth.Repository.Context;
using Kuva.Auth.Repository.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Kuva.Auth.Tests.Repository;

public sealed class UserRepositoryTests : TestBase
{
    [Test]
    public async Task GetByNormalizedEmailAsync_ShouldFindUser()
    {
        using var provider = CreateServices();
        var db = provider.GetRequiredService<AuthDbContext>();
        var email = EmailAddress.Create("repo@test.com");
        db.Users.Add(new User { Email = email.Value, NormalizedEmail = email.NormalizedValue, Name = "Repo" });
        await db.SaveChangesAsync();

        var user = await provider.GetRequiredService<IUserRepository>().GetByNormalizedEmailAsync(email.NormalizedValue, CancellationToken.None);

        user.Should().NotBeNull();
        user!.Email.Should().Be("repo@test.com");
    }
}

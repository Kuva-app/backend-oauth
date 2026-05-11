using FluentAssertions;
using Kuva.Auth.Entities.Entities;
using Kuva.Auth.Repository.Context;
using Kuva.Auth.Repository.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Kuva.Auth.Tests.Repository;

public sealed class RefreshTokenRepositoryTests : TestBase
{
    [Test]
    public async Task GetActiveByUserIdAsync_ShouldReturnOnlyActiveTokens()
    {
        using var provider = CreateServices();
        var db = provider.GetRequiredService<AuthDbContext>();
        var user = new User { Email = "token@test.com", NormalizedEmail = "TOKEN@TEST.COM" };
        db.Users.Add(user);
        db.RefreshTokens.Add(new RefreshToken { UserId = user.Id, TokenHash = "active", ExpiresAt = DateTimeOffset.UtcNow.AddDays(1) });
        db.RefreshTokens.Add(new RefreshToken { UserId = user.Id, TokenHash = "revoked", ExpiresAt = DateTimeOffset.UtcNow.AddDays(1), RevokedAt = DateTimeOffset.UtcNow });
        await db.SaveChangesAsync();

        var tokens = await provider.GetRequiredService<IRefreshTokenRepository>().GetActiveByUserIdAsync(user.Id, DateTimeOffset.UtcNow, CancellationToken.None);

        tokens.Should().ContainSingle(x => x.TokenHash == "active");
    }
}

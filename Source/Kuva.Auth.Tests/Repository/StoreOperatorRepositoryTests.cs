using Kuva.Auth.Entities.Entities;
using Kuva.Auth.Entities.Enums;
using Kuva.Auth.Repository.Context;
using Kuva.Auth.Repository.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Kuva.Auth.Tests.Repository;

public sealed class StoreOperatorRepositoryTests : TestBase
{
    [Test]
    public async Task GetActiveByUserIdAsync_ShouldIgnoreBlockedOperator()
    {
        using var provider = CreateServices();
        var db = provider.GetRequiredService<AuthDbContext>();
        var user = new User { Email = "operator@test.com", NormalizedEmail = "OPERATOR@TEST.COM" };
        db.Users.Add(user);
        db.StoreOperators.Add(new StoreOperator { UserId = user.Id, StoreId = Guid.NewGuid(), Role = "STORE_OPERATOR", Status = StoreOperatorStatus.Blocked });
        await db.SaveChangesAsync();

        var result = await provider.GetRequiredService<IStoreOperatorRepository>().GetActiveByUserIdAsync(user.Id, CancellationToken.None);

        Assert.That(result, Is.Null);
    }
}

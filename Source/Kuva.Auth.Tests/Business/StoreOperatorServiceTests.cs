using Kuva.Auth.Business.Interfaces;
using Kuva.Auth.Business.Models;
using Kuva.Auth.Entities.Constants;
using Kuva.Auth.Entities.Dtos.Internal;
using Kuva.Auth.Entities.Dtos.Requests;
using Kuva.Auth.Entities.Enums;
using Microsoft.Extensions.DependencyInjection;

namespace Kuva.Auth.Tests.Business;

public sealed class StoreOperatorServiceTests : TestBase
{
    [Test]
    public async Task CreateAsync_ShouldCreateOperatorAndAllowMerchantLoginWithStoreId()
    {
        using var provider = CreateServices();
        var service = provider.GetRequiredService<IStoreOperatorService>();
        var storeId = Guid.NewGuid();

        var created = await service.CreateAsync(storeId, Request(), RequestContext.Empty, CancellationToken.None);
        var login = await provider.GetRequiredService<IAuthService>().LoginAsync(new LoginRequest("operador@loja.com", "SenhaSegura@123"), RequestContext.Empty, CancellationToken.None);

        Assert.That(created.StoreId, Is.EqualTo(storeId));
        Assert.That(login.User.Roles, Does.Contain(RoleNames.StoreOperator));
        Assert.That(login.User.StoreId, Is.EqualTo(storeId));
        Assert.That(login.User.Permissions, Does.Contain(PermissionNames.MerchantOrdersRead));
        Assert.That(login.User.Permissions, Does.Contain(PermissionNames.CatalogView));
        Assert.That(login.User.Permissions, Does.Contain(PermissionNames.PriceEdit));
        Assert.That(login.User.Permissions, Does.Not.Contain(PermissionNames.CatalogEdit));
        Assert.That(login.User.Permissions, Does.Not.Contain(PermissionNames.SkuEnableDisable));
    }

    [Test]
    public async Task CreateAsync_ShouldRejectDuplicateOperatorInStore()
    {
        using var provider = CreateServices();
        var service = provider.GetRequiredService<IStoreOperatorService>();
        var storeId = Guid.NewGuid();
        await service.CreateAsync(storeId, Request(), RequestContext.Empty, CancellationToken.None);

        var ex = Assert.ThrowsAsync<AuthException>(async () => await service.CreateAsync(storeId, Request(), RequestContext.Empty, CancellationToken.None));
        Assert.That(ex!.StatusCode, Is.EqualTo(409));
    }

    [Test]
    public async Task UpdateStatusAsync_ShouldBlockOperatorLogin()
    {
        using var provider = CreateServices();
        var service = provider.GetRequiredService<IStoreOperatorService>();
        var created = await service.CreateAsync(Guid.NewGuid(), Request(), RequestContext.Empty, CancellationToken.None);

        await service.UpdateStatusAsync(created.Id, StoreOperatorStatus.Blocked, RequestContext.Empty, CancellationToken.None);

        var ex = Assert.ThrowsAsync<AuthException>(async () => await provider.GetRequiredService<IAuthService>().LoginAsync(new LoginRequest("operador@loja.com", "SenhaSegura@123"), RequestContext.Empty, CancellationToken.None));
        Assert.That(ex!.StatusCode, Is.EqualTo(403));
    }

    [Test]
    public async Task ListByStoreAsync_ShouldReturnOperators()
    {
        using var provider = CreateServices();
        var service = provider.GetRequiredService<IStoreOperatorService>();
        var storeId = Guid.NewGuid();
        await service.CreateAsync(storeId, Request(), RequestContext.Empty, CancellationToken.None);

        var operators = await service.ListByStoreAsync(storeId, CancellationToken.None);

        Assert.That(operators, Has.Exactly(1).Matches<StoreOperatorResponse>(x => x.Email == "operador@loja.com"));
    }

    private static CreateStoreOperatorRequest Request() =>
        new("Operador Loja", "operador@loja.com", "+5511999999999", "SenhaSegura@123", RoleNames.StoreOperator);
}

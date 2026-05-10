using FluentAssertions;
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

        created.StoreId.Should().Be(storeId);
        login.User.Roles.Should().Contain(RoleNames.StoreOperator);
        login.User.StoreId.Should().Be(storeId);
        login.User.Permissions.Should().Contain(PermissionNames.MerchantOrdersRead);
    }

    [Test]
    public async Task CreateAsync_ShouldRejectDuplicateOperatorInStore()
    {
        using var provider = CreateServices();
        var service = provider.GetRequiredService<IStoreOperatorService>();
        var storeId = Guid.NewGuid();
        await service.CreateAsync(storeId, Request(), RequestContext.Empty, CancellationToken.None);

        var act = () => service.CreateAsync(storeId, Request(), RequestContext.Empty, CancellationToken.None);
        await act.Should().ThrowAsync<AuthException>().Where(x => x.StatusCode == 409);
    }

    [Test]
    public async Task UpdateStatusAsync_ShouldBlockOperatorLogin()
    {
        using var provider = CreateServices();
        var service = provider.GetRequiredService<IStoreOperatorService>();
        var created = await service.CreateAsync(Guid.NewGuid(), Request(), RequestContext.Empty, CancellationToken.None);

        await service.UpdateStatusAsync(created.Id, StoreOperatorStatus.Blocked, RequestContext.Empty, CancellationToken.None);

        var act = () => provider.GetRequiredService<IAuthService>().LoginAsync(new LoginRequest("operador@loja.com", "SenhaSegura@123"), RequestContext.Empty, CancellationToken.None);
        await act.Should().ThrowAsync<AuthException>().Where(x => x.StatusCode == 403);
    }

    [Test]
    public async Task ListByStoreAsync_ShouldReturnOperators()
    {
        using var provider = CreateServices();
        var service = provider.GetRequiredService<IStoreOperatorService>();
        var storeId = Guid.NewGuid();
        await service.CreateAsync(storeId, Request(), RequestContext.Empty, CancellationToken.None);

        var operators = await service.ListByStoreAsync(storeId, CancellationToken.None);

        operators.Should().ContainSingle(x => x.Email == "operador@loja.com");
    }

    private static CreateStoreOperatorRequest Request() =>
        new("Operador Loja", "operador@loja.com", "+5511999999999", "SenhaSegura@123", RoleNames.StoreOperator);
}

using FluentAssertions;
using Kuva.Auth.Business.Interfaces;
using Kuva.Auth.Business.Models;
using Kuva.Auth.Entities.Constants;
using Kuva.Auth.Entities.Dtos.Requests;
using Kuva.Auth.Entities.Enums;
using Kuva.Auth.Repository.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Kuva.Auth.Tests.Business;

public sealed class AuthServiceTests : TestBase
{
    [Test]
    public async Task RegisterConsumerAsync_ShouldCreateConsumerWithSessionAndConsents()
    {
        using var provider = CreateServices();
        var service = provider.GetRequiredService<IAuthService>();

        var response = await service.RegisterConsumerAsync(ConsumerRequest(), RequestContext.Empty, CancellationToken.None);

        response.AccessToken.Should().NotBeNullOrWhiteSpace();
        response.RefreshToken.Should().NotBeNullOrWhiteSpace();
        response.User.Email.Should().Be("cliente@email.com");
        response.User.Roles.Should().Contain(RoleNames.Consumer);
        response.User.Permissions.Should().Contain(PermissionNames.OrdersCreate);

        var db = provider.GetRequiredService<AuthDbContext>();
        var user = await db.Users.Include(x => x.Credential).FirstAsync();
        user.NormalizedEmail.Should().Be("CLIENTE@EMAIL.COM");
        user.Credential!.PasswordHash.Should().NotBe("SenhaSegura@123");
        db.TermsAcceptances.Should().HaveCount(1);
        db.UserConsents.Should().HaveCount(1);
        db.AuthAuditLogs.Should().Contain(x => x.EventType == AuthAuditEventType.UserRegistered && x.Result == AuthAuditResult.Succeeded);
    }

    [Test]
    public async Task RegisterConsumerAsync_ShouldRejectDuplicateEmail()
    {
        using var provider = CreateServices();
        var service = provider.GetRequiredService<IAuthService>();
        await service.RegisterConsumerAsync(ConsumerRequest(), RequestContext.Empty, CancellationToken.None);

        var act = () => service.RegisterConsumerAsync(ConsumerRequest(), RequestContext.Empty, CancellationToken.None);
        await act.Should().ThrowAsync<AuthException>().Where(x => x.StatusCode == 409);
    }

    [Test]
    public async Task RegisterConsumerAsync_ShouldRejectWeakPassword()
    {
        using var provider = CreateServices();
        var service = provider.GetRequiredService<IAuthService>();

        var act = () => service.RegisterConsumerAsync(ConsumerRequest() with { Password = "weak" }, RequestContext.Empty, CancellationToken.None);
        await act.Should().ThrowAsync<AuthException>().Where(x => x.Code == "weak_password");
    }

    [Test]
    public async Task LoginAsync_ShouldAuthenticateActiveUserAndUpdateLastLogin()
    {
        using var provider = CreateServices();
        var service = provider.GetRequiredService<IAuthService>();
        await service.RegisterConsumerAsync(ConsumerRequest(), RequestContext.Empty, CancellationToken.None);

        var response = await service.LoginAsync(new LoginRequest("cliente@email.com", "SenhaSegura@123"), RequestContext.Empty, CancellationToken.None);

        response.AccessToken.Should().NotBeNullOrWhiteSpace();
        provider.GetRequiredService<AuthDbContext>().Users.Single().LastLoginAt.Should().NotBeNull();
    }

    [Test]
    public async Task LoginAsync_ShouldRejectInvalidPasswordAndInactiveUser()
    {
        using var provider = CreateServices();
        var service = provider.GetRequiredService<IAuthService>();
        await service.RegisterConsumerAsync(ConsumerRequest(), RequestContext.Empty, CancellationToken.None);

        var badPassword = () => service.LoginAsync(new LoginRequest("cliente@email.com", "SenhaErrada@123"), RequestContext.Empty, CancellationToken.None);
        await badPassword.Should().ThrowAsync<AuthException>().Where(x => x.StatusCode == 401);

        var db = provider.GetRequiredService<AuthDbContext>();
        db.Users.Single().Status = UserStatus.Blocked;
        await db.SaveChangesAsync();

        var blocked = () => service.LoginAsync(new LoginRequest("cliente@email.com", "SenhaSegura@123"), RequestContext.Empty, CancellationToken.None);
        await blocked.Should().ThrowAsync<AuthException>().Where(x => x.StatusCode == 401);
    }

    private static RegisterConsumerRequest ConsumerRequest() =>
        new("Cliente Kuva", "cliente@email.com", "+5511999999999", "SenhaSegura@123", "1.0", "1.0", true);
}

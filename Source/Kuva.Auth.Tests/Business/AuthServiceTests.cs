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

        Assert.That(string.IsNullOrWhiteSpace(response.AccessToken), Is.False);
        Assert.That(string.IsNullOrWhiteSpace(response.RefreshToken), Is.False);
        Assert.That(response.User.Email, Is.EqualTo("cliente@email.com"));
        Assert.That(response.User.Roles, Does.Contain(RoleNames.Consumer));
        Assert.That(response.User.Permissions, Does.Contain(PermissionNames.OrdersCreate));

        var db = provider.GetRequiredService<AuthDbContext>();
        var user = await db.Users.Include(x => x.Credential).FirstAsync();
        Assert.That(user.NormalizedEmail, Is.EqualTo("CLIENTE@EMAIL.COM"));
        Assert.That(user.Credential!.PasswordHash, Is.Not.EqualTo("SenhaSegura@123"));
        Assert.That(db.TermsAcceptances.Count(), Is.EqualTo(1));
        Assert.That(db.UserConsents.Count(), Is.EqualTo(1));
        Assert.That(db.AuthAuditLogs, Has.Some.Matches<Entities.Entities.AuthAuditLog>(x => x.EventType == AuthAuditEventType.UserRegistered && x.Result == AuthAuditResult.Succeeded));
    }

    [Test]
    public async Task RegisterConsumerAsync_ShouldRejectDuplicateEmail()
    {
        using var provider = CreateServices();
        var service = provider.GetRequiredService<IAuthService>();
        await service.RegisterConsumerAsync(ConsumerRequest(), RequestContext.Empty, CancellationToken.None);

        var ex = Assert.ThrowsAsync<AuthException>(async () => await service.RegisterConsumerAsync(ConsumerRequest(), RequestContext.Empty, CancellationToken.None));
        Assert.That(ex!.StatusCode, Is.EqualTo(409));
    }

    [Test]
    public async Task RegisterConsumerAsync_ShouldRejectWeakPassword()
    {
        using var provider = CreateServices();
        var service = provider.GetRequiredService<IAuthService>();

        var ex = Assert.ThrowsAsync<AuthException>(async () => await service.RegisterConsumerAsync(ConsumerRequest() with { Password = "weak" }, RequestContext.Empty, CancellationToken.None));
        Assert.That(ex!.Code, Is.EqualTo("weak_password"));
    }

    [Test]
    public async Task LoginAsync_ShouldAuthenticateActiveUserAndUpdateLastLogin()
    {
        using var provider = CreateServices();
        var service = provider.GetRequiredService<IAuthService>();
        await service.RegisterConsumerAsync(ConsumerRequest(), RequestContext.Empty, CancellationToken.None);

        var response = await service.LoginAsync(new LoginRequest("cliente@email.com", "SenhaSegura@123"), RequestContext.Empty, CancellationToken.None);

        Assert.That(string.IsNullOrWhiteSpace(response.AccessToken), Is.False);
        Assert.That(provider.GetRequiredService<AuthDbContext>().Users.Single().LastLoginAt, Is.Not.Null);
    }

    [Test]
    public async Task LoginAsync_ShouldRejectInvalidPasswordAndInactiveUser()
    {
        using var provider = CreateServices();
        var service = provider.GetRequiredService<IAuthService>();
        await service.RegisterConsumerAsync(ConsumerRequest(), RequestContext.Empty, CancellationToken.None);

        var badPasswordException = Assert.ThrowsAsync<AuthException>(async () => await service.LoginAsync(new LoginRequest("cliente@email.com", "SenhaErrada@123"), RequestContext.Empty, CancellationToken.None));
        Assert.That(badPasswordException!.StatusCode, Is.EqualTo(401));

        var db = provider.GetRequiredService<AuthDbContext>();
        db.Users.Single().Status = UserStatus.Blocked;
        await db.SaveChangesAsync();

        var blockedException = Assert.ThrowsAsync<AuthException>(async () => await service.LoginAsync(new LoginRequest("cliente@email.com", "SenhaSegura@123"), RequestContext.Empty, CancellationToken.None));
        Assert.That(blockedException!.StatusCode, Is.EqualTo(401));
    }

    private static RegisterConsumerRequest ConsumerRequest() =>
        new("Cliente Kuva", "cliente@email.com", "+5511999999999", "SenhaSegura@123", "1.0", "1.0", true);
}

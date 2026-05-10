using Kuva.Auth.Business.Extensions;
using Kuva.Auth.Repository.Context;
using Kuva.Auth.Repository.Interfaces;
using Kuva.Auth.Repository.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Kuva.Auth.Tests;

public abstract class TestBase
{
    protected static ServiceProvider CreateServices()
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Jwt:Issuer"] = "https://tests.kuva/auth",
                ["Jwt:Audience"] = "kuva-api",
                ["Jwt:AccessTokenMinutes"] = "15",
                ["Jwt:KeyId"] = "test-key",
                ["RefreshToken:ConsumerExpirationDays"] = "30",
                ["RefreshToken:MerchantExpirationHours"] = "12",
                ["RefreshToken:TokenBytes"] = "32",
                ["PasswordPolicy:MinimumLength"] = "10",
                ["PasswordPolicy:RequireUppercase"] = "true",
                ["PasswordPolicy:RequireLowercase"] = "true",
                ["PasswordPolicy:RequireDigit"] = "true",
                ["PasswordPolicy:RequireSpecialCharacter"] = "true"
            })
            .Build();

        var services = new ServiceCollection();
        services.AddDbContext<AuthDbContext>(options => options.UseInMemoryDatabase(Guid.NewGuid().ToString()));
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        services.AddScoped<IStoreOperatorRepository, StoreOperatorRepository>();
        services.AddScoped<IAuthAuditRepository, AuthAuditRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddAuthBusiness(configuration);

        var provider = services.BuildServiceProvider();
        provider.GetRequiredService<AuthDbContext>().Database.EnsureCreated();
        return provider;
    }
}

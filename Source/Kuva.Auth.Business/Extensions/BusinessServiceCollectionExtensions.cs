using Kuva.Auth.Business.Interfaces;
using Kuva.Auth.Business.Models;
using Kuva.Auth.Business.Providers;
using Kuva.Auth.Business.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Kuva.Auth.Business.Extensions;

public static class BusinessServiceCollectionExtensions
{
    public static IServiceCollection AddAuthBusiness(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtOptions>(configuration.GetSection("Jwt"));
        services.Configure<RefreshTokenOptions>(configuration.GetSection("RefreshToken"));
        services.Configure<PasswordPolicyOptions>(configuration.GetSection("PasswordPolicy"));

        services.AddSingleton<IClock, SystemClock>();
        services.AddSingleton<IJwtKeyProvider, InMemoryJwtKeyProvider>();
        services.AddSingleton<IJwtTokenService, JwtTokenProvider>();
        services.AddSingleton<IJwksProvider, JwksProvider>();
        services.AddSingleton<IPasswordHashProvider, PasswordHashProvider>();
        services.AddSingleton<ITokenHashProvider, TokenHashProvider>();
        services.AddSingleton<IPasswordPolicyService, PasswordPolicyService>();
        services.AddScoped<IAuthAuditService, AuthAuditService>();
        services.AddScoped<IRefreshTokenService, RefreshTokenService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IStoreOperatorService, StoreOperatorService>();
        return services;
    }
}

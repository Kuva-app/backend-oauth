using Kuva.Auth.Repository.Context;
using Kuva.Auth.Repository.Interfaces;
using Kuva.Auth.Repository.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Kuva.Auth.Repository.Extensions;

public static class RepositoryServiceCollectionExtensions
{
    public static IServiceCollection AddAuthRepository(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AuthDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("AuthDatabase")));

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        services.AddScoped<IStoreOperatorRepository, StoreOperatorRepository>();
        services.AddScoped<IAuthAuditRepository, AuthAuditRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        return services;
    }
}

using Kuva.Auth.Service.Filters;
using Kuva.Auth.Service.Options;

namespace Kuva.Auth.Service.Extensions;

public static class ApiServiceCollectionExtensions
{
    public static IServiceCollection AddAuthApi(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<CorsOptions>(configuration.GetSection("Cors"));
        var allowedOrigins = configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? [];
        services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
            {
                if (allowedOrigins.Length == 0)
                {
                    policy.AllowAnyOrigin();
                }
                else
                {
                    policy.WithOrigins(allowedOrigins);
                }

                policy.AllowAnyHeader().AllowAnyMethod();
            });
        });

        services.AddControllers(options => options.Filters.Add<ValidateModelStateFilter>());
        services.AddProblemDetails();
        return services;
    }
}

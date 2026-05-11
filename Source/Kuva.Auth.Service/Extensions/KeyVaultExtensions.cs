using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Identity;
using Kuva.Auth.Service.Options;

namespace Kuva.Auth.Service.Extensions;

public static class KeyVaultExtensions
{
    public static IConfigurationBuilder AddAuthKeyVault(this IConfigurationBuilder builder, IConfiguration configuration)
    {
        var options = configuration.GetSection("KeyVault").Get<KeyVaultOptions>();
        if (!string.IsNullOrWhiteSpace(options?.Uri))
        {
            builder.AddAzureKeyVault(new Uri(options.Uri), new DefaultAzureCredential(), new AzureKeyVaultConfigurationOptions());
        }

        return builder;
    }
}

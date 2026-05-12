using Kuva.Auth.Entities.Constants;

namespace Kuva.Auth.Service.Extensions;

public static class AuthorizationExtensions
{
    public static IServiceCollection AddAuthAuthorization(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            options.AddPolicy(AuthPolicies.CanReadMerchantOrders, policy => policy.RequireClaim(AuthClaimTypes.Permissions, PermissionNames.MerchantOrdersRead));
            options.AddPolicy(AuthPolicies.CanUpdateMerchantOrderStatus, policy => policy.RequireClaim(AuthClaimTypes.Permissions, PermissionNames.MerchantOrdersUpdateStatus));
            options.AddPolicy(AuthPolicies.CanDownloadMerchantMedia, policy => policy.RequireClaim(AuthClaimTypes.Permissions, PermissionNames.MerchantMediaDownload));
            options.AddPolicy(AuthPolicies.CanManageMerchantPricing, policy => policy.RequireClaim(AuthClaimTypes.Permissions, PermissionNames.MerchantPricingUpdate));
            options.AddPolicy(AuthPolicies.CanReadAdminAudit, policy => policy.RequireClaim(AuthClaimTypes.Permissions, PermissionNames.AdminAuditRead));
            options.AddPolicy(AuthPolicies.CanViewCatalog, policy => policy.RequireClaim(AuthClaimTypes.Permissions, PermissionNames.CatalogView));
            options.AddPolicy(AuthPolicies.CanEditCatalog, policy => policy.RequireClaim(AuthClaimTypes.Permissions, PermissionNames.CatalogEdit));
            options.AddPolicy(AuthPolicies.CanEditPrice, policy => policy.RequireClaim(AuthClaimTypes.Permissions, PermissionNames.PriceEdit));
            options.AddPolicy(AuthPolicies.CanEnableDisableSku, policy => policy.RequireClaim(AuthClaimTypes.Permissions, PermissionNames.SkuEnableDisable));
        });

        return services;
    }
}

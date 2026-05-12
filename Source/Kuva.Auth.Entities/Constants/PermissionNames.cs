namespace Kuva.Auth.Entities.Constants;

public static class PermissionNames
{
    public const string OrdersCreate = "orders.create";
    public const string OrdersReadOwn = "orders.read_own";
    public const string OrdersCancelOwn = "orders.cancel_own";
    public const string ProfileReadOwn = "profile.read_own";
    public const string ProfileUpdateOwn = "profile.update_own";
    public const string MerchantOrdersRead = "merchant.orders.read";
    public const string MerchantOrdersUpdateStatus = "merchant.orders.update_status";
    public const string MerchantMediaDownload = "merchant.media.download";
    public const string MerchantStoreRead = "merchant.store.read";
    public const string MerchantPricingRead = "merchant.pricing.read";
    public const string MerchantPricingUpdate = "merchant.pricing.update";
    public const string MerchantOperatorsRead = "merchant.operators.read";
    public const string CatalogView = "CATALOG_VIEW";
    public const string CatalogEdit = "CATALOG_EDIT";
    public const string PriceEdit = "PRICE_EDIT";
    public const string SkuEnableDisable = "SKU_ENABLE_DISABLE";
    public const string AdminStoresRead = "admin.stores.read";
    public const string AdminStoresCreate = "admin.stores.create";
    public const string AdminStoresUpdate = "admin.stores.update";
    public const string AdminUsersRead = "admin.users.read";
    public const string AdminAuditRead = "admin.audit.read";

    public static readonly string[] Consumer =
    [
        OrdersCreate, OrdersReadOwn, OrdersCancelOwn, ProfileReadOwn, ProfileUpdateOwn
    ];

    public static readonly string[] StoreOperator =
    [
        MerchantOrdersRead, MerchantOrdersUpdateStatus, MerchantMediaDownload, MerchantStoreRead,
        CatalogView, PriceEdit
    ];

    public static readonly string[] StoreOwner =
    [
        MerchantOrdersRead, MerchantOrdersUpdateStatus, MerchantMediaDownload, MerchantStoreRead,
        MerchantPricingRead, MerchantPricingUpdate, MerchantOperatorsRead,
        CatalogView, CatalogEdit, PriceEdit, SkuEnableDisable
    ];

    public static readonly string[] KuvaAdmin =
    [
        AdminStoresRead, AdminStoresCreate, AdminStoresUpdate, AdminUsersRead, AdminAuditRead,
        CatalogView, CatalogEdit, PriceEdit, SkuEnableDisable
    ];
}

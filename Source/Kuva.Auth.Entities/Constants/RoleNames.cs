namespace Kuva.Auth.Entities.Constants;

public static class RoleNames
{
    public const string Consumer = "CONSUMER";
    public const string StoreOwner = "STORE_OWNER";
    public const string StoreOperator = "STORE_OPERATOR";
    public const string KuvaAdmin = "KUVA_ADMIN";

    public static readonly string[] MerchantRoles = [StoreOwner, StoreOperator];
}

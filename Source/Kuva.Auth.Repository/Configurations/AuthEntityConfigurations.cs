using Kuva.Auth.Entities.Constants;
using Kuva.Auth.Entities.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kuva.Auth.Repository.Configurations;

public sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Email).HasColumnName("email").HasMaxLength(255).IsRequired();
        builder.Property(x => x.NormalizedEmail).HasColumnName("normalized_email").HasMaxLength(255).IsRequired();
        builder.Property(x => x.Name).HasColumnName("name").HasMaxLength(160);
        builder.Property(x => x.Phone).HasColumnName("phone").HasMaxLength(32);
        builder.Property(x => x.Status).HasColumnName("status").HasMaxLength(32).HasConversion<string>().IsRequired();
        builder.Property(x => x.CreatedAt).HasColumnName("created_at").IsRequired();
        builder.Property(x => x.UpdatedAt).HasColumnName("updated_at");
        builder.Property(x => x.LastLoginAt).HasColumnName("last_login_at");
        builder.HasIndex(x => x.Email).IsUnique().HasDatabaseName("UX_users_email");
        builder.HasIndex(x => x.NormalizedEmail).IsUnique().HasDatabaseName("UX_users_normalized_email");
        builder.HasIndex(x => x.Status).HasDatabaseName("IX_users_status");
    }
}

public sealed class UserCredentialConfiguration : IEntityTypeConfiguration<UserCredential>
{
    public void Configure(EntityTypeBuilder<UserCredential> builder)
    {
        builder.ToTable("user_credentials");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.UserId).HasColumnName("user_id").IsRequired();
        builder.Property(x => x.PasswordHash).HasColumnName("password_hash").HasMaxLength(1024).IsRequired();
        builder.Property(x => x.PasswordAlgorithm).HasColumnName("password_algorithm").HasMaxLength(64).IsRequired();
        builder.Property(x => x.PasswordUpdatedAt).HasColumnName("password_updated_at").IsRequired();
        builder.Property(x => x.CreatedAt).HasColumnName("created_at").IsRequired();
        builder.HasOne(x => x.User).WithOne(x => x.Credential).HasForeignKey<UserCredential>(x => x.UserId);
        builder.HasIndex(x => x.UserId).IsUnique().HasDatabaseName("UX_user_credentials_user_id");
    }
}

public sealed class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("roles");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).HasColumnName("name").HasMaxLength(64).IsRequired();
        builder.Property(x => x.NormalizedName).HasColumnName("normalized_name").HasMaxLength(64).IsRequired();
        builder.Property(x => x.Description).HasColumnName("description").HasMaxLength(255);
        builder.Property(x => x.Active).HasColumnName("active").IsRequired();
        builder.Property(x => x.CreatedAt).HasColumnName("created_at").IsRequired();
        builder.HasIndex(x => x.Name).IsUnique().HasDatabaseName("UX_roles_name");
        builder.HasIndex(x => x.NormalizedName).IsUnique().HasDatabaseName("UX_roles_normalized_name");
        builder.HasData(AuthSeedData.Roles);
    }
}

public sealed class PermissionConfiguration : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.ToTable("permissions");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).HasColumnName("name").HasMaxLength(128).IsRequired();
        builder.Property(x => x.NormalizedName).HasColumnName("normalized_name").HasMaxLength(128).IsRequired();
        builder.Property(x => x.Description).HasColumnName("description").HasMaxLength(255);
        builder.Property(x => x.Active).HasColumnName("active").IsRequired();
        builder.Property(x => x.CreatedAt).HasColumnName("created_at").IsRequired();
        builder.HasIndex(x => x.Name).IsUnique().HasDatabaseName("UX_permissions_name");
        builder.HasIndex(x => x.NormalizedName).IsUnique().HasDatabaseName("UX_permissions_normalized_name");
        builder.HasData(AuthSeedData.Permissions);
    }
}

public sealed class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
{
    public void Configure(EntityTypeBuilder<UserRole> builder)
    {
        builder.ToTable("user_roles");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.UserId).HasColumnName("user_id").IsRequired();
        builder.Property(x => x.RoleId).HasColumnName("role_id").IsRequired();
        builder.Property(x => x.CreatedAt).HasColumnName("created_at").IsRequired();
        builder.HasOne(x => x.User).WithMany(x => x.UserRoles).HasForeignKey(x => x.UserId);
        builder.HasOne(x => x.Role).WithMany(x => x.UserRoles).HasForeignKey(x => x.RoleId);
        builder.HasIndex(x => new { x.UserId, x.RoleId }).IsUnique().HasDatabaseName("UX_user_roles_user_id_role_id");
    }
}

public sealed class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermission>
{
    public void Configure(EntityTypeBuilder<RolePermission> builder)
    {
        builder.ToTable("role_permissions");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.RoleId).HasColumnName("role_id").IsRequired();
        builder.Property(x => x.PermissionId).HasColumnName("permission_id").IsRequired();
        builder.Property(x => x.CreatedAt).HasColumnName("created_at").IsRequired();
        builder.HasOne(x => x.Role).WithMany(x => x.RolePermissions).HasForeignKey(x => x.RoleId);
        builder.HasOne(x => x.Permission).WithMany(x => x.RolePermissions).HasForeignKey(x => x.PermissionId);
        builder.HasIndex(x => new { x.RoleId, x.PermissionId }).IsUnique().HasDatabaseName("UX_role_permissions_role_id_permission_id");
        builder.HasData(AuthSeedData.RolePermissions);
    }
}

public sealed class StoreOperatorConfiguration : IEntityTypeConfiguration<StoreOperator>
{
    public void Configure(EntityTypeBuilder<StoreOperator> builder)
    {
        builder.ToTable("store_operators");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.StoreId).HasColumnName("store_id").IsRequired();
        builder.Property(x => x.UserId).HasColumnName("user_id").IsRequired();
        builder.Property(x => x.Role).HasColumnName("role").HasMaxLength(64).IsRequired();
        builder.Property(x => x.Status).HasColumnName("status").HasMaxLength(32).HasConversion<string>().IsRequired();
        builder.Property(x => x.CreatedAt).HasColumnName("created_at").IsRequired();
        builder.Property(x => x.UpdatedAt).HasColumnName("updated_at");
        builder.HasOne(x => x.User).WithMany(x => x.StoreOperators).HasForeignKey(x => x.UserId);
        builder.HasIndex(x => x.StoreId).HasDatabaseName("IX_store_operators_store_id");
        builder.HasIndex(x => x.UserId).HasDatabaseName("IX_store_operators_user_id");
        builder.HasIndex(x => new { x.StoreId, x.UserId }).IsUnique().HasDatabaseName("UX_store_operators_store_id_user_id");
    }
}

public sealed class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.ToTable("refresh_tokens");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.UserId).HasColumnName("user_id").IsRequired();
        builder.Property(x => x.TokenHash).HasColumnName("token_hash").HasMaxLength(512).IsRequired();
        builder.Property(x => x.ExpiresAt).HasColumnName("expires_at").IsRequired();
        builder.Property(x => x.RevokedAt).HasColumnName("revoked_at");
        builder.Property(x => x.ReplacedByTokenId).HasColumnName("replaced_by_token_id");
        builder.Property(x => x.IpAddress).HasColumnName("ip_address").HasMaxLength(64);
        builder.Property(x => x.UserAgent).HasColumnName("user_agent").HasMaxLength(512);
        builder.Property(x => x.CreatedAt).HasColumnName("created_at").IsRequired();
        builder.HasOne(x => x.User).WithMany(x => x.RefreshTokens).HasForeignKey(x => x.UserId);
        builder.HasIndex(x => x.TokenHash).IsUnique().HasDatabaseName("UX_refresh_tokens_token_hash");
        builder.HasIndex(x => x.UserId).HasDatabaseName("IX_refresh_tokens_user_id");
        builder.HasIndex(x => x.ExpiresAt).HasDatabaseName("IX_refresh_tokens_expires_at");
        builder.HasIndex(x => x.RevokedAt).HasDatabaseName("IX_refresh_tokens_revoked_at");
    }
}

public sealed class PasswordResetTokenConfiguration : IEntityTypeConfiguration<PasswordResetToken>
{
    public void Configure(EntityTypeBuilder<PasswordResetToken> builder)
    {
        builder.ToTable("password_reset_tokens");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.UserId).HasColumnName("user_id").IsRequired();
        builder.Property(x => x.TokenHash).HasColumnName("token_hash").HasMaxLength(512).IsRequired();
        builder.Property(x => x.ExpiresAt).HasColumnName("expires_at").IsRequired();
        builder.Property(x => x.UsedAt).HasColumnName("used_at");
        builder.Property(x => x.CreatedAt).HasColumnName("created_at").IsRequired();
        builder.HasIndex(x => x.TokenHash).IsUnique().HasDatabaseName("UX_password_reset_tokens_token_hash");
        builder.HasIndex(x => x.UserId).HasDatabaseName("IX_password_reset_tokens_user_id");
        builder.HasIndex(x => x.ExpiresAt).HasDatabaseName("IX_password_reset_tokens_expires_at");
    }
}

public sealed class UserConsentConfiguration : IEntityTypeConfiguration<UserConsent>
{
    public void Configure(EntityTypeBuilder<UserConsent> builder)
    {
        builder.ToTable("user_consents");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.UserId).HasColumnName("user_id").IsRequired();
        builder.Property(x => x.ConsentType).HasColumnName("consent_type").HasMaxLength(64).HasConversion<string>().IsRequired();
        builder.Property(x => x.Version).HasColumnName("version").HasMaxLength(32).IsRequired();
        builder.Property(x => x.AcceptedAt).HasColumnName("accepted_at").IsRequired();
        builder.Property(x => x.IpAddress).HasColumnName("ip_address").HasMaxLength(64);
        builder.Property(x => x.UserAgent).HasColumnName("user_agent").HasMaxLength(512);
        builder.HasOne(x => x.User).WithMany(x => x.UserConsents).HasForeignKey(x => x.UserId);
    }
}

public sealed class TermsAcceptanceConfiguration : IEntityTypeConfiguration<TermsAcceptance>
{
    public void Configure(EntityTypeBuilder<TermsAcceptance> builder)
    {
        builder.ToTable("terms_acceptances");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.UserId).HasColumnName("user_id").IsRequired();
        builder.Property(x => x.TermsVersion).HasColumnName("terms_version").HasMaxLength(32).IsRequired();
        builder.Property(x => x.PrivacyPolicyVersion).HasColumnName("privacy_policy_version").HasMaxLength(32).IsRequired();
        builder.Property(x => x.AcceptedAt).HasColumnName("accepted_at").IsRequired();
        builder.Property(x => x.IpAddress).HasColumnName("ip_address").HasMaxLength(64);
        builder.Property(x => x.UserAgent).HasColumnName("user_agent").HasMaxLength(512);
        builder.HasOne(x => x.User).WithMany(x => x.TermsAcceptances).HasForeignKey(x => x.UserId);
    }
}

public sealed class AuthAuditLogConfiguration : IEntityTypeConfiguration<AuthAuditLog>
{
    public void Configure(EntityTypeBuilder<AuthAuditLog> builder)
    {
        builder.ToTable("auth_audit_logs");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.UserId).HasColumnName("user_id");
        builder.Property(x => x.EventType).HasColumnName("event_type").HasMaxLength(64).HasConversion<string>().IsRequired();
        builder.Property(x => x.Result).HasColumnName("result").HasMaxLength(32).HasConversion<string>().IsRequired();
        builder.Property(x => x.IpAddress).HasColumnName("ip_address").HasMaxLength(64);
        builder.Property(x => x.UserAgent).HasColumnName("user_agent").HasMaxLength(512);
        builder.Property(x => x.CorrelationId).HasColumnName("correlation_id").HasMaxLength(128);
        builder.Property(x => x.MetadataJson).HasColumnName("metadata_json");
        builder.Property(x => x.CreatedAt).HasColumnName("created_at").IsRequired();
    }
}

internal static class AuthSeedData
{
    private static readonly DateTimeOffset CreatedAt = new(2026, 1, 1, 0, 0, 0, TimeSpan.Zero);

    public static readonly Role[] Roles =
    [
        Role("01000000-0000-0000-0000-000000000001", RoleNames.Consumer, "Consumidor"),
        Role("01000000-0000-0000-0000-000000000002", RoleNames.StoreOwner, "Dono de loja"),
        Role("01000000-0000-0000-0000-000000000003", RoleNames.StoreOperator, "Operador de loja"),
        Role("01000000-0000-0000-0000-000000000004", RoleNames.KuvaAdmin, "Administrador Kuva")
    ];

    public static readonly Permission[] Permissions =
    [
        Permission("02000000-0000-0000-0000-000000000001", PermissionNames.OrdersCreate),
        Permission("02000000-0000-0000-0000-000000000002", PermissionNames.OrdersReadOwn),
        Permission("02000000-0000-0000-0000-000000000003", PermissionNames.OrdersCancelOwn),
        Permission("02000000-0000-0000-0000-000000000004", PermissionNames.ProfileReadOwn),
        Permission("02000000-0000-0000-0000-000000000005", PermissionNames.ProfileUpdateOwn),
        Permission("02000000-0000-0000-0000-000000000006", PermissionNames.MerchantOrdersRead),
        Permission("02000000-0000-0000-0000-000000000007", PermissionNames.MerchantOrdersUpdateStatus),
        Permission("02000000-0000-0000-0000-000000000008", PermissionNames.MerchantMediaDownload),
        Permission("02000000-0000-0000-0000-000000000009", PermissionNames.MerchantStoreRead),
        Permission("02000000-0000-0000-0000-000000000010", PermissionNames.MerchantPricingRead),
        Permission("02000000-0000-0000-0000-000000000011", PermissionNames.MerchantPricingUpdate),
        Permission("02000000-0000-0000-0000-000000000012", PermissionNames.MerchantOperatorsRead),
        Permission("02000000-0000-0000-0000-000000000013", PermissionNames.AdminStoresRead),
        Permission("02000000-0000-0000-0000-000000000014", PermissionNames.AdminStoresCreate),
        Permission("02000000-0000-0000-0000-000000000015", PermissionNames.AdminStoresUpdate),
        Permission("02000000-0000-0000-0000-000000000016", PermissionNames.AdminUsersRead),
        Permission("02000000-0000-0000-0000-000000000017", PermissionNames.AdminAuditRead),
        Permission("02000000-0000-0000-0000-000000000018", PermissionNames.CatalogView),
        Permission("02000000-0000-0000-0000-000000000019", PermissionNames.CatalogEdit),
        Permission("02000000-0000-0000-0000-000000000020", PermissionNames.PriceEdit),
        Permission("02000000-0000-0000-0000-000000000021", PermissionNames.SkuEnableDisable)
    ];

    public static readonly RolePermission[] RolePermissions = BuildRolePermissions();

    private static Role Role(string id, string name, string description) => new()
    {
        Id = Guid.Parse(id),
        Name = name,
        NormalizedName = name,
        Description = description,
        Active = true,
        CreatedAt = CreatedAt
    };

    private static Permission Permission(string id, string name) => new()
    {
        Id = Guid.Parse(id),
        Name = name,
        NormalizedName = name.ToUpperInvariant(),
        Description = name,
        Active = true,
        CreatedAt = CreatedAt
    };

    private static RolePermission[] BuildRolePermissions()
    {
        var roleMap = Roles.ToDictionary(x => x.Name, x => x.Id);
        var permissionMap = Permissions.ToDictionary(x => x.Name, x => x.Id);
        var pairs = new List<(string Role, string Permission)>();
        pairs.AddRange(PermissionNames.Consumer.Select(x => (RoleNames.Consumer, x)));
        pairs.AddRange(new[]
        {
            PermissionNames.MerchantOrdersRead,
            PermissionNames.MerchantOrdersUpdateStatus,
            PermissionNames.MerchantMediaDownload,
            PermissionNames.MerchantStoreRead
        }.Select(x => (RoleNames.StoreOperator, x)));
        pairs.AddRange(new[]
        {
            PermissionNames.MerchantOrdersRead,
            PermissionNames.MerchantOrdersUpdateStatus,
            PermissionNames.MerchantMediaDownload,
            PermissionNames.MerchantStoreRead,
            PermissionNames.MerchantPricingRead,
            PermissionNames.MerchantPricingUpdate,
            PermissionNames.MerchantOperatorsRead
        }.Select(x => (RoleNames.StoreOwner, x)));
        pairs.AddRange(new[]
        {
            PermissionNames.AdminStoresRead,
            PermissionNames.AdminStoresCreate,
            PermissionNames.AdminStoresUpdate,
            PermissionNames.AdminUsersRead,
            PermissionNames.AdminAuditRead
        }.Select(x => (RoleNames.KuvaAdmin, x)));
        pairs.AddRange(new[]
        {
            (RoleNames.StoreOperator, PermissionNames.CatalogView),
            (RoleNames.StoreOperator, PermissionNames.PriceEdit),
            (RoleNames.StoreOwner, PermissionNames.CatalogView),
            (RoleNames.StoreOwner, PermissionNames.CatalogEdit),
            (RoleNames.StoreOwner, PermissionNames.PriceEdit),
            (RoleNames.StoreOwner, PermissionNames.SkuEnableDisable),
            (RoleNames.KuvaAdmin, PermissionNames.CatalogView),
            (RoleNames.KuvaAdmin, PermissionNames.CatalogEdit),
            (RoleNames.KuvaAdmin, PermissionNames.PriceEdit),
            (RoleNames.KuvaAdmin, PermissionNames.SkuEnableDisable)
        });

        return pairs.Select((pair, index) => new RolePermission
        {
            Id = Guid.Parse($"03000000-0000-0000-0000-{index + 1:000000000000}"),
            RoleId = roleMap[pair.Role],
            PermissionId = permissionMap[pair.Permission],
            CreatedAt = CreatedAt
        }).ToArray();
    }
}

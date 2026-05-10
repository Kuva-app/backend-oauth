using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Kuva.Auth.EFMigrations.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "identity");

            migrationBuilder.CreateTable(
                name: "permissions",
                schema: "identity",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    normalized_name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    active = table.Column<bool>(type: "bit", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_permissions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "roles",
                schema: "identity",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    normalized_name = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    active = table.Column<bool>(type: "bit", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                schema: "identity",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    normalized_email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    name = table.Column<string>(type: "nvarchar(160)", maxLength: 160, nullable: true),
                    phone = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    status = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    last_login_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "role_permissions",
                schema: "identity",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    role_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    permission_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_role_permissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_role_permissions_permissions_permission_id",
                        column: x => x.permission_id,
                        principalSchema: "identity",
                        principalTable: "permissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_role_permissions_roles_role_id",
                        column: x => x.role_id,
                        principalSchema: "identity",
                        principalTable: "roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "auth_audit_logs",
                schema: "identity",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    event_type = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    result = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    ip_address = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    user_agent = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    correlation_id = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    metadata_json = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_auth_audit_logs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_auth_audit_logs_users_user_id",
                        column: x => x.user_id,
                        principalSchema: "identity",
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "password_reset_tokens",
                schema: "identity",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    token_hash = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    expires_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    used_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_password_reset_tokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_password_reset_tokens_users_user_id",
                        column: x => x.user_id,
                        principalSchema: "identity",
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "refresh_tokens",
                schema: "identity",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    token_hash = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    expires_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    revoked_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    replaced_by_token_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ip_address = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    user_agent = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_refresh_tokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_refresh_tokens_users_user_id",
                        column: x => x.user_id,
                        principalSchema: "identity",
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "store_operators",
                schema: "identity",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    store_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    role = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    status = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_store_operators", x => x.Id);
                    table.ForeignKey(
                        name: "FK_store_operators_users_user_id",
                        column: x => x.user_id,
                        principalSchema: "identity",
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "terms_acceptances",
                schema: "identity",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    terms_version = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    privacy_policy_version = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    accepted_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ip_address = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    user_agent = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_terms_acceptances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_terms_acceptances_users_user_id",
                        column: x => x.user_id,
                        principalSchema: "identity",
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "user_consents",
                schema: "identity",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    consent_type = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    version = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    accepted_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ip_address = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    user_agent = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_consents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_user_consents_users_user_id",
                        column: x => x.user_id,
                        principalSchema: "identity",
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "user_credentials",
                schema: "identity",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    password_hash = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    password_algorithm = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    password_updated_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_credentials", x => x.Id);
                    table.ForeignKey(
                        name: "FK_user_credentials_users_user_id",
                        column: x => x.user_id,
                        principalSchema: "identity",
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "user_roles",
                schema: "identity",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    role_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_roles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_user_roles_roles_role_id",
                        column: x => x.role_id,
                        principalSchema: "identity",
                        principalTable: "roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_user_roles_users_user_id",
                        column: x => x.user_id,
                        principalSchema: "identity",
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                schema: "identity",
                table: "permissions",
                columns: new[] { "Id", "active", "created_at", "description", "name", "normalized_name" },
                values: new object[,]
                {
                    { new Guid("02000000-0000-0000-0000-000000000001"), true, new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "orders.create", "orders.create", "ORDERS.CREATE" },
                    { new Guid("02000000-0000-0000-0000-000000000002"), true, new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "orders.read_own", "orders.read_own", "ORDERS.READ_OWN" },
                    { new Guid("02000000-0000-0000-0000-000000000003"), true, new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "orders.cancel_own", "orders.cancel_own", "ORDERS.CANCEL_OWN" },
                    { new Guid("02000000-0000-0000-0000-000000000004"), true, new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "profile.read_own", "profile.read_own", "PROFILE.READ_OWN" },
                    { new Guid("02000000-0000-0000-0000-000000000005"), true, new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "profile.update_own", "profile.update_own", "PROFILE.UPDATE_OWN" },
                    { new Guid("02000000-0000-0000-0000-000000000006"), true, new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "merchant.orders.read", "merchant.orders.read", "MERCHANT.ORDERS.READ" },
                    { new Guid("02000000-0000-0000-0000-000000000007"), true, new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "merchant.orders.update_status", "merchant.orders.update_status", "MERCHANT.ORDERS.UPDATE_STATUS" },
                    { new Guid("02000000-0000-0000-0000-000000000008"), true, new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "merchant.media.download", "merchant.media.download", "MERCHANT.MEDIA.DOWNLOAD" },
                    { new Guid("02000000-0000-0000-0000-000000000009"), true, new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "merchant.store.read", "merchant.store.read", "MERCHANT.STORE.READ" },
                    { new Guid("02000000-0000-0000-0000-000000000010"), true, new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "merchant.pricing.read", "merchant.pricing.read", "MERCHANT.PRICING.READ" },
                    { new Guid("02000000-0000-0000-0000-000000000011"), true, new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "merchant.pricing.update", "merchant.pricing.update", "MERCHANT.PRICING.UPDATE" },
                    { new Guid("02000000-0000-0000-0000-000000000012"), true, new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "merchant.operators.read", "merchant.operators.read", "MERCHANT.OPERATORS.READ" },
                    { new Guid("02000000-0000-0000-0000-000000000013"), true, new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "admin.stores.read", "admin.stores.read", "ADMIN.STORES.READ" },
                    { new Guid("02000000-0000-0000-0000-000000000014"), true, new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "admin.stores.create", "admin.stores.create", "ADMIN.STORES.CREATE" },
                    { new Guid("02000000-0000-0000-0000-000000000015"), true, new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "admin.stores.update", "admin.stores.update", "ADMIN.STORES.UPDATE" },
                    { new Guid("02000000-0000-0000-0000-000000000016"), true, new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "admin.users.read", "admin.users.read", "ADMIN.USERS.READ" },
                    { new Guid("02000000-0000-0000-0000-000000000017"), true, new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "admin.audit.read", "admin.audit.read", "ADMIN.AUDIT.READ" }
                });

            migrationBuilder.InsertData(
                schema: "identity",
                table: "roles",
                columns: new[] { "Id", "active", "created_at", "description", "name", "normalized_name" },
                values: new object[,]
                {
                    { new Guid("01000000-0000-0000-0000-000000000001"), true, new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Consumidor", "CONSUMER", "CONSUMER" },
                    { new Guid("01000000-0000-0000-0000-000000000002"), true, new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Dono de loja", "STORE_OWNER", "STORE_OWNER" },
                    { new Guid("01000000-0000-0000-0000-000000000003"), true, new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Operador de loja", "STORE_OPERATOR", "STORE_OPERATOR" },
                    { new Guid("01000000-0000-0000-0000-000000000004"), true, new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Administrador Kuva", "KUVA_ADMIN", "KUVA_ADMIN" }
                });

            migrationBuilder.InsertData(
                schema: "identity",
                table: "role_permissions",
                columns: new[] { "Id", "created_at", "permission_id", "role_id" },
                values: new object[,]
                {
                    { new Guid("03000000-0000-0000-0000-000000000001"), new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("02000000-0000-0000-0000-000000000001"), new Guid("01000000-0000-0000-0000-000000000001") },
                    { new Guid("03000000-0000-0000-0000-000000000002"), new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("02000000-0000-0000-0000-000000000002"), new Guid("01000000-0000-0000-0000-000000000001") },
                    { new Guid("03000000-0000-0000-0000-000000000003"), new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("02000000-0000-0000-0000-000000000003"), new Guid("01000000-0000-0000-0000-000000000001") },
                    { new Guid("03000000-0000-0000-0000-000000000004"), new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("02000000-0000-0000-0000-000000000004"), new Guid("01000000-0000-0000-0000-000000000001") },
                    { new Guid("03000000-0000-0000-0000-000000000005"), new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("02000000-0000-0000-0000-000000000005"), new Guid("01000000-0000-0000-0000-000000000001") },
                    { new Guid("03000000-0000-0000-0000-000000000006"), new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("02000000-0000-0000-0000-000000000006"), new Guid("01000000-0000-0000-0000-000000000003") },
                    { new Guid("03000000-0000-0000-0000-000000000007"), new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("02000000-0000-0000-0000-000000000007"), new Guid("01000000-0000-0000-0000-000000000003") },
                    { new Guid("03000000-0000-0000-0000-000000000008"), new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("02000000-0000-0000-0000-000000000008"), new Guid("01000000-0000-0000-0000-000000000003") },
                    { new Guid("03000000-0000-0000-0000-000000000009"), new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("02000000-0000-0000-0000-000000000009"), new Guid("01000000-0000-0000-0000-000000000003") },
                    { new Guid("03000000-0000-0000-0000-000000000010"), new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("02000000-0000-0000-0000-000000000006"), new Guid("01000000-0000-0000-0000-000000000002") },
                    { new Guid("03000000-0000-0000-0000-000000000011"), new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("02000000-0000-0000-0000-000000000007"), new Guid("01000000-0000-0000-0000-000000000002") },
                    { new Guid("03000000-0000-0000-0000-000000000012"), new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("02000000-0000-0000-0000-000000000008"), new Guid("01000000-0000-0000-0000-000000000002") },
                    { new Guid("03000000-0000-0000-0000-000000000013"), new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("02000000-0000-0000-0000-000000000009"), new Guid("01000000-0000-0000-0000-000000000002") },
                    { new Guid("03000000-0000-0000-0000-000000000014"), new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("02000000-0000-0000-0000-000000000010"), new Guid("01000000-0000-0000-0000-000000000002") },
                    { new Guid("03000000-0000-0000-0000-000000000015"), new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("02000000-0000-0000-0000-000000000011"), new Guid("01000000-0000-0000-0000-000000000002") },
                    { new Guid("03000000-0000-0000-0000-000000000016"), new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("02000000-0000-0000-0000-000000000012"), new Guid("01000000-0000-0000-0000-000000000002") },
                    { new Guid("03000000-0000-0000-0000-000000000017"), new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("02000000-0000-0000-0000-000000000013"), new Guid("01000000-0000-0000-0000-000000000004") },
                    { new Guid("03000000-0000-0000-0000-000000000018"), new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("02000000-0000-0000-0000-000000000014"), new Guid("01000000-0000-0000-0000-000000000004") },
                    { new Guid("03000000-0000-0000-0000-000000000019"), new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("02000000-0000-0000-0000-000000000015"), new Guid("01000000-0000-0000-0000-000000000004") },
                    { new Guid("03000000-0000-0000-0000-000000000020"), new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("02000000-0000-0000-0000-000000000016"), new Guid("01000000-0000-0000-0000-000000000004") },
                    { new Guid("03000000-0000-0000-0000-000000000021"), new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("02000000-0000-0000-0000-000000000017"), new Guid("01000000-0000-0000-0000-000000000004") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_auth_audit_logs_user_id",
                schema: "identity",
                table: "auth_audit_logs",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_password_reset_tokens_expires_at",
                schema: "identity",
                table: "password_reset_tokens",
                column: "expires_at");

            migrationBuilder.CreateIndex(
                name: "IX_password_reset_tokens_user_id",
                schema: "identity",
                table: "password_reset_tokens",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "UX_password_reset_tokens_token_hash",
                schema: "identity",
                table: "password_reset_tokens",
                column: "token_hash",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UX_permissions_name",
                schema: "identity",
                table: "permissions",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UX_permissions_normalized_name",
                schema: "identity",
                table: "permissions",
                column: "normalized_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_refresh_tokens_expires_at",
                schema: "identity",
                table: "refresh_tokens",
                column: "expires_at");

            migrationBuilder.CreateIndex(
                name: "IX_refresh_tokens_revoked_at",
                schema: "identity",
                table: "refresh_tokens",
                column: "revoked_at");

            migrationBuilder.CreateIndex(
                name: "IX_refresh_tokens_user_id",
                schema: "identity",
                table: "refresh_tokens",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "UX_refresh_tokens_token_hash",
                schema: "identity",
                table: "refresh_tokens",
                column: "token_hash",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_role_permissions_permission_id",
                schema: "identity",
                table: "role_permissions",
                column: "permission_id");

            migrationBuilder.CreateIndex(
                name: "UX_role_permissions_role_id_permission_id",
                schema: "identity",
                table: "role_permissions",
                columns: new[] { "role_id", "permission_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UX_roles_name",
                schema: "identity",
                table: "roles",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UX_roles_normalized_name",
                schema: "identity",
                table: "roles",
                column: "normalized_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_store_operators_store_id",
                schema: "identity",
                table: "store_operators",
                column: "store_id");

            migrationBuilder.CreateIndex(
                name: "IX_store_operators_user_id",
                schema: "identity",
                table: "store_operators",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "UX_store_operators_store_id_user_id",
                schema: "identity",
                table: "store_operators",
                columns: new[] { "store_id", "user_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_terms_acceptances_user_id",
                schema: "identity",
                table: "terms_acceptances",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_consents_user_id",
                schema: "identity",
                table: "user_consents",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "UX_user_credentials_user_id",
                schema: "identity",
                table: "user_credentials",
                column: "user_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_user_roles_role_id",
                schema: "identity",
                table: "user_roles",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "UX_user_roles_user_id_role_id",
                schema: "identity",
                table: "user_roles",
                columns: new[] { "user_id", "role_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_users_status",
                schema: "identity",
                table: "users",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "UX_users_email",
                schema: "identity",
                table: "users",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UX_users_normalized_email",
                schema: "identity",
                table: "users",
                column: "normalized_email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "auth_audit_logs",
                schema: "identity");

            migrationBuilder.DropTable(
                name: "password_reset_tokens",
                schema: "identity");

            migrationBuilder.DropTable(
                name: "refresh_tokens",
                schema: "identity");

            migrationBuilder.DropTable(
                name: "role_permissions",
                schema: "identity");

            migrationBuilder.DropTable(
                name: "store_operators",
                schema: "identity");

            migrationBuilder.DropTable(
                name: "terms_acceptances",
                schema: "identity");

            migrationBuilder.DropTable(
                name: "user_consents",
                schema: "identity");

            migrationBuilder.DropTable(
                name: "user_credentials",
                schema: "identity");

            migrationBuilder.DropTable(
                name: "user_roles",
                schema: "identity");

            migrationBuilder.DropTable(
                name: "permissions",
                schema: "identity");

            migrationBuilder.DropTable(
                name: "roles",
                schema: "identity");

            migrationBuilder.DropTable(
                name: "users",
                schema: "identity");
        }
    }
}

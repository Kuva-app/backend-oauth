using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Kuva.Auth.EFMigrations.Migrations
{
    /// <inheritdoc />
    public partial class AddCatalogPricingPermissions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "identity",
                table: "permissions",
                columns: new[] { "Id", "active", "created_at", "description", "name", "normalized_name" },
                values: new object[,]
                {
                    { new Guid("02000000-0000-0000-0000-000000000018"), true, new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "CATALOG_VIEW", "CATALOG_VIEW", "CATALOG_VIEW" },
                    { new Guid("02000000-0000-0000-0000-000000000019"), true, new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "CATALOG_EDIT", "CATALOG_EDIT", "CATALOG_EDIT" },
                    { new Guid("02000000-0000-0000-0000-000000000020"), true, new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "PRICE_EDIT", "PRICE_EDIT", "PRICE_EDIT" },
                    { new Guid("02000000-0000-0000-0000-000000000021"), true, new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "SKU_ENABLE_DISABLE", "SKU_ENABLE_DISABLE", "SKU_ENABLE_DISABLE" }
                });

            migrationBuilder.InsertData(
                schema: "identity",
                table: "role_permissions",
                columns: new[] { "Id", "created_at", "permission_id", "role_id" },
                values: new object[,]
                {
                    { new Guid("03000000-0000-0000-0000-000000000022"), new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("02000000-0000-0000-0000-000000000018"), new Guid("01000000-0000-0000-0000-000000000003") },
                    { new Guid("03000000-0000-0000-0000-000000000023"), new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("02000000-0000-0000-0000-000000000020"), new Guid("01000000-0000-0000-0000-000000000003") },
                    { new Guid("03000000-0000-0000-0000-000000000024"), new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("02000000-0000-0000-0000-000000000018"), new Guid("01000000-0000-0000-0000-000000000002") },
                    { new Guid("03000000-0000-0000-0000-000000000025"), new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("02000000-0000-0000-0000-000000000019"), new Guid("01000000-0000-0000-0000-000000000002") },
                    { new Guid("03000000-0000-0000-0000-000000000026"), new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("02000000-0000-0000-0000-000000000020"), new Guid("01000000-0000-0000-0000-000000000002") },
                    { new Guid("03000000-0000-0000-0000-000000000027"), new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("02000000-0000-0000-0000-000000000021"), new Guid("01000000-0000-0000-0000-000000000002") },
                    { new Guid("03000000-0000-0000-0000-000000000028"), new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("02000000-0000-0000-0000-000000000018"), new Guid("01000000-0000-0000-0000-000000000004") },
                    { new Guid("03000000-0000-0000-0000-000000000029"), new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("02000000-0000-0000-0000-000000000019"), new Guid("01000000-0000-0000-0000-000000000004") },
                    { new Guid("03000000-0000-0000-0000-000000000030"), new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("02000000-0000-0000-0000-000000000020"), new Guid("01000000-0000-0000-0000-000000000004") },
                    { new Guid("03000000-0000-0000-0000-000000000031"), new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("02000000-0000-0000-0000-000000000021"), new Guid("01000000-0000-0000-0000-000000000004") }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "identity",
                table: "role_permissions",
                keyColumn: "Id",
                keyValue: new Guid("03000000-0000-0000-0000-000000000022"));

            migrationBuilder.DeleteData(
                schema: "identity",
                table: "role_permissions",
                keyColumn: "Id",
                keyValue: new Guid("03000000-0000-0000-0000-000000000023"));

            migrationBuilder.DeleteData(
                schema: "identity",
                table: "role_permissions",
                keyColumn: "Id",
                keyValue: new Guid("03000000-0000-0000-0000-000000000024"));

            migrationBuilder.DeleteData(
                schema: "identity",
                table: "role_permissions",
                keyColumn: "Id",
                keyValue: new Guid("03000000-0000-0000-0000-000000000025"));

            migrationBuilder.DeleteData(
                schema: "identity",
                table: "role_permissions",
                keyColumn: "Id",
                keyValue: new Guid("03000000-0000-0000-0000-000000000026"));

            migrationBuilder.DeleteData(
                schema: "identity",
                table: "role_permissions",
                keyColumn: "Id",
                keyValue: new Guid("03000000-0000-0000-0000-000000000027"));

            migrationBuilder.DeleteData(
                schema: "identity",
                table: "role_permissions",
                keyColumn: "Id",
                keyValue: new Guid("03000000-0000-0000-0000-000000000028"));

            migrationBuilder.DeleteData(
                schema: "identity",
                table: "role_permissions",
                keyColumn: "Id",
                keyValue: new Guid("03000000-0000-0000-0000-000000000029"));

            migrationBuilder.DeleteData(
                schema: "identity",
                table: "role_permissions",
                keyColumn: "Id",
                keyValue: new Guid("03000000-0000-0000-0000-000000000030"));

            migrationBuilder.DeleteData(
                schema: "identity",
                table: "role_permissions",
                keyColumn: "Id",
                keyValue: new Guid("03000000-0000-0000-0000-000000000031"));

            migrationBuilder.DeleteData(
                schema: "identity",
                table: "permissions",
                keyColumn: "Id",
                keyValue: new Guid("02000000-0000-0000-0000-000000000018"));

            migrationBuilder.DeleteData(
                schema: "identity",
                table: "permissions",
                keyColumn: "Id",
                keyValue: new Guid("02000000-0000-0000-0000-000000000019"));

            migrationBuilder.DeleteData(
                schema: "identity",
                table: "permissions",
                keyColumn: "Id",
                keyValue: new Guid("02000000-0000-0000-0000-000000000020"));

            migrationBuilder.DeleteData(
                schema: "identity",
                table: "permissions",
                keyColumn: "Id",
                keyValue: new Guid("02000000-0000-0000-0000-000000000021"));
        }
    }
}

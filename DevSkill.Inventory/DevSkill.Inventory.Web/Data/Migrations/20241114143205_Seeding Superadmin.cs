using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DevSkill.Inventory.Web.Migrations.ApplicationDb
{
    /// <inheritdoc />
    public partial class SeedingSuperadmin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { new Guid("b136d304-ed89-4084-8a3d-521ecda7bb7e"), null, "SuperAdmin", "SUPERADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "Status", "TwoFactorEnabled", "UserName" },
                values: new object[] { new Guid("dd5b6529-e2f1-4e98-ad4d-bd33432de0e0"), 0, "c04f12af-d2ce-41ea-8eb5-38878b3e8133", "superadmin@example.com", true, null, null, false, null, "SUPERADMIN@EXAMPLE.COM", "SUPERADMIN@EXAMPLE.COM", "AQAAAAIAAYagAAAAECXrhLKpHfsoIoR2zGHJOqccDw74gu+ccnsoEzT6txF5AzsUGm0FAF9wJ84QjIepaQ==", null, false, null, "Active", false, "superadmin@example.com" });

            migrationBuilder.InsertData(
                table: "AspNetRoleClaims",
                columns: new[] { "Id", "ClaimType", "ClaimValue", "RoleId" },
                values: new object[,]
                {
                    { -32, "Permission", "RoleDelete", new Guid("b136d304-ed89-4084-8a3d-521ecda7bb7e") },
                    { -31, "Permission", "RoleUpdate", new Guid("b136d304-ed89-4084-8a3d-521ecda7bb7e") },
                    { -30, "Permission", "RoleView", new Guid("b136d304-ed89-4084-8a3d-521ecda7bb7e") },
                    { -29, "Permission", "RoleCreate", new Guid("b136d304-ed89-4084-8a3d-521ecda7bb7e") },
                    { -28, "Permission", "UserDelete", new Guid("b136d304-ed89-4084-8a3d-521ecda7bb7e") },
                    { -27, "Permission", "UserUpdate", new Guid("b136d304-ed89-4084-8a3d-521ecda7bb7e") },
                    { -26, "Permission", "UserView", new Guid("b136d304-ed89-4084-8a3d-521ecda7bb7e") },
                    { -25, "Permission", "UserCreate", new Guid("b136d304-ed89-4084-8a3d-521ecda7bb7e") },
                    { -24, "Permission", "WarehouseDelete", new Guid("b136d304-ed89-4084-8a3d-521ecda7bb7e") },
                    { -23, "Permission", "WarehouseUpdate", new Guid("b136d304-ed89-4084-8a3d-521ecda7bb7e") },
                    { -22, "Permission", "WarehouseView", new Guid("b136d304-ed89-4084-8a3d-521ecda7bb7e") },
                    { -21, "Permission", "WarehouseCreate", new Guid("b136d304-ed89-4084-8a3d-521ecda7bb7e") },
                    { -20, "Permission", "TransferGenerateReport", new Guid("b136d304-ed89-4084-8a3d-521ecda7bb7e") },
                    { -19, "Permission", "TransferGetDetails", new Guid("b136d304-ed89-4084-8a3d-521ecda7bb7e") },
                    { -18, "Permission", "TransferDelete", new Guid("b136d304-ed89-4084-8a3d-521ecda7bb7e") },
                    { -17, "Permission", "TransferView", new Guid("b136d304-ed89-4084-8a3d-521ecda7bb7e") },
                    { -16, "Permission", "TransferCreate", new Guid("b136d304-ed89-4084-8a3d-521ecda7bb7e") },
                    { -15, "Permission", "ProductionGenerateReport", new Guid("b136d304-ed89-4084-8a3d-521ecda7bb7e") },
                    { -14, "Permission", "ProductionGetDetails", new Guid("b136d304-ed89-4084-8a3d-521ecda7bb7e") },
                    { -13, "Permission", "ProductionDelete", new Guid("b136d304-ed89-4084-8a3d-521ecda7bb7e") },
                    { -12, "Permission", "ProductionGet", new Guid("b136d304-ed89-4084-8a3d-521ecda7bb7e") },
                    { -11, "Permission", "ProductionCreate", new Guid("b136d304-ed89-4084-8a3d-521ecda7bb7e") },
                    { -10, "Permission", "ConsumptionGeneratePDF", new Guid("b136d304-ed89-4084-8a3d-521ecda7bb7e") },
                    { -9, "Permission", "ConsumptionGetDetails", new Guid("b136d304-ed89-4084-8a3d-521ecda7bb7e") },
                    { -8, "Permission", "ConsumptionDelete", new Guid("b136d304-ed89-4084-8a3d-521ecda7bb7e") },
                    { -7, "Permission", "ConsumptionView", new Guid("b136d304-ed89-4084-8a3d-521ecda7bb7e") },
                    { -6, "Permission", "ConsumptionCreate", new Guid("b136d304-ed89-4084-8a3d-521ecda7bb7e") },
                    { -5, "Permission", "ItemViewDetails", new Guid("b136d304-ed89-4084-8a3d-521ecda7bb7e") },
                    { -4, "Permission", "ItemGet", new Guid("b136d304-ed89-4084-8a3d-521ecda7bb7e") },
                    { -3, "Permission", "ItemDelete", new Guid("b136d304-ed89-4084-8a3d-521ecda7bb7e") },
                    { -2, "Permission", "ItemUpdate", new Guid("b136d304-ed89-4084-8a3d-521ecda7bb7e") },
                    { -1, "Permission", "ItemCreate", new Guid("b136d304-ed89-4084-8a3d-521ecda7bb7e") }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { new Guid("b136d304-ed89-4084-8a3d-521ecda7bb7e"), new Guid("dd5b6529-e2f1-4e98-ad4d-bd33432de0e0") });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: -32);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: -31);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: -30);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: -29);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: -28);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: -27);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: -26);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: -25);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: -24);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: -23);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: -22);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: -21);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: -20);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: -19);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: -18);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: -17);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: -16);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: -15);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: -14);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: -13);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: -12);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: -11);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: -10);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: -9);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: -8);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: -7);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: -6);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: -5);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: -4);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: -3);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: -2);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: -1);

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("b136d304-ed89-4084-8a3d-521ecda7bb7e"), new Guid("dd5b6529-e2f1-4e98-ad4d-bd33432de0e0") });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("b136d304-ed89-4084-8a3d-521ecda7bb7e"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("dd5b6529-e2f1-4e98-ad4d-bd33432de0e0"));
        }
    }
}

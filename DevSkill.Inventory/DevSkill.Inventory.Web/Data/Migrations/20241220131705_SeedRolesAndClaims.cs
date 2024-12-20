using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DevSkill.Inventory.Web.Migrations.ApplicationDb
{
    /// <inheritdoc />
    public partial class SeedRolesAndClaims : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { new Guid("3c1ae0ba-096f-4ad8-ab20-36632634663c"), null, "Member", "MEMBER" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("dd5b6529-e2f1-4e98-ad4d-bd33432de0e0"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "692ff217-eee3-48d4-a6ec-77a64c33cd30", "AQAAAAIAAYagAAAAEB0RQMRnrY9/96LuU1GP/QnCmzRVHWZBUHhwH09A9QyPWCqnVcoBGa3GfNxmr5aOag==", "f12b9222-1d56-4ca1-a966-17b52ad20a5d" });

            migrationBuilder.InsertData(
                table: "AspNetRoleClaims",
                columns: new[] { "Id", "ClaimType", "ClaimValue", "RoleId" },
                values: new object[,]
                {
                    { -105, "Permission", "WarehouseView", new Guid("3c1ae0ba-096f-4ad8-ab20-36632634663c") },
                    { -104, "Permission", "TransferView", new Guid("3c1ae0ba-096f-4ad8-ab20-36632634663c") },
                    { -103, "Permission", "ProductionGet", new Guid("3c1ae0ba-096f-4ad8-ab20-36632634663c") },
                    { -102, "Permission", "ConsumptionView", new Guid("3c1ae0ba-096f-4ad8-ab20-36632634663c") },
                    { -101, "Permission", "ItemViewDetails", new Guid("3c1ae0ba-096f-4ad8-ab20-36632634663c") }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: -105);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: -104);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: -103);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: -102);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: -101);

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("3c1ae0ba-096f-4ad8-ab20-36632634663c"));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("dd5b6529-e2f1-4e98-ad4d-bd33432de0e0"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "2f0722bf-0b0b-41e2-8405-d31e9c47a758", "AQAAAAIAAYagAAAAEGHZ+dc4+SlD4cHRTbmXVkVu4b878e5meUZaCB2K40zo9YsLnjaB0s31OAiLYyhR7A==", "0265dbd5-cac5-4123-94ec-d7ebfbcb105c" });
        }
    }
}

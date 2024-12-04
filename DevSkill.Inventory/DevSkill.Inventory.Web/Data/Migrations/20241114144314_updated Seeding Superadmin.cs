using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevSkill.Inventory.Web.Migrations.ApplicationDb
{
    /// <inheritdoc />
    public partial class updatedSeedingSuperadmin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("dd5b6529-e2f1-4e98-ad4d-bd33432de0e0"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "893b3e49-5eb0-4192-9f0e-c14812f1822b", "AQAAAAIAAYagAAAAEH+sWBC/IFsGrm5y3P4QYBsW2RDx46gi89+3l0pImKlhazhdzLK1qLyyiGscwJQtlA==", "5443d5ab-572b-44b3-ba42-2b1d03e8a256" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("dd5b6529-e2f1-4e98-ad4d-bd33432de0e0"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "c04f12af-d2ce-41ea-8eb5-38878b3e8133", "AQAAAAIAAYagAAAAECXrhLKpHfsoIoR2zGHJOqccDw74gu+ccnsoEzT6txF5AzsUGm0FAF9wJ84QjIepaQ==", null });
        }
    }
}

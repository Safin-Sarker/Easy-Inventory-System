using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevSkill.Inventory.Web.Migrations.ApplicationDb
{
    /// <inheritdoc />
    public partial class updateAddProfileFieldsToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("dd5b6529-e2f1-4e98-ad4d-bd33432de0e0"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "2f0722bf-0b0b-41e2-8405-d31e9c47a758", "AQAAAAIAAYagAAAAEGHZ+dc4+SlD4cHRTbmXVkVu4b878e5meUZaCB2K40zo9YsLnjaB0s31OAiLYyhR7A==", "0265dbd5-cac5-4123-94ec-d7ebfbcb105c" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("dd5b6529-e2f1-4e98-ad4d-bd33432de0e0"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "09fa8314-ffc2-45e0-957c-6498234a11cb", "AQAAAAIAAYagAAAAEMu50ddMarcNl3RHgGEMPpm9fWLCZchR9ySyfj4RaGEv5QOzIIrCSQdABwm5LOGE1A==", "d453d24c-ea13-4ee8-bd65-3a0c37d29418" });
        }
    }
}

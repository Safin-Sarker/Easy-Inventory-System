using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevSkill.Inventory.Web.Migrations.ApplicationDb
{
    /// <inheritdoc />
    public partial class AddProfileFieldsToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "BirthDate",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BloodGroup",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Gender",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Locale",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PermanentAddress",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PresentAddress",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfilePictureUrl",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TimeZone",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("dd5b6529-e2f1-4e98-ad4d-bd33432de0e0"),
                columns: new[] { "BirthDate", "BloodGroup", "ConcurrencyStamp", "FullName", "Gender", "Locale", "PasswordHash", "PermanentAddress", "PresentAddress", "ProfilePictureUrl", "SecurityStamp", "TimeZone", "UserName" },
                values: new object[] { null, null, "09fa8314-ffc2-45e0-957c-6498234a11cb", null, null, null, "AQAAAAIAAYagAAAAEMu50ddMarcNl3RHgGEMPpm9fWLCZchR9ySyfj4RaGEv5QOzIIrCSQdABwm5LOGE1A==", null, null, null, "d453d24c-ea13-4ee8-bd65-3a0c37d29418", null, "superadmin@example.com" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BirthDate",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "BloodGroup",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FullName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Locale",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PermanentAddress",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PresentAddress",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ProfilePictureUrl",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "TimeZone",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("dd5b6529-e2f1-4e98-ad4d-bd33432de0e0"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp", "UserName" },
                values: new object[] { "42de10ff-dbe9-4475-8160-6c58dc5a518b", "AQAAAAIAAYagAAAAEAhbUtPFztPS8woUOgCbZL5aN9HHeLntThm07vxCffv4Xbk+HJqIWRxCqDXFaZQlog==", "9d70dcf0-9ad0-4296-96de-2cc5ebb6bb1d", "SuperAdmin" });
        }
    }
}

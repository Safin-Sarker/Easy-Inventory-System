﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevSkill.Inventory.Web.Migrations.ApplicationDb
{
    /// <inheritdoc />
    public partial class UpdateUserRoleConfig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           // migrationBuilder.CreateTable(
           //    name: "AspNetUserRoles",
           //    columns: table => new
           //    {
           //        UserId = table.Column<Guid>(nullable: false),
           //        RoleId = table.Column<Guid>(nullable: false)
           //    },
           //    constraints: table =>
           //    {
           //        table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
           //        table.ForeignKey(
           //            name: "FK_AspNetUserRoles_AspNetUsers_UserId",
           //            column: x => x.UserId,
           //            principalTable: "AspNetUsers",
           //            principalColumn: "Id",
           //            onDelete: ReferentialAction.Cascade);
           //        table.ForeignKey(
           //            name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
           //            column: x => x.RoleId,
           //            principalTable: "AspNetRoles",
           //            principalColumn: "Id",
           //            onDelete: ReferentialAction.Cascade);
           //});

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropTable(
            //  name: "AspNetUserRoles");

        }
    }
}

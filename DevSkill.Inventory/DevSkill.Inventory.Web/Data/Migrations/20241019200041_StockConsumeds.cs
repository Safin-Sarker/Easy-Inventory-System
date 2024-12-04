using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevSkill.Inventory.Web.Migrations.InventoryDb
{
    /// <inheritdoc />
    public partial class StockConsumeds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Product",
                table: "StockConsumptions");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "StockConsumptions");

            migrationBuilder.DropColumn(
                name: "Warehouse",
                table: "StockConsumptions");

            migrationBuilder.AlterColumn<string>(
                name: "Details",
                table: "StockConsumptions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "WarehouseId",
                table: "StockConsumptions",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "StockConsumeds",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StockConsumptionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    UnitPrice = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockConsumeds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StockConsumeds_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StockConsumeds_StockConsumptions_StockConsumptionId",
                        column: x => x.StockConsumptionId,
                        principalTable: "StockConsumptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StockConsumptions_WarehouseId",
                table: "StockConsumptions",
                column: "WarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_StockConsumeds_ItemId",
                table: "StockConsumeds",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_StockConsumeds_StockConsumptionId",
                table: "StockConsumeds",
                column: "StockConsumptionId");

            migrationBuilder.AddForeignKey(
                name: "FK_StockConsumptions_Warehouses_WarehouseId",
                table: "StockConsumptions",
                column: "WarehouseId",
                principalTable: "Warehouses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StockConsumptions_Warehouses_WarehouseId",
                table: "StockConsumptions");

            migrationBuilder.DropTable(
                name: "StockConsumeds");

            migrationBuilder.DropIndex(
                name: "IX_StockConsumptions_WarehouseId",
                table: "StockConsumptions");

            migrationBuilder.DropColumn(
                name: "WarehouseId",
                table: "StockConsumptions");

            migrationBuilder.AlterColumn<string>(
                name: "Details",
                table: "StockConsumptions",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "Product",
                table: "StockConsumptions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "StockConsumptions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Warehouse",
                table: "StockConsumptions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}

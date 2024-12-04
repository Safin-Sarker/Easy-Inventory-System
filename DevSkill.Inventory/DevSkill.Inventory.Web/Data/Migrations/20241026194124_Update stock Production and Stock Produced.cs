using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevSkill.Inventory.Web.Migrations.InventoryDb
{
    /// <inheritdoc />
    public partial class UpdatestockProductionandStockProduced : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProductionNumber",
                table: "StockProductions",
                newName: "VoucherNumber");

            migrationBuilder.RenameColumn(
                name: "ProductionDate",
                table: "StockProductions",
                newName: "VoucherDate");

            migrationBuilder.RenameColumn(
                name: "UnitCost",
                table: "StockProduceds",
                newName: "UnitPrice");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "VoucherNumber",
                table: "StockProductions",
                newName: "ProductionNumber");

            migrationBuilder.RenameColumn(
                name: "VoucherDate",
                table: "StockProductions",
                newName: "ProductionDate");

            migrationBuilder.RenameColumn(
                name: "UnitPrice",
                table: "StockProduceds",
                newName: "UnitCost");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevSkill.Inventory.Web.Migrations.InventoryDb
{
    /// <inheritdoc />
    public partial class updateStockconsumptionTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_StockCosumptions",
                table: "StockCosumptions");

            migrationBuilder.RenameTable(
                name: "StockCosumptions",
                newName: "StockConsumptions");

            migrationBuilder.AddColumn<string>(
                name: "Product",
                table: "StockConsumptions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Quantity",
                table: "StockConsumptions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StockConsumptions",
                table: "StockConsumptions",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_StockConsumptions",
                table: "StockConsumptions");

            migrationBuilder.DropColumn(
                name: "Product",
                table: "StockConsumptions");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "StockConsumptions");

            migrationBuilder.RenameTable(
                name: "StockConsumptions",
                newName: "StockCosumptions");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StockCosumptions",
                table: "StockCosumptions",
                column: "Id");
        }
    }
}

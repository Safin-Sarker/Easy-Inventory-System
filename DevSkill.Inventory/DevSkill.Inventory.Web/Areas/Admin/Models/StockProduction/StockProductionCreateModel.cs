using Microsoft.AspNetCore.Mvc.Rendering;

namespace DevSkill.Inventory.Web.Areas.Admin.Models.StockProduction
{
    public class StockProductionCreateModel
    {
        public string VoucherNumber { get; set; }
    
        public DateTime VoucherDate { get; set; }

        public Guid WarehouseId { get; set; } 
        // Foreign key to Warehouse
        public string CreatedBy { get; set; }

        public string? Details { get; set; }


        public List<StockProducedCreateModel> StockProducedItems { get; set; } = new List<StockProducedCreateModel>();

        public SelectList? Warehouses { get; set; }

        public SelectList? Items { get; set; }
    }
}

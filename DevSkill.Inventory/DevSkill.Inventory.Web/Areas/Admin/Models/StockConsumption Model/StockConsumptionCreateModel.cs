using Microsoft.AspNetCore.Mvc.Rendering;

namespace DevSkill.Inventory.Web.Areas.Admin.Models.StockConsumption_Model
{
    public class StockConsumptionCreateModel
    {
        public string VoucherNumber { get; set; }  
        
        public DateTime VoucherDate { get; set; }   
        
        public Guid WarehouseId { get; set; }   
        
        public string? Details { get; set; }    
        
        public string CreatedBy { get; set; }

        public List<StockConsumedCreateModel> StockConsumeds { get; set; } = new List<StockConsumedCreateModel>();

        public SelectList? Warehouses { get; set; }

        public SelectList? Items { get; set; }

    }
}

using Microsoft.AspNetCore.Mvc.Rendering;

namespace DevSkill.Inventory.Web.Areas.Admin.Models.StockConsumption_Model
{
    public class StockConsumptionUpdateModel
    {

        public Guid Id { get; set; }  
        public string VoucherNumber { get; set; }  
        public DateTime VoucherDate { get; set; } 
        public Guid WarehouseId { get; set; }
        public string? Details { get; set; }  
        public string CreatedBy { get; set; }  
        // Dropdown for items to be consumed in the stock (e.g., product list)
        public SelectList? Items { get; set; }

        // Dropdown for selecting the warehouse
        public SelectList? Warehouses { get; set; }

        // StockConsumed collection that might be edited (if relevant)
        public ICollection<StockConsumedUpdateModel> StockConsumeds { get; set; } = new List<StockConsumedUpdateModel>();

        public Dictionary<Guid, List<SelectListItem>> ItemsByWarehouse { get; set; } = new Dictionary<Guid, List<SelectListItem>>();
    }
}

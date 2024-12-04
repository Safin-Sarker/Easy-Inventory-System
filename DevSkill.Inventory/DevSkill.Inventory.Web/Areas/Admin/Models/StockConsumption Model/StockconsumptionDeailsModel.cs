namespace DevSkill.Inventory.Web.Areas.Admin.Models.StockConsumption_Model
{
    public class StockconsumptionDeailsModel
    {

        public Guid Id { get; set; }  
        public string VoucherNumber { get; set; } 
        public DateTime VoucherDate { get; set; }  
        public string WarehouseName { get; set; }  
        public string CreatedBy { get; set; }  
        public string? Details { get; set; } 
        public List<StockConsumedDetailsModel> StockConsumeds { get; set; } = new List<StockConsumedDetailsModel>(); 

    }
}

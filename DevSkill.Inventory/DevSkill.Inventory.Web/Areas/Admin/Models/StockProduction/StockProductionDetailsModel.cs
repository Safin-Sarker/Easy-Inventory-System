using DevSkill.Inventory.Web.Areas.Admin.Models.StockConsumption_Model;

namespace DevSkill.Inventory.Web.Areas.Admin.Models.StockProduction
{
    public class StockProductionDetailsModel
    {
        public Guid Id { get; set; }
        public string VoucherNumber { get; set; }
        public DateTime VoucherDate { get; set; }
        public string WarehouseName { get; set; }
        public string CreatedBy { get; set; }
        public string? Details { get; set; }
        public List<StockProducedDetailsModel> StockProducedItems { get; set; } = new List<StockProducedDetailsModel>();

    }
}

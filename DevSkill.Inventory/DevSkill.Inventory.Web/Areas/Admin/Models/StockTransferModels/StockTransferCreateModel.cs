using Microsoft.AspNetCore.Mvc.Rendering;

namespace DevSkill.Inventory.Web.Areas.Admin.Models.StockTransferModels
{
    public class StockTransferCreateModel
    {
        public string VoucherNumber { get; set; }
        public DateTime VoucherDate { get; set; }
        public Guid SourceWarehouseId { get; set; }
        public string? Details { get; set; }
        public string CreatedBy { get; set; }
        public List<StockTransferItemCreateModel> TransferItems { get; set; } = new List<StockTransferItemCreateModel>();

        public SelectList? SourceWarehouses { get; set; }
        public SelectList? Products { get; set; }
        public SelectList? DestinationWarehouses { get; set; }
    }
}

namespace DevSkill.Inventory.Web.Areas.Admin.Models.StockTransferModels
{
    public class StockTransferDetailsModel
    {
        public Guid Id { get; set; }
        public string VoucherNumber { get; set; }
        public DateTime VoucherDate { get; set; }
        public Guid SourceWarehouseId { get; set; }
        public string SourceWarehouseName { get; set; }
        public string? Details { get; set; }
        public string CreatedBy { get; set; }
        public List<StockTransferItemsDetailsModel> TransferItems { get; set; } = new List<StockTransferItemsDetailsModel>();
    }
}

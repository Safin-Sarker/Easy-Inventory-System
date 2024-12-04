namespace DevSkill.Inventory.Web.Areas.Admin.Models.StockTransferModels
{
    public class StockTransferItemCreateModel
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public Guid DestinationWarehouseId { get; set; }
    }
}

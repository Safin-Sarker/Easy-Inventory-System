namespace DevSkill.Inventory.Web.Areas.Admin.Models.StockTransferModels
{
    public class StockTransferItemsDetailsModel
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public Guid DestinationWarehouseId { get; set; }
        public string DestinationWarehouseName { get; set; }
    }
}

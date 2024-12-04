namespace DevSkill.Inventory.Web.Areas.Admin.Models.StockConsumption_Model
{
    public class StockConsumedUpdateModel
    {
        public Guid Id { get; set; }  // Unique identifier for the stock consumed record
        public Guid ItemId { get; set; }  // The item that was consumed
        public int Quantity { get; set; }
        public double? UnitPrice { get; set; }
    }
}

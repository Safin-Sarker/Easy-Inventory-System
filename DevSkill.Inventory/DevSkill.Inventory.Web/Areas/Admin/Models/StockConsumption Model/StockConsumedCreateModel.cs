namespace DevSkill.Inventory.Web.Areas.Admin.Models.StockConsumption_Model
{
    public class StockConsumedCreateModel
    {
        public Guid ItemId { get; set; }          // The ID of the selected item (from the dropdown)
        public int Quantity { get; set; }         // The quantity of the product to be consumed
        public double? UnitPrice { get; set; }
    }
}

namespace DevSkill.Inventory.Web.Areas.Admin.Models.StockConsumption_Model
{
    public class StockConsumedDetailsModel
    {
        public string ItemName { get; set; }  // Name of the item/product
        public double Quantity { get; set; }  // Quantity of the product consumed
        public double? UnitPrice { get; set; }
    }
}

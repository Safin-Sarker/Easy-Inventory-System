namespace DevSkill.Inventory.Web.Areas.Admin.Models.StockProduction
{
    public class StockProducedCreateModel
    {
        public Guid ItemId { get; set; }          // The ID of the selected item (from the dropdown)
        public int Quantity { get; set; }         // The quantity of the product to be consumed
        public double? UnitPrice { get; set; }
    }
}

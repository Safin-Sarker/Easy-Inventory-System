namespace DevSkill.Inventory.Web.Areas.Admin.Models
{
    public class ItemDetailsModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? ProductCode { get; set; }
        public string? ProductCategory { get; set; }
        public string? Description { get; set; }
        public bool InventoryTracking { get; set; }
        public string? UnitOfMeasure { get; set; }
        public List<WarehouseDataModel>? Warehouses { get; set; }
    }
}

namespace DevSkill.Inventory.Web.Areas.Admin.Models
{
    public class WarehouseDataModel
    {
        public Guid Id { get; set; }
        public string? Name { get; set; } 
        public bool IsSelected { get; set; } 
        public int? Quantity { get; set; } 
        public double? UnitPrice { get; set; }
    }
}

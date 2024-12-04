using System.ComponentModel.DataAnnotations;

namespace DevSkill.Inventory.Web.Areas.Admin.Models.Warehouse_Models
{
    public class WarehouseUpdateModel
    {
        public Guid Id { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; }

        public string? Manager { get; set; }

        public string? Location { get; set; }

        public string? ContactNumber { get; set; }
    }
}

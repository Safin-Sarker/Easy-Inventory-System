using DevSkill.Inventory.Domain;
using System.Data;

namespace DevSkill.Inventory.Web.Areas.Admin.Models.Warehouse_Models
{
    public class WarehouseListModel : DataTables
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string? Manager { get; set; }

        public string? City { get; set; }

        public string? Phone { get; set; }
    }
}

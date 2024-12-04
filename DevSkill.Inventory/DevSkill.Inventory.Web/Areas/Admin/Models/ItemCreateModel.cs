using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Infrastructure;
using DevSkill.Inventory.Web.Areas.Admin.Models.Warehouse_Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace DevSkill.Inventory.Web.Areas.Admin.Models
{
	public class ItemCreateModel
	{
        [Required(ErrorMessage = "The Item Type field is required.")]
        public string ItemType { get; set; }

        [Required(ErrorMessage = "The Name field is required.")]
        public string Name { get; set; }

        public string? Description { get; set; }

        public string? ProductCode { get; set; }

        public string? UnitOfMeasure { get; set; }

        public string? Category { get; set; }

        public IFormFile? Image { get; set; }

        public bool TrackInventory { get; set; }

        public int? OpeningStock { get; set; }

        public int? ReorderLevel { get; set; }

        // List of available warehouses to display in the modal
        public List<WarehouseDataModel>? Warehouses { get; set; }

    }
}

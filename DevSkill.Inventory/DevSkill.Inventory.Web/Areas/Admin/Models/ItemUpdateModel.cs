using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Infrastructure;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace DevSkill.Inventory.Web.Areas.Admin.Models
{
    public class ItemUpdateModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string? ProductCode { get; set; }

        public string? Description { get; set; }

        public string? UnitOfMeasure { get; set; }

        public string? Category { get; set; }

        public string ItemType { get; set; }

        public bool TrackInventory { get; set; }

        public int? OpeningStock { get; set; }

        public int? ReorderLevel { get; set; }

        // Dropdown list for item types and categories

        public IList<string> ItemTypeList { get; set; } = new List<string>();

        public IList<string> CategoryList { get; set; } = new List<string>();

        // Existing image path for displaying the current image
        public string? ExistingImagePath { get; set; }

        // New image for updating the current image
        public IFormFile? NewImage { get; set; }
    }
}

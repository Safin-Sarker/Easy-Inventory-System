using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Dtos;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Infrastructure;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;

namespace DevSkill.Inventory.Web.Areas.Admin.Models
{
    public class ItemListModel :DataTables
    {
        public ItemSearchDto? SearchItem { get; set; }

        // Add these properties for dropdown lists
        public IList<string> ItemType { get; set; } = new List<string>();
        public IList<string> Category { get; set; } = new List<string>();



    }
}

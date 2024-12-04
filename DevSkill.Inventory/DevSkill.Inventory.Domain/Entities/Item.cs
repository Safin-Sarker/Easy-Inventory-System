using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Domain.Entities
{
    public class Item : IEntity<Guid>
    {
        public Guid Id { get; set; }

        // Product details
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? ProductCode { get; set; }
        public string? UnitOfMeasure { get; set; }
        public string? Category { get; set; } // Product category (e.g., Electronics, Apparel)
        public string ItemType { get; set; } // Can be 'Product' or 'Service'
        public string? Image { get; set; }


        // Inventory management fields
        public bool? TrackInventory { get; set; } // Indicates if inventory tracking is enabled
        public int? OpeningStock { get; set; } // Initial stock quantity when product is created
        public int? ReorderLevel { get; set; }
      
        // Navigation property to associate with Stock
        public ICollection<Stock>? Stocks { get; set; }
        public ICollection<StockTransferItem> StockTransferItems { get; set; } = new List<StockTransferItem>();

        public Item()
        {
            // Initialize the Stocks collection to avoid null references
            Stocks = new List<Stock>();
        }

    }
}

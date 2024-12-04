using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Domain.Entities
{
    public class StockProduced : IEntity<Guid>
    {
        public Guid Id { get; set; }
        public Guid StockProductionId { get; set; }  // Foreign key to StockProduction
        public Guid ItemId { get; set; }  // Foreign key to Item
        public int Quantity { get; set; }
        public double? UnitPrice { get; set; }

        // Navigation Properties
        public StockProduction StockProduction { get; set; }  // Not stored in the database as a column
        public Item Item { get; set; }
    }
}

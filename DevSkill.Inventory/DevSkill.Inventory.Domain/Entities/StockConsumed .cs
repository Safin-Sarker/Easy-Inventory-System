using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Domain.Entities
{
    public class StockConsumed : IEntity<Guid>
    {
        public Guid Id { get; set; }
        public Guid StockConsumptionId { get; set; }  // Foreign key to StockConsumption
        public Guid ItemId { get; set; }  // Foreign key to Item
        public int Quantity { get; set; }
        public double? UnitPrice { get; set; }

        // Navigation Properties
        public StockConsumption StockConsumption { get; set; }  // This will not create a column in the database
        public Item Item { get; set; }  //
    }
}

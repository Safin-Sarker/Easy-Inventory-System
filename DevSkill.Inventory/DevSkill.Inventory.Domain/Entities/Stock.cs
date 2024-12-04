using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Domain.Entities
{
    public class Stock : IEntity<Guid>
    {
        public Guid Id { get; set; }
        public Guid ItemId { get; set; }
        public Guid WarehouseId { get; set; }
        public int Quantity { get; set; }
        public double UnitPrice { get; set; }
// Navigation Properties
        public Item? Item { get; set; }
        public Warehouse? Warehouse { get; set; }
        
    }
}

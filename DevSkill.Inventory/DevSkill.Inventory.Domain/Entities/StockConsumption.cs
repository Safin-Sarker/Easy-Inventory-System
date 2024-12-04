using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Domain.Entities
{
    public class StockConsumption : IEntity<Guid>
    {
        public Guid Id { get; set; }
        public string VoucherNumber { get; set; }
        public DateTime VoucherDate { get; set; }
        public Guid WarehouseId { get; set; }  // Foreign key to Warehouse
        public string CreatedBy { get; set; }
        public string? Details { get; set; }

        // Navigation Property
        public Warehouse? Warehouse { get; set; }  

        // Navigation Property
        public ICollection<StockConsumed> StockConsumeds { get; set; } = new List<StockConsumed>(); 

    }
}

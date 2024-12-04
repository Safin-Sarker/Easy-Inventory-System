using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Domain.Entities
{
    public class StockTransfer : IEntity<Guid>
    {
        public Guid Id { get; set; }  // Primary Key
        public string VoucherNumber { get; set; }  // Unique identifier for the transfer
        public DateTime VoucherDate { get; set; }
        public Guid SourceWarehouseId { get; set; }  // Foreign Key to Warehouse (source)
        public string? Details { get; set; }
        public string CreatedBy { get; set; }

        // Navigation properties
        public Warehouse SourceWarehouse { get; set; }
        public ICollection<StockTransferItem> TransferItems { get; set; }  // Coll
    }
}

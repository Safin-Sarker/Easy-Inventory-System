using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Domain.Entities
{
    public class StockTransferItem :IEntity<Guid>   
    {
        public Guid Id { get; set; }  // Primary Key
        public Guid StockTransferId { get; set; }  // Foreign Key to StockTransfer
        public Guid ProductId { get; set; }  // Foreign Key to Product
        public int Quantity { get; set; }
        public Guid DestinationWarehouseId { get; set; }  // Foreign Key to Warehouse (destination)

        // Navigation properties
        public StockTransfer StockTransfer { get; set; }
        public Item Product { get; set; }
        public Warehouse DestinationWarehouse { get; set; }
    }
}

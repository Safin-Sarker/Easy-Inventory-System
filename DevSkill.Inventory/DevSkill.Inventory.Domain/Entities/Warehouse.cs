using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Domain.Entities
{
    public class Warehouse :IEntity<Guid>
    {
        public  Guid Id { get; set; }

        public string Name { get; set; }

        public string? Manager { get; set; }

        public string? city { get; set; }

        public string? Phone { get; set; }

        // Navigation Properties
        public ICollection<Stock> Stocks { get; set; }  // Stock items in this warehouse
        public ICollection<StockTransfer> SourceTransfers { get; set; }  // Transfers originating from this warehouse
        public ICollection<StockTransferItem> DestinationTransfers { get; set; }  
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Domain.Dtos
{
    public class ItemSearchDto
    {
        public string? Name { get; set; }
        public string? ProductCode { get; set; }
        public string? ItemType { get; set; }
        public string? Category { get; set; }
        public int? Quantity { get; set; }
        public string? UnitOfMeasure { get; set; }

    }
}

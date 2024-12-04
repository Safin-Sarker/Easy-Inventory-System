using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Domain.Dtos
{
    public class ItemDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? ProductCode { get; set; }
        public string ItemType { get; set; }
        public string? Category { get; set; }
        public int? OpeningStock { get; set; }
        public string? UnitOfMeasure { get; set; }
        public string? Image { get; set; }

    }
}

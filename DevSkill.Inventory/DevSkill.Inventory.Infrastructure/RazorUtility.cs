using DevSkill.Inventory.Domain.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Infrastructure
{
    public class RazorUtility
    {

        public static IList<SelectListItem> ConvertWarehouses(IList<Warehouse> warhouses)
        {
            var items = warhouses.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            }).ToList();

           

            return items;
        }
    }
}

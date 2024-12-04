using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain.RepositoryContracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Infrastructure.Repositories
{
    public class ItemRepository : Repository<Item, Guid>, IItemRepository
    {
        private readonly InventoryDbContext _context;

        public ItemRepository(InventoryDbContext context) : base(context)
        {
            _context=context;
        }

        public async Task<(IList<Item> data, int total, int totalDisplay)> GetPagedItemsAsync(int pageIndex, int pageSize, DataTablesSearch search, string? order)
        {
            if (string.IsNullOrWhiteSpace(search.Value))
                return await GetDynamicAsync(null, order, null,
                    pageIndex, pageSize, true);
            else
                return await GetDynamicAsync(x => x.Name.Contains(search.Value), order,
                  null, pageIndex, pageSize, true);
        }

        public async Task<Item> GetItemAsync(Guid id)
        {
            return (await GetAsync(x => x.Id == id, include: null)).FirstOrDefault();
        }

        public async Task<bool> IsTitleDuplicateAsync(string title, Guid? id = null)
        {
            if(id.HasValue)
            {
                return await GetCountAsync(x => x.Id != id.Value && x.Name == title) > 0;
            }
            else
            {
                return await GetCountAsync(x => x.Name == title) > 0;
            }
        }

        public async Task<int> GetTotalProductCountAsync()
        {
            return await _context.Items.CountAsync(i => i.ItemType == "Product");
        }

        public async Task<int> GetTotalServiceCountAsync()
        {
            return await _context.Items.CountAsync(i => i.ItemType == "Service");
        }


    }
}

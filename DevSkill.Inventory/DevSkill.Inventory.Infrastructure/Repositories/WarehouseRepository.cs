using DevSkill.Inventory.Application.Services;
using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Dtos;
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
    public class WarehouseRepository : Repository<Warehouse,Guid>,IWarehouseRepository
    {
        private InventoryDbContext _context;

        public WarehouseRepository(InventoryDbContext context): base (context)
        {
            _context = context;
            
        }
        public async Task<(IList<Warehouse> data, int total, int totalDisplay)>
            GetPagedWarehousesAsync(int pageIndex, int pageSize, DataTablesSearch search, string? order)
        {
            if (string.IsNullOrWhiteSpace(search.Value))
                return await GetDynamicAsync(null, order,null,
                    pageIndex, pageSize, true);
            else
                return await GetDynamicAsync(x => x.Name.Contains(search.Value), order,
                  null, pageIndex, pageSize, true);
        }



        public async Task<bool> IsTitleDuplicateAsync(string name, Guid id)
        {
            return await GetCountAsync(x => x.Id != id && x.Name == name) > 0;
        }

        public async Task<IEnumerable<Warehouse>> GetAllExceptAsync(Expression<Func<Warehouse, bool>> predicate)
        {
            return await _context.Warehouses.Where(predicate).ToListAsync();
        }

        public async Task<int> GetTotalWarehouseasync()
        {
            return await _context.Warehouses.CountAsync();
        }

        public async Task<List<Warehouse>> GetWarehousesByItemIdAsync(Guid itemId)
        {
            return await _context.Warehouses
                                 .Where(w => w.Stocks.Any(s => s.ItemId == itemId))
                                 .ToListAsync();
        }

        public async Task<List<WarehouseDataDto>> GetWarehousesWithQuantityByItemIdAsync(Guid itemId)
        {
            // Query to get warehouses with total quantity for the specified itemId
            return await _context.Warehouses
                .Where(w => w.Stocks.Any(s => s.ItemId == itemId))
                .Select(w => new WarehouseDataDto
                {
                    Name = w.Name,
                    Quantity = w.Stocks
                        .Where(s => s.ItemId == itemId)
                        .Sum(s => s.Quantity)
                })
                .ToListAsync();
        }


    }
}

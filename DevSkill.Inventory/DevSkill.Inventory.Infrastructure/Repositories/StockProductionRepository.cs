using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;

namespace DevSkill.Inventory.Infrastructure.Repositories
{
    public class StockProductionRepository: Repository<StockProduction, Guid> ,IStockProductionRepository
    {

        private readonly InventoryDbContext _context;

        public StockProductionRepository(InventoryDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<string?> GetLastVoucherAsync()
        {
            return await GetLastVoucherAsync<StockProduction>();
        }

        public async Task<(IList<StockProduction> data, int total, int totalDisplay)> GetPagedProductionsAsync(int pageIndex, int pageSize, DataTablesSearch search, string? order)
        {
            if (string.IsNullOrWhiteSpace(search.Value))
            {
                // Eagerly load the Warehouse entity
                return await GetDynamicAsync(null, order, sc => sc.Include(s => s.Warehouse), pageIndex, pageSize, true);
            }
            else
            {
                // Eagerly load the Warehouse entity
                return await GetDynamicAsync(x => x.VoucherNumber.Contains(search.Value), order, sc => sc.Include(s => s.Warehouse), pageIndex, pageSize, true);
            }
        }

        public async Task<StockProduction> GetIdWithItemAndWarehouseByAsync(Guid id)
        {
            return await _context.StockProductions
                .Include(sc => sc.StockProducedItems)  
                .ThenInclude(sc => sc.Item)        
                .Include(sc => sc.Warehouse)       
                .FirstOrDefaultAsync(sc => sc.Id == id); 
        }


    }
}

using DevSkill.Inventory.Application.Services.StockConsumption;
using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Dtos;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain.RepositoryContracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Infrastructure.Repositories
{
    public class StockconsumptionRepository: Repository<StockConsumption, Guid>, IStockConsumptionRepository
    {
        private readonly InventoryDbContext _context;

        public StockconsumptionRepository(InventoryDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<string?> GetLastVoucherAsync()
        {

            return await GetLastVoucherAsync<StockConsumption>();
        }

        public async Task<(IList<StockConsumption> data, int total, int totalDisplay)> GetPagedStockConsumptionsAsync(int pageIndex, int pageSize, DataTablesSearch search, string? order)
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




        public async Task<IList<Stock>> GetStockDetailsByStockConsumptionIdAsync(Guid stockConsumptionId)
        {
            // Fetch the stock details for a specific StockConsumptionId and include the Item
            return await _context.Stocks
                .Where(s => s.Id == stockConsumptionId)
                .Include(s => s.Item)  // Include the related Item entity
                .ToListAsync();
        }

        public async Task<StockConsumption> GetSingleAsync(Expression<Func<StockConsumption, bool>> filter)
        {
            return await _context.StockConsumptions
                .Where(filter)  // Apply the filter expression
                .FirstOrDefaultAsync();  // Fetch the first matching entity or null if none found
        }

        public Task<IList<StockConsumption>> GetAsync(Expression<Func<StockConsumption, bool>> filter)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> AddStockConsumptionAsync(StockConsumption stockConsumption)
        {
            await _context.StockConsumptions.AddAsync(stockConsumption);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<StockConsumption> GetIdWithItemAndWarehouseByAsync(Guid id)
        {
            return await _context.StockConsumptions
                .Include(sc => sc.StockConsumeds)  // Eagerly load the StockConsumeds collection
                .ThenInclude(sc => sc.Item)        // Optionally load related Item entity if needed
                .Include(sc => sc.Warehouse)       // Eagerly load the related Warehouse entity
                .FirstOrDefaultAsync(sc => sc.Id == id);  // Filter by the StockConsumption ID
        }




    }
}

using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain.RepositoryContracts;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Infrastructure.Repositories
{
    public class StockRepository: Repository<Stock, Guid>, IStockRepository
    {
        private readonly InventoryDbContext _context;

        public StockRepository(InventoryDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Stock> GetStockByItemAndWarehouseAsync(Guid itemId, Guid warehouseId)
        {
            // Use Entity Framework to find the stock for the specified item and warehouse
            return await _context.Stocks
                .FirstOrDefaultAsync(s => s.ItemId == itemId && s.WarehouseId == warehouseId);
        }


        public async Task<IQueryable<Stock>> GetAllSumAsync()
        {
            return _context.Stocks.AsQueryable(); // Assuming _dbSet is your DbSet<Stock>
        }


        public async Task<IEnumerable<Item>> GetItemsByWarehouseAsync(Guid warehouseId)
        {
            return await _context.Stocks
                                 .Include(stock => stock.Item) // Ensure Item is loaded
                                 .Where(stock => stock.WarehouseId == warehouseId && stock.Quantity > 0)
                                 .Select(stock => stock.Item)
                                 .Distinct() // Get distinct items
                                 .ToListAsync();
        }

        public async Task<bool> ExecuteInTransactionAsync(Func<Task> action)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    // Execute the business logic passed as 'action'
                    await action();

                    // Commit the transaction if successful
                    await transaction.CommitAsync();
                    return true;
                }
                catch (Exception)
                {
                    // Rollback the transaction in case of error
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

        public async Task<Stock?> GetStockByWarehouseAndProductAsync(Guid warehouseId, Guid ItemId)
        {
            return await _context.Stocks
                .FirstOrDefaultAsync(s => s.WarehouseId == warehouseId && s.ItemId == ItemId);
        }



    }
}

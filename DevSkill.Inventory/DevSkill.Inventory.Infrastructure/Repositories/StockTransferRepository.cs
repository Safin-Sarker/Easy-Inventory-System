using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain.RepositoryContracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Infrastructure.Repositories
{
    public class StockTransferRepository :Repository<StockTransfer,Guid>,IStockTransferRepository
    {
        private readonly InventoryDbContext _context;

        public StockTransferRepository(InventoryDbContext context) : base(context)
        {
            _context = context;
        }


        public async Task<(IList<StockTransfer> data, int total, int totalDisplay)> GetPagedStockTransfersAsync(
          int pageIndex, int pageSize, DataTablesSearch search, string? order)
        {
            if (string.IsNullOrWhiteSpace(search.Value))
            {
                // Eagerly load the Warehouse entity
                return await GetDynamicAsync(
                    null,
                    order,
                    st => st.Include(s => s.SourceWarehouse), // Adjust this if Warehouse navigation property differs in StockTransfer
                    pageIndex,
                    pageSize,
                    true
                );
            }
            else
            {
                // Eagerly load the Warehouse entity with a filter on VoucherNumber or other properties in StockTransfer
                return await GetDynamicAsync(
                    x => x.VoucherNumber.Contains(search.Value),
                    order,
                    st => st.Include(s => s.SourceWarehouse),
                    pageIndex,
                    pageSize,
                    true
                );
            }
        }
        public async Task<string?> GetLastVoucherAsync()
        {
            return await GetLastVoucherAsync<StockTransfer>();
        }

        public async Task<StockTransfer> GetStockTransferWithDetailsAsync(Guid id)
        {
            return await _context.StockTransfers
            .Include(st => st.SourceWarehouse)
           .Include(st => st.TransferItems)
               .ThenInclude(item => item.Product)
           .Include(st => st.TransferItems)
               .ThenInclude(item => item.DestinationWarehouse)
           .FirstOrDefaultAsync(st => st.Id == id);
        }


        public async Task<StockTransfer?> GetByIdAsync(Guid id)
        {
            return await _context.StockTransfers
                .Include(st => st.SourceWarehouse) // Include related SourceWarehouse details
                .Include(st => st.TransferItems)   // Include TransferItems collection
                    .ThenInclude(item => item.Product) // Include related Product for each item
                .Include(st => st.TransferItems)
                    .ThenInclude(item => item.DestinationWarehouse) // Include DestinationWarehouse for each item
                .FirstOrDefaultAsync(st => st.Id == id);
        }

        public async Task<IEnumerable<StockTransferItem>> GetByProductIdAsync(Guid productId)
        {
            return await _context.Set<StockTransferItem>()
                .Where(sti => sti.ProductId == productId)
                .ToListAsync();
        }

        public void RemoveRange(IEnumerable<StockTransferItem> entities)
        {
            _context.Set<StockTransferItem>().RemoveRange(entities);
        }




    }
}

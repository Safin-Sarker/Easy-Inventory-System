using DevSkill.Inventory.Application;
using DevSkill.Inventory.Domain.Dtos;
using DevSkill.Inventory.Domain.RepositoryContracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Infrastructure.UnitOfWorks
{
    public class InventoryUnitOfWork : UnitOfWork,IInventoryUnitOfWork
    {
        private readonly InventoryDbContext _dbContext;
        private IDbContextTransaction _currentTransaction;

        public IItemRepository ItemRepository {  get; private set; }
        public IWarehouseRepository WarehouseRepository { get; private set; }
        public IStockRepository StockRepository { get; private set; }
        public IStockConsumptionRepository StockConsumptionRepository { get; private set; }
        public IStockProductionRepository StockProductionRepository { get; private set; }
        public IStockTransferRepository StockTransferRepository { get; private set; }

        public InventoryUnitOfWork(IItemRepository itemRepository,
            IWarehouseRepository warehouseRepository,
            IStockRepository stockRepository,
            IStockConsumptionRepository stockConsumptionRepository,
            IStockProductionRepository stockProductionRepository,
            IStockTransferRepository stockTransferRepository,
            InventoryDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
            ItemRepository = itemRepository;
            WarehouseRepository = warehouseRepository;
            StockRepository = stockRepository;
            StockConsumptionRepository = stockConsumptionRepository;
            StockProductionRepository = stockProductionRepository;
            StockTransferRepository = stockTransferRepository;
        }

        public async Task<(IList<ItemDto> data, int total, int totalDisplay)> GetPagedItemsUsingSPAsync(int pageIndex, int pageSize, ItemSearchDto search, string? order)
        {
            var procedureName = "GetItemsWithPagination"; // Or your simplified stored procedure name, e.g., "GetItemsWithPagination"

            // Call the stored procedure and get the data and output values
            var (data, outValues) = await SqlUtility.QueryWithStoredProcedureAsync<ItemDto>(
                procedureName,
                new Dictionary<string, object>
                {
                    { "PageIndex", pageIndex },
                    { "PageSize", pageSize },
                    { "OrderBy", order ?? "Name" },
                    { "Name", string.IsNullOrEmpty(search.Name) ? "%" : "%" + search.Name + "%" },
                    { "ProductCode", string.IsNullOrEmpty(search.ProductCode) ? "%" : "%" + search.ProductCode + "%" },
                    { "ItemType", string.IsNullOrEmpty(search.ItemType) ? "%" : search.ItemType },
                    { "Category", string.IsNullOrEmpty(search.Category) ? "%" : search.Category }
                },
                new Dictionary<string, Type>
                {
                    { "Total", typeof(int) },
                    { "TotalDisplay", typeof(int) }
                });


            // Return only the necessary data and pagination values
            return (data, (int)outValues["Total"], (int)outValues["TotalDisplay"]);
        }





    }
}


using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Services.StockProduction
{
    public class StockProductionManagementService:IStockProductionManagementService
    {
        private readonly IInventoryUnitOfWork _inventoryUnitOfWork;

        public StockProductionManagementService(IInventoryUnitOfWork inventoryUnitOfWork)
        {
            _inventoryUnitOfWork = inventoryUnitOfWork;
        }


        public async Task<(IList<Domain.Entities.StockProduction> data, int total, int totalDisplay)> GetStockProductionsAsync(int pageIndex, int pageSize, DataTablesSearch search, string? order)
        {
            return await _inventoryUnitOfWork.StockProductionRepository.GetPagedProductionsAsync(pageIndex, pageSize, search, order);
        }


        public async Task<string> GetVoucherNumberAsync()
        {
            // Fetch the last voucher number (it should return an integer part, like 5)
            var lastVoucher = await _inventoryUnitOfWork.StockProductionRepository.GetLastVoucherAsync();

            return VoucherNumberGeneratorUtility.GenerateNextVoucherNumber(lastVoucher); ;
        }

       

        public async Task AddStockProductionAsync(Domain.Entities.StockProduction stockProduction)
        {

            await _inventoryUnitOfWork.StockProductionRepository.AddAsync(stockProduction);
            await _inventoryUnitOfWork.SaveAsync();
        }


        public async Task<bool> CreateStockProductionAsync(Domain.Entities.StockProduction stockProduction)
        {
            var updatedItemIds = new HashSet<Guid>();

            // Use the repository's transaction method to wrap the business logic
            bool stockAdjustmentResult = await _inventoryUnitOfWork.StockRepository.ExecuteInTransactionAsync(async () =>
            {
                // Business logic: Adjust stock and create StockProduction4
                foreach (var stockProduced in stockProduction.StockProducedItems)
                {
                    // Fetch current stock from the Stock table
                    var currentStock = await _inventoryUnitOfWork.StockRepository.GetStockByItemAndWarehouseAsync(stockProduced.ItemId, stockProduction.WarehouseId);

                    // If stock does not exist, initialize it
                    if (currentStock == null)
                    {
                        // Create a new stock entry for this item and warehouse
                        currentStock = new Domain.Entities.Stock
                        {
                            ItemId = stockProduced.ItemId,
                            WarehouseId = stockProduction.WarehouseId,
                            Quantity = stockProduced.Quantity // Initialize with produced quantity
                        };
                        await _inventoryUnitOfWork.StockRepository.AddAsync(currentStock);
                    }
                    else 
                    {
                        // Increase the stock quantity for existing stock
                        currentStock.Quantity += stockProduced.Quantity;

                        // Update the stock in the Stock table
                        await _inventoryUnitOfWork.StockRepository.EditAsync(currentStock);
                    }

                    updatedItemIds.Add(stockProduced.ItemId);
                }

                await _inventoryUnitOfWork.SaveAsync();
            });

            if (stockAdjustmentResult)
            {
                foreach (var itemId in updatedItemIds)
                {
                    // Fetch the item from the Item table
                    var item = await _inventoryUnitOfWork.ItemRepository.GetByIdAsync(itemId);

                    if (item != null && item.TrackInventory == true)
                    {
                        // Get the total stock for the item across all warehouses
                        var stocksForItem = await _inventoryUnitOfWork.StockRepository.GetAllSumAsync();

                        var totalStock = stocksForItem
                            .Where(s => s.ItemId == itemId)
                            .Sum(s => s.Quantity);

                        // Update the OpeningStock for the item
                        item.OpeningStock = totalStock;

                        // Save changes to the Item table
                        await _inventoryUnitOfWork.ItemRepository.EditAsync(item);

                        await _inventoryUnitOfWork.SaveAsync();
                    }
                }

            }

            return stockAdjustmentResult;
        }




        public async Task<Domain.Entities.StockProduction> GetIdByAsync(Guid id)
        {
            return await _inventoryUnitOfWork.StockProductionRepository.GetIdWithItemAndWarehouseByAsync(id);
        }

        public async Task DeleteItemAsync(Guid id)
        {
            await _inventoryUnitOfWork.StockProductionRepository.RemoveAsync(id);
            await _inventoryUnitOfWork.SaveAsync();
        }



    }
}

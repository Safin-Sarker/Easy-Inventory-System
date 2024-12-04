using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Dtos;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain.RepositoryContracts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Services
{
    public class StockManagementService : IStockManagementService
    {
        private readonly IInventoryUnitOfWork _inventoryUnitOfWork;

        public StockManagementService(IInventoryUnitOfWork inventoryUnitOfWork)
        {
            _inventoryUnitOfWork = inventoryUnitOfWork;
        }

        public async Task CreateStockAsync(IEnumerable<Stock> stock)
        {
            await _inventoryUnitOfWork.StockRepository.AddRangeAsync(stock);
            await _inventoryUnitOfWork.SaveAsync();
        }

        //public async Task<bool> AddStockConsumptionAsync(List<Stock> stocks)
        //{
        //    await _inventoryUnitOfWork.BeginTransactionAsync();
        //    try
        //    {
        //        foreach (var stock in stocks)
        //        {
        //            // Fetch the current stock for the item and warehouse
        //            var currentStock = await _inventoryUnitOfWork.StockRepository.GetStockByItemAndWarehouseAsync(stock.ItemId, stock.WarehouseId);

        //            if (currentStock == null)
        //            {
        //                throw new InvalidOperationException($"Stock does not exist for item {stock.ItemId} in warehouse {stock.WarehouseId}.");
        //            }

        //            // Check if there is sufficient stock
        //            if (currentStock.Quantity < stock.Quantity)
        //            {
        //                throw new InvalidOperationException($"Insufficient stock for item {stock.ItemId} in warehouse {stock.WarehouseId}.");
        //            }

        //            // Reduce the stock quantity
        //            currentStock.Quantity -= stock.Quantity;

        //            if (currentStock.Quantity < 0)
        //            {
        //                throw new InvalidOperationException($"Stock quantity cannot be negative for item {stock.ItemId}.");
        //            }

        //            // Update the stock record
        //            _inventoryUnitOfWork.StockRepository.Edit(currentStock);

        //            // Track the inventory in the Item entity if necessary
        //            var item = await _inventoryUnitOfWork.ItemRepository.GetByIdAsync(stock.ItemId);

        //            if (item != null && item.TrackInventory == true)
        //            {
        //                var stocksForItem = await _inventoryUnitOfWork.StockRepository.GetAllSumAsync();
        //                var totalStock = stocksForItem
        //                    .Where(s => s.ItemId == stock.ItemId)
        //                    .Sum(s => s.Quantity);

        //                item.OpeningStock = totalStock;
        //                _inventoryUnitOfWork.ItemRepository.Edit(item);
        //            }
        //        }

        //        // Delegate the actual save operation to the UnitOfWork
        //        await _inventoryUnitOfWork.SaveAsync();

        //        await _inventoryUnitOfWork.CommitTransactionAsync();

        //        return true;

        //    }

        //    catch (Exception ex)
        //    {
        //        // Rollback the transaction if an error occurs
        //        await _inventoryUnitOfWork.RollbackTransactionAsync();
        //        throw new InvalidOperationException("Error during stock consumption: " + ex.Message, ex);
        //    }



        //}

        public async Task<IEnumerable<SimpleItemDto>> GetItemsForWarehouseAsync(Guid warehouseId)
        {
            var itemsInWarehouse = await _inventoryUnitOfWork.StockRepository.GetItemsByWarehouseAsync(warehouseId);

            var result = itemsInWarehouse.Select(item => new SimpleItemDto
            {
                Id = item.Id,
                Name = item.Name
            });

            return result;
        }

        public async Task<IList<Stock>> GetAllStockAsync()
        {
            return await _inventoryUnitOfWork.StockRepository.GetAllAsync();
        }

        public async Task<Stock> GetStockByItemAndWarehouseAsync(Guid itemId, Guid warehouseId)
        {
            // Call the repository method to fetch the stock
            return await _inventoryUnitOfWork.StockRepository.GetStockByItemAndWarehouseAsync(itemId, warehouseId);


        }

        public async Task UpdateStockAsync(Stock stock)
        {
            _inventoryUnitOfWork.StockRepository.Edit(stock);
            await _inventoryUnitOfWork.SaveAsync();

        }
    }


}



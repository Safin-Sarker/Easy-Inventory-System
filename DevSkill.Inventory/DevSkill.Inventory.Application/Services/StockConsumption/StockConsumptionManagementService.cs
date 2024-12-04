using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Dtos;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DevSkill.Inventory.Application.Services.StockConsumption
{
    public class StockConsumptionManagementService:IStockConsumptionManagementService
    {
        private readonly IInventoryUnitOfWork _inventoryUnitOfWork;

        public StockConsumptionManagementService(IInventoryUnitOfWork inventoryUnitOfWork)
        {
            _inventoryUnitOfWork = inventoryUnitOfWork;
        }




        public async Task AddStockConsumptionAsync( Domain.Entities.StockConsumption stockConsumption)
        {
          
                await _inventoryUnitOfWork.StockConsumptionRepository.AddAsync(stockConsumption);
                await _inventoryUnitOfWork.SaveAsync();
        }


        public async Task<string> GetVoucherNumberAsync()
        {
            // Fetch the last voucher number (it should return an integer part, like 5)
            var lastVoucher = await _inventoryUnitOfWork.StockConsumptionRepository.GetLastVoucherAsync();

            return VoucherNumberGeneratorUtility.GenerateNextVoucherNumber(lastVoucher); ;
        }


        public async Task<(IList<Domain.Entities.StockConsumption> data, int total, int totalDisplay)> GetStockConsumptionsAsync(int pageIndex, int pageSize, DataTablesSearch search, string? order)
        {
            return await _inventoryUnitOfWork.StockConsumptionRepository.GetPagedStockConsumptionsAsync(pageIndex, pageSize, search, order);
        }



        public async Task<bool> CreateStockConsumptionAsync(Domain.Entities.StockConsumption stockConsumption)
        { 
            var updatedItemIds = new HashSet<Guid>();

            // Use the repository's transaction method to wrap the business logic
            bool stockAdjustmentResult=await _inventoryUnitOfWork.StockRepository.ExecuteInTransactionAsync(async () =>
            {
                // Business logic: Adjust stock and create StockConsumption

                foreach (var stockConsumed in stockConsumption.StockConsumeds)
                {
                    // Fetch current stock from the Stock table
                    var currentStock = await _inventoryUnitOfWork.StockRepository.GetStockByItemAndWarehouseAsync(stockConsumed.ItemId, stockConsumption.WarehouseId);

                    if (currentStock == null)
                    {
                        throw new InvalidOperationException("Stock does not exist for item");
                    }

                    // Check if there is sufficient stock
                    if (currentStock.Quantity < stockConsumed.Quantity)
                    {
                        throw new InvalidOperationException($"Insufficient stock for item {currentStock.Item.Name}. Only {currentStock.Quantity} available.");
                    }

                    // Adjust the stock quantity
                    currentStock.Quantity -= stockConsumed.Quantity;

                    // Update the stock in the Stock table
                    await _inventoryUnitOfWork.StockRepository.EditAsync(currentStock);

                    updatedItemIds.Add(stockConsumed.ItemId);

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


        public async Task<Domain.Entities.StockConsumption> GetIdByAsync(Guid id)
        {
           return await _inventoryUnitOfWork.StockConsumptionRepository.GetIdWithItemAndWarehouseByAsync(id);
        }


        public async Task DeleteItemAsync(Guid id)
        {
            await _inventoryUnitOfWork.StockConsumptionRepository.RemoveAsync(id);
            await _inventoryUnitOfWork.SaveAsync();
        }





    }
}

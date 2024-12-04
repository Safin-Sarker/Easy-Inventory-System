using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Services.StockTransferManagementService
{
    public class StockTransferManagementService:IStockTransferManagementService
    {
        private readonly IInventoryUnitOfWork _inventoryUnitOfWork;

        public StockTransferManagementService(IInventoryUnitOfWork inventoryUnitOfWork)
        {
            _inventoryUnitOfWork = inventoryUnitOfWork;
        }

        public async Task<(IList<Domain.Entities.StockTransfer> data, int total, int totalDisplay)> GetStockTransfersAsync(int pageIndex, int pageSize, DataTablesSearch search, string? order)
        {
            return await _inventoryUnitOfWork.StockTransferRepository.GetPagedStockTransfersAsync(pageIndex, pageSize, search, order);
        }

        public async Task<string> GetVoucherNumberAsync()
        {
            var lastVoucher = await _inventoryUnitOfWork.StockTransferRepository.GetLastVoucherAsync();

            return VoucherNumberGeneratorUtility.GenerateNextVoucherNumber(lastVoucher); 
        }

        public async Task<bool> StockTransferAsync(StockTransfer stockTransfer)
        {
      

            // Use the repository's transaction method to wrap the business logic
            var stockAdjustmentResult =await _inventoryUnitOfWork.StockRepository.ExecuteInTransactionAsync(async () =>
            {
                foreach (var transferItem in stockTransfer.TransferItems)
                {
                    // Fetch current stock for the product in the source warehouse
                    var sourceStock = await _inventoryUnitOfWork.StockRepository.GetStockByWarehouseAndProductAsync(stockTransfer.SourceWarehouseId, transferItem.ProductId);
                    if (sourceStock == null || sourceStock.Quantity < transferItem.Quantity)
                    {
                        throw new InvalidOperationException($"Insufficient stock in source warehouse for product ID {transferItem.ProductId}.");
                    }

                    // Deduct quantity from the source warehouse stock
                    sourceStock.Quantity -= transferItem.Quantity;
                    await _inventoryUnitOfWork.StockRepository.EditAsync(sourceStock);

                    // Fetch or create stock entry for the product in the destination warehouse
                    var destinationStock = await _inventoryUnitOfWork.StockRepository.GetStockByWarehouseAndProductAsync(transferItem.DestinationWarehouseId, transferItem.ProductId);
                    if (destinationStock == null)
                    {
                        // Create a new stock entry in the destination warehouse if none exists
                        destinationStock = new Stock
                        {
                            Id = Guid.NewGuid(),
                            WarehouseId = transferItem.DestinationWarehouseId,
                            ItemId = transferItem.ProductId,
                            Quantity = transferItem.Quantity,
                        
                        };
                        await _inventoryUnitOfWork.StockRepository.AddAsync(destinationStock);
                    }
                    else
                    {
                        destinationStock.Quantity += transferItem.Quantity;
                        await _inventoryUnitOfWork.StockRepository.EditAsync(destinationStock);
                    }
                }
                await _inventoryUnitOfWork.SaveAsync();
            });

            return stockAdjustmentResult;
        }


        public async Task CreateStockTransferAsync(StockTransfer stockTransfer)
        {

            await _inventoryUnitOfWork.StockTransferRepository.AddAsync(stockTransfer);
            await _inventoryUnitOfWork.SaveAsync();
        }

        public async Task<StockTransfer> GetStockTransferByIdAsync(Guid id)
        {
            return await _inventoryUnitOfWork.StockTransferRepository.GetStockTransferWithDetailsAsync(id);
        }

        public async Task DeleteItemAsync(Guid id)
        {
            await _inventoryUnitOfWork.StockTransferRepository.RemoveAsync(id);
            await _inventoryUnitOfWork.SaveAsync();
        }

        public async Task<StockTransfer?> GetByIdAsync(Guid id)
        {
            return await _inventoryUnitOfWork.StockTransferRepository.GetByIdAsync(id);
        }





    }
}

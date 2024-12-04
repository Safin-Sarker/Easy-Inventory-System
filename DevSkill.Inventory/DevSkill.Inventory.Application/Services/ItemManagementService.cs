using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Dtos;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Services
{
    public class ItemManagementService : IItemManagementService
    {
        private readonly IInventoryUnitOfWork _inventoryUnitOfWork;
        private readonly IStockTransferRepository _stockTransferRepository;

        public ItemManagementService(IInventoryUnitOfWork inventoryUnitOfWork, IStockTransferRepository stockTransferRepository)
        {
            _inventoryUnitOfWork = inventoryUnitOfWork;
            _stockTransferRepository = stockTransferRepository;
        }

        public async Task CreateItemAsync(Item item)
        {
            if (!await _inventoryUnitOfWork.ItemRepository.IsTitleDuplicateAsync(item.Name))
            {
                await _inventoryUnitOfWork.ItemRepository.AddAsync(item);
                await _inventoryUnitOfWork.SaveAsync();

            }

            else
            {
                throw new InvalidOperationException("There is already a Item with this Title");
            }


        }

        public async Task DeleteItemAsync(Guid id)
        {
            // Fetch related records in StockTransferItems
            var relatedRecords = await _stockTransferRepository.GetByProductIdAsync(id);

            if (relatedRecords.Any())
            {
                // Remove related records first
                _stockTransferRepository.RemoveRange(relatedRecords);
            }

            // Remove the item itself
            await _inventoryUnitOfWork.ItemRepository.RemoveAsync(id);

            // Save all changes in a single transaction
            await _inventoryUnitOfWork.SaveAsync();
        }


        public async Task<IList<Item>> GetAllAsync()
        {
            var allItems = await _inventoryUnitOfWork.ItemRepository.GetAllAsync();
            // Filter items where TrackInventory is true
            var filteredItems = allItems.Where(item => item.TrackInventory == true).ToList();
            return filteredItems;
        }

        public async Task<IList<string>> GetDistinctCategoriesAsync()
        {
            return await _inventoryUnitOfWork.ItemRepository.GetDistinctAsync(item => item.Category);
        }

        public async Task<IList<string>> GetDistinctItemTypesAsync()
        {
            return await _inventoryUnitOfWork.ItemRepository.GetDistinctAsync(item => item.ItemType);

        }

        public async Task<Item> GetItemByIdAsync(Guid id)
        {
            return await _inventoryUnitOfWork.ItemRepository.GetItemAsync(id);
        }

        public async Task<(IList<Item> data, int total, int totalDisplay)> GetItemsAsync(int pageIndex, int pageSize, DataTablesSearch search, string? order)
        {
            return await _inventoryUnitOfWork.ItemRepository.GetPagedItemsAsync(pageIndex, pageSize, search, order);
        }

        public async Task<(IList<ItemDto> data, int total, int totalDisplay)> GetItemsAsyncSp(int pageIndex, int pageSize, ItemSearchDto search, string? order)
        {
            return await _inventoryUnitOfWork.GetPagedItemsUsingSPAsync(pageIndex, pageSize, search, order);
        }

        public async Task UpdateItemAsync(Item item)
        {
            if(! await _inventoryUnitOfWork.ItemRepository.IsTitleDuplicateAsync(item.Name, item.Id)) 
            {
                await _inventoryUnitOfWork.ItemRepository.EditAsync(item);
                await _inventoryUnitOfWork.SaveAsync();
            }
            else
            {
                throw new InvalidOperationException("Title should be Unique");
            }
            
        }

        public async Task<int> GetTotalProductasync()
        {
            return await _inventoryUnitOfWork.ItemRepository.GetTotalProductCountAsync();
        }

        public async Task<int> GetTotalServiceasync()
        {
            return await _inventoryUnitOfWork.ItemRepository.GetTotalServiceCountAsync();
        }



    }
}

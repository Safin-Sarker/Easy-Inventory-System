using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Dtos;
using DevSkill.Inventory.Domain.Entities;

namespace DevSkill.Inventory.Application.Services
{
    public interface IItemManagementService
    {
        Task CreateItemAsync(Item item);
        Task DeleteItemAsync(Guid id);

        Task<Item> GetItemByIdAsync(Guid id);
        
        Task<(IList<Item> data, int total, int totalDisplay)> GetItemsAsync(int pageIndex, int pageSize,
            DataTablesSearch search, string? order);

        Task<(IList<ItemDto> data, int total, int totalDisplay)> GetItemsAsyncSp(int pageIndex, int pageSize,
        ItemSearchDto search, string? order);

        Task<IList<Item>> GetAllAsync();

        Task UpdateItemAsync(Item item);

        Task<IList<string>> GetDistinctItemTypesAsync();

        Task<IList<string>> GetDistinctCategoriesAsync();

        Task<int> GetTotalProductasync();

        Task<int> GetTotalServiceasync();
    }
}
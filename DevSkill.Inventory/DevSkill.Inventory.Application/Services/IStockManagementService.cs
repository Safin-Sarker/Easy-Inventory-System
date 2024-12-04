using DevSkill.Inventory.Domain.Dtos;
using DevSkill.Inventory.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Services
{
    public interface IStockManagementService
    {
        public  Task CreateStockAsync(IEnumerable<Stock> stock);

       // public  Task<bool> AddStockConsumptionAsync(List<Stock> stock);

        Task<IEnumerable<SimpleItemDto>> GetItemsForWarehouseAsync(Guid warehouseId);

        Task<IList<Stock>> GetAllStockAsync();

        Task<Stock> GetStockByItemAndWarehouseAsync(Guid itemId, Guid warehouseId);

        Task UpdateStockAsync(Stock stock);


    }
}

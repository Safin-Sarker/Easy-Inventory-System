using DevSkill.Inventory.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Domain.RepositoryContracts
{
    public interface IStockRepository: IRepositoryBase<Stock, Guid>
    {
        Task<Stock> GetStockByItemAndWarehouseAsync(Guid itemId, Guid warehouseId);

        Task<IQueryable<Stock>> GetAllSumAsync();

        Task<IEnumerable<Item>> GetItemsByWarehouseAsync(Guid warehouseId);

        Task<bool> ExecuteInTransactionAsync(Func<Task> action);

        Task<Stock?> GetStockByWarehouseAndProductAsync(Guid warehouseId, Guid ItemId);
    }
}

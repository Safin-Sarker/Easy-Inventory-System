using DevSkill.Inventory.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Domain.RepositoryContracts
{
    public interface IStockProductionRepository: IRepositoryBase<StockProduction, Guid>
    {
        Task<(IList<StockProduction> data, int total, int totalDisplay)> GetPagedProductionsAsync(int pageIndex, int pageSize, DataTablesSearch search, string? order);

        Task<string?> GetLastVoucherAsync();

        Task<StockProduction> GetIdWithItemAndWarehouseByAsync(Guid id);

    }
}

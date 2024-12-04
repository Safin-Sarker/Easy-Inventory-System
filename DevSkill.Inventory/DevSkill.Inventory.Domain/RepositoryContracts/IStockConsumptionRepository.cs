using DevSkill.Inventory.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Domain.RepositoryContracts
{
    public interface IStockConsumptionRepository:IRepositoryBase<StockConsumption,Guid>
    {
        Task<string?> GetLastVoucherAsync();

        Task<(IList<StockConsumption> data, int total, int totalDisplay)> GetPagedStockConsumptionsAsync(int pageIndex, int pageSize, DataTablesSearch search, string? order);

        Task<IList<Stock>> GetStockDetailsByStockConsumptionIdAsync(Guid stockConsumptionId);

        Task<IList<StockConsumption>> GetAsync(Expression<Func<StockConsumption, bool>> filter);

        Task<StockConsumption> GetSingleAsync(Expression<Func<StockConsumption, bool>> filter);

        Task<bool> AddStockConsumptionAsync(StockConsumption stockConsumption);

        Task<StockConsumption> GetIdWithItemAndWarehouseByAsync(Guid id);

    }
}

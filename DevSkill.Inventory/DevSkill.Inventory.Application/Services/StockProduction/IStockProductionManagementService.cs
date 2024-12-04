using DevSkill.Inventory.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Services.StockProduction
{
    public interface IStockProductionManagementService
    {

        Task<(IList<Domain.Entities.StockProduction> data, int total, int totalDisplay)> GetStockProductionsAsync(int pageIndex, int pageSize, DataTablesSearch search, string? order);

        Task<string> GetVoucherNumberAsync();

        Task<bool> CreateStockProductionAsync(Domain.Entities.StockProduction stockProduction);

        Task AddStockProductionAsync(Domain.Entities.StockProduction stockProduction);

        Task<Domain.Entities.StockProduction> GetIdByAsync(Guid id);

        Task DeleteItemAsync(Guid id);
    }


}

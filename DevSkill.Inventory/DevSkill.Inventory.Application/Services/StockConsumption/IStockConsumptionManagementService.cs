using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Dtos;
using DevSkill.Inventory.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;


namespace DevSkill.Inventory.Application.Services.StockConsumption
{
    public interface IStockConsumptionManagementService
    {


        Task AddStockConsumptionAsync(DevSkill.Inventory.Domain.Entities.StockConsumption stockConsumption);

        Task<(IList<Domain.Entities.StockConsumption> data, int total, int totalDisplay)> GetStockConsumptionsAsync(int pageIndex, int pageSize, DataTablesSearch search, string? order);

        Task<bool> CreateStockConsumptionAsync(Domain.Entities.StockConsumption stockConsumption);

        Task<Domain.Entities.StockConsumption> GetIdByAsync(Guid id);

        Task DeleteItemAsync(Guid id);

        Task<string> GetVoucherNumberAsync();











    }
}

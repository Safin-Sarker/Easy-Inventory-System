using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Domain.Dtos;

namespace DevSkill.Inventory.Application.Services
{
    public interface IWarehouseManagementService
    {
        Task CreateWarehouseAsync(Warehouse warehouse);

        void DeleteWarehouse(Guid id);

        Task<Warehouse> GetWarehouseAsync(Guid id);

        Task<IList<WarehouseDto>> GetWarehouseListAsync();

        Task<(IList<Warehouse> data, int total, int totalDisplay)> GetWarehousesAsync(int pageIndex, int pageSize,
          DataTablesSearch search, string? order);

        public  Task UpdateWarehouseAsync(Warehouse warehouse);

        Task<IEnumerable<Warehouse>> GetAllWarehousesExceptAsync(Guid excludeWarehouseId);

        Task<int> GetTotalWarehouseCountAsync();

        Task<List<WarehouseDataDto>> GetWarehouseDataByItemIdAsync(Guid itemId);

    }
}

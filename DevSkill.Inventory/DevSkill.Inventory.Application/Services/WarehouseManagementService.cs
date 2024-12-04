using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Dtos;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Services
{
    public class WarehouseManagementService : IWarehouseManagementService
    {

        private readonly IInventoryUnitOfWork _inventoryUnitOfWork;

        public WarehouseManagementService(IInventoryUnitOfWork inventoryUnitOfWork)
        {
            _inventoryUnitOfWork = inventoryUnitOfWork;
        }

        public async Task CreateWarehouseAsync(Warehouse warehouse)
        {
            if (!await _inventoryUnitOfWork.WarehouseRepository.IsTitleDuplicateAsync(warehouse.Name,warehouse.Id))
            {
                await _inventoryUnitOfWork.WarehouseRepository.AddAsync(warehouse);
                await _inventoryUnitOfWork.SaveAsync();

            }

            else
            {
                throw new InvalidOperationException("There is already a warehouse with this Title");
            }
        }

        public void DeleteWarehouse(Guid id)
        {
            _inventoryUnitOfWork.WarehouseRepository.Remove(id);
            _inventoryUnitOfWork.Save();
        }

        public async Task<IList<WarehouseDto>> GetWarehouseListAsync()
        {
            var warehouses = await _inventoryUnitOfWork.WarehouseRepository.GetAllAsync();

            // Map the domain entities to WarehouseDto
            var warehouseDtos = warehouses.Select(w => new WarehouseDto
            {
                Id = w.Id,
                Name = w.Name,
                Manager = w.Manager,
                City = w.city,
                Phone = w.Phone
            }).ToList();

            return warehouseDtos;
        }


        public async Task<Warehouse> GetWarehouseAsync(Guid id)
        {
            return await _inventoryUnitOfWork.WarehouseRepository.GetByIdAsync(id);

        }

        public async Task<(IList<Warehouse> data, int total, int totalDisplay)> GetWarehousesAsync(int pageIndex, int pageSize, DataTablesSearch search, string? order)
        {
            return await _inventoryUnitOfWork.WarehouseRepository.GetPagedWarehousesAsync(pageIndex, pageSize, search, order);
        }

       

        public async Task UpdateWarehouseAsync(Warehouse warehouse)
        {
            if (!await _inventoryUnitOfWork.WarehouseRepository.IsTitleDuplicateAsync(warehouse.Name, warehouse.Id))
            {
                await _inventoryUnitOfWork.WarehouseRepository.EditAsync(warehouse);
                await _inventoryUnitOfWork.SaveAsync();
            }
            else
            {
                throw new InvalidOperationException("Name should be Unique");
            }
        }

        public async Task<IEnumerable<Warehouse>> GetAllWarehousesExceptAsync(Guid excludeWarehouseId)
        {
            var warehouses = await _inventoryUnitOfWork.WarehouseRepository.GetAllExceptAsync(w => w.Id != excludeWarehouseId);
            return warehouses;
        }

        public async Task<int> GetTotalWarehouseCountAsync()
        {
            return await _inventoryUnitOfWork.WarehouseRepository.GetTotalWarehouseasync();
        }

        public async Task<List<WarehouseDataDto>> GetWarehouseDataByItemIdAsync(Guid itemId)
        {
           
            var warehouseDtos = await _inventoryUnitOfWork.WarehouseRepository.GetWarehousesWithQuantityByItemIdAsync(itemId);

            // Map the data to WarehouseDto
            return warehouseDtos.Select(dto => new WarehouseDataDto
            {
                Name = dto.Name,
                Quantity = dto.Quantity
            }).ToList();
        }

    }
}

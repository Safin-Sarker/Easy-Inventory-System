using Autofac.Extras.Moq;
using DevSkill.Inventory.Application.Services;
using DevSkill.Inventory.Application.Services.StockTransferManagementService;
using DevSkill.Inventory.Domain.Dtos;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain.RepositoryContracts;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Tests
{
    public class WarehouseManagementServiceTest
    {
        private AutoMock _moq;

        private IWarehouseManagementService _warehouseManagementService;
        private Mock<IInventoryUnitOfWork> _inventoryUnitOfWorkMock;
        private Mock<IWarehouseRepository> _warehouseRepositoryMock;




        [SetUp]
        public void Setup()
        {
            _warehouseManagementService=_moq.Create<WarehouseManagementService>();
            _inventoryUnitOfWorkMock = _moq.Mock<IInventoryUnitOfWork>();
            _warehouseRepositoryMock = _moq.Mock<IWarehouseRepository>();
        }

        [TearDown]
        public void TearDown()
        {
            _inventoryUnitOfWorkMock?.Reset();
            _warehouseRepositoryMock?.Reset();
        }

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _moq = AutoMock.GetLoose();

        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {

            _moq?.Dispose();

        }

        [Test]
        public async Task CreateWarehouseAsync_ValidWarehouse_AddsWarehouseAndSaves()
        {
            // Arrange
            var warehouse = new Warehouse
            {
                Id = Guid.NewGuid(),
                Name = "Main Warehouse",
                Manager = "Admin",
                city = "New York",
                Phone = "123456789"
            };

            _inventoryUnitOfWorkMock.Setup(uow => uow.WarehouseRepository).Returns(_warehouseRepositoryMock.Object);
            _warehouseRepositoryMock.Setup(repo => repo.IsTitleDuplicateAsync(warehouse.Name,warehouse.Id)).ReturnsAsync(false);
            _warehouseRepositoryMock.Setup(repo => repo.AddAsync(warehouse)).Verifiable();
            _inventoryUnitOfWorkMock.Setup(uow => uow.SaveAsync()).Verifiable();

            // Act
            await _warehouseManagementService.CreateWarehouseAsync(warehouse);

            // Assert
            _warehouseRepositoryMock.Verify(repo => repo.IsTitleDuplicateAsync(warehouse.Name, warehouse.Id), Times.Once);
            _warehouseRepositoryMock.Verify(repo => repo.AddAsync(warehouse), Times.Once);
            _inventoryUnitOfWorkMock.Verify(uow => uow.SaveAsync(), Times.Once);
        }




        [Test]
        public void CreateWarehouseAsync_DuplicateWarehouseTitle_ThrowsInvalidOperationException()
        {
            // Arrange
            var warehouse = new Warehouse
            {
                Id = Guid.NewGuid(),
                Name = "Warehouse 1"
            };

            _inventoryUnitOfWorkMock
                .Setup(x => x.WarehouseRepository.IsTitleDuplicateAsync(warehouse.Name, warehouse.Id))
                .ReturnsAsync(true);

            // Act & Assert
            var exception = Assert.ThrowsAsync<InvalidOperationException>(() =>
                _warehouseManagementService.CreateWarehouseAsync(warehouse));
            Assert.AreEqual("There is already a warehouse with this Title", exception.Message);

            _inventoryUnitOfWorkMock.Verify(x => x.WarehouseRepository.AddAsync(It.IsAny<Warehouse>()), Times.Never);
            _inventoryUnitOfWorkMock.Verify(x => x.SaveAsync(), Times.Never);
        }

        [Test]
        public async Task GetWarehouseListAsync_ReturnsMappedWarehouseDtos()
        {
            // Arrange
            var warehouses = new List<Warehouse>
            {
                new Warehouse
                {
                    Id = Guid.NewGuid(),
                    Name = "Warehouse A",
                    Manager = "John Doe",
                    city = "New York",
                    Phone = "123456789"
                },
                new Warehouse
                {
                    Id = Guid.NewGuid(),
                    Name = "Warehouse B",
                    Manager = "Jane Smith",
                    city = "Los Angeles",
                    Phone = "987654321"
                }
            };

            _inventoryUnitOfWorkMock.Setup(uow => uow.WarehouseRepository.GetAllAsync())
                .ReturnsAsync(warehouses);

            // Act
            var result = await _warehouseManagementService.GetWarehouseListAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(warehouses.Count, result.Count);

            for (int i = 0; i < warehouses.Count; i++)
            {
                Assert.AreEqual(warehouses[i].Id, result[i].Id);
                Assert.AreEqual(warehouses[i].Name, result[i].Name);
                Assert.AreEqual(warehouses[i].Manager, result[i].Manager);
                Assert.AreEqual(warehouses[i].city, result[i].City);
                Assert.AreEqual(warehouses[i].Phone, result[i].Phone);
            }

            _inventoryUnitOfWorkMock.Verify(uow => uow.WarehouseRepository.GetAllAsync(), Times.Once);
        }

        [Test]
        public async Task UpdateWarehouseAsync_ValidWarehouse_UpdatesAndSaves()
        {
            // Arrange
            var warehouse = new Warehouse
            {
                Id = Guid.NewGuid(),
                Name = "Updated Warehouse",
                Manager = "Admin",
                city = "New York",
                Phone = "123456789"
            };

            _inventoryUnitOfWorkMock.Setup(uow => uow.WarehouseRepository).Returns(_warehouseRepositoryMock.Object);
            _warehouseRepositoryMock
                .Setup(repo => repo.IsTitleDuplicateAsync(warehouse.Name, warehouse.Id))
                .ReturnsAsync(false); // No duplicate found
            _warehouseRepositoryMock.Setup(repo => repo.EditAsync(warehouse)).Verifiable();
            _inventoryUnitOfWorkMock.Setup(uow => uow.SaveAsync()).Verifiable();

            // Act
            await _warehouseManagementService.UpdateWarehouseAsync(warehouse);

            // Assert
            _warehouseRepositoryMock.Verify(repo => repo.IsTitleDuplicateAsync(warehouse.Name, warehouse.Id), Times.Once);
            _warehouseRepositoryMock.Verify(repo => repo.EditAsync(warehouse), Times.Once);
            _inventoryUnitOfWorkMock.Verify(uow => uow.SaveAsync(), Times.Once);
        }

        [Test]
        public void UpdateWarehouseAsync_DuplicateName_ThrowsInvalidOperationException()
        {
            // Arrange
            var warehouse = new Warehouse
            {
                Id = Guid.NewGuid(),
                Name = "Duplicate Warehouse",
                Manager = "Admin",
                city = "New York",
                Phone = "123456789"
            };

            _inventoryUnitOfWorkMock.Setup(uow => uow.WarehouseRepository).Returns(_warehouseRepositoryMock.Object);
            _warehouseRepositoryMock
                .Setup(repo => repo.IsTitleDuplicateAsync(warehouse.Name, warehouse.Id))
                .ReturnsAsync(true); // Duplicate name found

            // Act & Assert
            var exception = Assert.ThrowsAsync<InvalidOperationException>(() =>
                _warehouseManagementService.UpdateWarehouseAsync(warehouse));

            Assert.AreEqual("Name should be Unique", exception.Message);

            _warehouseRepositoryMock.Verify(repo => repo.IsTitleDuplicateAsync(warehouse.Name, warehouse.Id), Times.Once);
            _warehouseRepositoryMock.Verify(repo => repo.EditAsync(It.IsAny<Warehouse>()), Times.Never);
            _inventoryUnitOfWorkMock.Verify(uow => uow.SaveAsync(), Times.Never);
        }

        [Test]
        public async Task GetAllWarehousesExceptAsync_ValidExcludeId_ReturnsFilteredWarehouses()
        {
            // Arrange
            var excludeWarehouseId = Guid.NewGuid();
            var warehouses = new List<Warehouse>
            {
                new Warehouse { Id = Guid.NewGuid(), Name = "Warehouse A" },
                new Warehouse { Id = excludeWarehouseId, Name = "Warehouse B" }, // This one should be excluded
                new Warehouse { Id = Guid.NewGuid(), Name = "Warehouse C" }
            };

            _inventoryUnitOfWorkMock.Setup(uow => uow.WarehouseRepository.GetAllExceptAsync(It.IsAny<Expression<Func<Warehouse, bool>>>()))
                .ReturnsAsync((Expression<Func<Warehouse, bool>> predicate) => warehouses.AsQueryable().Where(predicate.Compile()).ToList());

            // Act
            var result = await _warehouseManagementService.GetAllWarehousesExceptAsync(excludeWarehouseId);

            // Assert
            Assert.IsNotNull(result);
            var resultList = result.ToList();
            Assert.AreEqual(2, resultList.Count); // Only two warehouses should be returned
            Assert.IsFalse(resultList.Any(w => w.Id == excludeWarehouseId)); // Ensure excluded warehouse is not in the result

            _inventoryUnitOfWorkMock.Verify(uow => uow.WarehouseRepository.GetAllExceptAsync(It.IsAny<Expression<Func<Warehouse, bool>>>()), Times.Once);
        }


        [Test]
        public async Task GetWarehouseDataByItemIdAsync_ValidItemId_ReturnsMappedWarehouseDataDtos()
        {
            // Arrange
            var itemId = Guid.NewGuid();

            // Mock data returned from repository
            var warehouseData = new List<WarehouseDataDto>
            {
                new WarehouseDataDto { Name = "Warehouse A", Quantity = 50 },
                new WarehouseDataDto { Name = "Warehouse B", Quantity = 30 }
            };

            // Setup the repository mock to return mock data
            _inventoryUnitOfWorkMock.Setup(uow => uow.WarehouseRepository.GetWarehousesWithQuantityByItemIdAsync(itemId))
                .ReturnsAsync(warehouseData);

            // Act
            var result = await _warehouseManagementService.GetWarehouseDataByItemIdAsync(itemId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(warehouseData.Count, result.Count);

            for (int i = 0; i < warehouseData.Count; i++)
            {
                Assert.AreEqual(warehouseData[i].Name, result[i].Name);
                Assert.AreEqual(warehouseData[i].Quantity, result[i].Quantity);
            }

            _inventoryUnitOfWorkMock.Verify(uow => uow.WarehouseRepository.GetWarehousesWithQuantityByItemIdAsync(itemId), Times.Once);
        }







    }
}

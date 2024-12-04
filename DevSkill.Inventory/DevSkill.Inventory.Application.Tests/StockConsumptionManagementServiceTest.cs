using Autofac.Core;
using Autofac.Extras.Moq;
using DevSkill.Inventory.Application.Services;
using DevSkill.Inventory.Application.Services.StockConsumption;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain.RepositoryContracts;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Tests
{
    public class StockConsumptionManagementServiceTest
    {
        private AutoMock _moq;

        private IStockConsumptionManagementService _stockConsumptionManagementService;
        private Mock<IInventoryUnitOfWork> _inventoryUnitOfWorkMock;
        private Mock<IStockConsumptionManagementService> _stockConsumptionManagementServiceMock;
        private Mock<IItemRepository> _itemRepositoryMock;
        private Mock<IStockRepository> _stockRepositoryMock;


        [SetUp]
        public void Setup()
        {
        
            _stockConsumptionManagementService = _moq.Create<StockConsumptionManagementService>();
            _inventoryUnitOfWorkMock = _moq.Mock<IInventoryUnitOfWork>();
            _stockConsumptionManagementServiceMock = _moq.Mock<IStockConsumptionManagementService>();
            _itemRepositoryMock = _moq.Mock<IItemRepository>();
            _stockRepositoryMock = _moq.Mock<IStockRepository>();

            _inventoryUnitOfWorkMock.Setup(u => u.StockRepository).Returns(_stockRepositoryMock.Object);
            _inventoryUnitOfWorkMock.Setup(u => u.ItemRepository).Returns(_itemRepositoryMock.Object);

            _stockConsumptionManagementService = new StockConsumptionManagementService(_inventoryUnitOfWorkMock.Object);

        }

        [TearDown]
        public void TearDown()
        {
            _inventoryUnitOfWorkMock?.Reset();
            _stockConsumptionManagementServiceMock?.Reset();

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
        public async Task CreateStockConsumptionAsync_ShouldAdjustStockSuccessfully()
        {
            // Arrange
            var warehouseId = Guid.NewGuid();
            var itemId = Guid.NewGuid();

            var stockConsumption = new StockConsumption
            {
                WarehouseId = warehouseId,
                StockConsumeds = new List<StockConsumed>
                {
                    new StockConsumed { ItemId = itemId, Quantity = 5 }
                }
            };

            var existingStock = new Stock
            {
                ItemId = itemId,
                Quantity = 10,
                WarehouseId = warehouseId
            };

            // Mock repository behaviors
            _stockRepositoryMock.Setup(r => r.GetStockByItemAndWarehouseAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
                .ReturnsAsync(existingStock);

            _stockRepositoryMock.Setup(r => r.EditAsync(It.IsAny<Stock>())).Returns(Task.CompletedTask);

            _stockRepositoryMock.Setup(r => r.ExecuteInTransactionAsync(It.IsAny<Func<Task>>()))
                .Callback<Func<Task>>(async transaction => await transaction()) // Execute the provided transaction delegate
                .ReturnsAsync(true);

            _inventoryUnitOfWorkMock.Setup(u => u.SaveAsync()).Returns(Task.CompletedTask);

            // Act
            var result = await _stockConsumptionManagementService.CreateStockConsumptionAsync(stockConsumption);

            // Assert
            Assert.IsTrue(result);

            // Verify that the expected methods were called
            _stockRepositoryMock.Verify(r => r.GetStockByItemAndWarehouseAsync(itemId, warehouseId), Times.Once);
            _stockRepositoryMock.Verify(r => r.EditAsync(It.Is<Stock>(s => s.ItemId == itemId && s.Quantity == 5)), Times.Once);
            _inventoryUnitOfWorkMock.Verify(u => u.SaveAsync(), Times.AtLeastOnce);
            _stockRepositoryMock.Verify(r => r.ExecuteInTransactionAsync(It.IsAny<Func<Task>>()), Times.Once);
        }

        [Test]
        public async Task CreateStockConsumptionAsync_ShouldUpdateItemOpeningStock_WhenItemTracksInventory()
        {
            // Arrange
            var itemId = Guid.NewGuid();
            var warehouseId = Guid.NewGuid();

            var stockConsumption = new StockConsumption
            {
                WarehouseId = warehouseId,
                StockConsumeds = new List<StockConsumed>
                {
                    new StockConsumed { ItemId = itemId, Quantity = 5 }
                }
            };

            var existingStock = new Stock { ItemId = itemId, WarehouseId = warehouseId, Quantity = 10 };
            var item = new Item { Id = itemId, TrackInventory = true, OpeningStock = 50 };

            var stocksForItem = new List<Stock>
            {
                new Stock { ItemId = itemId, Quantity = 10 },
                new Stock { ItemId = itemId, Quantity = 20 } // Matching ItemId
            }.AsQueryable();

            _stockRepositoryMock.Setup(r => r.GetStockByItemAndWarehouseAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
                .ReturnsAsync(existingStock);

            _stockRepositoryMock.Setup(r => r.GetAllSumAsync()).ReturnsAsync(stocksForItem);

            _itemRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(item);

            _stockRepositoryMock.Setup(r => r.ExecuteInTransactionAsync(It.IsAny<Func<Task>>()))
                .Callback<Func<Task>>(async func => await func())
                .ReturnsAsync(true);

            _inventoryUnitOfWorkMock.Setup(u => u.SaveAsync()).Returns(Task.CompletedTask);

            // Act
            var result = await _stockConsumptionManagementService.CreateStockConsumptionAsync(stockConsumption);

            // Assert
            Assert.IsTrue(result);
            _itemRepositoryMock.VerifyAll(); 
            _inventoryUnitOfWorkMock.VerifyAll();
        }










    }
}

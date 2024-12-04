using Autofac.Extras.Moq;
using DevSkill.Inventory.Application.Services;
using DevSkill.Inventory.Application.Services.StockConsumption;
using DevSkill.Inventory.Application.Services.StockProduction;
using DevSkill.Inventory.Domain;
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
    public class StockProductionManagementTest
    {
        private AutoMock _moq;

        private IStockProductionManagementService _stockProductionManagementService;
        private Mock<IInventoryUnitOfWork> _inventoryUnitOfWorkMock;
        private Mock<IStockProductionManagementService> _stockProductionManagementServiceMock;
        private Mock<IItemRepository> _itemRepositoryMock;
        private Mock<IStockRepository> _stockRepositoryMock;




        [SetUp]
        public void Setup()
        {
            _stockProductionManagementService = _moq.Create<StockProductionManagementService>();
            _inventoryUnitOfWorkMock = _moq.Mock<IInventoryUnitOfWork>();
            _stockProductionManagementServiceMock = _moq.Mock<IStockProductionManagementService>();
            _itemRepositoryMock = _moq.Mock<IItemRepository>();
            _stockRepositoryMock = _moq.Mock<IStockRepository>();
        }


        [TearDown]
        public void TearDown()
        {
            _inventoryUnitOfWorkMock?.Reset();
            _stockProductionManagementServiceMock?.Reset();

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
        public async Task CreateStockProductionAsync_Should_Add_New_Stock_For_New_Items()
        {
            // Arrange
            var itemId = Guid.NewGuid();
            var warehouseId = Guid.NewGuid();
            var stockProduction = new StockProduction
            {
                WarehouseId = warehouseId,
                StockProducedItems = new List<StockProduced>
                {
                    new StockProduced { ItemId = itemId, Quantity = 10 }
                }
            };
            var item = new Item
            {
                Id = itemId,
                TrackInventory = true,
                OpeningStock = 0
            };

            _itemRepositoryMock.Setup(r => r.GetByIdAsync(itemId)).ReturnsAsync(item);
            _itemRepositoryMock.Setup(r => r.EditAsync(item)).Returns(Task.CompletedTask);

            _inventoryUnitOfWorkMock.SetupGet(u => u.ItemRepository).Returns(_itemRepositoryMock.Object);


            _stockRepositoryMock
                .Setup(r => r.GetStockByItemAndWarehouseAsync(itemId, warehouseId))
                .ReturnsAsync((Domain.Entities.Stock)null);

            _stockRepositoryMock
                .Setup(r => r.AddAsync(It.IsAny<Domain.Entities.Stock>()))
                .Returns(Task.CompletedTask);

            _stockRepositoryMock
                .Setup(r => r.ExecuteInTransactionAsync(It.IsAny<Func<Task>>()))
                .Callback<Func<Task>>(async action => await action())
                .ReturnsAsync(true);

            _inventoryUnitOfWorkMock.SetupGet(u => u.StockRepository).Returns(_stockRepositoryMock.Object);
            _inventoryUnitOfWorkMock.Setup(u => u.SaveAsync()).Returns(Task.CompletedTask);

            // Act
            var result = await _stockProductionManagementService.CreateStockProductionAsync(stockProduction);

            // Assert
            Assert.IsTrue(result);
            _stockRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Domain.Entities.Stock>()), Times.Once);
        }


        [Test]
        public async Task CreateStockProductionAsync_Should_Update_Existing_Stock()
        {
            // Arrange
            var itemId = Guid.NewGuid();
            var warehouseId = Guid.NewGuid();
            var stockProduction = new StockProduction
            {
                WarehouseId = warehouseId,
                StockProducedItems = new List<StockProduced>
                {
                    new StockProduced { ItemId = itemId, Quantity = 10 }
                }
            };

            var existingStock = new Domain.Entities.Stock
            {
                ItemId = itemId,
                WarehouseId = warehouseId,
                Quantity = 20
            };

            var item = new Item
            {
                Id = itemId,
                TrackInventory = true,
                OpeningStock = 0
            };

            _itemRepositoryMock.Setup(r => r.GetByIdAsync(itemId)).ReturnsAsync(item);
            _itemRepositoryMock.Setup(r => r.EditAsync(item)).Returns(Task.CompletedTask);

            _inventoryUnitOfWorkMock.SetupGet(u => u.ItemRepository).Returns(_itemRepositoryMock.Object);


            _stockRepositoryMock
                .Setup(r => r.GetStockByItemAndWarehouseAsync(itemId, warehouseId))
                .ReturnsAsync(existingStock);

            _stockRepositoryMock
                .Setup(r => r.EditAsync(existingStock))
                .Returns(Task.CompletedTask);

            _stockRepositoryMock
                .Setup(r => r.ExecuteInTransactionAsync(It.IsAny<Func<Task>>()))
                .Callback<Func<Task>>(async action => await action())
                .ReturnsAsync(true);

            _inventoryUnitOfWorkMock.SetupGet(u => u.StockRepository).Returns(_stockRepositoryMock.Object);
            _inventoryUnitOfWorkMock.Setup(u => u.SaveAsync()).Returns(Task.CompletedTask);

            // Act
            var result = await _stockProductionManagementService.CreateStockProductionAsync(stockProduction);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(30, existingStock.Quantity); // Ensure quantity is updated
            _stockRepositoryMock.Verify(r => r.EditAsync(existingStock), Times.Once);
        }


        [Test]
        public async Task CreateStockProductionAsync_Should_Skip_Non_Trackable_Items()
        {
            // Arrange
            var itemId = Guid.NewGuid();
            var warehouseId = Guid.NewGuid();
            var stockProduction = new StockProduction
            {
                WarehouseId = warehouseId,
                StockProducedItems = new List<StockProduced>
                {
                    new StockProduced { ItemId = itemId, Quantity = 10 }
                }
            };

            var item = new Item
            {
                Id = itemId,
                TrackInventory = false
            };

            _itemRepositoryMock.Setup(r => r.GetByIdAsync(itemId)).ReturnsAsync(item);
            _inventoryUnitOfWorkMock.SetupGet(u => u.ItemRepository).Returns(_itemRepositoryMock.Object);

            _stockRepositoryMock
                .Setup(r => r.ExecuteInTransactionAsync(It.IsAny<Func<Task>>()))
                .Callback<Func<Task>>(async action => await action())
                .ReturnsAsync(true);

            _inventoryUnitOfWorkMock.SetupGet(u => u.StockRepository).Returns(_stockRepositoryMock.Object);
            _inventoryUnitOfWorkMock.Setup(u => u.SaveAsync()).Returns(Task.CompletedTask);

            // Act
            var result = await _stockProductionManagementService.CreateStockProductionAsync(stockProduction);

            // Assert
            Assert.IsTrue(result);
            _stockRepositoryMock.VerifyAll();
            _itemRepositoryMock.VerifyAll();
        }

        [Test]
        public async Task CreateStockProductionAsync_Should_Update_OpeningStock()
        {
            // Arrange
            var itemId = Guid.NewGuid();
            var warehouseId = Guid.NewGuid();
            var stockProduction = new StockProduction
            {
                WarehouseId = warehouseId,
                StockProducedItems = new List<StockProduced>
                {
                    new StockProduced { ItemId = itemId, Quantity = 10 }
                }
            };

            var item = new Item
            {
                Id = itemId,
                TrackInventory = true,
                OpeningStock = 0
            };

            var stocks = new List<Domain.Entities.Stock>
            {
                new Domain.Entities.Stock { ItemId = itemId, Quantity = 10 },
                new Domain.Entities.Stock { ItemId = itemId, Quantity = 20 }
            }.AsQueryable();

            _itemRepositoryMock.Setup(r => r.GetByIdAsync(itemId)).ReturnsAsync(item);
            _itemRepositoryMock.Setup(r => r.EditAsync(item)).Returns(Task.CompletedTask);
            _stockRepositoryMock.Setup(r => r.GetAllSumAsync()).ReturnsAsync(stocks);

            _stockRepositoryMock
                .Setup(r => r.ExecuteInTransactionAsync(It.IsAny<Func<Task>>()))
                .Callback<Func<Task>>(async action => await action())
                .ReturnsAsync(true);

            _inventoryUnitOfWorkMock.SetupGet(u => u.StockRepository).Returns(_stockRepositoryMock.Object);
            _inventoryUnitOfWorkMock.SetupGet(u => u.ItemRepository).Returns(_itemRepositoryMock.Object);
            _inventoryUnitOfWorkMock.Setup(u => u.SaveAsync()).Returns(Task.CompletedTask);

            // Act
            var result = await _stockProductionManagementService.CreateStockProductionAsync(stockProduction);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(30, item.OpeningStock); 
            _itemRepositoryMock.VerifyAll();
        }

        [Test]
        public async Task GetStockProductionsAsync_ValidParameters_ReturnsExpectedResults()
        {
            // Arrange
            var pageIndex = 1;
            var pageSize = 2;
            var search = new DataTablesSearch { Value = "Production" };
            var order = "VoucherNumber";

            var expectedData = new List<StockProduction>
            {
                new StockProduction
                {
                    Id = Guid.NewGuid(),
                    VoucherNumber = "STK-001",
                    VoucherDate = DateTime.UtcNow,
                    CreatedBy = "Admin",
                    WarehouseId = Guid.NewGuid(),
                    Details = "Test Production"
                },
                new StockProduction
                {
                    Id = Guid.NewGuid(),
                    VoucherNumber = "STK-002",
                    VoucherDate = DateTime.UtcNow,
                    CreatedBy = "Admin",
                    WarehouseId = Guid.NewGuid(),
                    Details = "Another Test Production"
                }
            };

            var expectedTotal = 10;
            var expectedTotalDisplay = 2;

            _inventoryUnitOfWorkMock
                .Setup(x => x.StockProductionRepository.GetPagedProductionsAsync(pageIndex, pageSize, search, order))
                .ReturnsAsync((expectedData, expectedTotal, expectedTotalDisplay));

            // Act
            var result = await _stockProductionManagementService.GetStockProductionsAsync(pageIndex, pageSize, search, order);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedData.Count, result.data.Count);
            Assert.AreEqual(expectedTotal, result.total);
            Assert.AreEqual(expectedTotalDisplay, result.totalDisplay);

            _inventoryUnitOfWorkMock.Verify(
                x => x.StockProductionRepository.GetPagedProductionsAsync(pageIndex, pageSize, search, order),
                Times.Once);
        }

        [Test]
        public async Task GetStockProductionsAsync_NoResults_ReturnsEmptyData()
        {
            // Arrange
            var pageIndex = 1;
            var pageSize = 2;
            var search = new DataTablesSearch();
            string? order = null;

            _inventoryUnitOfWorkMock
                .Setup(x => x.StockProductionRepository.GetPagedProductionsAsync(pageIndex, pageSize, search, order))
                .ReturnsAsync((new List<StockProduction>(), 0, 0));

            // Act
            var result = await _stockProductionManagementService.GetStockProductionsAsync(pageIndex, pageSize, search, order);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsEmpty(result.data);
            Assert.AreEqual(0, result.total);
            Assert.AreEqual(0, result.totalDisplay);

            _inventoryUnitOfWorkMock.Verify(
                x => x.StockProductionRepository.GetPagedProductionsAsync(pageIndex, pageSize, search, order),
                Times.Once);
        }

      










    }
}

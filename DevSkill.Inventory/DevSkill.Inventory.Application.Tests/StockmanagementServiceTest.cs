using Autofac.Extras.Moq;
using DevSkill.Inventory.Application.Services;
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
    public class StockmanagementServiceTest
    {
        private AutoMock _moq;

        private IStockManagementService _stockManagementService;

        private Mock<IInventoryUnitOfWork> _inventoryUnitOfWorkMock;

        private Mock<IStockRepository> _stockRepositoryMock;

        [SetUp]
        public void Setup()
        {
            _stockManagementService = _moq.Create<StockManagementService>();
            _inventoryUnitOfWorkMock = _moq.Mock<IInventoryUnitOfWork>();
            _stockRepositoryMock = _moq.Mock<IStockRepository>();

        }

        public void TearDown()
        {
            _inventoryUnitOfWorkMock?.Reset();
            _stockRepositoryMock?.Reset();

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
        public async Task CreateStockAsync_ValidStock_ShouldAddStocksAndSave()
        {
            // Arrange
            var stocks = new List<Stock>
            {
                new Stock
                {
                    Id = Guid.NewGuid(),
                    ItemId = Guid.NewGuid(),
                    WarehouseId = Guid.NewGuid(),
                    Quantity = 10,
                    UnitPrice = 100.50
                },
                new Stock
                {
                    Id = Guid.NewGuid(),
                    ItemId = Guid.NewGuid(),
                    WarehouseId = Guid.NewGuid(),
                    Quantity = 5,
                    UnitPrice = 50.25
                }
            };

            _inventoryUnitOfWorkMock.Setup(x => x.StockRepository).Returns(_stockRepositoryMock.Object);
            _stockRepositoryMock.Setup(x => x.AddRangeAsync(It.IsAny<IEnumerable<Stock>>())).Verifiable();
            _inventoryUnitOfWorkMock.Setup(x => x.SaveAsync()).Verifiable();

            // Act
            await _stockManagementService.CreateStockAsync(stocks);

            // Assert
            _stockRepositoryMock.VerifyAll();
            _inventoryUnitOfWorkMock.VerifyAll();
        }

      







    }
}

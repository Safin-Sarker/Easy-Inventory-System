using Autofac.Extras.Moq;
using DevSkill.Inventory.Application.Services.StockProduction;
using DevSkill.Inventory.Application.Services.StockTransferManagementService;
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
    public class StockTransferManagementServiceTest
    {
        private AutoMock _moq;

        private IStockTransferManagementService _stockTransferManagementService;
        private Mock<IInventoryUnitOfWork> _inventoryUnitOfWorkMock;
        private Mock<IStockTransferManagementService> _stockTransferManagementServiceMock;
        private Mock<IStockRepository> _stockRepositoryMock;


        [SetUp]
        public void Setup()
        {
            _stockTransferManagementService = _moq.Create<StockTransferManagementService>();
            _inventoryUnitOfWorkMock = _moq.Mock<IInventoryUnitOfWork>();
            _stockTransferManagementServiceMock = _moq.Mock<IStockTransferManagementService>();
            _stockRepositoryMock = _moq.Mock<IStockRepository>();

            _inventoryUnitOfWorkMock.Setup(u => u.StockRepository).Returns(_stockRepositoryMock.Object);

            _stockTransferManagementService = new StockTransferManagementService(_inventoryUnitOfWorkMock.Object);
        }


        [TearDown]
        public void TearDown()
        {
            _inventoryUnitOfWorkMock?.Reset();
            _stockTransferManagementServiceMock?.Reset();

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
        public async Task GetVoucherNumberAsync_ShouldReturnNextVoucherNumber()
        {
            // Arrange
            var lastVoucher = "STKISUE/05"; 
            var expectedNextVoucher = "STKISUE/06"; 

            _inventoryUnitOfWorkMock
                .Setup(u => u.StockTransferRepository.GetLastVoucherAsync())
                .ReturnsAsync(lastVoucher);


            // Act
            var actualVoucher = await _stockTransferManagementService.GetVoucherNumberAsync();

            // Assert
            Assert.AreEqual(expectedNextVoucher, actualVoucher);

            _inventoryUnitOfWorkMock
                .Verify(u => u.StockTransferRepository.GetLastVoucherAsync(), Times.Once);
        }


        [Test]
        public async Task CreateStockTransferAsync_ShouldAddStockTransferAndSaveChanges()
        {
            // Arrange
            var stockTransfer = new StockTransfer
            {
                Id = new Guid(),
                VoucherNumber = "STKISUE/0006",
            };

            _inventoryUnitOfWorkMock
                .Setup(u => u.StockTransferRepository.AddAsync(stockTransfer))
                .Returns(Task.CompletedTask);

            _inventoryUnitOfWorkMock
                .Setup(u => u.SaveAsync())
                .Returns(Task.CompletedTask);

            // Act
            await _stockTransferManagementService.CreateStockTransferAsync(stockTransfer);

            // Assert
            _inventoryUnitOfWorkMock.Verify(u => u.StockTransferRepository.AddAsync(stockTransfer), Times.Once);
            _inventoryUnitOfWorkMock.Verify(u => u.SaveAsync(), Times.Once);
        }

        [Test]
        public async Task DeleteItemAsync_ShouldRemoveItemAndSaveChanges()
        {
            // Arrange
            var id = Guid.NewGuid(); // Example ID

            _inventoryUnitOfWorkMock
                .Setup(u => u.StockTransferRepository.RemoveAsync(id))
                .Returns(Task.CompletedTask);

            _inventoryUnitOfWorkMock
                .Setup(u => u.SaveAsync())
                .Returns(Task.CompletedTask);

            // Act
            await _stockTransferManagementService.DeleteItemAsync(id);

            // Assert
            _inventoryUnitOfWorkMock.Verify(u => u.StockTransferRepository.RemoveAsync(id), Times.Once);
            _inventoryUnitOfWorkMock.Verify(u => u.SaveAsync(), Times.Once);
        }


        [Test]
        public async Task GetByIdAsync_ShouldReturnStockTransferIfFound()
        {
            // Arrange
            var id = Guid.NewGuid();
            var expectedStockTransfer = new StockTransfer
            {
                Id = id,
                VoucherNumber = "STKISUE/0006",
            };

            _inventoryUnitOfWorkMock
                .Setup(u => u.StockTransferRepository.GetByIdAsync(id))
                .ReturnsAsync(expectedStockTransfer);

            // Act
            var actualStockTransfer = await _stockTransferManagementService.GetByIdAsync(id);

            // Assert
            Assert.IsNotNull(actualStockTransfer);
            Assert.AreEqual(expectedStockTransfer, actualStockTransfer);
            _inventoryUnitOfWorkMock.Verify(u => u.StockTransferRepository.GetByIdAsync(id), Times.Once);
        }

        [Test]
        public async Task GetStockTransfersAsync_ShouldReturnPagedStockTransfers()
        {
            // Arrange
            int pageIndex = 1;
            int pageSize = 10;
            var search = new DataTablesSearch { Value = "test search", Regex = false };
            string? order = "VoucherNumber";

            var expectedData = new List<StockTransfer>
            {
                new StockTransfer { Id = Guid.NewGuid(), VoucherNumber = "STKISUE/0001" },
                new StockTransfer { Id = Guid.NewGuid(), VoucherNumber = "STKISUE/0002" }
            };

            int expectedTotal = 50;
            int expectedTotalDisplay = 10;

            // Mock the repository method to return expected results
            _inventoryUnitOfWorkMock
                .Setup(u => u.StockTransferRepository.GetPagedStockTransfersAsync(pageIndex, pageSize, search, order))
                .ReturnsAsync((expectedData, expectedTotal, expectedTotalDisplay));

            // Act
            var (actualData, actualTotal, actualTotalDisplay) = await _stockTransferManagementService.GetStockTransfersAsync(pageIndex, pageSize, search, order);

            // Assert
            Assert.IsNotNull(actualData);
            Assert.AreEqual(expectedData, actualData);
            Assert.AreEqual(expectedTotal, actualTotal);
            Assert.AreEqual(expectedTotalDisplay, actualTotalDisplay);

            _inventoryUnitOfWorkMock.Verify(
                u => u.StockTransferRepository.GetPagedStockTransfersAsync(pageIndex, pageSize, search, order),
                Times.Once
            );
        }













    }
}

using Autofac.Extras.Moq;
using DevSkill.Inventory.Application.Services;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain.RepositoryContracts;
using Moq;
using NUnit.Framework;
using Shouldly;

namespace DevSkill.Inventory.Application.Tests
{
    public class ItemManagementServiceTest
    {
        private AutoMock _moq;

        private IItemManagementService _itemManagementService;
        private Mock<IInventoryUnitOfWork> _inventoryUnitOfWorkMock;
        private Mock<IItemRepository> _itemRepositoryMock;

        [SetUp]
        public void Setup()
        {
            _itemManagementService = _moq.Create<ItemManagementService>();
            _inventoryUnitOfWorkMock = _moq.Mock<IInventoryUnitOfWork>();
            _itemRepositoryMock = _moq.Mock<IItemRepository>();
        }

        [TearDown] 
        public void TearDown()
        {
            _inventoryUnitOfWorkMock?.Reset();
            _itemRepositoryMock?.Reset();

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
        public void CreateItem_TitleNotDuplicate_CreatePost()
        {
           //Arrange
            Item item = new Item();
            item.Name = "Laptop";
            item.Description = "This is Gadget";
            item.ProductCode = "Lp-1203";
            item.UnitOfMeasure = "Pcs";
            item.Category = "Gadget";
            item.ItemType = "Product";
            item.TrackInventory= true;
            item.OpeningStock = 40;
            item.ReorderLevel = 0;

            //Act
            _inventoryUnitOfWorkMock.Setup(x => x.ItemRepository).Returns(_itemRepositoryMock.Object);
            _itemRepositoryMock.Setup(x => x.IsTitleDuplicateAsync(item.Name, null)).Returns(Task.FromResult(false));
            _itemRepositoryMock.Setup(x => x.AddAsync(item)).Verifiable();
            _inventoryUnitOfWorkMock.Setup(x => x.SaveAsync()).Verifiable();

             _itemManagementService.CreateItemAsync(item);

            // Assert
            _inventoryUnitOfWorkMock.VerifyAll();
            _itemRepositoryMock.VerifyAll();

        }


        [Test]
        public void CreateItem_TitleDuplicate_ThrowsException()
        {
            //Arrange
            Item item = new Item();
            item.Name = "Laptop";
            item.Description = "This is Gadget";
            item.ProductCode = "Lp-1203";
            item.UnitOfMeasure = "Pcs";
            item.Category = "Gadget";
            item.ItemType = "Product";
            item.TrackInventory = true;
            item.OpeningStock = 40;
            item.ReorderLevel = 0;

            var error = "There is already a Item with this Title";


            _inventoryUnitOfWorkMock.Setup(x => x.ItemRepository).Returns(_itemRepositoryMock.Object);
            _itemRepositoryMock.Setup(x => x.IsTitleDuplicateAsync(item.Name, null)).Returns(Task.FromResult(true));

            //Act
            var message = Should.Throw<InvalidOperationException>(() =>
            _itemManagementService.CreateItemAsync(item)).Message;

        }

        [Test]
        public async Task DeleteItemAsync_ValidId_ShouldRemoveItem()
        {
            // Arrange
            var itemId = Guid.NewGuid();

            // Mocking RemoveAsync and SaveAsync
            _itemRepositoryMock.Setup(x => x.RemoveAsync(itemId)).Verifiable();
            _inventoryUnitOfWorkMock.Setup(x => x.SaveAsync()).Verifiable();

            // Ensuring the ItemRepository is returned by the UnitOfWork mock
            _inventoryUnitOfWorkMock.Setup(x => x.ItemRepository).Returns(_itemRepositoryMock.Object);

            // Act
            await _itemManagementService.DeleteItemAsync(itemId);

            // Assert
            _inventoryUnitOfWorkMock.VerifyAll();
            _itemRepositoryMock.VerifyAll();

        }

        [Test]
        public async Task UpdateItemAsync_TitleNotDuplicate_ShouldUpdateItem()
        {
            // Arrange
            var item = new Item
            {
                Id = Guid.NewGuid(),
                Name = "Updated Laptop",
                Description = "This is an updated Gadget",
                ProductCode = "Lp-1204",
                UnitOfMeasure = "Pcs",
                Category = "Gadget",
                ItemType = "Product",
                TrackInventory = true,
                OpeningStock = 50,
                ReorderLevel = 10
            };

            _inventoryUnitOfWorkMock.Setup(x => x.ItemRepository).Returns(_itemRepositoryMock.Object);
            _itemRepositoryMock.Setup(x => x.IsTitleDuplicateAsync(item.Name, item.Id)).Returns(Task.FromResult(false));
            _itemRepositoryMock.Setup(x => x.EditAsync(item)).Verifiable();
            _inventoryUnitOfWorkMock.Setup(x => x.SaveAsync()).Verifiable();

            // Act
            await _itemManagementService.UpdateItemAsync(item);

            // Assert
            _itemRepositoryMock.VerifyAll();
            _inventoryUnitOfWorkMock.VerifyAll();
        }


        [Test]
        public void UpdateItemAsync_TitleDuplicate_ShouldThrowInvalidOperationException()
        {
            // Arrange
            var item = new Item
            {
                Id = Guid.NewGuid(),
                Name = "Duplicate Laptop",
                Description = "This is a duplicate Gadget",
                ProductCode = "Lp-1205",
                UnitOfMeasure = "Pcs",
                Category = "Gadget",
                ItemType = "Product",
                TrackInventory = true,
                OpeningStock = 30,
                ReorderLevel = 5
            };

            _inventoryUnitOfWorkMock.Setup(x => x.ItemRepository).Returns(_itemRepositoryMock.Object);
            _itemRepositoryMock.Setup(x => x.IsTitleDuplicateAsync(item.Name, item.Id)).Returns(Task.FromResult(true));

            // Act & Assert
            var ex = Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await _itemManagementService.UpdateItemAsync(item));

            Assert.That(ex.Message, Is.EqualTo("Title should be Unique"));

            // Verify that EditAsync and SaveAsync were never called
            _itemRepositoryMock.VerifyAll();
            _inventoryUnitOfWorkMock.Verify();
        }






    }
}
using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Dtos;
using DevSkill.Inventory.Domain.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application
{
    public interface IInventoryUnitOfWork:IUnitOfWork
    {
        IItemRepository ItemRepository { get; }

        IWarehouseRepository WarehouseRepository { get; }

        IStockRepository StockRepository { get; }

        IStockConsumptionRepository StockConsumptionRepository { get; }

        IStockProductionRepository StockProductionRepository { get; }

        IStockTransferRepository StockTransferRepository { get; }



        Task<(IList<ItemDto> data, int total, int totalDisplay)> GetPagedItemsUsingSPAsync(int pageIndex, int pageSize, ItemSearchDto search, string? order);
    }
}

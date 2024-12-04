using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Services.StockTransferManagementService
{
    public interface IStockTransferManagementService
    {
        Task<(IList<Domain.Entities.StockTransfer> data, int total, int totalDisplay)> GetStockTransfersAsync(int pageIndex, int pageSize, DataTablesSearch search, string? order);

        Task<string> GetVoucherNumberAsync();

        Task<bool> StockTransferAsync(StockTransfer stockTransfer);

        Task CreateStockTransferAsync(StockTransfer stockTransfer);

        Task<StockTransfer> GetStockTransferByIdAsync(Guid id);

        Task DeleteItemAsync(Guid id);

        Task<StockTransfer?> GetByIdAsync(Guid id);



    }
}

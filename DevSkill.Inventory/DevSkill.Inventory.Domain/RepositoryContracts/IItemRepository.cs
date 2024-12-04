using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Domain.RepositoryContracts
{
    public interface IItemRepository:IRepositoryBase<Item,Guid>
    {
        Task<(IList<Item> data, int total, int totalDisplay)> GetPagedItemsAsync(int pageIndex, int pageSize,
            DataTablesSearch search, string? order);

        Task<bool> IsTitleDuplicateAsync(string title, Guid? id = null);

        Task<Item> GetItemAsync(Guid id);

        Task<IList<string>> GetDistinctAsync(Expression<Func<Item, string>> selector);

        Task<int> GetTotalProductCountAsync();

        Task<int> GetTotalServiceCountAsync();





    }
}

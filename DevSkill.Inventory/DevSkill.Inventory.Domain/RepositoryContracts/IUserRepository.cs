using DevSkill.Inventory.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Domain.RepositoryContracts
{
    public interface IUserRepository
    {
        Task<(IList<UserDto> data, int total, int totalDisplay)> GetPagedUsersAsync(
            int pageIndex, int pageSize, DataTablesSearch search, string? order);
    }
}

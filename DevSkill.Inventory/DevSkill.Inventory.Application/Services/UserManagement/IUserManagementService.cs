using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Services.UserManagement
{
    public interface IUserManagementService
    {
        Task<(IList<UserDto> data, int total, int totalDisplay)> GetPagedUsersAsync(int pageIndex, int pageSize, DataTablesSearch search, string sortColumn);
    }
}

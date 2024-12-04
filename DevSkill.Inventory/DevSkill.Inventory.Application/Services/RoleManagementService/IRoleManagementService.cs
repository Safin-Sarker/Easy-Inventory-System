using DevSkill.Inventory.Domain.Dtos;
using DevSkill.Inventory.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Services.RoleManagementService
{
    public interface IRoleManagementService
    {
        Task<(IList<RoleDto> data, int total, int totalDisplay)> GetPagedRolesAsync(
       int pageIndex, int pageSize, DataTablesSearch search, string? order);
    }
}

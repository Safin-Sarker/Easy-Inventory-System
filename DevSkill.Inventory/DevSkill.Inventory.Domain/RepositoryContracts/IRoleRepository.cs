
using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

public interface IRoleRepository
{
    Task<(IList<RoleDto> data, int total, int totalDisplay)> GetPagedRolesAsync(
            int pageIndex, int pageSize, DataTablesSearch search, string? order);

}

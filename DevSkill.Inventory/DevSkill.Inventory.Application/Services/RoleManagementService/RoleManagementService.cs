using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Services.RoleManagementService
{
    public class RoleManagementService:IRoleManagementService
    {
        private readonly IRoleRepository _roleRepository;

        public RoleManagementService(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }


        public async Task<(IList<RoleDto> data, int total, int totalDisplay)> GetPagedRolesAsync(
       int pageIndex, int pageSize, DataTablesSearch search, string? order)

        {
            return await _roleRepository.GetPagedRolesAsync(pageIndex, pageSize, search, order);
        }


    }
}

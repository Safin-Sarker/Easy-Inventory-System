using DevSkill.Inventory.Domain.Dtos;
using DevSkill.Inventory.Domain.RepositoryContracts;
using DevSkill.Inventory.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Services.UserManagement
{
    public class UserManagementService: IUserManagementService
    {
        private readonly IUserRepository _userRepository;

        public UserManagementService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<(IList<UserDto> data, int total, int totalDisplay)> GetPagedUsersAsync(int pageIndex, int pageSize, DataTablesSearch search, string sortColumn)
        {
            return await _userRepository.GetPagedUsersAsync(pageIndex, pageSize, search, sortColumn);
        }
    }
}

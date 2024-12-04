using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Dtos;
using DevSkill.Inventory.Domain.RepositoryContracts;
using DevSkill.Inventory.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Infrastructure.Repositories
{
    public class UserRepository : ApplicationRepository<ApplicationUser, UserDto>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context) : base(context) { }

        public async Task<(IList<UserDto> data, int total, int totalDisplay)> GetPagedUsersAsync(
            int pageIndex, int pageSize, DataTablesSearch search, string? order)
        {
            Expression<Func<ApplicationUser, bool>> filter = null;

            if (!string.IsNullOrWhiteSpace(search.Value))
            {
                filter = x => x.UserName.Contains(search.Value) || x.Email.Contains(search.Value);
            }

            return await GetPagedAsync(
                filter: filter,
                orderBy: order,
                include: q => q.Include(u => u.UserRoles).ThenInclude(ur => ur.Role),
                pageIndex: pageIndex,
                pageSize: pageSize,
                isTrackingOff: true,
                selector: u => new UserDto
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    Email = u.Email,
                    PhoneNumber = u.PhoneNumber ?? "N/A",
                    Status = u.Status,
                    Roles = u.UserRoles.Select(ur => ur.Role.Name).ToList()
                }
            );
        }
    }
}

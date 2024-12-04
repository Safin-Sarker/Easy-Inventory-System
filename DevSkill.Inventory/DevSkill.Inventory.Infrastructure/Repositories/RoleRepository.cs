using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.RepositoryContracts;
using DevSkill.Inventory.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using DevSkill.Inventory.Domain.Dtos;

namespace DevSkill.Inventory.Infrastructure.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<ApplicationRole> _roles;

        public RoleRepository(ApplicationDbContext context)
        {
            _context = context;
            _roles = _context.Set<ApplicationRole>();
        }



        public async Task<(IList<RoleDto> data, int total, int totalDisplay)> GetPagedRolesAsync(
            int pageIndex, int pageSize, DataTablesSearch search, string? order)
        {
            if (string.IsNullOrWhiteSpace(search.Value))
            {
                return await GetDynamicAsync(
                    null,
                    order,
                    null,  
                    pageIndex,
                    pageSize,
                    true
                );
            }
            else
            {
                return await GetDynamicAsync(
                    x => x.Name.Contains(search.Value),
                    order,
                    null, 
                    pageIndex,
                    pageSize,
                    true
                );
            }
        }

        // Define the GetDynamicAsync method here
        private async Task<(IList<RoleDto> data, int total, int totalDisplay)> GetDynamicAsync(
       Expression<Func<ApplicationRole, bool>> filter = null, // Changed filter to ApplicationRole
       string orderBy = null,
       Func<IQueryable<ApplicationRole>, IIncludableQueryable<ApplicationRole, object>> include = null,
       int pageIndex = 1,
       int pageSize = 10,
       bool isTrackingOff = false)
        {
            IQueryable<ApplicationRole> query = _roles;

            // Total count before filtering
            int total = await query.CountAsync();
            int totalDisplay = total;

            // Apply filtering if provided
            if (filter != null)
            {
                query = query.Where(filter);
                totalDisplay = await query.CountAsync();
            }

            // Apply include if needed
            if (include != null)
            {
                query = include(query);
            }

            // Apply sorting if provided
            if (!string.IsNullOrWhiteSpace(orderBy))
            {
                query = query.OrderBy(orderBy); // Requires System.Linq.Dynamic.Core for dynamic ordering
            }

            // Apply pagination
            query = query.Skip((pageIndex - 1) * pageSize).Take(pageSize);

            // Execute query with or without tracking
            var roles = isTrackingOff ? await query.AsNoTracking().ToListAsync() : await query.ToListAsync();

            // Map ApplicationRole to RoleDto
            var data = roles.Select(r => new RoleDto
            {
                Id = r.Id,
                Name = r.Name
            }).ToList();

            return (data, total, totalDisplay);
        }
    }
}

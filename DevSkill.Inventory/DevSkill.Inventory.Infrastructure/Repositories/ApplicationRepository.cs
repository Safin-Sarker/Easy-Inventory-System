using DevSkill.Inventory.Domain.Entities;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;

namespace DevSkill.Inventory.Infrastructure.Repositories
{
    public abstract class ApplicationRepository<TEntity, TDto> where TEntity : class
    {
        private readonly DbContext _context;
        private readonly DbSet<TEntity> _dbSet;

        protected ApplicationRepository(DbContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        protected async Task<(IList<TDto> data, int total, int totalDisplay)> GetPagedAsync(
        Expression<Func<TEntity, bool>> filter = null,
        string orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            int pageIndex = 1,
        int pageSize = 10,
            bool isTrackingOff = false,
            Func<TEntity, TDto> selector = null)
        {
            IQueryable<TEntity> query = _dbSet;

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
                query = query.OrderBy(orderBy);
            }

            // Apply pagination
            query = query.Skip((pageIndex - 1) * pageSize).Take(pageSize);

            // Execute query with or without tracking
            var entities = isTrackingOff ? await query.AsNoTracking().ToListAsync() : await query.ToListAsync();

            // Map TEntity to TDto
            var data = entities.Select(selector).ToList();

            return (data, total, totalDisplay);
        }
    }
}


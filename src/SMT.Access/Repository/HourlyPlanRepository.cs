using Microsoft.EntityFrameworkCore;
using SMT.Access.Data;
using SMT.Access.Repository.Base;
using SMT.Access.Repository.Interfaces;
using SMT.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SMT.Access.Repository
{
    public class HourlyPlanRepository : BaseRepository<HourlyPlan>, IHourlyPlanRepository
    {
        public HourlyPlanRepository(AppDbContext context) : base(context)
        {
        }

        public async override Task<HourlyPlan> FindAsync(Expression<Func<HourlyPlan, bool>> expression)
        {
            return await DbSet.Where(expression)
                            .Include(m => m.Line)
                            .Include(m => m.Model)
                            .ThenInclude(m => m.ProductBrand)
                            .ThenInclude(m => m.Product)
                            .Include(m => m.Model)
                            .ThenInclude(m => m.ProductBrand)
                            .ThenInclude(m => m.Brand)
                            .FirstOrDefaultAsync();
        }

        public async override Task<IEnumerable<HourlyPlan>> GetAllAsync()
        {
            return await DbSet.Include(m => m.Line)
                            .Include(m => m.Model)
                            .ThenInclude(m => m.ProductBrand)
                            .ThenInclude(m => m.Product)
                            .Include(m => m.Model)
                            .ThenInclude(m => m.ProductBrand)
                            .ThenInclude(m => m.Brand)
                            .ToListAsync();
        }

        public async Task<IEnumerable<HourlyPlan>> GetByAsync(Expression<Func<HourlyPlan, bool>> expression)
        {
            return await DbSet.Where(expression)
                            .Include(m => m.Line)
                            .Include(m => m.Model)
                            .ThenInclude(m => m.ProductBrand)
                            .ThenInclude(m => m.Product)
                            .Include(m => m.Model)
                            .ThenInclude(m => m.ProductBrand)
                            .ThenInclude(m => m.Brand)
                            .ToListAsync();
        }
    }
}

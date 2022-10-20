using Microsoft.EntityFrameworkCore;
using SMT.Access.Data;
using SMT.Access.Repository.Base;
using SMT.Access.Repository.Interfaces;
using SMT.Domain;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace SMT.Access.Repository
{
    public class PlanRepository : BaseRepository<Plan>, IPlanRepository
    {
        public PlanRepository(AppDbContext context) : base(context)
        {
        }

        public async override Task<Plan> FindAsync(Expression<Func<Plan, bool>> expression)
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

        public async override Task<IEnumerable<Plan>> GetAllAsync()
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

        public async Task<IEnumerable<Plan>> GetByAsync(Expression<Func<Plan, bool>> expression)
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

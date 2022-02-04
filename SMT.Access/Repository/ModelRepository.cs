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
    public class ModelRepository : BaseRepository<Model>, IModelRepository
    {
        public ModelRepository(AppDbContext context) : base(context)
        {
        }

        public async override Task<Model> FindAsync(Expression<Func<Model, bool>> expression) => await DbSet.Where(expression)
                            .Include(m => m.ProductBrand)
                            .ThenInclude(m => m.Product)
                            .Include(m => m.ProductBrand)
                            .ThenInclude(m => m.Brand)
                            .FirstOrDefaultAsync();

        public async override Task<IEnumerable<Model>> GetAllAsync()
        {
            return await DbSet.Include(m => m.ProductBrand)
                        .ThenInclude(m => m.Product)
                        .Include(m => m.ProductBrand)
                        .ThenInclude(m => m.Brand)
                        .ToListAsync();
        }

        public async Task<IEnumerable<Model>> GetByAsync(Expression<Func<Model, bool>> expression)
        {
            return await DbSet.Where(expression)
                            .Include(m => m.ProductBrand)
                            .ThenInclude(m => m.Product)
                            .Include(m => m.ProductBrand)
                            .ThenInclude(m => m.Brand)
                            .ToListAsync();
        }
    }
}

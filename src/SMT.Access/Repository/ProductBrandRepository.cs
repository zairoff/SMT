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
    public class ProductBrandRepository : BaseRepository<ProductBrand>, IProductBrandRepository
    {
        public ProductBrandRepository(AppDbContext context) : base(context)
        {
        }

        public async override Task<ProductBrand> FindAsync(Expression<Func<ProductBrand, bool>> expression)
        {
            return await DbSet.Include(p => p.Product).Include(p => p.Brand).Where(expression).FirstOrDefaultAsync();
        }

        public async override Task<IEnumerable<ProductBrand>> GetAllAsync()
        {
            return await DbSet.Include(p => p.Product).Include(p => p.Brand).ToListAsync();
        }

        public async Task<IEnumerable<ProductBrand>> GetByAsync(Expression<Func<ProductBrand, bool>> expression)
        {
            return await DbSet.Where(expression).Include(p => p.Product).Include(p => p.Brand).ToListAsync();
        }
    }
}

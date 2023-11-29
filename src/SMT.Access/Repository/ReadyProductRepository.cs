using SMT.Access.Data;
using SMT.Access.Repository.Base;
using SMT.Access.Repository.Interfaces;
using SMT.Domain;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Threading.Tasks;
using System;
using Microsoft.EntityFrameworkCore;

namespace SMT.Access.Repository
{
    public class ReadyProductRepository : BaseRepository<ReadyProduct>, IReadyProductRepository
    {
        public ReadyProductRepository(AppDbContext context) : base(context)
        {
        }

        public async override Task<ReadyProduct> FindAsync(Expression<Func<ReadyProduct, bool>> expression)
        {
            return await _context.ReadyProducts
                                .FromSqlRaw("SELECT * FROM dbo.ReadyProducts WITH (UPDLOCK)")
                                .Where(expression)
                                .Include(m => m.Model)
                                .ThenInclude(m => m.ProductBrand)
                                .ThenInclude(m => m.Product)
                                .Include(m => m.Model)
                                .ThenInclude(m => m.ProductBrand)
                                .ThenInclude(m => m.Brand)
                                .FirstOrDefaultAsync();
        }
                           

        public async override Task<IEnumerable<ReadyProduct>> GetAllAsync()
        {
            return await DbSet.Include(m => m.Model)
                           .ThenInclude(m => m.ProductBrand)
                           .ThenInclude(m => m.Product)
                           .Include(m => m.Model)
                           .ThenInclude(m => m.ProductBrand)
                           .ThenInclude(m => m.Brand)
                           .OrderBy(x => x.Count)
                           .ToListAsync();
        }


        public async Task<IEnumerable<ReadyProduct>> GetByAsync(Expression<Func<ReadyProduct, bool>> expression)
        {
            return await DbSet.Where(expression).Include(m => m.Model)
                           .ThenInclude(m => m.ProductBrand)
                           .ThenInclude(m => m.Product)
                           .Include(m => m.Model)
                           .ThenInclude(m => m.ProductBrand)
                           .ThenInclude(m => m.Brand)
                           .OrderBy(x => x.Count)
                           .ToListAsync();
        }
    }
}

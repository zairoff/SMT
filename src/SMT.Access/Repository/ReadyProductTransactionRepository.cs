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
    public class ReadyProductTransactionRepository : BaseRepository<ReadyProductTransaction>, IReadyProductTransactionRepository
    {
        public ReadyProductTransactionRepository(AppDbContext context) : base(context)
        {
        }

        public async override Task<ReadyProductTransaction> FindAsync(Expression<Func<ReadyProductTransaction, bool>> expression) =>
                           await DbSet.Where(expression)
                           .Include(m => m.Model)
                           .ThenInclude(m => m.ProductBrand)
                           .ThenInclude(m => m.Product)
                           .Include(m => m.Model)
                           .ThenInclude(m => m.ProductBrand)
                           .ThenInclude(m => m.Brand)
                           .FirstOrDefaultAsync();

        public async override Task<IEnumerable<ReadyProductTransaction>> GetAllAsync()
        {
            return await DbSet.Include(m => m.Model)
                           .ThenInclude(m => m.ProductBrand)
                           .ThenInclude(m => m.Product)
                           .Include(m => m.Model)
                           .ThenInclude(m => m.ProductBrand)
                           .ThenInclude(m => m.Brand)
                           .OrderBy(x => x.Date)
                           .ToListAsync();
        }


        public async Task<IEnumerable<ReadyProductTransaction>> GetByAsync(Expression<Func<ReadyProductTransaction, bool>> expression)
        {
            return await DbSet.Where(expression).Include(m => m.Model)
                           .ThenInclude(m => m.ProductBrand)
                           .ThenInclude(m => m.Product)
                           .Include(m => m.Model)
                           .ThenInclude(m => m.ProductBrand)
                           .ThenInclude(m => m.Brand)
                           .OrderBy(x => x.Date)
                           .ToListAsync();
        }
    }
}

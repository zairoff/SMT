using Microsoft.EntityFrameworkCore;
using SMT.Access.Data;
using SMT.Access.Repository.Base;
using SMT.Access.Repository.Interfaces.ReturnedProducts;
using SMT.Domain.ReturnedProducts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SMT.Access.Repository.ReturnedProducts
{
    public class ReturnedProductScanRepository : BaseRepository<ReturnedProductScan>, IReturnedProductScanRepository
    {
        public ReturnedProductScanRepository(AppDbContext context) : base(context)
        {
        }

        public async override Task<ReturnedProductScan> FindAsync(Expression<Func<ReturnedProductScan, bool>> expression) =>
                           await DbSet.Where(expression)
                           .Include(m => m.Model)
                           .FirstOrDefaultAsync();

        public async Task<IEnumerable<ReturnedProductScan>> GetByAsync(Expression<Func<ReturnedProductScan, bool>> expression)
        {
            return await DbSet.Where(expression)
                           .Include(m => m.Model)
                           .OrderBy(x => x.Id)
                           .ToListAsync();
        }
    }
}

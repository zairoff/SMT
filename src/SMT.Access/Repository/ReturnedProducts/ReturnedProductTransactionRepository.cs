using Microsoft.EntityFrameworkCore;
using SMT.Access.Data;
using SMT.Access.Repository.Base;
using SMT.Access.Repository.Interfaces.ReturnedProducts;
using SMT.Domain;
using SMT.Domain.ReturnedProducts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SMT.Access.Repository.ReturnedProducts
{
    public class ReturnedProductTransactionRepository : BaseRepository<ReturnedProductTransaction>, IReturnedProductTransactionRepository
    {
        public ReturnedProductTransactionRepository(AppDbContext context) : base(context)
        {
        }

        public async override Task<ReturnedProductTransaction> FindAsync(Expression<Func<ReturnedProductTransaction, bool>> expression) =>
                           await DbSet.Where(expression)
                           .Include(m => m.Model)
                           .ThenInclude(m => m.ProductBrand)
                           .ThenInclude(m => m.Product)
                           .Include(m => m.Model)
                           .ThenInclude(m => m.ProductBrand)
                           .ThenInclude(m => m.Brand)
                           .FirstOrDefaultAsync();

        public async override Task<IEnumerable<ReturnedProductTransaction>> GetAllAsync()
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


        public async Task<IEnumerable<ReturnedProductTransaction>> GetByAsync(Expression<Func<ReturnedProductTransaction, bool>> expression)
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

        public async Task<IEnumerable<ReturnedProductTransaction>> GetGroupByModelAsync(Expression<Func<ReturnedProductTransaction, bool>> expression)
        {
            return await DbSet.Where(expression)
                           .Select(m => new { m.Model, m.Count })
                           .GroupBy(x => new { x.Model.Id, x.Model.Name, x.Model.SapCode, x.Model.Barcode })
                           .Select(x => new ReturnedProductTransaction
                           {
                               Model = new Model { Id = x.Key.Id, Name = x.Key.Name, SapCode = x.Key.SapCode, Barcode = x.Key.Barcode },
                               Count = x.Sum(x => x.Count),
                           })
                           .OrderByDescending(x => x.Count)
                           .ToListAsync();
        }
    }
}

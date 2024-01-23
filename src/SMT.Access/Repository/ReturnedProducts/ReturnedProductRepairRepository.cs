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
    public class ReturnedProductRepairRepository : BaseRepository<ReturnedProductRepair>, IReturnedProductRepairRepository
    {
        public ReturnedProductRepairRepository(AppDbContext context) : base(context)
        {
        }

        public async override Task<ReturnedProductRepair> FindAsync(Expression<Func<ReturnedProductRepair, bool>> expression) =>
                           await DbSet.Where(expression)
                           .Include(m => m.Model)
                           .ThenInclude(m => m.ProductBrand)
                           .ThenInclude(m => m.Product)
                           .Include(m => m.Model)
                           .ThenInclude(m => m.ProductBrand)
                           .ThenInclude(m => m.Brand)
                           .FirstOrDefaultAsync();

        public async Task<int> FindSumAsync(Expression<Func<ReturnedProductRepair, bool>> expression)
        {
            return await DbSet.Where(expression).SumAsync(x => x.Count);
        }

        public async override Task<IEnumerable<ReturnedProductRepair>> GetAllAsync()
        {
            return await DbSet.Include(m => m.Model)
                           .ThenInclude(m => m.ProductBrand)
                           .ThenInclude(m => m.Product)
                           .Include(m => m.Model)
                           .ThenInclude(m => m.ProductBrand)
                           .ThenInclude(m => m.Brand)
                           .ToListAsync();
        }

        public async Task<IEnumerable<ReturnedProductRepair>> GetGroupByModelAsync()
        {
            return await DbSet
                           .Select(m => new { m.Model, m.Count })
                           .GroupBy(x => new { x.Model.Id, x.Model.Name, x.Model.SapCode })
                           .Select(x => new ReturnedProductRepair
                           {
                               Model = new Model { Id = x.Key.Id, Name = x.Key.Name, SapCode = x.Key.SapCode },
                               Count = x.Sum(x => x.Count),
                           })
                           .OrderByDescending(x => x.Count)
                           .ToListAsync();
        }
    }
}

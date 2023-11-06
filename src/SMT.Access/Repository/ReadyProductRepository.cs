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

        public async Task<IEnumerable<ReadyProduct>> GetByAsync(Expression<Func<ReadyProduct, bool>> expression)
        {
            return await DbSet.Where(expression).OrderBy(x => x.Enter).ToListAsync();
        }
    }
}

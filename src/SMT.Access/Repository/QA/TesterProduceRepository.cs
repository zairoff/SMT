using Microsoft.EntityFrameworkCore;
using SMT.Access.Data;
using SMT.Access.Repository.Base;
using SMT.Access.Repository.Interfaces.QA;
using SMT.Domain.QA;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System;
using System.Linq;

namespace SMT.Access.Repository.QA
{
    public class TesterProduceRepository : BaseRepository<TesterProduce>, ITesterProduceRepository
    {
        public TesterProduceRepository(AppDbContext context) : base(context)
        {
        }

        public async override Task<TesterProduce> FindAsync(Expression<Func<TesterProduce, bool>> expression)
        {
            return await DbSet.Include(e => e.Tester).Include(x => x.Employee).Where(expression).FirstOrDefaultAsync();
        }

        public async override Task<IEnumerable<TesterProduce>> GetAllAsync()
        {
            return await DbSet.Include(e => e.Tester).Include(x => x.Employee).ToListAsync();
        }
    }
}

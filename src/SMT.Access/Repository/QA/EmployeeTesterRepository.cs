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
    public class EmployeeTesterRepository : BaseRepository<EmployeeTester>, IEmployeeTesterRepository
    {
        public EmployeeTesterRepository(AppDbContext context) : base(context)
        {
        }

        public async override Task<EmployeeTester> FindAsync(Expression<Func<EmployeeTester, bool>> expression)
        {
            return await DbSet.Include(e => e.Employee).Where(expression).FirstOrDefaultAsync();
        }

        public async override Task<IEnumerable<EmployeeTester>> GetAllAsync()
        {
            return await DbSet.Include(e => e.Employee).ToListAsync();
        }
    }
}

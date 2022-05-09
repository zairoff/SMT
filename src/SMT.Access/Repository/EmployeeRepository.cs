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
    public class EmployeeRepository : BaseRepository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(AppDbContext context) : base(context)
        {
        }

        public async override Task<Employee> FindAsync(Expression<Func<Employee, bool>> expression)
        {
            return await DbSet.Include(e => e.Department).Where(expression).FirstOrDefaultAsync();
        }

        public async override Task<IEnumerable<Employee>> GetAllAsync()
        {
            return await DbSet.Include(e => e.Department).ToListAsync();
        }

        public async Task<IEnumerable<Employee>> GetByAsync(Expression<Func<Employee, bool>> expression)
        {
            return await DbSet.Where(expression).Include(e => e.Department).ToListAsync();
        }
    }
}

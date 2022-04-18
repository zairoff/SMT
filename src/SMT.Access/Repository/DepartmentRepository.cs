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
    public class DepartmentRepository : BaseRepository<Department>, IDepartmentRepository
    {
        public DepartmentRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Department>> GetByHierarchyIdsync(Expression<Func<Department, bool>> expression)
        {
            return await DbSet.Where(expression).ToListAsync();
        }
    }
}

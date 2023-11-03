using SMT.Access.Repository.Base;
using SMT.Domain;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SMT.Access.Repository.Interfaces
{
    public interface IDepartmentRepository : IBaseRepository<Department>
    {
        public Task<IEnumerable<Department>> GetByHierarchyIdsync(string departmentId);

        Task<IEnumerable<Department>> GetByAsync(Expression<Func<Department, bool>> expression);
    }
}

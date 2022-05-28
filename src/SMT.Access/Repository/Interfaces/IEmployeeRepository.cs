using SMT.Access.Repository.Base;
using SMT.Domain;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SMT.Access.Repository.Interfaces
{
    public interface IEmployeeRepository : IBaseRepository<Employee>
    {
        Task<IEnumerable<Employee>> GetByAsync(Expression<Func<Employee, bool>> expression);

        Task<IEnumerable<Employee>> GetByDepartmentAsync(string departmentId, bool isActive);
    }
}

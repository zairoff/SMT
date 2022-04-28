using SMT.Access.Repository.Base;
using SMT.Domain;
using System.Threading.Tasks;

namespace SMT.Access.Repository.Interfaces
{
    public interface IDepartmentRepository : IBaseRepository<Department>
    {
        public Task<string> GetByHierarchyIdsync(string departmentId, int level);
    }
}

using SMT.Access.Repository.Base;
using SMT.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SMT.Access.Repository.Interfaces
{
    public interface IComponentRepository : IBaseRepository<Component>
    {
        Task<IEnumerable<Component>> GetComponentsAsync(int page, int pageSize);
    }
}

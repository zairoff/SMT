using SMT.Access.Repository.Base;
using SMT.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SMT.Access.Repository.Interfaces
{
    public interface IComponentRequestItemRepository : IBaseRepository<ComponentRequestItem>
    {
        Task<IEnumerable<ComponentRequestItem>> GetByIdsAsync(IEnumerable<int> ids);
    }
}

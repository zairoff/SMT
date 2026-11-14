using SMT.Access.Repository.Base;
using SMT.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SMT.Access.Repository.Interfaces
{
    public interface IComponentRepository : IBaseRepository<Component>
    {
        Task<IEnumerable<Component>> GetComponentsAsync(int page, int pageSize);

        Task<Component> GetByPartNumberAsync(string partNumber);

        Task<HashSet<string>> GetAllPartNumbersAsync();

        // All active components, untracked - for matching many codes (incl. alternate part numbers,
        // which GetByPartNumberAsync can't find since it only looks at the primary one) in memory.
        Task<List<Component>> GetActiveAsync();

        Task<List<Component>> GetByIdsAsync(IEnumerable<int> ids);
    }
}

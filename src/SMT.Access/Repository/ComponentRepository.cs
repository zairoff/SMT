using Microsoft.EntityFrameworkCore;
using SMT.Access.Data;
using SMT.Access.Repository.Base;
using SMT.Access.Repository.Interfaces;
using SMT.Domain;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMT.Access.Repository
{
    public class ComponentRepository : BaseRepository<Component>, IComponentRepository
    {
        public ComponentRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Component>> GetComponentsAsync(int page, int pageSize)
        {
            return await DbSet.Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<Component> GetByPartNumberAsync(string partNumber)
        {
            return await _context.Components
                    .FromSqlRaw("SELECT * FROM Components WHERE IsActive = 1 AND JSON_VALUE(PartNumber, '$[0]') = {0}", partNumber)
                    .OrderBy(c => c.Id)
                    .FirstOrDefaultAsync();
        }

        public async Task<HashSet<string>> GetAllPartNumbersAsync()
        {
            var partNumbers = await _context.Components
                .Where(c => c.IsActive)
                .Select(c => c.PartNumber)
                .ToListAsync();

            return partNumbers
                .Where(pn => pn != null && pn.Count > 0)
                .Select(pn => pn[0])
                .ToHashSet();
        }
    }
}

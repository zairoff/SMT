using Microsoft.EntityFrameworkCore;
using SMT.Access.Data;
using SMT.Access.Repository.Base;
using SMT.Domain.BoardFlow.V2;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMT.Access.Repository.BoardFlowV2
{
    public class QrReaderV2LinkRepository : BaseRepository<QrReaderV2Link>, IQrReaderV2LinkRepository
    {
        public QrReaderV2LinkRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<QrReaderV2Link>> GetByToReaderAsync(int toReaderId)
        {
            return await DbSet.Where(l => l.ToReaderId == toReaderId)
                .Include(l => l.FromReader)
                    .ThenInclude(r => r.Line)
                .ToListAsync();
        }

        public Task<bool> HasOutgoingAsync(int fromReaderId)
        {
            return DbSet.AnyAsync(l => l.FromReaderId == fromReaderId);
        }
    }
}

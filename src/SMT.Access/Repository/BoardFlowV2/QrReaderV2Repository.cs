using Microsoft.EntityFrameworkCore;
using SMT.Access.Data;
using SMT.Access.Repository.Base;
using SMT.Domain.BoardFlow.V2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SMT.Access.Repository.BoardFlowV2
{
    public class QrReaderV2Repository : BaseRepository<QrReaderV2>, IQrReaderV2Repository
    {
        public QrReaderV2Repository(AppDbContext context) : base(context)
        {
        }

        public async override Task<QrReaderV2> FindAsync(Expression<Func<QrReaderV2, bool>> expression)
        {
            return await DbSet.Include(q => q.Line)
                .Where(expression)
                .FirstOrDefaultAsync();
        }

        public async override Task<IEnumerable<QrReaderV2>> GetAllAsync()
        {
            return await DbSet.Include(q => q.Line)
                .OrderBy(q => q.LineId)
                .ThenBy(q => q.Position)
                .ToListAsync();
        }

        public async Task<IEnumerable<QrReaderV2>> GetByAsync(Expression<Func<QrReaderV2, bool>> expression)
        {
            return await DbSet.Where(expression)
                .Include(q => q.Line)
                .OrderBy(q => q.LineId)
                .ThenBy(q => q.Position)
                .ToListAsync();
        }
    }
}

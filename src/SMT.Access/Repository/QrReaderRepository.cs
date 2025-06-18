using Microsoft.EntityFrameworkCore;
using SMT.Access.Data;
using SMT.Access.Repository.Base;
using SMT.Access.Repository.Interfaces;
using SMT.Domain.BoardFlow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SMT.Access.Repository
{
    public class QrReaderRepository : BaseRepository<QrReader>, IQrReaderRepository
    {
        public QrReaderRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<QrReader>> GetByAsync(Expression<Func<QrReader, bool>> expression)
        {
            return await DbSet.Where(expression).OrderBy(x => x.Position).ToListAsync();
        }
    }
}

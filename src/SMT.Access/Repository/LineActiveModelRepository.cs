using Microsoft.EntityFrameworkCore;
using SMT.Access.Data;
using SMT.Access.Repository.Base;
using SMT.Access.Repository.Interfaces;
using SMT.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SMT.Access.Repository
{
    public class LineActiveModelRepository : BaseRepository<LineActiveModel>, ILineActiveModelRepository
    {
        public LineActiveModelRepository(AppDbContext context) : base(context)
        {

        }

        public async override Task<LineActiveModel> FindAsync(Expression<Func<LineActiveModel, bool>> expression)
        {
            return await DbSet.Include(l => l.Line)
                .Include(l => l.Model)
                .Where(expression)
                .FirstOrDefaultAsync();
        }

        public async override Task<IEnumerable<LineActiveModel>> GetAllAsync()
        {
            return await DbSet.Include(l => l.Line)
                .Include(l => l.Model)
                .ToListAsync();
        }

        public async Task<LineActiveModel> GetByLineAsync(int lineId)
        {
            return await FindAsync(l => l.LineId == lineId);
        }
    }
}

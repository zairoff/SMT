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
    public class LineDefectRepository : BaseRepository<LineDefect>, ILineDefectRepository
    {
        public LineDefectRepository(AppDbContext context) : base(context)
        {
        }

        public async override Task<LineDefect> FindAsync(Expression<Func<LineDefect, bool>> expression)
        {
            return await DbSet.Include(p => p.Line).Include(p => p.Defect).Where(expression).FirstOrDefaultAsync();
        }

        public async override Task<IEnumerable<LineDefect>> GetAllAsync()
        {
            return await DbSet.Include(p => p.Line).Include(p => p.Defect).OrderBy(x => x.Defect.Name).ToListAsync();
        }

        public async Task<IEnumerable<LineDefect>> GetByAsync(Expression<Func<LineDefect, bool>> expression)
        {
            return await DbSet.Where(expression).Include(p => p.Line).Include(p => p.Defect).OrderBy(x => x.Defect.Name).ToListAsync();
        }
    }
}

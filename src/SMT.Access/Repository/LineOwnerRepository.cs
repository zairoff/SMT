using Microsoft.EntityFrameworkCore;
using SMT.Access.Data;
using SMT.Access.Repository.Base;
using SMT.Access.Repository.Interfaces;
using SMT.Domain;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System;
using System.Linq;

namespace SMT.Access.Repository
{
    public class LineOwnerRepository : BaseRepository<LineOwner>, ILineOwnerRepository
    {
        public LineOwnerRepository(AppDbContext context) : base(context)
        {
        }

        public async override Task<LineOwner> FindAsync(Expression<Func<LineOwner, bool>> expression)
        {
            return await DbSet.Include(e => e.Employee)
                .Include(x => x.Line)
                .Where(expression)
                .FirstOrDefaultAsync();
        }

        public async override Task<IEnumerable<LineOwner>> GetAllAsync()
        {
            return await DbSet.Include(e => e.Employee)
                .Include(x => x.Line)
                .ToListAsync();
        }

        public async Task<IEnumerable<LineOwner>> GetByAsync(Expression<Func<LineOwner, bool>> expression)
        {
            return await DbSet.Where(expression)
                .Include(e => e.Line)
                .Include(e => e.Employee)
                .ToListAsync();
        }
    }
}

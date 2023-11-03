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
    public class LineRepository : BaseRepository<Line>, ILineRepository
    {
        public LineRepository(AppDbContext context) : base(context)
        {

        }

        public async Task<IEnumerable<Line>> GetByAsync(Expression<Func<Line, bool>> expression)
        {
            return await DbSet.Where(expression).OrderBy(x => x.Name).ToListAsync();
        }

        public override async Task<IEnumerable<Line>> GetAllAsync()
        {
            return await DbSet.OrderBy(x => x.Name).ToListAsync();
        }
    }
}

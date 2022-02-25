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
    public class DefectRepository : BaseRepository<Defect>, IDefectRepository
    {
        public DefectRepository(AppDbContext context) : base(context)
        {
        }

        public async override Task<Defect> FindAsync(Expression<Func<Defect, bool>> expression)
        {
            return await DbSet.Where(expression).Include(d => d.Line).FirstOrDefaultAsync();
        }

        public async override Task<IEnumerable<Defect>> GetAllAsync()
        {
            return await DbSet.Include(d => d.Line).ToListAsync();
        }
    }
}

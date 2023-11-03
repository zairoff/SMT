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

        public override async Task<IEnumerable<Defect>> GetAllAsync()
        {
            return await DbSet.OrderBy(x => x.Name).ToListAsync();
        }

        public async Task<IEnumerable<Defect>> GetByAsync(Expression<Func<Defect, bool>> expression)
        {
            return await DbSet.Where(expression).OrderBy(x => x.Name).ToListAsync();
        }
    }
}

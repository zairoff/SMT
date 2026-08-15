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
    public class ModelInstructionImageRepository : BaseRepository<ModelInstructionImage>, IModelInstructionImageRepository
    {
        public ModelInstructionImageRepository(AppDbContext context) : base(context)
        {

        }

        public async override Task<ModelInstructionImage> FindAsync(Expression<Func<ModelInstructionImage, bool>> expression)
        {
            return await DbSet.Include(i => i.Model)
                .Include(i => i.InstructionPosition)
                .Where(expression)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<ModelInstructionImage>> GetByAsync(Expression<Func<ModelInstructionImage, bool>> expression)
        {
            return await DbSet.Where(expression)
                .Include(i => i.Model)
                .Include(i => i.InstructionPosition)
                .ToListAsync();
        }
    }
}

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
    public class PcbInstructionRepository : BaseRepository<PcbInstruction>, IPcbInstructionRepository
    {
        public PcbInstructionRepository(AppDbContext context) : base(context)
        {

        }

        public async Task<IEnumerable<PcbInstruction>> GetByAsync(Expression<Func<PcbInstruction, bool>> expression)
        {
            return await DbSet.Where(expression).ToListAsync();
        }
    }
}

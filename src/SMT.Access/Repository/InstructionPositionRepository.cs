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
    public class InstructionPositionRepository : BaseRepository<InstructionPosition>, IInstructionPositionRepository
    {
        public InstructionPositionRepository(AppDbContext context) : base(context)
        {

        }

        public async override Task<InstructionPosition> FindAsync(Expression<Func<InstructionPosition, bool>> expression)
        {
            return await DbSet.Include(p => p.Line)
                .Where(expression)
                .FirstOrDefaultAsync();
        }

        public async override Task<IEnumerable<InstructionPosition>> GetAllAsync()
        {
            return await DbSet.Include(p => p.Line)
                .OrderBy(p => p.LineId)
                .ThenBy(p => p.Order)
                .ToListAsync();
        }

        public async Task<IEnumerable<InstructionPosition>> GetByAsync(Expression<Func<InstructionPosition, bool>> expression)
        {
            return await DbSet.Where(expression)
                .Include(p => p.Line)
                .OrderBy(p => p.LineId)
                .ThenBy(p => p.Order)
                .ToListAsync();
        }

        public async Task<IEnumerable<InstructionPosition>> GetByLineAsync(int lineId)
        {
            return await GetByAsync(p => p.LineId == lineId);
        }
    }
}

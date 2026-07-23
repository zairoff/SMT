using Microsoft.EntityFrameworkCore;
using SMT.Access.Data;
using SMT.Access.Repository.Base;
using SMT.Domain.BoardFlow.V2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMT.Access.Repository.BoardFlowV2
{
    public class BoardMovementV2Repository : BaseRepository<BoardMovementV2>, IBoardMovementV2Repository
    {
        public BoardMovementV2Repository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<BoardMovementV2>> GetByBoardAsync(int boardId)
        {
            return await DbSet.Where(m => m.BoardId == boardId)
                .Include(m => m.QrReader)
                .OrderBy(m => m.DateTime)
                .ToListAsync();
        }

        public async Task<IEnumerable<BoardMovementV2>> GetByDateAsync(DateTime date)
        {
            return await DbSet.Where(m => m.DateTime.Date == date.Date)
                .Include(m => m.Board)
                    .ThenInclude(b => b.Model)
                .Include(m => m.QrReader)
                    .ThenInclude(r => r.Line)
                .OrderByDescending(m => m.DateTime)
                .ToListAsync();
        }
    }
}

using Microsoft.EntityFrameworkCore;
using SMT.Access.Data;
using SMT.Access.Repository.Base;
using SMT.Access.Repository.Interfaces;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Threading.Tasks;
using System;
using SMT.Domain.BoardFlow;

namespace SMT.Access.Repository
{
    public class BoardReportRepository : BaseRepository<BoardReport>, IBoardReportRepository
    {
        public BoardReportRepository(AppDbContext context) : base(context)
        {
        }

        public async override Task<BoardReport> FindAsync(Expression<Func<BoardReport, bool>> expression)
        {
            return await DbSet.Include(p => p.QrReader)
                .Include(x => x.Model)
                .Where(expression)
                .FirstOrDefaultAsync();
        }

        public async override Task<IEnumerable<BoardReport>> GetAllAsync()
        {
            return await DbSet.Include(p => p.QrReader)
                .Include(x => x.Model)
                .OrderBy(x => x.QrReader.Position)
                .ToListAsync();
        }

        public async Task<IEnumerable<BoardReport>> GetByAsync(Expression<Func<BoardReport, bool>> expression)
        {
            return await DbSet.Where(expression)
                .Include(p => p.QrReader)
                .Include(x => x.Model)
                .OrderBy(x => x.QrReader.Position)
                .ToListAsync();
        }
    }
}

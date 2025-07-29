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
        private readonly AppDbContext appDbContext;

        public BoardReportRepository(AppDbContext context) : base(context)
        {
            appDbContext = context;
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

        public async Task<IReadOnlyCollection<BoardFlowReport>> GetBoardFlowReportsAsync(DateTime from, DateTime to)
        {
            var boardReport = await DbSet
                .Where(x => x.DateTime.Date >= from.Date && x.DateTime.Date <= to.Date && x.Status == BoardPassStatus.Passed)
                .GroupBy(br => new { br.QrReaderId })
                .Select(g => new
                {
                    ReaderId = g.Key.QrReaderId,
                    PassedBoards = g.Select(x => x.QrCode).Distinct().Count()
                })
                .Join(
                    appDbContext.QrReaders,                             // Table to join
                    br => br.ReaderId,                             // Key from first table
                    qr => qr.Id,                                   // Key from second table
                    (br, qr) => new                                // Result projection
                    {
                        br.ReaderId,
                        ReaderName = qr.Name,
                        br.PassedBoards
                    }
                )
                .OrderBy(x => x.ReaderId)
                .ToListAsync();

            // Calculate Missing and Previous Passed
            return boardReport
                .Select((current, index) => new BoardFlowReport
                {
                    ReaderId = current.ReaderId,
                    ReaderName = current.ReaderName,  // Pass name here
                    Passed = current.PassedBoards,
                    PreviousPassed = index == 0 ? 0 : boardReport[index - 1].PassedBoards,
                    Missing = index == 0 ? 0 : boardReport[index - 1].PassedBoards - current.PassedBoards
                })
                .ToList();
        }

        public async Task<IReadOnlyCollection<BoardReport>> GetMissingAsync(int readerId, DateTime from, DateTime to)
        {
            var passedBoards = await DbSet
                .Where(x => x.QrReaderId == readerId && x.DateTime.Date >= from.Date && x.DateTime.Date <= to.Date && x.Status == BoardPassStatus.Passed)
                .Include(x => x.QrReader)
                .Include(x => x.Model)
                .Distinct()
                .ToListAsync();

            var previousReaderId = readerId - 1;

            var previousReaderBoards = await DbSet
                .Where(x => x.QrReaderId == previousReaderId && x.DateTime.Date >= from.Date && x.DateTime.Date <= to.Date && x.Status == BoardPassStatus.Passed)
                .Include(x => x.QrReader)
                .Include(x => x.Model)
                .Distinct()
                .ToListAsync();

            return previousReaderBoards.Except(passedBoards).ToList();
        }

        public async Task<IReadOnlyCollection<BoardReport>> GetPassedAsync(int readerId, DateTime from, DateTime to)
        {
            return await DbSet
                .Where(x => x.QrReaderId == readerId && x.DateTime.Date >= from.Date && x.DateTime.Date <= to.Date && x.Status == BoardPassStatus.Passed)
                .Include(x => x.QrReader)
                .Include(x => x.Model)
                .Distinct()
                .ToListAsync();
        }
    }
}

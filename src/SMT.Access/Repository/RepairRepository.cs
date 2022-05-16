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
    public class RepairRepository : BaseRepository<Repair>, IRepairRepository
    {
        public RepairRepository(AppDbContext context) : base(context)
        {
        }

        public override async Task<Repair> FindAsync(Expression<Func<Repair, bool>> expression)
        {
            return await DbSet.Where(expression)
                        .Include(r => r.Employee)
                        .Include(r => r.Report)
                        .ThenInclude(r => r.Line)
                        .Include(r => r.Report)
                        .ThenInclude(r => r.Model)
                        .Include(r => r.Report)
                        .ThenInclude(r => r.Defect)
                        .FirstOrDefaultAsync();
        }

        public async override Task<IEnumerable<Repair>> GetAllAsync()
        {
            return await DbSet.Include(r => r.Employee)
                            .Include(r => r.Report)
                            .ThenInclude(r => r.Line)
                            .Include(r => r.Report)
                            .ThenInclude(r => r.Model)
                            .Include(r => r.Report)
                            .ThenInclude(r => r.Defect)
                            .ToListAsync();
        }

        public async Task<IEnumerable<Repair>> GetByAsync(Expression<Func<Repair, bool>> expression)
        {
            return await DbSet.Where(expression)
                        .Include(r => r.Employee)
                        .Include(r => r.Report)
                        .ThenInclude(r => r.Line)
                        .Include(r => r.Report)
                        .ThenInclude(r => r.Model)
                        .Include(r => r.Report)
                        .ThenInclude(r => r.Defect)
                        .ToListAsync();
        }
    }
}

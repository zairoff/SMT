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
    public class RepairAuditRepository : BaseRepository<RepairAudit>, IRepairAuditRepository
    {
        public RepairAuditRepository(AppDbContext context) : base(context)
        {
        }

        public async override Task<RepairAudit> FindAsync(Expression<Func<RepairAudit, bool>> expression)
        {
            return await DbSet.Where(expression)
                            .Include(a => a.Report).ThenInclude(r => r.Model)
                            .Include(a => a.Report).ThenInclude(r => r.Line)
                            .FirstOrDefaultAsync();
        }

        public async Task<RepairAudit> FindByBarcodeAsync(string barcode)
        {
            return await FindAsync(a => a.Barcode == barcode);
        }

        public async override Task<IEnumerable<RepairAudit>> GetAllAsync()
        {
            return await DbSet.Include(a => a.Report).ThenInclude(r => r.Model)
                            .Include(a => a.Report).ThenInclude(r => r.Line)
                            .ToListAsync();
        }

        public async Task<IEnumerable<RepairAudit>> GetByAsync(Expression<Func<RepairAudit, bool>> expression)
        {
            return await DbSet.Where(expression)
                            .Include(a => a.Report).ThenInclude(r => r.Model)
                            .Include(a => a.Report).ThenInclude(r => r.Line)
                            .ToListAsync();
        }
    }
}

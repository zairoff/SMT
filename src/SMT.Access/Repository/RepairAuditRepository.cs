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
                            .Include(a => a.Model)
                            .FirstOrDefaultAsync();
        }

        public async Task<RepairAudit> FindByBarcodeAsync(string barcode, RepairAuditType type)
        {
            return await FindAsync(a => a.Barcode == barcode && a.Type == type);
        }

        public async override Task<IEnumerable<RepairAudit>> GetAllAsync()
        {
            return await DbSet.Include(a => a.Model)
                            .ToListAsync();
        }

        public async Task<IEnumerable<RepairAudit>> GetByAsync(Expression<Func<RepairAudit, bool>> expression)
        {
            return await DbSet.Where(expression)
                            .Include(a => a.Model)
                            .ToListAsync();
        }
    }
}

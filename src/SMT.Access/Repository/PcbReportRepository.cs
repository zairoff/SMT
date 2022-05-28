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
    public class PcbReportRepository : BaseRepository<PcbReport>, IPcbReportRepository
    {
        public PcbReportRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<int> CountAsync(Expression<Func<PcbReport, bool>> expression)
        {
            return await DbSet.Where(expression).CountAsync();
        }

        public async override Task<PcbReport> FindAsync(Expression<Func<PcbReport, bool>> expression)
        {
            return await DbSet.Where(expression)
                                    .Include(r => r.Employee)
                                    .Include(r => r.Model)
                                    .ThenInclude(r => r.ProductBrand)
                                    .ThenInclude(r => r.Product)
                                    .Include(r => r.Model)
                                    .ThenInclude(r => r.ProductBrand)
                                    .ThenInclude(r => r.Brand)
                                    .Include(r => r.Model)
                                    .Include(r => r.Defect)
                                    .Include(r => r.Line)
                                    .FirstOrDefaultAsync();
        }

        public async override Task<IEnumerable<PcbReport>> GetAllAsync()
        {
            return await DbSet.Include(r => r.Employee).Include(r => r.Model)
                                    .ThenInclude(r => r.ProductBrand)
                                    .ThenInclude(r => r.Product)
                                    .Include(r => r.Model)
                                    .ThenInclude(r => r.ProductBrand)
                                    .ThenInclude(r => r.Brand)
                                    .Include(r => r.Model)
                                    .Include(r => r.Defect)
                                    .Include(r => r.Line)
                                    .ToListAsync();
        }

        public async Task<IEnumerable<PcbReport>> GetByAsync(Expression<Func<PcbReport, bool>> expression)
        {
            return await DbSet.Where(expression)
                                    .Include(r => r.Employee)
                                    .Include(r => r.Model)
                                    .ThenInclude(r => r.ProductBrand)
                                    .ThenInclude(r => r.Product)
                                    .Include(r => r.Model)
                                    .ThenInclude(r => r.ProductBrand)
                                    .ThenInclude(r => r.Brand)
                                    .Include(r => r.Model)
                                    .Include(r => r.Defect)
                                    .Include(r => r.Line)
                                    .ToListAsync();
        }
    }
}

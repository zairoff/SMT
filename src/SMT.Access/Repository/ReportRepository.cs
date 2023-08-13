﻿using Microsoft.EntityFrameworkCore;
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
    public class ReportRepository : BaseRepository<Report>, IReportRepository
    {
        public ReportRepository(AppDbContext context) : base(context)
        {

        }

        public async override Task<Report> FindAsync(Expression<Func<Report, bool>> expression) =>
                            await DbSet.Where(expression)
                            .Include(m => m.Defect)
                            .Include(m => m.Line)
                            .Include(m => m.Model)
                            .FirstOrDefaultAsync();

        public async override Task<IEnumerable<Report>> GetAllAsync()
        {
            return await DbSet.Include(m => m.Defect)
                            .Include(m => m.Line)
                            .Include(m => m.Model)
                            .ToListAsync();
        }

        public async Task<IEnumerable<Report>> GetByAsync(Expression<Func<Report, bool>> expression)
        {
            return await DbSet.Where(expression)
                            .Include(m => m.Defect)
                            .Include(m => m.Line)
                            .Include(m => m.Model)
                            .OrderByDescending(m => m.CreatedDate)
                            .ToListAsync();
        }

        public async Task<IEnumerable<Report>> GetByOrderAsync(Expression<Func<Report, bool>> expression)
        {
            return await DbSet.Where(expression)
                            .Include(m => m.Defect)
                            .Include(m => m.Line)
                            .Include(m => m.Model)
                            .OrderByDescending(m => m.Id)
                            .ToListAsync();
        }
    }
}

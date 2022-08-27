using Microsoft.EntityFrameworkCore;
using SMT.Access.Data;
using SMT.Domain.Statics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMT.Access.Repository.Statics
{
    public class StaticsRepository : IStaticsRepository
    {
        private readonly AppDbContext _context;

        public StaticsRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<StaticsModel>> GroupByBrandAsync(DateTime from, DateTime to)
        {
            return await _context.Reports.Where(r => r.CreatedDate >= from && r.CreatedDate <= to)
                                       .Select(r => r.Model.ProductBrand.Brand)
                                           .GroupBy(g => g.Name)
                                           .Select(g => new StaticsModel
                                           {
                                               Name = g.Key,
                                               Count = g.Count()
                                           })
                                           .OrderByDescending(o => o.Count)
                                           .ToListAsync();
        }

        public async Task<IEnumerable<StaticsModel>> GroupByLineAsync(DateTime from, DateTime to)
        {
            return await _context.Reports.Where(r => r.CreatedDate >= from && r.CreatedDate <= to)
                                       .Select(r => r.Line)
                                           .GroupBy(g => g.Name)
                                           .Select(g => new StaticsModel
                                           {
                                               Name = g.Key,
                                               Count = g.Count()
                                           })
                                           .OrderByDescending(o => o.Count)
                                           .ToListAsync();
        }

        public async Task<IEnumerable<StaticsModel>> GroupByModelAsync(DateTime from, DateTime to)
        {
            //return await _context.Reports.GroupBy(r => r.ModelId)
            //                            .Select(g => new StaticsModel
            //                                        {
            //                                            Model = g.AsEnumerable().Select(g => g.Model.Name).FirstOrDefault(),
            //                                            Count = g.Count()
            //                                        }).ToListAsync();

           return await _context.Reports.Where(r => r.CreatedDate >= from && r.CreatedDate <= to)
                                        .Select(r => r.Model)
                                            .GroupBy(g => g.Name)
                                            .Select(g => new StaticsModel
                                                { 
                                                    Name = g.Key,
                                                    Count = g.Count()
                                                })
                                            .OrderByDescending(o => o.Count)
                                            .ToListAsync();
        }

        public async Task<IEnumerable<StaticsModel>> GroupByProductAsync(DateTime from, DateTime to)
        {
            return await _context.Reports.Where(r => r.CreatedDate >= from && r.CreatedDate <= to)
                                       .Select(r => r.Model.ProductBrand.Product)
                                           .GroupBy(g => g.Name)
                                           .Select(g => new StaticsModel
                                           {
                                               Name = g.Key,
                                               Count = g.Count()
                                           })
                                           .OrderByDescending(o => o.Count)
                                           .ToListAsync();
        }

        public async Task<IEnumerable<StaticsModel>> GroupByDefectAsync(DateTime from, DateTime to)
        {
            return await _context.Reports.Where(r => r.CreatedDate >= from && r.CreatedDate <= to)
                                       .Select(r => r.Defect)
                                           .GroupBy(g => g.Name)
                                           .Select(g => new StaticsModel
                                           {
                                               Name = g.Key,
                                               Count = g.Count()
                                           })
                                           .OrderByDescending(o => o.Count)
                                           .ToListAsync();
        }
    }
}

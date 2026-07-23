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
            return await _context.Reports.Where(r => r.CreatedDate.Date >= from.Date && r.CreatedDate.Date <= to.Date)
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
            return await _context.Reports.Where(r => r.CreatedDate.Date >= from.Date && r.CreatedDate.Date <= to.Date)
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

           return await _context.Reports.Where(r => r.CreatedDate.Date >= from.Date && r.CreatedDate.Date <= to.Date)
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
            return await _context.Reports.Where(r => r.CreatedDate.Date >= from.Date && r.CreatedDate.Date <= to.Date)
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
            return await _context.Reports.Where(r => r.CreatedDate.Date >= from.Date && r.CreatedDate.Date <= to.Date)
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

        public async Task<IEnumerable<StaticsModel>> GroupByDefectAsync(int lineId, DateTime from, DateTime to)
        {
            return await _context.Reports.Where(r => r.LineId == lineId && r.CreatedDate.Date >= from.Date && r.CreatedDate.Date <= to.Date)
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

        public async Task<StaticsModel> GroupByDefectAsync(int lineId, string name, bool status, DateTime from, DateTime to)
        {
            var count = await _context.Reports.Where(r => r.LineId == lineId && r.Status == status && r.CreatedDate.Date >= from.Date && r.CreatedDate.Date <= to.Date)
                                           .CountAsync();

            return new StaticsModel { Name = name, Count = count };
        }

        // GetAll
        public async Task<StaticsModel> GroupByDefectAsync(int lineId, string name, DateTime from, DateTime to)
        {
            var count = await _context.Reports.Where(r => r.LineId == lineId && r.CreatedDate.Date >= from.Date && r.CreatedDate.Date <= to.Date)
                                           .CountAsync();

            return new StaticsModel { Name = name, Count = count };
        }

        public async Task<IEnumerable<PlanStaticsModel>> GetPlanStaticsAsync(DateTime from, DateTime to, int? lineId = null)
        {
            var grouped = await _context.Plans
                                       .Where(p => p.Date.Date >= from.Date && p.Date.Date <= to.Date
                                                && (lineId == null || p.LineId == lineId))
                                       .GroupBy(p => p.Date.Date)
                                       .Select(g => new PlanStaticsModel
                                       {
                                           Date = g.Key,
                                           Planned = g.Sum(p => p.RequiredCount),
                                           Produced = g.Sum(p => p.ProducedCount)
                                       })
                                       .ToListAsync();

            var result = new List<PlanStaticsModel>();
            for (var date = from.Date; date <= to.Date; date = date.AddDays(1))
            {
                var existing = grouped.FirstOrDefault(g => g.Date == date);

                result.Add(existing ?? new PlanStaticsModel { Date = date, Planned = 0, Produced = 0 });
            }

            return result;
        }

        public async Task<IEnumerable<QualityStaticsModel>> GetQualityStaticsAsync(DateTime from, DateTime to, int? lineId = null)
        {
            var produced = await _context.Plans
                                       .Where(p => p.Date.Date >= from.Date && p.Date.Date <= to.Date
                                                && (lineId == null || p.LineId == lineId))
                                       .GroupBy(p => p.Date.Date)
                                       .Select(g => new { Date = g.Key, Produced = g.Sum(p => p.ProducedCount) })
                                       .ToListAsync();

            var defects = await _context.Reports
                                       .Where(r => r.CreatedDate.Date >= from.Date && r.CreatedDate.Date <= to.Date
                                                && (lineId == null || r.LineId == lineId))
                                       .GroupBy(r => r.CreatedDate.Date)
                                       .Select(g => new { Date = g.Key, Count = g.Count() })
                                       .ToListAsync();

            var result = new List<QualityStaticsModel>();
            for (var date = from.Date; date <= to.Date; date = date.AddDays(1))
            {
                var producedCount = produced.FirstOrDefault(p => p.Date == date)?.Produced ?? 0;
                var defectCount = defects.FirstOrDefault(d => d.Date == date)?.Count ?? 0;

                var ftq = producedCount > 0 ? 100 - (defectCount * 100.0 / producedCount) : 0;

                result.Add(new QualityStaticsModel { Date = date, Ftq = Math.Round(ftq, 2) });
            }

            return result;
        }

        public async Task<IEnumerable<PlanDetailStaticsModel>> GetPlanDetailsByDateAsync(DateTime date, int? lineId = null)
        {
            return await _context.Plans
                                       .Where(p => p.Date.Date == date.Date && (lineId == null || p.LineId == lineId))
                                       .GroupBy(p => new { p.LineId, LineName = p.Line.Name, p.ModelId, ModelName = p.Model.Name })
                                       .Select(g => new PlanDetailStaticsModel
                                       {
                                           LineId = g.Key.LineId,
                                           LineName = g.Key.LineName,
                                           ModelId = g.Key.ModelId,
                                           ModelName = g.Key.ModelName,
                                           Planned = g.Sum(p => p.RequiredCount),
                                           Produced = g.Sum(p => p.ProducedCount)
                                       })
                                       .OrderBy(o => o.LineName)
                                       .ThenBy(o => o.ModelName)
                                       .ToListAsync();
        }

        public async Task<IEnumerable<QualityDetailStaticsModel>> GetQualityDetailsByDateAsync(DateTime date, int? lineId = null)
        {
            var produced = await _context.Plans
                                       .Where(p => p.Date.Date == date.Date && (lineId == null || p.LineId == lineId))
                                       .GroupBy(p => new { p.LineId, LineName = p.Line.Name, p.ModelId, ModelName = p.Model.Name })
                                       .Select(g => new
                                       {
                                           g.Key.LineId,
                                           g.Key.LineName,
                                           g.Key.ModelId,
                                           g.Key.ModelName,
                                           Produced = g.Sum(p => p.ProducedCount)
                                       })
                                       .ToListAsync();

            var defects = await _context.Reports
                                       .Where(r => r.CreatedDate.Date == date.Date && (lineId == null || r.LineId == lineId))
                                       .GroupBy(r => new { r.LineId, r.ModelId })
                                       .Select(g => new { g.Key.LineId, g.Key.ModelId, Count = g.Count() })
                                       .ToListAsync();

            return produced.Select(p =>
                                {
                                    var defectCount = defects.FirstOrDefault(d => d.LineId == p.LineId && d.ModelId == p.ModelId)?.Count ?? 0;
                                    var ftq = p.Produced > 0 ? 100 - (defectCount * 100.0 / p.Produced) : 0;

                                    return new QualityDetailStaticsModel
                                    {
                                        LineId = p.LineId,
                                        LineName = p.LineName,
                                        ModelId = p.ModelId,
                                        ModelName = p.ModelName,
                                        Produced = p.Produced,
                                        DefectCount = defectCount,
                                        Ftq = Math.Round(ftq, 2)
                                    };
                                })
                                .OrderBy(o => o.LineName)
                                .ThenBy(o => o.ModelName)
                                .ToList();
        }
    }
}

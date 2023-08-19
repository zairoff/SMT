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

        public async Task<IEnumerable<StaticsModel>> GroupByLineAsync(string shift, DateTime from, DateTime to)
        {
            if (string.IsNullOrEmpty(shift))
            {
                return await _context.Reports.Where(r => r.CreatedDate.Date >= from.Date && r.CreatedDate.Date <= to.Date)
                                      .Select(r => new { r.Line, r.Defect })
                                          .GroupBy(g => g.Line.Name)
                                          .Select(g => new StaticsModel
                                          {
                                              Name = g.Key,
                                              Count = g.Count(),
                                              Size = g.Sum(x => x.Defect.Size)
                                          })
                                          .OrderByDescending(o => o.Count)
                                          .ToListAsync();
            }
            else
            {
                return await _context.Reports.Where(r => r.CreatedDate.Date >= from.Date && r.CreatedDate.Date <= to.Date && r.Shift == shift)
                                      .Select(r => new { r.Line, r.Defect })
                                          .GroupBy(g => g.Line.Name)
                                          .Select(g => new StaticsModel
                                          {
                                              Name = g.Key,
                                              Count = g.Count(),
                                              Size = g.Sum(x => x.Defect.Size)
                                          })
                                          .OrderByDescending(o => o.Count)
                                          .ToListAsync();
            }
        }

        public async Task<IEnumerable<StaticsModel>> GroupByModelAsync(string shift, DateTime from, DateTime to)
        {
            //return await _context.Reports.GroupBy(r => r.ModelId)
            //                            .Select(g => new StaticsModel
            //                                        {
            //                                            Model = g.AsEnumerable().Select(g => g.Model.Name).FirstOrDefault(),
            //                                            Count = g.Count()
            //                                        }).ToListAsync();

            if (string.IsNullOrEmpty(shift))
            {
                return await _context.Reports.Where(r => r.CreatedDate.Date >= from.Date && r.CreatedDate.Date <= to.Date)
                                        .Select(r => new { r.Model, r.Defect })
                                            .GroupBy(g => g.Model.Name)
                                            .Select(g => new StaticsModel
                                            {
                                                Name = g.Key,
                                                Count = g.Count(),
                                                Size = g.Sum(x => x.Defect.Size)
                                            })
                                            .OrderByDescending(o => o.Count)
                                            .ToListAsync();
            }
            else
            {
                return await _context.Reports.Where(r => r.CreatedDate.Date >= from.Date && r.CreatedDate.Date <= to.Date && r.Shift == shift)
                                        .Select(r => new { r.Model, r.Defect })
                                            .GroupBy(g => g.Model.Name)
                                            .Select(g => new StaticsModel
                                            {
                                                Name = g.Key,
                                                Count = g.Count(),
                                                Size = g.Sum(x => x.Defect.Size)
                                            })
                                            .OrderByDescending(o => o.Count)
                                            .ToListAsync();
            }
        }

        public async Task<IEnumerable<StaticsModel>> GroupByDefectAsync(string shift, DateTime from, DateTime to)
        {
            if (string.IsNullOrEmpty(shift))
            {
                return await _context.Reports.Where(r => r.CreatedDate.Date >= from.Date && r.CreatedDate.Date <= to.Date)
                                       .Select(r => r.Defect)
                                           .GroupBy(g => g.Name)
                                           .Select(g => new StaticsModel
                                           {
                                               Name = g.Key,
                                               Count = g.Count(),
                                               Size = g.Sum(x => x.Size)
                                           })
                                           .OrderByDescending(o => o.Count)
                                           .ToListAsync();
            }
            else
            {
                return await _context.Reports.Where(r => r.CreatedDate.Date >= from.Date && r.CreatedDate.Date <= to.Date && r.Shift == shift)
                                       .Select(r => r.Defect)
                                           .GroupBy(g => g.Name)
                                           .Select(g => new StaticsModel
                                           {
                                               Name = g.Key,
                                               Count = g.Count(),
                                               Size = g.Sum(x => x.Size)
                                           })
                                           .OrderByDescending(o => o.Count)
                                           .ToListAsync();
            }
            
        }

        public async Task<IEnumerable<StaticsModel>> GroupByDefectAsync(int lineId, string shift, DateTime from, DateTime to)
        {
            if (string.IsNullOrEmpty(shift))
            {
                return await _context.Reports.Where(r => r.LineId == lineId && r.CreatedDate.Date >= from.Date && r.CreatedDate.Date <= to.Date)
                                       .Select(r => r.Defect)
                                           .GroupBy(g => g.Name)
                                           .Select(g => new StaticsModel
                                           {
                                               Name = g.Key,
                                               Count = g.Count(),
                                               Size = g.Sum(x => x.Size)
                                           })
                                           .OrderByDescending(o => o.Count)
                                           .ToListAsync();
            }
            else
            {
                return await _context.Reports.Where(r => r.LineId == lineId && r.CreatedDate.Date >= from.Date && r.CreatedDate.Date <= to.Date && r.Shift == shift)
                                       .Select(r => r.Defect)
                                           .GroupBy(g => g.Name)
                                           .Select(g => new StaticsModel
                                           {
                                               Name = g.Key,
                                               Count = g.Count(),
                                               Size = g.Sum(x => x.Size)
                                           })
                                           .OrderByDescending(o => o.Count)
                                           .ToListAsync();
            }
            
        }

        public async Task<StaticsModel> GroupByDefectAsync(int lineId, string name, string shift, DateTime from, DateTime to)
        {
            if (string.IsNullOrEmpty(shift))
            {
                return await _context.Reports.Where(r => r.LineId == lineId && r.CreatedDate.Date >= from.Date && r.CreatedDate.Date <= to.Date)
                                          .Select(x => x.Defect)
                                          .GroupBy(x => 1)
                                          .Select(g => new StaticsModel
                                          {
                                              Name = name,
                                              Count = g.Count(),
                                              Size = g.Sum(g => g.Size)
                                          })
                                          .FirstOrDefaultAsync();
            }
            else
            {
                return await _context.Reports.Where(r => r.LineId == lineId && r.CreatedDate.Date >= from.Date && r.CreatedDate.Date <= to.Date && r.Shift == shift)
                                          .Select(x => x.Defect)
                                          .GroupBy(x => 1)
                                          .Select(g => new StaticsModel
                                          {
                                              Name = name,
                                              Count = g.Count(),
                                              Size = g.Sum(g => g.Size)
                                          })
                                          .FirstOrDefaultAsync();
            }
        }
    }
}

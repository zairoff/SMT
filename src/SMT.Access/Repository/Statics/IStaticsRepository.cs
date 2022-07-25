using SMT.Domain.Statics;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SMT.Access.Repository.Statics
{
    public interface IStaticsRepository
    {
        Task<IEnumerable<StaticsModel>> GroupByModelAsync(DateTime from, DateTime to);
        Task<IEnumerable<StaticsModel>> GroupByProductAsync(DateTime from, DateTime to);
        Task<IEnumerable<StaticsModel>> GroupByBrandAsync(DateTime from, DateTime to);
        Task<IEnumerable<StaticsModel>> GroupByLineAsync(DateTime from, DateTime to);
        Task<IEnumerable<StaticsModel>> GroupByDefectAsync(DateTime from, DateTime to);
    }
}

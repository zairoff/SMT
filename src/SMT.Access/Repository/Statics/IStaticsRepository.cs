using SMT.Domain.Statics;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SMT.Access.Repository.Statics
{
    public interface IStaticsRepository
    {
        Task<IEnumerable<StaticsModel>> GroupByModelAsync(string shift, DateTime from, DateTime to);
        Task<IEnumerable<StaticsModel>> GroupByLineAsync(string shift, DateTime from, DateTime to);
        Task<IEnumerable<StaticsModel>> GroupByDefectAsync(string shift, DateTime from, DateTime to);
        Task<IEnumerable<StaticsModel>> GroupByDefectAsync(int lineId, string shift, DateTime from, DateTime to);
        Task<StaticsModel> GroupByDefectAsync(int lineId, string name, string shift, DateTime from, DateTime to);
    }
}

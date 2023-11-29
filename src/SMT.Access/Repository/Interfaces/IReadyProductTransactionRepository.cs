using SMT.Access.Repository.Base;
using SMT.Domain;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System;

namespace SMT.Access.Repository.Interfaces
{
    public interface IReadyProductTransactionRepository : IBaseRepository<ReadyProductTransaction>
    {
        Task<IEnumerable<ReadyProductTransaction>> GetByAsync(Expression<Func<ReadyProductTransaction, bool>> expression);

        Task<IEnumerable<ReadyProductTransaction>> GetGroupByModelAsync(Expression<Func<ReadyProductTransaction, bool>> expression);

        Task<IEnumerable<ReadyProductTransaction>> GetGroupByModelAsync();

        Task<int> FindSumAsync(Expression<Func<ReadyProductTransaction, bool>> expression);
    }
}

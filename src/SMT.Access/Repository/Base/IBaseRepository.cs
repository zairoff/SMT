using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SMT.Access.Repository.Base
{
    public interface IBaseRepository<T> where T : class
    {
        Task AddAsync(T entity);
        void Delete(T entity);
        void Update(T entity);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> FindAsync(Expression<Func<T, bool>> expression);
    }
}

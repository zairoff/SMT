using Microsoft.EntityFrameworkCore;
using SMT.Access.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SMT.Access.Repository.Base
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected readonly AppDbContext _context;

        private DbSet<T> _dbSet;

        public DbSet<T> DbSet => _dbSet ??= _context.Set<T>();

        public BaseRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(T entity)
        {
            await DbSet.AddAsync(entity);
        }

        public virtual void Delete(T entity)
        {
            DbSet.Remove(entity);
        }

        public void Update(T entity)
        {
            DbSet.Update(entity);
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await DbSet.ToListAsync();
        }

        public virtual async Task<T> FindAsync(Expression<Func<T, bool>> expression)
        {
            return await DbSet.Where(expression).FirstOrDefaultAsync();
        }
    }
}

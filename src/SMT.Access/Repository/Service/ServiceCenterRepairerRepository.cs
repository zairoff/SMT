using SMT.Access.Data;
using SMT.Access.Repository.Base;
using SMT.Access.Repository.Interfaces.Service;
using SMT.Domain.Service;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Threading.Tasks;
using System;
using Microsoft.EntityFrameworkCore;

namespace SMT.Access.Repository.Service
{
    public class ServiceCenterRepairerRepository : BaseRepository<ServiceCenterRepairer>, IServiceCenterRepairerRepository
    {
        public ServiceCenterRepairerRepository(AppDbContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<ServiceCenterRepairer>> GetAllAsync()
        {
            return await DbSet.OrderBy(x => x.Name).ToListAsync();
        }

        public async Task<IEnumerable<ServiceCenterRepairer>> GetByAsync(Expression<Func<ServiceCenterRepairer, bool>> expression)
        {
            return await DbSet.Where(expression).OrderBy(x => x.Name).ToListAsync();
        }
    }
}

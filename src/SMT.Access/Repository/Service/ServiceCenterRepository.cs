using Microsoft.EntityFrameworkCore;
using SMT.Access.Data;
using SMT.Access.Repository.Base;
using SMT.Access.Repository.Interfaces.Service;
using SMT.Domain.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SMT.Access.Repository.Service
{
    public class ServiceCenterRepository : BaseRepository<ServiceCenter>, IServiceCenterRepository
    {
        public ServiceCenterRepository(AppDbContext context) : base(context)
        {

        }

        public override async Task<IEnumerable<ServiceCenter>> GetAllAsync()
        {
            return await DbSet.OrderBy(x => x.Name).ToListAsync();
        }

        public async Task<IEnumerable<ServiceCenter>> GetByAsync(Expression<Func<ServiceCenter, bool>> expression)
        {
            return await DbSet.Where(expression).OrderBy(x => x.Name).ToListAsync();
        }
    }
}

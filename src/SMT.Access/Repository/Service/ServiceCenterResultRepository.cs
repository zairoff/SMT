using SMT.Access.Data;
using SMT.Access.Repository.Base;
using SMT.Access.Repository.Interfaces.Service;
using SMT.Domain.Service;
using System.Linq.Expressions;
using System.Linq;
using System.Threading.Tasks;
using System;
using Microsoft.EntityFrameworkCore;

namespace SMT.Access.Repository.Service
{
    public class ServiceCenterResultRepository : BaseRepository<ServiceCenterResult>, IServiceCenterResultRepository
    {
        public ServiceCenterResultRepository(AppDbContext context) : base(context)
        {
        }

        public async override Task<ServiceCenterResult> FindAsync(Expression<Func<ServiceCenterResult, bool>> expression)
        {
            return await DbSet.Where(expression).Include(x => x.ServiceCenterRepairer).OrderByDescending(x => x.CreatedAt).FirstOrDefaultAsync();
        }
    }
}

using SMT.Access.Data;
using SMT.Access.Repository.Base;
using SMT.Access.Repository.Interfaces.Service;
using SMT.Domain.Service;

namespace SMT.Access.Repository.Service
{
    public class ServiceCenterRequestRepository : BaseRepository<ServiceCenterRequest>, IServiceCenterRequestRepository
    {
        public ServiceCenterRequestRepository(AppDbContext context) : base(context)
        {
        }
    }
}

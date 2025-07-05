using Microsoft.EntityFrameworkCore;
using SMT.Access.Data;
using SMT.Access.Repository.Base;
using SMT.Access.Repository.Interfaces.Service;
using SMT.Domain.Service;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SMT.Access.Repository.Service
{
    public class RequestSenderRepository : BaseRepository<ServiceCenterRequestSender>, IRequestSenderRepository
    {
        public RequestSenderRepository(AppDbContext context) : base(context)
        {
        }

        public async override Task<IEnumerable<ServiceCenterRequestSender>> GetAllAsync()
        {
            return await DbSet.Include(e => e.ServiceCenter).ToListAsync();
        }
    }
}

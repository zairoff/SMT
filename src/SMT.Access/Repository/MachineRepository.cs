using SMT.Access.Data;
using SMT.Access.Repository.Base;
using SMT.Access.Repository.Interfaces;
using SMT.Domain;

namespace SMT.Access.Repository
{
    public class MachineRepository : BaseRepository<Machine>, IMachineRepository
    {
        public MachineRepository(AppDbContext context) : base(context)
        {
        }
    }
}

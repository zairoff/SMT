using SMT.Access.Data;
using SMT.Access.Repository.Base;
using SMT.Access.Repository.Interfaces;
using SMT.Domain;

namespace SMT.Access.Repository
{
    public class RepairerRepository : BaseRepository<Repairer>, IRepairerRepository
    {
        public RepairerRepository(AppDbContext context) : base(context)
        {
        }
    }
}

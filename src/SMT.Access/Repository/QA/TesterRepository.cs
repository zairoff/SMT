using SMT.Access.Data;
using SMT.Access.Repository.Base;
using SMT.Access.Repository.Interfaces.QA;
using SMT.Domain.QA;

namespace SMT.Access.Repository.QA
{
    public class TesterRepository : BaseRepository<Tester>, ITesterRepository
    {
        public TesterRepository(AppDbContext context) : base(context)
        {
        }
    }
}

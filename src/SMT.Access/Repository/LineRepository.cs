using SMT.Access.Data;
using SMT.Access.Repository.Base;
using SMT.Access.Repository.Interfaces;
using SMT.Domain;

namespace SMT.Access.Repository
{
    public class LineRepository : BaseRepository<Line>, ILineRepository
    {
        public LineRepository(AppDbContext context) : base(context)
        {
        }
    }
}

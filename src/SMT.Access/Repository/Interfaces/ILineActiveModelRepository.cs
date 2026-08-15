using SMT.Access.Repository.Base;
using SMT.Domain;
using System.Threading.Tasks;

namespace SMT.Access.Repository.Interfaces
{
    public interface ILineActiveModelRepository : IBaseRepository<LineActiveModel>
    {
        Task<LineActiveModel> GetByLineAsync(int lineId);
    }
}

using System.Threading.Tasks;

namespace SMT.Access.UnitOfWork
{
    public interface IUnitOfWork
    {
        Task SaveAsync();
    }
}

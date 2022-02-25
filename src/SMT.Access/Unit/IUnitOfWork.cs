using System.Threading.Tasks;

namespace SMT.Access.Unit
{
    public interface IUnitOfWork
    {
        Task SaveAsync();
    }
}

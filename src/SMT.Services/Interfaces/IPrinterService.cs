using System.Threading.Tasks;

namespace SMT.Services.Interfaces
{
    public interface IPrinterService
    {
        Task Print(string partNumber);
    }
}

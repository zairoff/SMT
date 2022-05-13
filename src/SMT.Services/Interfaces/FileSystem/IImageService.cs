using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace SMT.Services.Interfaces.FileSystem
{
    public interface IImageService
    {
        Task<string> SaveAsync(IFormFile file);

        string LoadUrl(string fileName);
    }
}

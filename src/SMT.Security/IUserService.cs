using SMT.ViewModel.Dto.UserDto;
using System.Threading.Tasks;

namespace SMT.Security
{
    public interface IUserService
    {
        Task<UserResponse> Create(UserCreate userCreate, string role);

        Task Delete(int id);

        Task<UserResponse> Authenticate(string username, string password);

        Task<UserResponse> GetUserByUsername(string username);
    }
}

using SMT.ViewModel.Dto.UserDto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SMT.Security
{
    public interface IUserService
    {
        Task<UserResponse> Create(UserCreate userCreate, string role);

        Task Delete(int id);

        Task<UserResponse> Authenticate(string username, string password);

        Task<UserResponse> GetUserByUsername(string username);

        Task<bool> AnyUserInRoleAsync(string role);

        Task<IList<string>> GetUsernamesInRoleAsync(string role);

        Task<bool> IsInRoleAsync(string username, string role);

        Task AddToRoleAsync(string username, string role);

        Task ResetPasswordAsync(string username, string newPassword);

        Task DeleteByUsernameAsync(string username);

        /// <summary>Value of the user's first claim of this type, or null.</summary>
        Task<string> GetClaimValueAsync(string username, string claimType);

        /// <summary>Replaces all of the user's claims of this type with one claim (none when value is empty).</summary>
        Task SetClaimValueAsync(string username, string claimType, string value);
    }
}

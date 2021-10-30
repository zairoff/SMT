using SMT.Common.Dto.UserDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMT.Security
{
    public interface IUserService
    {
        Task<UserResponse> Create(UserCreate userCreate);

        Task<UserResponse> Update(int id, UserUpdate userUpdate);

        Task Delete(int id);

        Task<string> Authenticate(string username, string password);
    }
}

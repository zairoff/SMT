using SMT.Access.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMT.Common.Dto.UserDto
{
    public class UserResponse
    {
        public string Username { get; set; }

        public string Token { get; set; }

        public static explicit operator UserResponse(ApplicationUser user)
        {
            return new UserResponse
            {
                Username = user.UserName
            };
        }
    }
}

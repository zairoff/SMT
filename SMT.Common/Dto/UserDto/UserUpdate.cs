using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMT.Common.Dto.UserDto
{
    public class UserUpdate
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public string Role { get; set; }

        public string Telegram { get; set; }
    }
}

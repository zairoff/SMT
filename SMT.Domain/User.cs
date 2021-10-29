using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMT.Domain
{
    public enum Role
    {
        User,
        Brigadir,
        Manager,
        Admin
    }

    public class User
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public Role Role { get; set; }

        public string Telegram { get; set; }
    }
}

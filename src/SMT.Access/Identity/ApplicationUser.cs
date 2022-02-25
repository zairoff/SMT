using Microsoft.AspNetCore.Identity;

namespace SMT.Access.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public string Telegram { get; set; }
    }
}

using System.Security.Claims;

namespace SMT.Api.Controllers
{
    // Feeder verification endpoints require a logged-in user so every import, activation and scan is
    // attributable. Changing which setup is live on a line is limited to these roles. Role checks are
    // case-sensitive, and UserService creates roles in lower case ("admin", "user").
    public static class FeederRoles
    {
        public const string SetupManagers = "admin,engineer";

        public static string Username(ClaimsPrincipal user)
        {
            return user.FindFirst("username")?.Value ?? user.Identity?.Name;
        }
    }
}

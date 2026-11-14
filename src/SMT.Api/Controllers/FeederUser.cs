using System.Security.Claims;

namespace SMT.Api.Controllers
{
    // Feeder endpoints are open (no login on the line - the line's operators are responsible for it).
    // When someone does happen to be logged in, their name is kept on imports/activations.
    public static class FeederUser
    {
        public static string Username(ClaimsPrincipal user)
        {
            return user.FindFirst("username")?.Value ?? user.Identity?.Name;
        }
    }
}

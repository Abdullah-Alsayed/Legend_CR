using System.Security.Claims;

namespace LegendCR.Helpers
{
    public class AuthHelper
    {
        public static string GetClaimValue(ClaimsPrincipal user, string type)
        {
            var identity = user.Identity as ClaimsIdentity;
            var claim = identity.FindFirst(c => c.Type == type);
            if (claim != null)
                return claim.Value;
            return string.Empty;
        }

        public static string GetUserName(ClaimsPrincipal user)
        {
            var identity = user.Identity as ClaimsIdentity;
            var claim = identity.FindFirst(c => c.Type == "Name");
            if (claim != null)
                return claim.Value;
            return null;
        }
    }
}

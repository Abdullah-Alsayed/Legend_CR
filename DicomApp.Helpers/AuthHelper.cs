using System.Security.Claims;

namespace DicomApp.Helpers
{
    public class AuthHelper
    {
        public static int GetClaimValue(ClaimsPrincipal user, string type)
        {
            var identity = user.Identity as ClaimsIdentity;
            var claim = identity.FindFirst(c => c.Type == type);
            if (claim != null)
                return int.Parse(claim.Value);
            return 0;
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
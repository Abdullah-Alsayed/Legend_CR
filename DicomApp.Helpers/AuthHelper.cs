using System.Security.Claims;
using Microsoft.AspNetCore.Http;

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

        public static string GetClaimStringValue(ClaimsPrincipal user, string Name = "Name")
        {
            var identity = user.Identity as ClaimsIdentity;
            var claim = identity.FindFirst(c => c.Type == Name);
            if (claim != null)
                return claim.Value;
            return null;
        }

        public static string GetLanguageValue(ClaimsPrincipal user, HttpContext httpContext)
        {
            if (user.Identity.IsAuthenticated)
            {
                var identity = user.Identity as ClaimsIdentity;
                var claim = identity?.FindFirst(c => c.Type == SystemConstants.Claims.Language);

                if (claim != null)
                    return claim.Value;
            }

            if (
                !user.Identity.IsAuthenticated
                && httpContext.Request.Cookies.ContainsKey("PreferredLanguage")
            )
                return httpContext.Request.Cookies["PreferredLanguage"];

            return null;
        }

        public static string GetRoleName(
            ClaimsPrincipal user,
            string name = SystemConstants.Claims.RoleName
        )
        {
            var identity = user.Identity as ClaimsIdentity;
            var claim = identity.FindFirst(c => c.Type == name);
            if (claim != null)
                return claim.Value;
            return null;
        }
    }
}

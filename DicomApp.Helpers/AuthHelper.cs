﻿using System.Security.Claims;

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

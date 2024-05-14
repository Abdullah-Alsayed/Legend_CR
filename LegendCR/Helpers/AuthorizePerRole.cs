using LegendCR.BL.Services;
using LegendCR.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace LegendCR.Portal.Helpers
{
    public class AuthorizePerRoleAttribute : TypeFilterAttribute
    {
        public AuthorizePerRoleAttribute(string serviceName)
            : base(typeof(AuthorizePerRoleFilter))
        {
            Arguments = new object[] { serviceName };
        }
    }

    public class AuthorizePerRoleFilter : IAuthorizationFilter
    {
        readonly string serviceName;

        public AuthorizePerRoleFilter(string serviceName)
        {
            this.serviceName = serviceName;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var roleID = context
                .HttpContext.User.Claims.FirstOrDefault(c => c.Type == "RoleID")
                .Value;
            string connectionString = Constants.connectionString;
            var canAccess = true;

            canAccess = BaseService.CheckRoleAccessability(
                roleID,
                serviceName,
                connectionString: connectionString
            );
            if (!canAccess)
            {
                context.Result = new ForbidResult();
                if (!context.HttpContext.User.Identity.IsAuthenticated)
                    context.Result = new UnauthorizedResult();
                else
                    context.Result = new RedirectToRouteResult(
                        new RouteValueDictionary(new { controller = "User", action = "NotFound" })
                    );
            }
        }
    }
}

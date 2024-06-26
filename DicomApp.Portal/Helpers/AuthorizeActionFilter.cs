﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
namespace DicomApp.API.Helpers
{

    public class AuthorizeActionAttribute : TypeFilterAttribute
    {
        public AuthorizeActionAttribute(string serviceName) : base(typeof(AuthorizeActionFilter))
        {
            Arguments = new object[] { serviceName };
        }
    }
    public class AuthorizeActionFilter
    {
        private readonly RequestDelegate _next;
        public AuthorizeActionFilter(RequestDelegate next)
        {
            _next = next;
        }
        public Task Invoke(HttpContext context)
        {
            return BeginInvoke(context);
        }
        private Task BeginInvoke(HttpContext context)
        {
            //string token1 = context.Request.Headers["Authorization"];
            ////do the checking
            var token = context.GetTokenAsync("access_token").Result;
            if (token != null)
            {
                var handler = new JwtSecurityTokenHandler();
                if (handler.ReadToken(token) is JwtSecurityToken tokenS)
                {
                    int.TryParse(tokenS.Claims.First(claim => claim.Type == "UserID").Value, out int userId);
                    int.TryParse(tokenS.Claims.First(claim => claim.Type == "RoleID").Value, out int roleId);
                    if (!TokenHelper.Validate(token, userId, roleId) && roleId != 5)
                    {
                        context.Response.Headers.Add("Access-Control-Allow-Origin", new[] { (string)context.Request.Headers["Origin"] });
                        context.Response.Headers.Add("Access-Control-Allow-Headers", new[] { "Origin, X-Requested-With, Content-Type, Accept" });
                        context.Response.Headers.Add("Access-Control-Allow-Methods", new[] { "GET, POST, PUT, DELETE, OPTIONS" });
                        context.Response.Headers.Add("Access-Control-Allow-Credentials", new[] { "true" });
                        context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        return context.Response.WriteAsync("Session Expired, Please logout");

                    }
                }
            }
            return _next.Invoke(context);
        }
        //public async Task Invoke(HttpContext context)
        //{
        //    //string token1 = context.Request.Headers["Authorization"];
        //    ////do the checking
        //    var token = context.GetTokenAsync("access_token").Result;
        //    if (token != null)
        //    {
        //        var handler = new JwtSecurityTokenHandler();
        //        if (handler.ReadToken(token) is JwtSecurityToken tokenS)
        //        {
        //            long.TryParse(tokenS.Claims.First(claim => claim.Type == "UserID").Value, out long userId);
        //            long.TryParse(tokenS.Claims.First(claim => claim.Type == "RoleID").Value, out long roleId);
        //            if (!TokenHelper.Validate(token, userId, roleId))
        //            {
        //                //context.Request.Headers.Remove("Authorization");
        //                context.Response.ContentType = "application/json";
        //                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
        //                await context.Response.WriteAsync("Invalid Token", Encoding.UTF8);
        //                return;
        //            }
        //        }
        //    }
        //    await _next(context);
        //}
    }
}
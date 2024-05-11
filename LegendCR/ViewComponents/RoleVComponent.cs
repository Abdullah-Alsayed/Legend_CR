using LegendCR.BL.Services;
using LegendCR.CommonDefinitions.Requests;
using LegendCR.DAL.DB;
using LegendCR.Helpers;
using LegendCR.Portal.Helpers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LegendCR.Portal.ViewComponents
{

    public class RoleVComponent : ViewComponent
    {
        private readonly ApplicationDBContext _context;

        public RoleVComponent(ApplicationDBContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke(string orderByColumn = null, string searchVal = null, int pageIndex = 0)
        {
            ViewBag.pageIndex = pageIndex;
            var roleRequest = new RoleRequest
            {
                context = _context,
                OrderByColumn = orderByColumn,
                PageSize = BaseRequest.DefaultPageSize,
                PageIndex = pageIndex,
                RoleID = AuthHelper.GetClaimValue(UserClaimsPrincipal, "RoleID"),
                UserID = AuthHelper.GetClaimValue(UserClaimsPrincipal, "UserID"),
            };
            var roleResponse = RoleService.ListRole(roleRequest);
            if (roleResponse.RoleDTOs != null)
            {
                ViewBag.noOfPages = Math.Ceiling((double)roleResponse.TotalCount / roleRequest.PageSize);
                ViewBag.hasMore = roleResponse.TotalCount > (roleResponse.RoleDTOs.Count + (roleRequest.PageIndex * roleRequest.PageSize));
                return View(roleResponse.RoleDTOs);
            }
            ViewBag.error = roleResponse.Message;
            return View();
        }

    }
}

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
using LegendCR.CommonDefinitions.DTO;

namespace LegendCR.Portal.ViewComponents
{

    public class UserVComponent : ViewComponent
    {
        private readonly ApplicationDBContext _context;

        public UserVComponent(ApplicationDBContext context)
        {
            _context = context;
        }
        public IViewComponentResult Invoke(bool IsDesc , string orderByColumn = null, string Search = null, int pageIndex = 0, int roleID = 0,bool staffOnly=false)
        {
            ViewBag.pageIndex = pageIndex;
            var userRequest = new UserRequest
            {
                context = _context,
                OrderByColumn = orderByColumn,
                PageSize = BaseHelper.Constants.PageSize,
                PageIndex = pageIndex,
                RoleID = AuthHelper.GetClaimValue(UserClaimsPrincipal, "RoleID"),
                UserID = AuthHelper.GetClaimValue(UserClaimsPrincipal, "UserID"),
                StaffOnly = staffOnly,
                IsDesc = IsDesc
            };
            if (roleID > 0)
                userRequest.UserDTO = new UserDTO() { RoleID = roleID , Search = Search};
            else
                userRequest.UserDTO = new UserDTO() { StaffOnly = staffOnly , Search = Search };

            var userResponse = UserService.GetAllUsers(userRequest);
            if (userResponse.UserDTOs != null)
            {
                ViewBag.noOfPages = Math.Ceiling((double)userResponse.TotalCount / userRequest.PageSize);
                ViewBag.hasMore = userResponse.TotalCount > (userResponse.UserDTOs.Count + (userRequest.PageIndex * userRequest.PageSize));
                var records = userResponse.UserDTOs;
                return View(records);
            }
            ViewBag.error = userResponse.Message;
            return View();
        }
    }
}

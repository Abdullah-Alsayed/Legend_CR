
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using LegendCR.BL.Services;
using LegendCR.CommonDefinitions.DTO;
using LegendCR.CommonDefinitions.Requests;
using LegendCR.DAL.DB;
using LegendCR.Helpers;
using LegendCR.Portal.Helpers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using LegendCR.Helpers;
using LegendCR.CommonDefinitions.Responses;

namespace LegendCR.Portal.Controllers
{
    [Authorize]
    public class RoleController : Controller
    {
        private readonly ApplicationDBContext _context;

        public RoleController(ApplicationDBContext context)
        {
            _context = context;
        }

        [AuthorizePerRole("ListRole")]
        public ActionResult ListRole(string Search, string ActionType = null)
        {
            var filter = new RoleDTO();
            filter.Name = Search;
            var roleRequest = new RoleRequest
            {
                context = _context,
                OrderByColumn = "Id",
                RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
                UserID = AuthHelper.GetClaimValue(User, "UserID"),
                RoleDTO = filter
            };

            var roleResponse = RoleService.ListRole(roleRequest);
            roleResponse.RoleDTOs = roleResponse.RoleDTOs.Where(r => r.Id != (int)EnumRole.SuperAdmin && r.Id != (int)EnumRole.Admin && r.Id != (int)EnumRole.Vendor).ToList();

            if (ActionType == Constants.ActionType.PartialView)
                return PartialView(roleResponse.RoleDTOs);

            else if (ActionType == Constants.ActionType.Table)
                return PartialView("_ListRole", roleResponse.RoleDTOs);

            else
                return View(roleResponse.RoleDTOs);
        }

        [AuthorizePerRole("ListRole")]
        public ActionResult LoadRole(string orderByColumn = null, string searchVal = null, int pageIndex = 0)
        {
            return ViewComponent("RoleVComponent", new { orderByColumn = orderByColumn, searchVal = searchVal, pageIndex = pageIndex });
        }

        [AuthorizePerRole("DeleteRole")]
        public string DeleteRole(int ID)
        {
            var roleRequest = new RoleRequest
            {
                context = _context,
                RoleDTO = new RoleDTO { Id = ID },
                RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
                UserID = AuthHelper.GetClaimValue(User, "UserID")
            };
            var roleResponse = RoleService.DeleteRole(roleRequest);
            return roleResponse.Message;
        }

        [AuthorizePerRole("AddRole")]
        public ActionResult AddRole()
        {
            return View();
        }

        [AuthorizePerRole("AddRole")]
        [HttpPost]
        public ActionResult AddRole(RoleDTO model)
        {
            var roleRequest = new RoleRequest
            {
                RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
                UserID = AuthHelper.GetClaimValue(User, "UserID"),
                context = _context,
                RoleDTO = model
            };
            var roleResponse = RoleService.AddRole(roleRequest);
            if (roleResponse.Success)
                return PartialView("_ListRole", new List<RoleDTO> { model });
            else
                return Json(roleResponse);
        }

        [AuthorizePerRole("EditRole")]
        public ActionResult EditRole(int id)
        {
            var roleRequest = new RoleRequest
            {
                RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
                UserID = AuthHelper.GetClaimValue(User, "UserID"),
                context = _context,
                RoleDTO = new RoleDTO { Id = id }
            };
            var roleResponse = RoleService.ListRole(roleRequest);

            if (roleResponse.Success)
                return View(roleResponse.RoleDTOs[0]);

            ViewBag.error = roleResponse.Message;
            return View();
        }

        [AuthorizePerRole("EditRole")]
        [HttpPost]
        public ActionResult EditRole(RoleDTO model)
        {
            var roleRequest = new RoleRequest
            {
                RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
                UserID = AuthHelper.GetClaimValue(User, "UserID"),
                context = _context,
                RoleDTO = model
            };
            var roleResponse = RoleService.EditRole(roleRequest);
            return Json(roleResponse);
        }

        [AuthorizePerRole("UpdateRoleAppService")]
        public ActionResult UpdateRoleAppService(int roleID, string ActionType = null)
        {
            if (roleID <= 0)
                return RedirectToAction(nameof(ListRole));

            var roleAppServiceRequest = new RoleAppServiceRequest
            {
                RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
                UserID = AuthHelper.GetClaimValue(User, "UserID"),
                context = _context,
                RoleAppServiceDTO = new RoleAppServiceDTO { RoleId = roleID }
            };

            var roleAppServiceResponse = PermissionService.ListAppService(roleAppServiceRequest);

            if (roleAppServiceResponse.Success)
                if (ActionType == Constants.ActionType.PartialView)
                    return PartialView(roleAppServiceResponse.AppServiceDTOs);
                else
                    return View(roleAppServiceResponse.AppServiceDTOs);

            else return RedirectToAction(nameof(ListRole));
        }

        [AuthorizePerRole("UpdateRoleAppService")]
        [HttpPost]
        public ActionResult UpdateRoleAppService(AppServiceDTO model)
        {
            var roleAppServiceRequest = new RoleAppServiceRequest
            {
                RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
                UserID = AuthHelper.GetClaimValue(User, "UserID"),
                context = _context,
                AppServiceDTO = model
            };

            var roleAppServiceResponse = PermissionService.UpdateRoleAppService(roleAppServiceRequest);

            if (roleAppServiceResponse.Success)
                TempData["SuccessMsg"] = "Saved Successfully";
            else
                TempData["ErrorMsg"] = "Faild";

            return RedirectToAction("UpdateRoleAppService");
        }
    }
}
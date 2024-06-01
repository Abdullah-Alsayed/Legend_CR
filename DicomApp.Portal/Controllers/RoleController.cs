using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;
using DicomApp.BL.Services;
using DicomApp.CommonDefinitions.DTO;
using DicomApp.CommonDefinitions.Requests;
using DicomApp.CommonDefinitions.Responses;
using DicomApp.DAL.DB;
using DicomApp.Helpers;
using DicomApp.Helpers;
using DicomApp.Portal.Helpers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DicomApp.Portal.Controllers
{
    [Authorize]
    public class RoleController : Controller
    {
        private readonly ShippingDBContext _context;

        public RoleController(ShippingDBContext context)
        {
            _context = context;
        }

        [AuthorizePerRole(SystemConstants.Permission.ListRole)]
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
            roleResponse.RoleDTOs = roleResponse
                .RoleDTOs.Where(r =>
                    r.Id != (int)EnumRole.SuperAdmin
                    && r.Id != (int)EnumRole.Admin
                    && r.Id != (int)EnumRole.Vendor
                )
                .ToList();

            if (ActionType == SystemConstants.ActionType.PartialView)
                return PartialView(roleResponse.RoleDTOs);
            else if (ActionType == SystemConstants.ActionType.Table)
                return PartialView("_ListRole", roleResponse.RoleDTOs);
            else
                return View(roleResponse.RoleDTOs);
        }

        [AuthorizePerRole(SystemConstants.Permission.ListRole)]
        public ActionResult LoadRole(
            string orderByColumn = null,
            string searchVal = null,
            int pageIndex = 0
        )
        {
            return ViewComponent(
                "RoleVComponent",
                new
                {
                    orderByColumn = orderByColumn,
                    searchVal = searchVal,
                    pageIndex = pageIndex
                }
            );
        }

        [AuthorizePerRole(SystemConstants.Permission.DeleteRole)]
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

        [AuthorizePerRole(SystemConstants.Permission.AddRole)]
        public ActionResult AddRole()
        {
            return View();
        }

        [AuthorizePerRole(SystemConstants.Permission.AddRole)]
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

        [AuthorizePerRole(SystemConstants.Permission.EditRole)]
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

        [AuthorizePerRole(SystemConstants.Permission.EditRole)]
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

        [AuthorizePerRole(SystemConstants.Permission.EditRoleAppService)]
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
                if (ActionType == SystemConstants.ActionType.PartialView)
                    return PartialView(roleAppServiceResponse.AppServiceDTOs);
                else
                    return View(roleAppServiceResponse.AppServiceDTOs);
            else
                return RedirectToAction(nameof(ListRole));
        }

        [AuthorizePerRole(SystemConstants.Permission.EditRoleAppService)]
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

            var roleAppServiceResponse = PermissionService.UpdateRoleAppService(
                roleAppServiceRequest
            );

            if (roleAppServiceResponse.Success)
                TempData["SuccessMsg"] = "Saved Successfully";
            else
                TempData["ErrorMsg"] = "Faild";

            return RedirectToAction("UpdateRoleAppService");
        }

        public IActionResult SeedPermission()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();

            Dictionary<string, List<string>> Permissions = new Dictionary<string, List<string>>();
            foreach (Type type in assembly.GetTypes())
            {
                if (type.Name.EndsWith("Controller"))
                {
                    List<string> roles = new List<string>();
                    foreach (MethodInfo method in type.GetMethods())
                    {
                        var attributes = method.GetCustomAttributes(
                            typeof(AuthorizePerRoleAttribute),
                            false
                        );
                        foreach (AuthorizePerRoleAttribute attribute in attributes)
                            if (attribute.Arguments != null && attribute.Arguments.Length > 0)
                                roles.Add(attribute.Arguments[0]?.ToString());
                    }

                    if (roles.Count > 0)
                        Permissions[type.Name] = roles;
                }
            }

            var roleAppServiceResponse = PermissionService.SeedPermission(
                new SeedPermissionRequest
                {
                    context = _context,
                    Permissions = Permissions,
                    RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
                    UserID = AuthHelper.GetClaimValue(User, "UserID"),
                }
            );
            return Ok(roleAppServiceResponse.Success);
        }
    }
}

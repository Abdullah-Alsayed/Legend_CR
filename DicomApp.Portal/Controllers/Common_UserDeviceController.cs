using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DicomApp.BL.Services;
using DicomApp.CommonDefinitions.DTO;
using DicomApp.CommonDefinitions.Requests;
using DicomApp.DAL.DB;
using DicomApp.Helpers;
using DicomApp.Portal.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace DicomApp.Portal.Controllers
{
    public class Common_UserDeviceController : Controller
    {
        private readonly LegendDBContext _context;

        public Common_UserDeviceController(LegendDBContext context)
        {
            _context = context;
        }

        [HttpPost]
        public ActionResult ListCommon_UserDevice([FromBody] Common_UserDeviceRequest request)
        {
            request.context = _context;
            request.RoleID = AuthHelper.GetClaimValue(User, "RoleID");
            request.UserID = AuthHelper.GetClaimValue(User, "UserID");
            var Common_UserDeviceResponse = Common_UserDeviceService.ListCommon_UserDevice(request);
            return Ok(new { Common_UserDeviceResponse });
        }

        [HttpPost]
        public ActionResult DeleteCommon_UserDevice([FromBody] Common_UserDeviceRequest request)
        {
            request.context = _context;
            request.RoleID = AuthHelper.GetClaimValue(User, "RoleID");
            request.UserID = AuthHelper.GetClaimValue(User, "UserID");
            var Common_UserDeviceResponse = Common_UserDeviceService.DeleteCommon_UserDevice(
                request
            );
            return Ok(new { Common_UserDeviceResponse });
        }

        [HttpPost]
        [AuthorizePerRole("AddCommon_UserDevice")]
        public ActionResult AddCommon_UserDevice([FromBody] Common_UserDeviceRequest request)
        {
            request.context = _context;
            request.RoleID = AuthHelper.GetClaimValue(User, "RoleID");
            request.UserID = AuthHelper.GetClaimValue(User, "UserID");
            var Common_UserDeviceResponse = Common_UserDeviceService.AddCommon_UserDevice(request);
            return Ok(new { Common_UserDeviceResponse });
        }

        [HttpPost]
        [AuthorizePerRole("EditCommon_UserDevice")]
        public ActionResult EditCommon_UserDevice([FromBody] Common_UserDeviceRequest request)
        {
            request.context = _context;
            request.RoleID = AuthHelper.GetClaimValue(User, "RoleID");
            request.UserID = AuthHelper.GetClaimValue(User, "UserID");
            var Common_UserDeviceResponse = Common_UserDeviceService.EditCommon_UserDevice(request);
            return Ok(new { Common_UserDeviceResponse });
        }
    }
}

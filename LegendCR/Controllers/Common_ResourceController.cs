//using System;
//using System.Collections.Generic;
//using System.IdentityModel.Tokens.Jwt;
//using System.Linq;
//using System.Security.Claims;
//using System.Text;
//using System.Threading.Tasks;
//using LegendCR.BL.Services;
//using LegendCR.CommonDefinitions.DTO;
//using LegendCR.CommonDefinitions.Requests;
//using LegendCR.DAL.DB;
//using Microsoft.AspNetCore.Cors;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.IdentityModel.Tokens;
//using Microsoft.AspNetCore.Authorization;
//using LegendCR.Helpers;
//using LegendCR.Portal.Helpers;

//namespace LegendCR.Portal.Controllers
//{

//    public class Common_ResourceController : Controller
//    {
//        private readonly ApplicationDBContext _context;
//        public Common_ResourceController(ApplicationDBContext context)
//        {
//            _context = context;
//        }

//        [HttpPost]
//        public ActionResult ListCommon_Resource([FromBody] Common_ResourceRequest request)
//        {
//            request.context = _context;
//            request.RoleID = AuthHelper.GetClaimValue(User, "RoleID");
//            request.UserID = AuthHelper.GetClaimValue(User, "UserID");
//            var Common_ResourceResponse = Common_ResourceService.ListCommon_Resource(request);
//            return Ok(new
//            {
//                Common_ResourceResponse
//            });
//        }


//        [HttpPost]
//        public ActionResult DeleteCommon_Resource([FromBody] Common_ResourceRequest request)
//        {
//            request.context = _context;
//            request.RoleID = AuthHelper.GetClaimValue(User, "RoleID");
//            request.UserID = AuthHelper.GetClaimValue(User, "UserID");
//            var Common_ResourceResponse = Common_ResourceService.DeleteCommon_Resource(request);
//            return Ok(new
//            {
//                Common_ResourceResponse
//            });
//        }



//        [HttpPost]
//        [AuthorizePerRole("AddCommon_Resource")]
//        public ActionResult AddCommon_Resource([FromBody] Common_ResourceRequest request)
//        {
//            request.context = _context;
//            request.RoleID = AuthHelper.GetClaimValue(User, "RoleID");
//            request.UserID = AuthHelper.GetClaimValue(User, "UserID");
//            var Common_ResourceResponse = Common_ResourceService.AddCommon_Resource(request);
//            return Ok(new
//            {
//                Common_ResourceResponse
//            });
//        }






//        [HttpPost]
//        [AuthorizePerRole("EditCommon_Resource")]
//        public ActionResult EditCommon_Resource([FromBody] Common_ResourceRequest request)
//        {
//            request.context = _context;
//            request.RoleID = AuthHelper.GetClaimValue(User, "RoleID");
//            request.UserID = AuthHelper.GetClaimValue(User, "UserID");
//            var Common_ResourceResponse = Common_ResourceService.EditCommon_Resource(request);
//            return Ok(new
//            {
//                Common_ResourceResponse
//            });
//        }


//    }
//}

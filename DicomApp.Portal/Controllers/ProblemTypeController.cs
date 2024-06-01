using System.Collections.Generic;
using DicomApp.BL.Services;
using DicomApp.CommonDefinitions.DTO;
using DicomApp.CommonDefinitions.DTO;
using DicomApp.CommonDefinitions.Requests;
using DicomApp.DAL.DB;
using DicomApp.Helpers;
using DicomApp.Helpers;
using DicomApp.Portal.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace DicomApp.Portal.Controllers
{
    public class ProblemTypeController : Controller
    {
        private readonly ShippingDBContext _context;

        public ProblemTypeController(ShippingDBContext context)
        {
            _context = context;
        }

        [AuthorizePerRole("ProblemTypeList")]
        public ActionResult ProblemTypeList(string ActionType, string Search)
        {
            ProblemTypeDTO filter = new ProblemTypeDTO();
            filter.Search = Search;
            var ProblemTypeRequest = new ProblemTypeRequest
            {
                RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
                UserID = AuthHelper.GetClaimValue(User, "UserID"),
                context = _context,
                ProblemTypeDTO = filter,
            };
            var ProblemTypeRespons = ProblemTypeService.ListProblemType(ProblemTypeRequest);
            if (ActionType == SystemConstants.ActionType.PartialView)
                return PartialView(ProblemTypeRespons.ProblemTypeDTOs);
            else if (ActionType == SystemConstants.ActionType.Table)
                return PartialView("_ProblemTypeList", ProblemTypeRespons.ProblemTypeDTOs);
            else
                return View(ProblemTypeRespons.ProblemTypeDTOs);
        }

        [HttpPost]
        [AuthorizePerRole("ProblemTypeAdd")]
        public ActionResult AddProblemType(ProblemTypeDTO model)
        {
            var ProblemTypeRequst = new ProblemTypeRequest
            {
                RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
                UserID = AuthHelper.GetClaimValue(User, "UserID"),
                ProblemTypeDTO = model,
                context = _context
            };
            var ProblemTypeRespons = ProblemTypeService.AddProblemType(ProblemTypeRequst);
            var ProblemTypeData = ProblemTypeService.GetLastProblemType(ProblemTypeRequst);

            if (ProblemTypeRespons.Success)
                return PartialView(
                    "_ProblemTypeList",
                    new List<ProblemTypeDTO> { ProblemTypeData.ProblemTypeDTO }
                );
            else
                return Json(ProblemTypeRespons);
        }

        [HttpPost]
        [AuthorizePerRole("ProblemTypeEdit")]
        public ActionResult EditProblemType(ProblemTypeDTO model)
        {
            var ProblemTypeRequst = new ProblemTypeRequest
            {
                RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
                UserID = AuthHelper.GetClaimValue(User, "UserID"),
                ProblemTypeDTO = model,
                context = _context
            };
            var ProblemTypeRespons = ProblemTypeService.EditProblemType(ProblemTypeRequst);
            return Json(ProblemTypeRespons);
        }

        [AuthorizePerRole("ProblemTypeDelete")]
        [HttpGet]
        public ActionResult DeleteProblemType(int ID)
        {
            var ProblemTypeRequst = new ProblemTypeRequest
            {
                RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
                UserID = AuthHelper.GetClaimValue(User, "UserID"),
                ProblemTypeDTO = new ProblemTypeDTO { ProblemTypeId = ID },
                context = _context
            };
            var ProblemTypeRespons = ProblemTypeService.DeleteProblemType(ProblemTypeRequst);
            if (ProblemTypeRespons.Success)
                return Json(new { success = "Delete successfully" });
            else
                return Json(false);
        }
    }
}

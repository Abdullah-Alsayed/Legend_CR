using System.Collections.Generic;
using DicomApp.BL.Services;
using DicomApp.CommonDefinitions.DTO;
using DicomApp.CommonDefinitions.Requests;
using DicomApp.DAL.DB;
using DicomApp.Helpers;
using DicomApp.Portal.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace DicomApp.Portal.Controllers
{
    public class BranchController : Controller
    {
        private readonly ShippingDBContext _context;

        public BranchController(ShippingDBContext context)
        {
            _context = context;
        }

        [AuthorizePerRole("BranchList")]
        public ActionResult BranchList(string ActionType, string Search)
        {
            BranchDTO filter = new BranchDTO();
            filter.Search = Search;
            var BranchRequest = new BranchRequest
            {
                RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
                UserID = AuthHelper.GetClaimValue(User, "UserID"),
                context = _context,
                BranchDTO = filter,
            };
            var BranchRespons = BranchService.GetBranchs(BranchRequest);
            if (ActionType == SystemConstants.ActionType.PartialView)
                return PartialView(BranchRespons.BranchDTOs);
            else if (ActionType == SystemConstants.ActionType.Table)
                return PartialView("_BranchList", BranchRespons.BranchDTOs);
            else
                return View(BranchRespons.BranchDTOs);
        }

        [HttpPost]
        [AuthorizePerRole("BranchAdd")]
        public ActionResult AddBranch(BranchDTO model)
        {
            model.CurrencyId = 2;
            var BranchRequst = new BranchRequest
            {
                RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
                UserID = AuthHelper.GetClaimValue(User, "UserID"),
                BranchDTO = model,
                context = _context
            };
            var BranchRespons = BranchService.AddBranch(BranchRequst);

            if (BranchRespons.Success)
                return PartialView("_BranchList", new List<BranchDTO> { model });
            else
                return Json(BranchRespons);
        }

        [HttpPost]
        [AuthorizePerRole("BranchEdit")]
        public ActionResult EditBranch(BranchDTO model)
        {
            var BranchRequst = new BranchRequest
            {
                RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
                UserID = AuthHelper.GetClaimValue(User, "UserID"),
                BranchDTO = model,
                context = _context
            };
            var BranchRespons = BranchService.EditBranch(BranchRequst);

            return Json(BranchRespons);
        }

        [HttpGet]
        [AuthorizePerRole("BranchDelete")]
        public ActionResult DeleteBranch(int ID)
        {
            var branchRequest = new BranchRequest
            {
                RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
                UserID = AuthHelper.GetClaimValue(User, "UserID"),
                context = _context,
                BranchDTO = new BranchDTO { BranchId = ID }
            };
            var BranchResponse = BranchService.DeleteBranch(branchRequest);
            if (BranchResponse.Success)
                return Json(new { success = "Branch Delete successfully" });
            else
                return Json(new { success = false });
        }
    }
}

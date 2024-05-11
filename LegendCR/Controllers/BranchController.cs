
//using LegendCR.BL.Services;
//using LegendCR.CommonDefinitions.DTO;
//using LegendCR.CommonDefinitions.Requests;
//using LegendCR.DAL.DB;
//using LegendCR.Helpers;
//using LegendCR.Portal.Helpers;
//using Microsoft.AspNetCore.Mvc;
//using System.Collections.Generic;

//namespace LegendCR.Portal.Controllers
//{
//    public class BranchController : Controller
//    {
//        private readonly ApplicationDBContext _context;
//        public BranchController(ApplicationDBContext context)
//        {
//            _context = context;
//        }

//        [AuthorizePerRole("BranchList")]
//        public ActionResult BranchList(string ActionType, string Search)
//        {
//            BranchDTO filter = new BranchDTO();
//            filter.Search = Search;
//            var BranchRequest = new BranchRequest
//            {
//                RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
//                UserID = AuthHelper.GetClaimValue(User, "UserID"),
//                context = _context,
//                BranchDTO = filter,
//            };
//            var BranchRespons = BranchService.GetBranchs(BranchRequest);
//            if (ActionType == Constants.ActionType.PartialView)
//                return PartialView(BranchRespons.BranchDTOs);
//            else if (ActionType == Constants.ActionType.Table)
//                return PartialView("_BranchList", BranchRespons.BranchDTOs);
//            else
//                return View(BranchRespons.BranchDTOs);
//        }

//        [HttpPost]
//        [AuthorizePerRole("BranchAdd")]
//        public ActionResult AddBranch(BranchDTO model)
//        {
//            model.CurrencyId = 2;
//            var BranchRequst = new BranchRequest
//            {
//                RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
//                UserID = AuthHelper.GetClaimValue(User, "UserID"),
//                BranchDTO = model,
//                context = _context
//            };
//            var BranchRespons = BranchService.AddBranch(BranchRequst);

//            if (BranchRespons.Success)
//                return PartialView("_BranchList", new List<BranchDTO> { model });
//            else
//                return Json(BranchRespons); 
//        }

//        [HttpPost]
//        [AuthorizePerRole("BranchEdit")]
//        public ActionResult EditBranch(BranchDTO model)
//        {
//            var BranchRequst = new BranchRequest
//            {
//                RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
//                UserID = AuthHelper.GetClaimValue(User, "UserID"),
//                BranchDTO = model,
//                context = _context
//            };
//            var BranchRespons = BranchService.EditBranch(BranchRequst);
           
//            return Json(BranchRespons);
               

//        }

//        [HttpGet]
//        [AuthorizePerRole("BranchDelete")]
//        public ActionResult DeleteBranch(int ID)
//        {
//            var branchRequest = new BranchRequest
//            {
//                RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
//                UserID = AuthHelper.GetClaimValue(User, "UserID"),
//                context = _context,
//                BranchDTO = new BranchDTO { BranchId = ID }
//            };
//            var BranchResponse = BranchService.DeleteBranch(branchRequest);
//            if (BranchResponse.Success)
//                return Json(new { success = "Branch Delete successfully" });
//            else
//                return Json(new { success = false });
//        }
//    }
//}
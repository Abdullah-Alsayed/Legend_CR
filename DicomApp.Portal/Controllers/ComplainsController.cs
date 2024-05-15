
using DicomApp.BL.Services;
using DicomApp.CommonDefinitions.DTO;
using DicomApp.DAL.DB;
using DicomApp.Helpers;
using DicomApp.Portal.Helpers;
using DicomDB.BL.Services;
using DicomDB.CommonDefinitions.DTO;
using DicomDB.CommonDefinitions.Requests;
using Microsoft.AspNetCore.Mvc;
using Rotativa.AspNetCore;
using Rotativa.AspNetCore.Options;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DicomDB.Portal.Controllers
{

    public class ComplainsController : Controller
    {
        private readonly ShippingDBContext _context;
        public ComplainsController(ShippingDBContext context)
        {
            _context = context;
        }

        [AuthorizePerRole("ComplainsList")]
        public ActionResult ComplainsManagement(string ActionType, string Search, int VendorId, string OrderByColumn, bool? IsDesc, int PageIndex,
                                                int EmployeeId, bool? Solved, string Department, DateTime? From, DateTime? To)
        {
           
            ViewModel<ComplainsDTO> ViewData = new ViewModel<ComplainsDTO>();
            ComplainsDTO filter = new ComplainsDTO();
            filter.Search = Search;
            filter.IsDeleted = false;
            filter.VendorId = VendorId;
            filter.EmployeeId = EmployeeId;
            filter.Department = Department;
            filter.IsSolved = Solved;
            if (Solved.HasValue)
                filter.Solved = Solved.Value;

            if (From.HasValue)
                filter.DateFrom = From.Value;

            if (To.HasValue)
                filter.DateTo = To.Value;

            var ComplainsRequest = new ComplainsRequest
            {
                RoleID = (AuthHelper.GetClaimValue(User, "RoleID")),
                UserID = (AuthHelper.GetClaimValue(User, "UserID")),
                context = _context,
                ComplainsDTO = filter,
                IsDesc = IsDesc ?? true,
                PageIndex = PageIndex,
                PageSize = BaseHelper.Constants.PageSize,
                OrderByColumn = OrderByColumn,
            };
            var ComplainsRespons = ComplainsService.GetComplains(ComplainsRequest);
            ViewData.ObjDTOs = ComplainsRespons.ComplainsDTOs;
            if (ActionType != Constants.ActionType.Table && ActionType != Constants.ActionType.Print)
                ViewData.Lookup = BaseHelper.GetLookup(new List<byte> {
                        (byte)EnumSelectListType.Vendor,
                        (byte)EnumSelectListType.Employee
                    }, _context);

            if (ActionType == Constants.ActionType.PartialView)
                return PartialView(ViewData);

            else if (ActionType == Constants.ActionType.Table)
                return PartialView("_ComplainsList", ViewData.ObjDTOs);

            else if (ActionType == Constants.ActionType.Print)
                return BaseHelper.GeneratePDF<List<ComplainsDTO>>("ComplainsManagementPDF.cshtml", ViewData.ObjDTOs);
            else
                return View(ViewData);
        }

        [HttpPost]
        [AuthorizePerRole("DeleteComplain")]
        public ActionResult DeleteComplains(int id)
        {
            var ComplainsRequst = new ComplainsRequest
            {
                RoleID = (AuthHelper.GetClaimValue(User, "RoleID")),
                UserID = (AuthHelper.GetClaimValue(User, "UserID")),
                ComplainsDTO = new ComplainsDTO() { ComplainsId = id },
                context = _context
            };
            var ComplainsRespons = ComplainsService.DeleteComplains(ComplainsRequst);

            if (ComplainsRespons.Success) return Json(true);
            else return Json(false);
        }

        [HttpPost]
        [AuthorizePerRole("AddComplain")]
        public ActionResult AddComplains(ComplainsDTO model)
        {
            model.Date = DateTime.Now;
            model.ActionBy = (AuthHelper.GetClaimValue(User, "UserID"));
            var ComplainsRequst = new ComplainsRequest
            {
                RoleID = (AuthHelper.GetClaimValue(User, "RoleID")),
                UserID = (AuthHelper.GetClaimValue(User, "UserID")),
                ComplainsDTO = model,
                context = _context
            };
            var ComplainsRespons = ComplainsService.AddComplains(ComplainsRequst);
            var ComplainsResult = ComplainsService.GetComplains(ComplainsRequst);
            if (ComplainsRespons.Success)
                return PartialView("_ComplainsList", ComplainsResult.ComplainsDTOs);
            else
                return Json(new { message = "Failed" });

        }

        [HttpPost]
        [AuthorizePerRole("SolveComplain")]
        public ActionResult SolveComplains(int id, string Comment)
        {
            var ComplainsRequst = new ComplainsRequest
            {
                RoleID = (AuthHelper.GetClaimValue(User, "RoleID")),
                UserID = (AuthHelper.GetClaimValue(User, "UserID")),
                ComplainsDTO = new ComplainsDTO() { ComplainsId = id, Comments = Comment },
                context = _context
            };
            var ComplainsRespons = ComplainsService.SolveComplains(ComplainsRequst);
            if (ComplainsRespons.Success) return Json(true);
            else return Json(false);
        }
    }
}
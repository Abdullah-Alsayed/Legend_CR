//using System;
//using System.Collections.Generic;
//using System.Linq;
//using LegendCR.BL.Services;
//using LegendCR.CommonDefinitions.DTO;
//using LegendCR.CommonDefinitions.Requests;
//using LegendCR.DAL.DB;
//using Microsoft.AspNetCore.Mvc;
//using LegendCR.Helpers;
//using LegendCR.Portal.Helpers;
//using Rotativa.AspNetCore.Options;
//using Rotativa.AspNetCore;
//using LegendCR.CommonDefinitions.DTO.CashDTOs;
//using LegendCR.CommonDefinitions.DTO.ShipmentDTOs;
//using LegendCR.CommonDefinitions.Responses;

//namespace LegendCR.Portal.Controllers
//{
//    public class CashController : Controller
//    {
//        private readonly ApplicationDBContext _context;

//        public CashController(ApplicationDBContext context)
//        {
//            _context = context;
//        }

//        #region Receive Cash

//        [AuthorizePerRole("ReceiveCash")]
//        public IActionResult Receive(string ActionType)
//        {
//            ViewBag.Couriers = GeneralHelper.GetUsers((int)EnumRole.DeliveryMan, _context);
//            if (ActionType == Constants.ActionType.PartialView)
//                return PartialView();
//            else
//                return View();
//        }

//        public IActionResult GetCourierCodes(int DeliveryManId = 0)
//        {
//            if (DeliveryManId == 0)
//                return PartialView("_ReceiveCodes");

//            var filter = new ShipDTO();
//            filter.IsForReceiveCash = true;
//            filter.DeliveryManId = DeliveryManId;
//            filter.StatusIDs = new List<int> { (int)EnumStatus.Delivered, (int)EnumStatus.Cancelled, (int)EnumStatus.Paid_To_Vendor };

//            var request = new ShipmentRequest
//            {
//                RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
//                UserID = AuthHelper.GetClaimValue(User, "UserID"),
//                context = _context,
//                ShipDTO = filter,
//                applyFilter = true,
//                HasVendorDetailsDTO = true,
//                HasFeesDetailsDTO = true,
//                HasSettingDTO = true,
//                HasShipItemDTOs = true,
//            };
//            var response = LegendCR.BL.Services.ShipmentService.GetAllShipments(request);
//            return PartialView("_ReceiveCodes", response.ShipDTOs);
//        }

//        [AuthorizePerRole("ConfirmReceiveCash")]
//        public IActionResult ReceiveCash(int DeliveryManId)
//        {
//            if (DeliveryManId == 0)
//                return Json(new { success = false, message = "Please select courier" });

//            var request = new ShipmentRequest
//            {
//                RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
//                UserID = AuthHelper.GetClaimValue(User, "UserID"),
//                context = _context,
//                ShipDTO = new ShipDTO { DeliveryManId = DeliveryManId }
//            };

//            var response = LegendCR.BL.Services.ShipmentService.ReceiveCash(request);

//            //if (response.Success)
//            //    TempData["SuccessMsg"] = response.Message;
//            //else
//            //    TempData["ErrorMsg"] = response.Message;

//            return Json(new { success = response.Success, message = response.Message });
//        }

//        #endregion

//        #region Treasury

//        [AuthorizePerRole("Treasury")]
//        public IActionResult Treasury(DateTime? From, DateTime? To, int VendorId, string Search, string ActionType)
//        {
//            if (ActionType != Constants.ActionType.Table && ActionType != Constants.ActionType.Print)
//                ViewBag.Vendor = GeneralHelper.GetUsers((int)EnumRole.Vendor, _context);

//            var request = new AccountRequest
//            {
//                context = _context,
//                RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
//                UserID = AuthHelper.GetClaimValue(User, "UserID"),
//            };

//            var response = AccountService.GetForTreasury(request);

//            if (ActionType == Constants.ActionType.PartialView)
//                return PartialView(response.TreasuryDTOs);

//            else if (ActionType == Constants.ActionType.Table)
//                return PartialView("_Treasury", response.TreasuryDTOs);

//            else if (ActionType == Constants.ActionType.Print)
//            {
//                var pdfFile = new ViewAsPdf("InvoiceReportsPrint", response.TreasuryDTOs)
//                {
//                    PageMargins = new Margins(2, 2, 2, 2),
//                    PageOrientation = Orientation.Landscape,
//                    PageSize = Size.A4,
//                };
//                return pdfFile;
//            }

//            else
//                return View(response.TreasuryDTOs);
//        }

//        #endregion

//        #region Transfer Cash

//        [AuthorizePerRole("TransferCash")]
//        public IActionResult Transfer(string ActionType)
//        {
//            ViewBag.Vendors = GeneralHelper.GetUsers((int)EnumRole.Vendor, _context);
//            ViewBag.Zones = _context.Zone.ToList();

//            if (ActionType == Constants.ActionType.PartialView)
//                return PartialView();
//            else
//                return View();
//        }

//        public IActionResult GetAccountDetails(int Id = 0)
//        {
//            if (Id > 0)
//            {
//                var request = new UserRequest
//                {
//                    RoleID = (AuthHelper.GetClaimValue(User, "RoleID")),
//                    UserID = (AuthHelper.GetClaimValue(User, "UserID")),
//                    context = _context,
//                    UserDTO = new UserDTO() { Id = Id }
//                };
//                var response = UserService.GetUser(request);

//                return Json(response.UserDTO);
//            }
//            else
//                return Json(false);
//        }

//        public IActionResult GetVendorShipments(int VendorId = 0)
//        {
//            if (VendorId == 0)
//                return PartialView("_TransferCodes");

//            var request = new AccountRequest
//            {
//                context = _context,
//                RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
//                UserID = AuthHelper.GetClaimValue(User, "UserID"),
//                AccountDTO = new AccountDTO { UserId = VendorId }
//            };

//            var response = AccountService.GetForTransferCash(request);

//            return PartialView("_TransferCodes", response.TreasuryDTO);
//        }

//        [AuthorizePerRole("ConfirmTransferCash")]
//        [HttpPost]
//        public IActionResult TransferCash(CashTransferDTO model, string TransIDs)
//        {
//            model.TransIDs = TransIDs;

//            var request = new CashTransferRequest
//            {
//                RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
//                UserID = AuthHelper.GetClaimValue(User, "UserID"),
//                context = _context,
//                CashTransferDTO = model
//            };

//            var response = LegendCR.BL.Services.CashTransferService.AddCashTransfer(request);

//            //if (response.Success)
//            //    TempData["SuccessMsg"] = response.Message;
//            //else
//            //    TempData["ErrorMsg"] = response.Message;


//            return Json(new { success = response.Success, message = response.Message });
//        }

//        #endregion

//        #region Invoices

//        [HttpGet]
//        [AuthorizePerRole("Invoices")]
//        public ActionResult Invoices(string ActionType = null, int VendorID = 0)
//        {
//            var request = new CashTransferRequest
//            {
//                context = _context,
//                RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
//                UserID = AuthHelper.GetClaimValue(User, "UserID"),
//                //CashTransferDTO = new CashTransferDTO() { VendorId = VendorID }
//            };

//            var response = CashTransferService.GetAllCashTransfers(request);

//            if (ActionType == Constants.ActionType.PartialView)
//                return PartialView(response.InvoiceDTO);
//            else if (ActionType == Constants.ActionType.Table)
//                return PartialView("_Invoices", response.InvoiceDTO);
//            else
//                return View(response.InvoiceDTO);
//        }

//        #endregion
//    }
//}
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using DicomApp.BL.Services;
//using DicomApp.CommonDefinitions.DTO;
//using DicomApp.CommonDefinitions.DTO.AdvertisementDTOs;
//using DicomApp.CommonDefinitions.DTO.CashDTOs;
//using DicomApp.CommonDefinitions.DTO.AdvertisementDTOs;
//using DicomApp.CommonDefinitions.Requests;
//using DicomApp.CommonDefinitions.Responses;
//using DicomApp.DAL.DB;
//using DicomApp.Helpers;
//using DicomApp.Portal.Helpers;
//using Microsoft.AspNetCore.Mvc;
//using Rotativa.AspNetCore;
//using Rotativa.AspNetCore.Options;

//namespace DicomApp.Portal.Controllers
//{
//    public class CashController : Controller
//    {
//        private readonly ShippingDBContext _context;

//        public CashController(ShippingDBContext context)
//        {
//            _context = context;
//        }

//        #region Receive Cash

//        [AuthorizePerRole("ReceiveCash")]
//        public IActionResult Receive(string ActionType)
//        {
//            ViewBag.Couriers = GeneralHelper.GetUsers((int)EnumRole.DeliveryMan, _context);
//            if (ActionType == SystemConstants.ActionType.PartialView)
//                return PartialView();
//            else
//                return View();
//        }

//        [AuthorizePerRole("ConfirmReceiveCash")]
//        public IActionResult ReceiveCash(int DeliveryManId)
//        {
//            if (DeliveryManId == 0)
//                return Json(new { success = false, message = "Please select courier" });

//            var request = new AdvertisementRequest
//            {
//                RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
//                UserID = AuthHelper.GetClaimValue(User, "UserID"),
//                context = _context,
//                AdsDTO = new AdsDTO { DeliveryManId = DeliveryManId }
//            };

//            var response = DicomApp.BL.Services.AdvertisementService.ReceiveCash(request);

//            //if (response.Success)
//            //    TempData["SuccessMsg"] = response.Message;
//            //else
//            //    TempData["ErrorMsg"] = response.Message;

//            return Json(new { success = response.Success, message = response.Message });
//        }

//        #endregion

//        #region Treasury

//        [AuthorizePerRole("Treasury")]
//        public IActionResult Treasury(
//            DateTime? From,
//            DateTime? To,
//            int VendorId,
//            string Search,
//            string ActionType
//        )
//        {
//            if (
//                ActionType != SystemConstants.ActionType.Table
//                && ActionType != SystemConstants.ActionType.Print
//            )
//                ViewBag.Vendor = GeneralHelper.GetUsers((int)EnumRole.Vendor, _context);

//            var request = new AccountRequest
//            {
//                context = _context,
//                RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
//                UserID = AuthHelper.GetClaimValue(User, "UserID"),
//            };

//            var response = AccountService.GetForTreasury(request);

//            if (ActionType == SystemConstants.ActionType.PartialView)
//                return PartialView(response.TreasuryDTOs);
//            else if (ActionType == SystemConstants.ActionType.Table)
//                return PartialView("_Treasury", response.TreasuryDTOs);
//            else if (ActionType == SystemConstants.ActionType.Print)
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

//            if (ActionType == SystemConstants.ActionType.PartialView)
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

//            var response = DicomApp.BL.Services.CashTransferService.AddCashTransfer(request);

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

//            if (ActionType == SystemConstants.ActionType.PartialView)
//                return PartialView(response.InvoiceDTO);
//            else if (ActionType == SystemConstants.ActionType.Table)
//                return PartialView("_Invoices", response.InvoiceDTO);
//            else
//                return View(response.InvoiceDTO);
//        }

//        #endregion
//    }
//}

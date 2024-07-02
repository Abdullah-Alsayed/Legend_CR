using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Threading.Tasks;
using DicomApp.BL.Services;
using DicomApp.CommonDefinitions.DTO;
using DicomApp.CommonDefinitions.DTO.AdvertisementDTOs;
using DicomApp.CommonDefinitions.DTO.AdvertisementDTOs;
using DicomApp.CommonDefinitions.DTO.CashDTOs;
using DicomApp.CommonDefinitions.DTO.PickupDTOs;
using DicomApp.CommonDefinitions.DTO.VendorProductDTOs;
using DicomApp.CommonDefinitions.Requests;
using DicomApp.CommonDefinitions.Responses;
using DicomApp.DAL.DB;
using DicomApp.Helpers;
using DicomApp.Portal.Helpers;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;

namespace DicomApp.Portal.Controllers
{
    public class VendorController : Controller
    {
        private readonly ShippingDBContext _context;

        public VendorController(ShippingDBContext context)
        {
            _context = context;
        }

        #region Reports
        [AuthorizePerRole("Vendor_ReportsShipments")]
        public IActionResult OrderReports(
            int ZoneId,
            int DeliveryManId,
            DateTime? From,
            DateTime? To,
            int StatusId,
            string Search,
            int Status,
            string OrderByColumn,
            int PageIndex,
            string ActionType = null
        )
        {
            var model = new AdsDTO();

            if (StatusId == 0)
                model.StatusId = Status;
            else
                model.StatusId = StatusId;

            model.Search = Search;
            model.GamerId = AuthHelper.GetClaimValue(User, "UserID");
            if (From.HasValue)
                model.DateFrom = From.Value;

            if (To.HasValue)
                model.DateTo = To.Value;

            var PageSize =
                ActionType == SystemConstants.ActionType.Print ? 0 : BaseHelper.Constants.PageSize;

            var shipmentRequest = new AdvertisementRequest
            {
                RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
                UserID = AuthHelper.GetClaimValue(User, "UserID"),
                context = _context,
                AdsDTO = model,

                applyFilter = true,
                OrderByColumn = OrderByColumn ?? "id",
                PageSize = PageSize,
                PageIndex = PageIndex,
            };

            var shipmentResponse = BL.Services.AdvertisementService.GetAllAdvertisements(
                shipmentRequest
            );

            if (ActionType == SystemConstants.ActionType.PartialView)
                return PartialView(
                    "/Views/Vendor/Reports/Order/_OrderReports.cshtml",
                    shipmentResponse.AdsDTOs
                );
            else if (ActionType == SystemConstants.ActionType.Table)
                return PartialView(
                    "/Views/Vendor/Reports/Order/_OrderReportsTable.cshtml",
                    shipmentResponse.AdsDTOs
                );
            else if (ActionType == SystemConstants.ActionType.Print)
                return BaseHelper.GeneratePDF<List<AdsDTO>>(
                    "/Views/Vendor/Reports/Order/OrderReportsPDF.cshtml",
                    shipmentResponse.AdsDTOs
                );
            else
                return View(
                    "/Views/Vendor/Reports/Order/OrderReports.cshtml",
                    shipmentResponse.AdsDTOs
                );
        }

        [AuthorizePerRole("Vendor_ReportsInvoices")]
        public ActionResult InvoiceReport(
            DateTime? From,
            DateTime? To,
            int VendorId,
            string Search,
            int PageIndex,
            string ActionType
        )
        {
            ViewModel<InvoiceDTO> ViewData = new ViewModel<InvoiceDTO>();

            CashTransferDTO filter = new CashTransferDTO();
            filter.Search = Search;
            filter.IsDeleted = false;
            filter.VendorId = VendorId;

            if (From.HasValue)
                filter.DateFrom = From.Value;

            if (To.HasValue)
                filter.DateTo = To.Value;

            var request = new CashTransferRequest
            {
                context = _context,
                RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
                UserID = AuthHelper.GetClaimValue(User, "UserID"),
                PageIndex = PageIndex,
                PageSize = BaseHelper.Constants.PageSize,
                CashTransferDTO = filter
            };

            var response = CashTransferService.GetAllCashTransfers(request);

            ViewData.ObjDTO = response.InvoiceDTO;

            if (
                ActionType != SystemConstants.ActionType.Table
                && ActionType != SystemConstants.ActionType.Print
            )
            {
                ViewData.Lookup = BaseHelper.GetLookup(
                    new List<byte> { (byte)EnumSelectListType.Vendor, },
                    _context
                );
            }

            if (ActionType == SystemConstants.ActionType.PartialView)
                return PartialView("/Views/Vendor/Reports/Invoice/_InvoiceReport.cshtml", ViewData);
            else if (ActionType == SystemConstants.ActionType.Table)
                return PartialView(
                    "/Views/Vendor/Reports/Invoice/_InvoiceReportTable.cshtml",
                    ViewData.ObjDTO
                );
            else if (ActionType == SystemConstants.ActionType.Print)
                return BaseHelper.GeneratePDF<InvoiceDTO>(
                    "/Views/Vendor/Reports/Invoice/InvoiceReportPDF.cshtml",
                    ViewData.ObjDTO
                );
            else
                return View("/Views/Vendor/Reports/Invoice/InvoiceReport.cshtml", ViewData);
        }

        [AuthorizePerRole("Vendor_ReportsStock")]
        public IActionResult StockReport(
            string Search,
            DateTime? From,
            DateTime? To,
            int PageIndex,
            string ActionType
        )
        {
            ViewModel<VendorProductDTO> ViewData = new ViewModel<VendorProductDTO>();
            var userID = AuthHelper.GetClaimValue(User, "UserID");
            var Filter = new VendorProductDTO();
            Filter.VendorId = userID;
            Filter.Search = Search;

            if (From.HasValue)
                Filter.DateFrom = From.Value;

            if (To.HasValue)
                Filter.DateTo = To.Value;

            var PageSize =
                ActionType == SystemConstants.ActionType.Print ? 0 : BaseHelper.Constants.PageSize;

            var ProductRequest = new VendorProductRequest
            {
                RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
                UserID = userID,
                context = _context,
                VendorProductDTO = Filter,
                PageIndex = PageIndex,
                PageSize = PageSize,
                IsDesc = true,
                applyFilter = true,
            };

            var ProductResponse = VendorProductService.GetAllProducts(ProductRequest);
            ViewData.ObjDTOs = ProductResponse.VendorProductDTOs;
            ViewData.ObjDTO = ProductResponse.VendorProductDTO;
            if (ActionType == SystemConstants.ActionType.PartialView)
                return PartialView("/Views/Vendor/Reports/Stock/_StockReport.cshtml", ViewData);
            else if (ActionType == SystemConstants.ActionType.Table)
                return PartialView(
                    "/Views/Vendor/Reports/Stock/_StockReportTable.cshtml",
                    ProductResponse.VendorProductDTOs
                );
            else if (ActionType == SystemConstants.ActionType.Print)
                return BaseHelper.GeneratePDF<List<VendorProductDTO>>(
                    "/Views/Vendor/Reports/Stock/StockReportPDF.cshtml",
                    ProductResponse.VendorProductDTOs
                );
            else
                return View("/Views/Vendor/Reports/Stock/StockReport.cshtml", ViewData);
        }

        [AuthorizePerRole("Vendor_ReportsProblem")]
        public IActionResult ProblemReport(
            DateTime? From,
            DateTime? To,
            int StatusId,
            string Search,
            int PageIndex,
            string ActionType
        )
        {
            var userID = AuthHelper.GetClaimValue(User, "UserID");
            var filter = new AdsDTO();
            filter.Search = Search;
            filter.GamerId = userID;
            filter.StatusId = StatusId;
            if (From.HasValue)
                filter.DateFrom = From.Value;

            if (To.HasValue)
                filter.DateTo = To.Value;

            var PageSize =
                ActionType == SystemConstants.ActionType.Print ? 0 : BaseHelper.Constants.PageSize;
            var shipmentRequest = new AdvertisementRequest
            {
                RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
                UserID = userID,
                context = _context,
                AdsDTO = filter,
                IsDesc = true,
                PageIndex = PageIndex,
                PageSize = PageSize,
                applyFilter = true
            };

            var ShipmentResponse = BL.Services.AdvertisementService.GetAllAdvertisements(
                shipmentRequest
            );

            if (ActionType == SystemConstants.ActionType.PartialView)
                return PartialView(
                    "/Views/Vendor/Reports/Problem/_ProblemReport.cshtml",
                    ShipmentResponse.AdsDTOs
                );

            if (ActionType == SystemConstants.ActionType.Table)
                return PartialView(
                    "/Views/Vendor/Reports/Problem/_ProblemReportTable.cshtml",
                    ShipmentResponse.AdsDTOs
                );
            else if (ActionType == SystemConstants.ActionType.Print)
                return BaseHelper.GeneratePDF<List<AdsDTO>>(
                    "/Views/Vendor/Reports/Problem/ProblemReportPDF.cshtml",
                    ShipmentResponse.AdsDTOs
                );
            else
                return View(
                    "/Views/Vendor/Reports/Problem/ProblemReport.cshtml",
                    ShipmentResponse.AdsDTOs
                );
        }

        #endregion

        [AuthorizePerRole("Vendor_ShipmentList")]
        public IActionResult ShipmentList(
            string Search,
            int PageIndex,
            DateTime? From,
            DateTime? To,
            string ActionType = null
        )
        {
            var userID = AuthHelper.GetClaimValue(User, "UserID");
            var model = new AdsDTO();
            model.GamerId = userID;
            model.Search = Search;
            if (From.HasValue)
                model.DateFrom = From.Value;

            if (To.HasValue)
                model.DateTo = To.Value;

            var PageSize =
                ActionType == SystemConstants.ActionType.Print ? 0 : BaseHelper.Constants.PageSize;

            var shipmentRequest = new AdvertisementRequest
            {
                RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
                UserID = userID,
                context = _context,
                AdsDTO = model,
                applyFilter = true,
                PageIndex = PageIndex,
                PageSize = PageSize
            };
            var shipmentResponse = DicomApp.BL.Services.AdvertisementService.GetAllAdvertisements(
                shipmentRequest
            );
            if (ActionType == SystemConstants.ActionType.PartialView)
                return PartialView("_ShipmentList", shipmentResponse.AdsDTOs);
            else if (ActionType == SystemConstants.ActionType.Print)
                return BaseHelper.GeneratePDF<List<AdsDTO>>(
                    "ShipmentListPDF",
                    shipmentResponse.AdsDTOs
                );
            else if (ActionType == SystemConstants.ActionType.Table)
                return PartialView("_ShipmentListTable", shipmentResponse.AdsDTOs);
            else
                return View(shipmentResponse.AdsDTOs);
        }

        [AuthorizePerRole("Vendor_AddShipment")]
        public IActionResult AddOrder(string ActionType = null)
        {
            ViewBag.Category = _context.Category.ToList();

            ViewBag.ZoneList = _context.Zone.ToList();
            ViewBag.Vendors = GeneralHelper.GetUsers(SystemConstants.Role.Gamer, _context);

            if (ActionType == SystemConstants.ActionType.PartialView)
                return PartialView("/Views/Shared/Advertisement/_AddOrder.cshtml");
            else
                return View();
        }

        [HttpGet]
        [AuthorizePerRole("Vendor_PickupDelivery")]
        public IActionResult PickupRequest(
            DateTime? From,
            DateTime? To,
            string Search,
            bool? IsDesc,
            string ActionType = null
        )
        {
            ViewModel<AdsDTO> viewModel = new ViewModel<AdsDTO>();
            var filter = new AdsDTO();
            filter.Search = Search;
            filter.StatusId = (int)EnumStatus.Ready_For_Pickup;
            filter.GamerId = AuthHelper.GetClaimValue(User, "UserID");

            if (From.HasValue)
                filter.DateFrom = From.Value;

            if (To.HasValue)
                filter.DateTo = To.Value;

            var request = new AdvertisementRequest()
            {
                RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
                UserID = AuthHelper.GetClaimValue(User, "UserID"),
                context = _context,
                AdsDTO = filter,
                IsDesc = IsDesc ?? true,
                applyFilter = true,
            };

            var response = BL.Services.AdvertisementService.GetAllAdvertisements(request);

            viewModel.ObjDTOs = response.AdsDTOs;

            if (
                ActionType != SystemConstants.ActionType.Table
                && ActionType != SystemConstants.ActionType.Print
            )
            {
                viewModel.Lookup = BaseHelper.GetLookup(
                    new List<byte> { (byte)EnumSelectListType.Zone, (byte)EnumSelectListType.Area },
                    _context
                );
            }

            if (ActionType == SystemConstants.ActionType.PartialView)
                return PartialView("/Views/Vendor/PickUp/_PickUpRequest.cshtml", viewModel);
            else if (ActionType == SystemConstants.ActionType.Table)
                return PartialView(
                    "/Views/Vendor/PickUp/_PickUpRequestTable.cshtml",
                    viewModel.ObjDTOs
                );
            else if (ActionType == SystemConstants.ActionType.Print)
                return BaseHelper.GeneratePDF<List<AdsDTO>>(
                    "/Views/Vendor/PickUp/PickUpRequestPDF.cshtml",
                    viewModel.ObjDTOs
                );
            else
                return View("/Views/Vendor/PickUp/PickUpRequest.cshtml", viewModel);
        }

        [AuthorizePerRole("Vendor_PickupStock")]
        public IActionResult FulfillmentRequest(string ActionType)
        {
            ViewModel<AdsDTO> viewModel = new ViewModel<AdsDTO>();

            if (
                ActionType != SystemConstants.ActionType.Table
                && ActionType != SystemConstants.ActionType.Print
            )
            {
                viewModel.Lookup = BaseHelper.GetLookup(
                    new List<byte>
                    {
                        (byte)EnumSelectListType.Zone,
                        (byte)EnumSelectListType.Area,
                        (byte)EnumSelectListType.Users,
                    },
                    _context
                );
            }

            viewModel.Lookup.UserDTO = UserService
                .GetAllUsers(
                    new UserRequest
                    {
                        context = _context,
                        applyFilter = true,
                        UserDTO = new UserDTO { Id = AuthHelper.GetClaimValue(User, "UserID") }
                    }
                )
                .UserDTOs.FirstOrDefault();

            if (ActionType == SystemConstants.ActionType.PartialView)
                return PartialView("/Views/Vendor/PickUp/_FulfillmentRequest.cshtml", viewModel);
            else
                return View("/Views/Vendor/PickUp/FulfillmentRequest.cshtml", viewModel);
        }

        //[AuthorizePerRole("Vendor_PickupList")]
        //public IActionResult PickupList(
        //    string Search,
        //    string OrderByColumn,
        //    bool? IsDesc,
        //    int PageIndex,
        //    string ActionType = null
        //)
        //{
        //    ViewModel<PickupDTO> viewModel = new ViewModel<PickupDTO>();
        //    var model = new PickupDTO();
        //    model.IsDeleted = false;
        //    model.VendorId = AuthHelper.GetClaimValue(User, "UserID");
        //    model.Search = Search;
        //    var PickUpRequestRequest = new PickUpRequestRequest
        //    {
        //        RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
        //        UserID = AuthHelper.GetClaimValue(User, "UserID"),
        //        context = _context,
        //        PickupDTO = model,
        //        PageIndex = PageIndex,
        //        PageSize = BaseHelper.Constants.PageSize,
        //        IsDesc = IsDesc ?? true,
        //        OrderByColumn = OrderByColumn ?? "PickUpRequestId",
        //        HasPickupItemDTO = true,
        //        applyFilter = true
        //    };

        //    viewModel.Lookup = BaseHelper.GetLookup(
        //        new List<byte> { (byte)EnumSelectListType.Area },
        //        _context
        //    );

        //    var PickUpRequestResponse = PickUpRequestService.GetAllPickUpRequests(
        //        PickUpRequestRequest
        //    );
        //    viewModel.ObjDTOs = PickUpRequestResponse.PickupDTOs;

        //    if (ActionType == SystemConstants.ActionType.PartialView)
        //        return PartialView("/Views/Vendor/PickUp/_PickupList.cshtml", viewModel);
        //    else if (ActionType == SystemConstants.ActionType.Print)
        //        return BaseHelper.GeneratePDF<List<PickupDTO>>(
        //            "/Views/Vendor/PickUp/PickupListPDF.cshtml",
        //            viewModel.ObjDTOs
        //        );
        //    else if (ActionType == SystemConstants.ActionType.Table)
        //        return PartialView("/Views/Vendor/PickUp/_PickupListTable.cshtml", viewModel);
        //    else
        //        return View("/Views/Vendor/PickUp/PickupList.cshtml", viewModel);
        //}

        [AuthorizePerRole("Vendor_ShippingCalculator")]
        public IActionResult ShippingCalculator(string ActionType = null)
        {
            ViewBag.Area = _context.City.Where(c => c.ZoneId.HasValue).ToList();
            var model = new ShippingCalculatorDTO();

            if (ActionType == SystemConstants.ActionType.PartialView)
                return PartialView("_ShippingCalculator", model);
            else
                return View(model);
        }

        [AuthorizePerRole("Vendor_ContactUs")]
        public IActionResult ContactUs(string ActionType = null)
        {
            if (ActionType == SystemConstants.ActionType.PartialView)
                return PartialView("_ContactUs");
            else
                return View();
        }

        [AuthorizePerRole("Vendor_ContactUs")]
        [HttpPost]
        public IActionResult ContactUs(ContactUsDTO model)
        {
            var ContactUsRequest = new ContactUsRequest
            {
                RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
                UserID = AuthHelper.GetClaimValue(User, "UserID"),
                context = _context,
                ContactUsDTO = model
            };
            var ContactUsRespons = ContactUsService.AddContactUs(ContactUsRequest);
            if (ContactUsRespons.Success)
                return Json(ContactUsRespons.Message);
            else
                return Json(false);
        }

        public IActionResult MyAccount(string ActionType = null)
        {
            ViewModel<UserDTO> ViewModel = new ViewModel<UserDTO>();
            var userRequest = new UserRequest
            {
                RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
                UserID = AuthHelper.GetClaimValue(User, "UserID"),
                context = _context,
                UserDTO = new UserDTO { Id = AuthHelper.GetClaimValue(User, "UserID") }
            };
            var userResponse = UserService.GetUser(userRequest);
            ViewModel.ObjDTO = userResponse.UserDTO;
            if (userResponse.Success)
            {
                ViewModel.Lookup = BaseHelper.GetLookup(
                    new List<byte> { (byte)EnumSelectListType.Area, (byte)EnumSelectListType.Zone },
                    _context
                );

                if (ActionType == SystemConstants.ActionType.PartialView)
                    return PartialView("_MyAccount", ViewModel);
                else
                    return View(ViewModel);
            }
            else
                return RedirectToAction("AccountDashboard", "Home");
        }

        //[HttpPost]
        //public ActionResult EditUser(UserRecord model)
        //{
        //    var userRequest = new UserRequest
        //    {
        //        RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
        //        CreatedBy = AuthHelper.GetClaimValue(User, "UserID"),
        //        _context = _context,
        //        UserRecord = model
        //    };
        //    var userResponse = UserService.EditUser(userRequest);
        //    if (userResponse.Success)
        //    {
        //        TempData["SuccessMsg"] = userResponse.Message;
        //        if (model.RoleID == (long)RoleType.Vendor)
        //            return RedirectToAction("ListAccount");
        //        else
        //            return RedirectToAction("ListUser");
        //    }
        //    TempData["ErrorMsg"] = userResponse.Message;
        //    ViewBag.error = userResponse.Message;
        //    ViewBag.areas = _context.City.ToList();
        //    ViewBag.Zone = _context.Zone.ToList();
        //    return View(model);
        //}
        public IActionResult ShipmentDetails(int id)
        {
            //var model = new ShipmentDTO();
            //model.IsDeleted = false;
            //model.AdvertisementId = id;
            //model.VendorId = AuthHelper.GetClaimValue(User, "UserID");
            //var shipmentRequest = new ShipmentRequest
            //{
            //    RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
            //    UserID = AuthHelper.GetClaimValue(User, "UserID"),
            //    context = _context,
            //    ShipmentDTO = model,
            //    IsDetails = true,
            //    OrderByColumn = "AdvertisementId"
            //};
            //var userResponse = DicomApp.BL.Services.ShipmentService.ListShipment(shipmentRequest);
            //return PartialView("_ShipmentDetails", userResponse.AdvertisementDTOs.FirstOrDefault());
            return View();
        }

        [HttpPost]
        public IActionResult ChangePassword(
            string ConfirmPassword,
            string NewPassword,
            string Password
        )
        {
            int UserID = AuthHelper.GetClaimValue(User, "UserID");
            CommonUser user = _context.CommonUser.Find(UserID);

            if (user.Password == Password)
            {
                if (NewPassword == ConfirmPassword)
                {
                    user.Password = NewPassword;
                    _context.SaveChanges();

                    return Json("Password has been changed successfully");
                }
                else
                    return Json("New password does not match");
            }
            else
                return Json("Old password is incorrect");
        }

        [HttpPost]
        public ActionResult EditUser(UserDTO model)
        {
            var userRequest = new UserRequest
            {
                RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
                UserID = AuthHelper.GetClaimValue(User, "UserID"),
                context = _context,
                UserDTO = model
            };
            var userResponse = UserService.EditUser(userRequest);
            if (userResponse.Success)
            {
                return Json(userResponse.Message);
            }
            return Json(userResponse.Message);
        }
    }
}

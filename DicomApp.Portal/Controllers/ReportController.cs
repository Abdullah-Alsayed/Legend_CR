﻿using System;
using System.Collections.Generic;
using System.Linq;
using DicomApp.BL.Services;
using DicomApp.CommonDefinitions.DTO;
using DicomApp.CommonDefinitions.DTO.AdvertisementDTOs;
using DicomApp.CommonDefinitions.DTO.AdvertisementDTOs;
using DicomApp.CommonDefinitions.DTO.PickupDTOs;
using DicomApp.CommonDefinitions.DTO.VendorProductDTOs;
using DicomApp.CommonDefinitions.Requests;
using DicomApp.DAL.DB;
using DicomApp.Helpers;
using DicomApp.Portal.Helpers;
using DicomDB.BL.Services;
using DicomDB.CommonDefinitions.DTO;
using DicomDB.CommonDefinitions.Requests;
using Microsoft.AspNetCore.Mvc;
using Rotativa.AspNetCore;
using Rotativa.AspNetCore.Options;

namespace DicomApp.Portal.Controllers
{
    public class ReportController : Controller
    {
        private readonly ShippingDBContext _context;

        public ReportController(ShippingDBContext context)
        {
            _context = context;
        }

        [AuthorizePerRole("ReportsShipments")]
        public IActionResult Shipments(
            int VendorId,
            int ZoneId,
            int DeliveryManId,
            DateTime? From,
            DateTime? To,
            int StatusId,
            string Search,
            int Status,
            int PageIndex,
            string ActionType = null
        )
        {
            ViewModel<AdsDTO> ViewData = new ViewModel<AdsDTO>();
            var model = new AdsDTO();

            if (StatusId == 0)
                model.StatusId = Status;
            else
                model.StatusId = StatusId;

            model.Search = Search;
            model.GamerId = VendorId;
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
                PageSize = PageSize,
                PageIndex = PageIndex,
            };

            var shipmentResponse = BL.Services.AdvertisementService.GetAllAdvertisements(
                shipmentRequest
            );

            ViewData.ObjDTOs = shipmentResponse.AdsDTOs;

            if (
                ActionType != SystemConstants.ActionType.Table
                && ActionType != SystemConstants.ActionType.Print
            )
            {
                ViewData.Lookup = BaseHelper.GetLookup(
                    new List<byte>
                    {
                        (byte)EnumSelectListType.Zone,
                        (byte)EnumSelectListType.Vendor,
                        (byte)EnumSelectListType.Courier
                    },
                    _context
                );
            }

            if (ActionType == SystemConstants.ActionType.PartialView)
                return PartialView("/Views/Report/Shipment/Shipments.cshtml", ViewData);
            else if (ActionType == SystemConstants.ActionType.Table)
                return PartialView("/Views/Report/Shipment/_Shipments.cshtml", ViewData.ObjDTOs);
            else if (ActionType == SystemConstants.ActionType.Print)
                return BaseHelper.GeneratePDF<List<AdsDTO>>(
                    "/Views/Report/Shipment/ShipmentsPDF.cshtml",
                    ViewData.ObjDTOs
                );
            else
                return View("/Views/Report/Shipment/Shipments.cshtml", ViewData);
        }

        //[AuthorizePerRole("ReportsPickups")]
        //public IActionResult Pickups(
        //    DateTime? From,
        //    DateTime? To,
        //    int VendorId,
        //    int ZoneID,
        //    string Search,
        //    bool? IsDesc,
        //    int PageIndex,
        //    int StatusId,
        //    string ActionType = null
        //)
        //{
        //    ViewModel<PickupDTO> ViewData = new ViewModel<PickupDTO>();
        //    var model = new PickupDTO();
        //    model.IsDeleted = false;
        //    model.Search = Search;
        //    model.VendorId = VendorId;
        //    model.ZoneId = ZoneID;
        //    model.StatusId = StatusId;

        //    //if (From.HasValue)
        //    //    model.DateFrom = From.Value;

        //    //if (To.HasValue)
        //    //    model.DateTo = To.Value;

        //    var PageSize =
        //        ActionType == SystemConstants.ActionType.Print ? 0 : BaseHelper.Constants.PageSize;
        //    var PickUpRequestRequest = new PickUpRequestRequest
        //    {
        //        RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
        //        UserID = AuthHelper.GetClaimValue(User, "UserID"),
        //        context = _context,
        //        PickupDTO = model,
        //        PageIndex = PageIndex,
        //        PageSize = PageSize,
        //        IsDesc = IsDesc ?? true,
        //        applyFilter = true,
        //    };
        //    if (
        //        ActionType != SystemConstants.ActionType.Table
        //        && ActionType != SystemConstants.ActionType.Print
        //    )
        //    {
        //        ViewData.Lookup = BaseHelper.GetLookup(
        //            new List<byte>
        //            {
        //                (byte)EnumSelectListType.Zone,
        //                (byte)EnumSelectListType.Vendor
        //            },
        //            _context
        //        );
        //    }
        //    var PickupResponse = PickUpRequestService.GetAllPickUpRequests(PickUpRequestRequest);

        //    ViewData.ObjDTOs = PickupResponse.PickupDTOs;
        //    if (ActionType == SystemConstants.ActionType.PartialView)
        //        return PartialView("/Views/Report/Pickup/Pickups.cshtml", ViewData);
        //    else if (ActionType == SystemConstants.ActionType.Table)
        //        return PartialView("/Views/Report/Pickup/_Pickups.cshtml", ViewData.ObjDTOs);
        //    else if (ActionType == SystemConstants.ActionType.Print)
        //        return BaseHelper.GeneratePDF<List<PickupDTO>>(
        //            "/Views/Report/Pickup/PickupsPDF.cshtml",
        //            ViewData.ObjDTOs
        //        );
        //    else
        //        return View("/Views/Report/Pickup/Pickups.cshtml", ViewData);
        //}

        [AuthorizePerRole("ReportsStock")]
        public IActionResult Stock(
            int VendorId,
            string Search,
            DateTime? From,
            DateTime? To,
            string ActionType
        )
        {
            ViewModel<VendorProductDTO> ViewData = new ViewModel<VendorProductDTO>();
            var Filter = new VendorProductDTO();
            Filter.VendorId = VendorId;
            Filter.Search = Search;

            if (From.HasValue)
                Filter.DateFrom = From.Value;

            if (To.HasValue)
                Filter.DateTo = To.Value;
            var ProductRequest = new VendorProductRequest
            {
                RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
                UserID = AuthHelper.GetClaimValue(User, "UserID"),
                context = _context,
                VendorProductDTO = Filter,
                IsDesc = true,
                applyFilter = true,
            };

            var ProductResponse = VendorProductService.GetAllProducts(ProductRequest);

            ViewData.ObjDTOs = ProductResponse.VendorProductDTOs;

            if (
                ActionType != SystemConstants.ActionType.Table
                && ActionType != SystemConstants.ActionType.Print
            )
                ViewData.Lookup = BaseHelper.GetLookup(
                    new List<byte> { (byte)EnumSelectListType.Vendor },
                    _context
                );

            if (ActionType == SystemConstants.ActionType.PartialView)
                return PartialView("/Views/Report/Stock/Stock.cshtml", ViewData);
            else if (ActionType == SystemConstants.ActionType.Table)
                return PartialView("/Views/Report/Stock/_Stock.cshtml", ViewData.ObjDTOs);
            else if (ActionType == SystemConstants.ActionType.Print)
                return BaseHelper.GeneratePDF<List<VendorProductDTO>>(
                    "/Views/Report/Stock/StockPDF.cshtml",
                    ViewData.ObjDTOs
                );
            else
                return View("/Views/Report/Stock/Stock.cshtml", ViewData);
        }

        [AuthorizePerRole("ReportsCouriers")]
        public IActionResult Couriers(
            DateTime? From,
            DateTime? To,
            int DeliveryManId,
            string Search,
            int PageIndex,
            string ActionType
        )
        {
            ViewModel<AdsDTO> ViewData = new ViewModel<AdsDTO>();
            var model = new AdsDTO();
            model.Search = Search;

            if (From.HasValue)
                model.DateFrom = From.Value;
            if (To.HasValue)
                model.DateTo = To.Value;
            var shipmentRequest = new AdvertisementRequest
            {
                RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
                UserID = AuthHelper.GetClaimValue(User, "UserID"),
                context = _context,
                AdsDTO = model,
                PageIndex = PageIndex,
                PageSize = BaseHelper.Constants.PageSize,
                IsDesc = true,
                applyFilter = true,
            };

            var ShipmentResponse = DicomApp.BL.Services.AdvertisementService.GetAllAdvertisements(
                shipmentRequest
            );
            ViewData.ObjDTOs = ShipmentResponse.AdsDTOs;

            if (
                ActionType != SystemConstants.ActionType.Table
                && ActionType != SystemConstants.ActionType.Print
            )
                ViewData.Lookup = BaseHelper.GetLookup(
                    new List<byte> { (byte)EnumSelectListType.Courier },
                    _context
                );

            if (ActionType == SystemConstants.ActionType.PartialView)
                return PartialView("/Views/Report/Courier/Couriers.cshtml", ViewData);
            else if (ActionType == SystemConstants.ActionType.Table)
                return PartialView("/Views/Report/Courier/_Couriers.cshtml", ViewData.ObjDTOs);
            else if (ActionType == SystemConstants.ActionType.Print)
                return BaseHelper.GeneratePDF<List<AdsDTO>>(
                    "/Views/Report/Courier/CouriersPDF.cshtml",
                    ViewData.ObjDTOs
                );
            else
                return View("/Views/Report/Courier/Couriers.cshtml", ViewData);
        }

        [AuthorizePerRole("ReportsGame")]
        public IActionResult Games(
            DateTime? From,
            DateTime? To,
            int? Quantity,
            int CategoryId,
            string Search,
            int PageIndex,
            string ActionType = null
        )
        {
            ViewModel<GameDTO> ViewData = new ViewModel<GameDTO>();
            var filter = new GameDTO();
            filter.search = Search;
            filter.CategoryId = CategoryId;

            if (From.HasValue)
                filter.DateFrom = From.Value;

            if (To.HasValue)
                filter.DateTo = To.Value;

            var GameRequest = new GameRequest
            {
                RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
                UserID = AuthHelper.GetClaimValue(User, "UserID"),
                context = _context,
                GameDTO = filter,
                PageIndex = PageIndex,
                PageSize = BaseHelper.Constants.PageSize,
                IsDesc = true,
                applyFilter = true,
            };

            var ShipmentResponse = DicomApp.BL.Services.GameService.GetGames(GameRequest);

            ViewData.ObjDTOs = ShipmentResponse.GameDTOs;

            if (
                ActionType != SystemConstants.ActionType.Table
                && ActionType != SystemConstants.ActionType.Print
            )
            {
                ViewData.Lookup = BaseHelper.GetLookup(
                    new List<byte>
                    {
                        (byte)EnumSelectListType.Category,
                        (byte)EnumSelectListType.Quantity,
                    },
                    _context
                );
            }

            if (ActionType == SystemConstants.ActionType.PartialView)
                return PartialView("/Views/Report/Game/Games.cshtml", ViewData);
            else if (ActionType == SystemConstants.ActionType.Table)
                return PartialView("/Views/Report/Game/_Games.cshtml", ViewData.ObjDTOs);
            else if (ActionType == SystemConstants.ActionType.Print)
                return BaseHelper.GeneratePDF<List<GameDTO>>(
                    "/Views/Report/Game/GamesPDF.cshtml",
                    ViewData.ObjDTOs
                );
            else
                return View("/Views/Report/Game/Games.cshtml", ViewData);
        }

        [AuthorizePerRole("ReportsCustomerFollowup")]
        public IActionResult CustomerFollowup(
            DateTime? From,
            DateTime? To,
            int VendorId,
            int StatusId,
            string Search,
            int PageIndex,
            string ActionType
        )
        {
            ViewModel<AdsDTO> ViewData = new ViewModel<AdsDTO>();
            var filter = new AdsDTO();
            filter.Search = Search;
            filter.GamerId = VendorId;
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
                UserID = AuthHelper.GetClaimValue(User, "UserID"),
                context = _context,
                AdsDTO = filter,
                IsDesc = true,
                PageIndex = PageIndex,
                PageSize = PageSize,
                applyFilter = true
            };

            if (
                ActionType != SystemConstants.ActionType.Table
                && ActionType != SystemConstants.ActionType.Print
            )
                ViewData.Lookup = BaseHelper.GetLookup(
                    new List<byte>
                    {
                        (byte)EnumSelectListType.Vendor,
                        (byte)EnumSelectListType.Status,
                    },
                    _context
                );

            var ShipmentResponse = BL.Services.AdvertisementService.GetAllAdvertisements(
                shipmentRequest
            );
            ViewData.ObjDTOs = ShipmentResponse.AdsDTOs;

            if (ActionType == SystemConstants.ActionType.PartialView)
                return PartialView(
                    "/Views/Report/CustomerFollowup/CustomerFollowup.cshtml",
                    ViewData
                );

            if (ActionType == SystemConstants.ActionType.Table)
                return PartialView(
                    "/Views/Report/CustomerFollowup/_CustomerFollowup.cshtml",
                    ViewData.ObjDTOs
                );
            else if (ActionType == SystemConstants.ActionType.Print)
                return BaseHelper.GeneratePDF<List<AdsDTO>>(
                    "/Views/Report/CustomerFollowup/CustomerFollowupPDF.cshtml",
                    ViewData.ObjDTOs
                );
            else
                return View("/Views/Report/CustomerFollowup/CustomerFollowup.cshtml", ViewData);
        }

        [AuthorizePerRole("ReportsComplains")]
        public ActionResult Complains(
            string ActionType,
            string Search,
            int VendorId,
            string OrderByColumn,
            bool? IsDesc,
            int PageIndex,
            int EmployeeId,
            bool? Solved,
            string Department,
            DateTime? From,
            DateTime? To
        )
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
                applyFilter = true,
                IsDesc = true,
                PageIndex = PageIndex,
                PageSize = BaseHelper.Constants.PageSize,
            };

            var ComplainsRespons = ComplainsService.GetComplains(ComplainsRequest);

            ViewData.ObjDTOs = ComplainsRespons.ComplainsDTOs;

            if (
                ActionType != SystemConstants.ActionType.Table
                && ActionType != SystemConstants.ActionType.Print
            )
                ViewData.Lookup = BaseHelper.GetLookup(
                    new List<byte>
                    {
                        (byte)EnumSelectListType.Vendor,
                        (byte)EnumSelectListType.Employee
                    },
                    _context
                );

            if (ActionType == SystemConstants.ActionType.PartialView)
                return PartialView("/Views/Report/Complain/Complains.cshtml", ViewData);
            else if (ActionType == SystemConstants.ActionType.Table)
                return PartialView("/Views/Report/Complain/_Complains.cshtml", ViewData.ObjDTOs);
            else if (ActionType == SystemConstants.ActionType.Print)
                return BaseHelper.GeneratePDF<List<ComplainsDTO>>(
                    "/Views/Report/Complain/ComplainsPDF.cshtml",
                    ViewData.ObjDTOs
                );
            else
                return View("/Views/Report/Complain/Complains.cshtml", ViewData);
        }

        [AuthorizePerRole("ReportsInvoices")]
        public IActionResult Invoices(
            int VendorId,
            DateTime? From,
            DateTime? To,
            string Search,
            int PageIndex,
            string ActionType = null
        )
        {
            //ViewModel<ShipDTO> ViewData = new ViewModel<ShipDTO>();

            //var request = new CashTransferRequest
            //{
            //    context = _context,
            //    RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
            //    UserID = AuthHelper.GetClaimValue(User, "UserID"),
            //    //CashTransferDTO = new CashTransferDTO() { VendorId = VendorID }
            //};

            //var response = CashTransferService.GetAllCashTransfers(request);

            //if (ActionType == Constants.ActionType.PartialView)
            //    return PartialView(response.InvoiceDTO);
            //else if (ActionType == Constants.ActionType.Table)
            //    return PartialView("_Invoices", response.InvoiceDTO);
            //else
            //    return View(response.InvoiceDTO);


            //var model = new ShipDTO();
            //if (StatusId == (int)EnumStatus.Current)
            //    model.StatusIDs = new List<int>
            //    {
            //        (int)EnumStatus.Ready_For_Delivery,
            //        (int)EnumStatus.Assigned_For_Delivery,
            //        (int)EnumStatus.Assigned_For_Pickup,
            //        (int)EnumStatus.At_Warehouse,
            //        (int)EnumStatus.Picked_Up,
            //        (int)EnumStatus.Out_For_Delivery
            //    };
            //else if (StatusId == 0)
            //    model.StatusId = Status;
            //else
            //    model.StatusId = StatusId;

            //model.Search = Search;
            //model.VendorDetailsDTO = new VendorDetailsDTO { VendorId = VendorId };
            //model.ZoneId = ZoneId;
            //model.DeliveryManId = DeliveryManId;
            //if (From.HasValue)
            //    model.DateFrom = From.Value;

            //if (To.HasValue)
            //    model.DateTo = To.Value;

            //var PageSize = ActionType == Constants.ActionType.Print ? 0 : BaseHelper.Constants.PageSize;

            //var shipmentRequest = new ShipmentRequest
            //{
            //    RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
            //    UserID = AuthHelper.GetClaimValue(User, "UserID"),
            //    context = _context,
            //    ShipDTO = model,
            //    HasCustomerDetailsDTO = true,
            //    HasVendorDetailsDTO = true,
            //    HasFeesDetailsDTO = true,
            //    HasSettingDTO = true,
            //    applyFilter = true,
            //    PageSize = PageSize,
            //    PageIndex = PageIndex,
            //};

            //var shipmentResponse = BL.Services.ShipmentService.GetAllShipments(shipmentRequest);

            //ViewData.ObjDTOs = shipmentResponse.ShipDTOs;

            //if (ActionType != Constants.ActionType.Table && ActionType != Constants.ActionType.Print)
            //{
            //    ViewData.Lookup = BaseHelper.GetLookup(new List<byte> {
            //        (byte)EnumSelectListType.Zone,
            //        (byte)EnumSelectListType.Vendor,
            //        (byte)EnumSelectListType.Courier
            //    }, _context);
            //}

            //if (ActionType == Constants.ActionType.PartialView)
            //    return PartialView("/Views/Report/Shipment/Shipments.cshtml", ViewData);

            //else if (ActionType == Constants.ActionType.Table)
            //    return PartialView("/Views/Report/Shipment/_Shipments.cshtml", ViewData.ObjDTOs);

            //else if (ActionType == Constants.ActionType.Print)
            //    return BaseHelper.GeneratePDF<List<ShipDTO>>("/Views/Report/Shipment/ShipmentsPDF.cshtml", ViewData.ObjDTOs);

            //else
            //    return View("/Views/Report/Shipment/Shipments.cshtml", ViewData);

            return View();
        }

        [AuthorizePerRole("ReportsVendor")]
        public IActionResult Vendor(
            DateTime? From,
            DateTime? To,
            int VendorId,
            string Search,
            string ActionType
        )
        {
            var shipmentRequest = new AdvertisementRequest
            {
                RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
                UserID = AuthHelper.GetClaimValue(User, "UserID"),
                context = _context,
                AdsDTO = new AdsDTO { GamerId = 0 }
            };

            var shipmentResponse = BL.Services.AdvertisementService.GetAllAdvertisements(
                shipmentRequest
            );

            if (ActionType == SystemConstants.ActionType.PartialView)
                return PartialView(
                    "/Views/Report/Vendor/Vendor.cshtml",
                    shipmentResponse.VendorReportDTOs
                );
            else if (ActionType == SystemConstants.ActionType.Table)
                return PartialView(
                    "/Views/Report/Vendor/Vendor.cshtml",
                    shipmentResponse.VendorReportDTOs
                );
            else
                return View(
                    "/Views/Report/Vendor/Vendor.cshtml",
                    shipmentResponse.VendorReportDTOs
                );
        }

        public IActionResult pdf(string v, int s)
        {
            int vID = int.Parse(EncryptionManager.Decrypt(v));

            var shipmentRequest = new AdvertisementRequest
            {
                RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
                UserID = AuthHelper.GetClaimValue(User, "UserID"),
                context = _context,
                AdsDTO = new AdsDTO { StatusId = s, GamerId = vID }
            };

            var shipmentResponse = BL.Services.AdvertisementService.GetAdvertisement(
                shipmentRequest
            );

            var data = shipmentResponse.VendorReportDTOs.FirstOrDefault().lstShips.ToList();

            var pdfFile = new ViewAsPdf("~/Views/Report/Vendor/pdf.cshtml", data)
            {
                PageMargins = new Margins(2, 2, 2, 2),
                PageOrientation = Orientation.Portrait,
                PageSize = Size.A4
            };

            return pdfFile;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DicomApp.BL.Services;
using DicomApp.BL.Services.Helpers;
using DicomApp.CommonDefinitions.DTO;
using DicomApp.CommonDefinitions.DTO.AdvertisementDTOs;
using DicomApp.CommonDefinitions.Requests;
using DicomApp.DAL.DB;
using DicomApp.Helpers;
using DicomApp.Portal.Helpers;
using DicomApp.Portal.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SystemConstants = DicomApp.Helpers.SystemConstants;

namespace DicomApp.Portal.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly ShippingDBContext _context;

        public DashboardController(ShippingDBContext context) => _context = context;

        public IActionResult Alerts()
        {
            var notificationRequest = new UserNotificationRequest
            {
                context = _context,
                RoleID = AuthHelper.GetClaimValue(User, SystemConstants.Claims.RoleID),
                UserID = AuthHelper.GetClaimValue(User, SystemConstants.Claims.UserID),
                //GetMineOnly = true,
                MarkAsSeen = true,
            };

            var notificationResponse = UserNotificationService.ListUserNotification(
                notificationRequest
            );
            return View(notificationResponse.UserNotificationRecords);
        }

        [AuthorizePerRole("Dashboard")]
        public IActionResult Main(DateTime? From, DateTime? To, string ActionType)
        {
            var roleName = AuthHelper.GetClaimStringValue(User, SystemConstants.Claims.RoleName);
            var roleID = AuthHelper.GetClaimValue(User, SystemConstants.Claims.RoleID);
            //if (roleName == EnumRole.Gamer.ToString())
            //    return RedirectToAction(nameof(BayerDashboard));

            #region Filter Data
            var filter = new DashboardDTO();
            if (From.HasValue)
                filter.DateFrom = From.Value;

            if (To.HasValue)
                filter.DateTo = To.Value;
            #endregion
            var Request = new DashboardRequest()
            {
                RoleID = roleID,
                UserID = AuthHelper.GetClaimValue(User, SystemConstants.Claims.UserID),
                context = _context,
                DashboardDTO = filter,
                TopBuyer = 15,
                TopGames = 20,
                TopDriver = 20,
                PackagingStock = 15,
            };

            if (ActionType == SystemConstants.ActionType.PartialView)
                return PartialView(DashboardHelper.GenerateDashboard(Request));
            else
                return View(DashboardHelper.GenerateDashboard(Request));
        }

        //[AuthorizePerRole("AccountDashboard")]
        //public IActionResult BayerDashboard(
        //    DateTime? From,
        //    DateTime? To,
        //    int VendorID,
        //    string ActionType
        //)
        //{
        //    int UserID = AuthHelper.GetClaimValue(User, "UserID");
        //    int RolID = AuthHelper.GetClaimValue(User, "RoleID");

        //    #region Filter Data
        //    var ViewData = new ViewModel<DashboardDTO>();
        //    var filter = new AdsDTO();
        //    if (VendorID == 0)
        //        filter.GamerId = UserID;
        //    else
        //        filter.GamerId = VendorID;

        //    if (From.HasValue)
        //        filter.DateFrom = From.Value;

        //    if (To.HasValue)
        //        filter.DateTo = To.Value;
        //    #endregion

        //    if (RolID != (int)EnumRole.Gamer)
        //        ViewData.Lookup = BaseHelper.GetLookup(
        //            new List<byte> { (byte)EnumSelectListType.Vendor, },
        //            _context
        //        );

        //    var Request = new DashboardRequest()
        //    {
        //        RoleID = RolID,
        //        UserID = UserID,
        //        context = _context,
        //        DashboardDTO = filter,
        //        TopAccount = 15,
        //        PackagingStock = 15,
        //        TopArea = 20,
        //        TopDriver = 20
        //    };

        //    ViewData.ObjDTO = DashboardHelper.GenerateAccountDashboard(Request, VendorID);

        //    if (ActionType == SystemConstants.ActionType.PartialView)
        //        return PartialView("_AccountDashboard", ViewData);
        //    else
        //        return View(ViewData);
        //}

        [AuthorizePerRole("Dashboard")]
        public IActionResult IndexPartial(DateTime? From, DateTime? To)
        {
            ViewBag.dateFrom = null;
            ViewBag.dateTo = null;

            #region Get Data
            //List<ShipmentDTO> AllOrders, TodayOrders = null;
            //AllOrders = TodayOrders = _context.Shipment
            //    .Include(c => c.Vendor)
            //    .Include(c => c.DeliveryMan)
            //    .Include(c => c.Area)
            //    .Where(p => p.StatusId != (int)EnumStatus.Archive)
            //    .Select(p => new ShipmentDTO
            //    {
            //        AdvertisementId = p.AdvertisementId,
            //        CreatedAt = p.ShipmentDate,
            //        LastStatusAt = p.DeliveryDate,
            //        PaidAt = p.OrderDate,
            //        StatusId = p.StatusId,
            //        Freight = p.Freight,
            //        IsFullShipment = p.IsFullShipment,
            //        Tax = p.Tax,
            //        PaidTax = p.PaidTax,
            //        SubTotal = p.SubTotal,
            //        Total = p.Total,
            //        ReturnedAmount = p.ReturnedAmount,
            //        IsAfford = p.IsAfford,
            //        IsPaid = p.IsPaid,
            //        IsFeesPaid = p.IsFeesPaid,
            //        VendorName = p.Vendor.Name,
            //        VendorId = p.VendorId,
            //        CustomerRefPhone = p.CustomerRefPhone,
            //        DeliveryManName = p.DeliveryMan.Name,
            //        DeliveryManId = p.DeliveryManId,
            //        AreaName = p.Area.CityNameAr,
            //        AreaId = p.AreaId
            //    }).ToList();
            //TodayOrders = TodayOrders.Where(p => p.LastStatusAt.Date == DateTime.Today).ToList();
            #endregion

            #region Filter Data
            //if (From.HasValue)
            //{
            //    ViewBag.dateFrom = From.Value.ToString("MM/dd/yyyy");
            //    AllOrders = AllOrders.Where(p => p.CreatedAt.Date >= From).ToList();
            //}
            //if (To.HasValue)
            //{
            //    ViewBag.dateTo = To.Value.ToString("MM/dd/yyyy");
            //    AllOrders = AllOrders.Where(p => p.CreatedAt.Date <= To).ToList();
            //}
            #endregion

            var record = new DashboardDTO();
            #region Charts
            //record.Chart_Year = AllOrders.Where(p => (p.StatusId == (int)EnumStatus.Delivered || p.StatusId == (int)EnumStatus.Cancelled) && p.LastStatusAt.Year == DateTime.Now.Year)
            //    .GroupBy(p => p.LastStatusAt.Year)
            //    .Select(p => new ChartRecord
            //    {
            //        Key = p.Key.ToString(),
            //        Value1 = p.Where(c => c.LastStatusAt.Month == 1 && c.StatusId == (int)EnumStatus.Cancelled).Sum(t => t.PaidTax).Value
            //               + p.Where(c => c.LastStatusAt.Month == 1 && c.StatusId == (int)EnumStatus.Delivered).Sum(t => t.Tax),
            //        Value2 = p.Where(c => c.LastStatusAt.Month == 2 && c.StatusId == (int)EnumStatus.Cancelled).Sum(t => t.PaidTax).Value
            //               + p.Where(c => c.LastStatusAt.Month == 2 && c.StatusId == (int)EnumStatus.Delivered).Sum(t => t.Tax),
            //        Value3 = p.Where(c => c.LastStatusAt.Month == 3 && c.StatusId == (int)EnumStatus.Cancelled).Sum(t => t.PaidTax).Value
            //               + p.Where(c => c.LastStatusAt.Month == 3 && c.StatusId == (int)EnumStatus.Delivered).Sum(t => t.Tax),
            //        Value4 = p.Where(c => c.LastStatusAt.Month == 4 && c.StatusId == (int)EnumStatus.Cancelled).Sum(t => t.PaidTax).Value
            //               + p.Where(c => c.LastStatusAt.Month == 4 && c.StatusId == (int)EnumStatus.Delivered).Sum(t => t.Tax),
            //        Value5 = p.Where(c => c.LastStatusAt.Month == 5 && c.StatusId == (int)EnumStatus.Cancelled).Sum(t => t.PaidTax).Value
            //               + p.Where(c => c.LastStatusAt.Month == 5 && c.StatusId == (int)EnumStatus.Delivered).Sum(t => t.Tax),
            //        Value6 = p.Where(c => c.LastStatusAt.Month == 6 && c.StatusId == (int)EnumStatus.Cancelled).Sum(t => t.PaidTax).Value
            //               + p.Where(c => c.LastStatusAt.Month == 6 && c.StatusId == (int)EnumStatus.Delivered).Sum(t => t.Tax),
            //        Value7 = p.Where(c => c.LastStatusAt.Month == 7 && c.StatusId == (int)EnumStatus.Cancelled).Sum(t => t.PaidTax).Value
            //               + p.Where(c => c.LastStatusAt.Month == 7 && c.StatusId == (int)EnumStatus.Delivered).Sum(t => t.Tax),
            //        Value8 = p.Where(c => c.LastStatusAt.Month == 8 && c.StatusId == (int)EnumStatus.Cancelled).Sum(t => t.PaidTax).Value
            //               + p.Where(c => c.LastStatusAt.Month == 8 && c.StatusId == (int)EnumStatus.Delivered).Sum(t => t.Tax),
            //        Value9 = p.Where(c => c.LastStatusAt.Month == 9 && c.StatusId == (int)EnumStatus.Cancelled).Sum(t => t.PaidTax).Value
            //               + p.Where(c => c.LastStatusAt.Month == 9 && c.StatusId == (int)EnumStatus.Delivered).Sum(t => t.Tax),
            //        Value10 = p.Where(c => c.LastStatusAt.Month == 10 && c.StatusId == (int)EnumStatus.Cancelled).Sum(t => t.PaidTax).Value
            //               + p.Where(c => c.LastStatusAt.Month == 10 && c.StatusId == (int)EnumStatus.Delivered).Sum(t => t.Tax),
            //        Value11 = p.Where(c => c.LastStatusAt.Month == 11 && c.StatusId == (int)EnumStatus.Cancelled).Sum(t => t.PaidTax).Value
            //               + p.Where(c => c.LastStatusAt.Month == 11 && c.StatusId == (int)EnumStatus.Delivered).Sum(t => t.Tax),
            //        Value12 = p.Where(c => c.LastStatusAt.Month == 12 && c.StatusId == (int)EnumStatus.Cancelled).Sum(t => t.PaidTax).Value
            //               + p.Where(c => c.LastStatusAt.Month == 12 && c.StatusId == (int)EnumStatus.Delivered).Sum(t => t.Tax),
            //    }).ToArray();

            //record.Chart_Top10Account = AllOrders.Where(p => p.VendorId != null)
            //    .GroupBy(p => p.VendorName)
            //    .Select(p => new ChartRecord
            //    {
            //        Key = p.Key,
            //        Value1 = p.Where(c => c.StatusId == (int)EnumStatus.Ready_For_Pickup
            //                           || c.StatusId == (int)EnumStatus.Assigned_For_Pickup
            //                           || c.StatusId == (int)EnumStatus.Out_For_Delivery
            //                           || c.StatusId == (int)EnumStatus.Assigned_For_Delivery).Count(),
            //        Value2 = p.Where(c => c.StatusId == (int)EnumStatus.Delivered).Count(),
            //        Value3 = p.Where(c => c.StatusId == (int)EnumStatus.Cancelled).Count()
            //    })
            //    .OrderByDescending(p => p.Value2).Take(10).ToArray();

            //record.Chart_Top10Driver = AllOrders.Where(p => p.DeliveryManId != null)
            //    .GroupBy(p => p.DeliveryManName)
            //    .Select(p => new ChartRecord
            //    {
            //        Key = p.Key,
            //        Value1 = p.Where(c => c.StatusId == (int)EnumStatus.Assigned_For_Pickup
            //                           || c.StatusId == (int)EnumStatus.Assigned_For_Delivery).Count(),
            //        Value2 = p.Where(c => c.StatusId == (int)EnumStatus.Delivered).Count(),
            //        Value3 = p.Where(c => c.StatusId == (int)EnumStatus.Cancelled).Count()
            //    })
            //    .OrderByDescending(p => p.Value2).Take(10).ToArray();

            //record.Chart_Top10Area = AllOrders.Where(p => p.AreaId != null)
            //    .GroupBy(p => p.AreaName ?? "Other")
            //    .Select(p => new ChartRecord
            //    {
            //        Key = p.Key,
            //        Value1 = p.Where(c => c.StatusId == (int)EnumStatus.Ready_For_Pickup
            //                           || c.StatusId == (int)EnumStatus.Assigned_For_Pickup
            //                           || c.StatusId == (int)EnumStatus.Out_For_Delivery
            //                           || c.StatusId == (int)EnumStatus.Assigned_For_Delivery).Count(),
            //        Value2 = p.Where(c => c.StatusId == (int)EnumStatus.Delivered).Count(),
            //        Value3 = p.Where(c => c.StatusId == (int)EnumStatus.Cancelled).Count()
            //    })
            //    .OrderByDescending(p => p.Value2).Take(10).ToArray();
            #endregion
            return PartialView("_IndexPartial", record);
        }

        [AuthorizePerRole("Dashboard")]
        public IActionResult AllOrdersIncome(DateTime? From, DateTime? To)
        {
            #region Get Data
            //List<ShipmentDTO> AllOrders, TodayOrders = null;
            //AllOrders = TodayOrders = _context.Shipment
            //    .Where(p => p.StatusId != (int)EnumStatus.Archive)
            //    .Select(p => new ShipmentDTO
            //    {
            //        AdvertisementId = p.AdvertisementId,
            //        CreatedAt = p.ShipmentDate,
            //        LastStatusAt = p.DeliveryDate,
            //        PaidAt = p.OrderDate,
            //        StatusId = p.StatusId,
            //        Freight = p.Freight,
            //        IsFullShipment = p.IsFullShipment,
            //        Tax = p.Tax,
            //        PaidTax = p.PaidTax,
            //        SubTotal = p.SubTotal,
            //        Total = p.Total,
            //        ReturnedAmount = p.ReturnedAmount,
            //        IsAfford = p.IsAfford,
            //        IsPaid = p.IsPaid,
            //        IsFeesPaid = p.IsFeesPaid,
            //        CustomerRefPhone = p.CustomerRefPhone
            //    }).ToList();
            //TodayOrders = TodayOrders.Where(p => p.LastStatusAt.Date == DateTime.Today).ToList();
            #endregion

            #region Filter Data
            //if (From.HasValue)
            //{
            //    ViewBag.dateFrom = From.Value.ToString("MM/dd/yyyy");
            //    AllOrders = AllOrders.Where(p => p.CreatedAt.Date >= From).ToList();
            //}
            //if (To.HasValue)
            //{
            //    ViewBag.dateTo = To.Value.ToString("MM/dd/yyyy");
            //    AllOrders = AllOrders.Where(p => p.CreatedAt.Date <= To).ToList();
            //}
            #endregion

            var record = new DashboardDTO();

            #region All Orders Income
            //record.AllOrders_Total = AllOrders.Sum(s => s.Total);
            //record.AllOrders_Tax = AllOrders.Where(p => p.StatusId == (int)EnumStatus.Cancelled).Sum(p => p.PaidTax).Value + AllOrders.Where(p => p.StatusId == (int)EnumStatus.Delivered).Sum(p => p.Tax);
            //record.AllOrders_Subtotal = record.AllOrders_Total - record.AllOrders_Tax;
            #endregion
            return Json(record);
        }

        [AuthorizePerRole("Dashboard")]
        public IActionResult TodayOrdersIncome(DateTime? From, DateTime? To)
        {
            #region Get Data
            //List<ShipmentDTO> AllOrders, TodayOrders = null;
            //AllOrders = TodayOrders = _context.Shipment
            //    .Where(p => p.StatusId != (int)EnumStatus.Archive)
            //    .Select(p => new ShipmentDTO
            //    {
            //        AdvertisementId = p.AdvertisementId,
            //        CreatedAt = p.ShipmentDate,
            //        LastStatusAt = p.DeliveryDate,
            //        PaidAt = p.OrderDate,
            //        StatusId = p.StatusId,
            //        Freight = p.Freight,
            //        IsFullShipment = p.IsFullShipment,
            //        Tax = p.Tax,
            //        PaidTax = p.PaidTax,
            //        SubTotal = p.SubTotal,
            //        Total = p.Total,
            //        ReturnedAmount = p.ReturnedAmount,
            //        IsAfford = p.IsAfford,
            //        IsPaid = p.IsPaid,
            //        IsFeesPaid = p.IsFeesPaid,
            //        CustomerRefPhone = p.CustomerRefPhone
            //    }).ToList();
            //TodayOrders = TodayOrders.Where(p => p.LastStatusAt.Date == DateTime.Today).ToList();
            #endregion

            //#region Filter Data
            //if (From.HasValue)
            //{
            //    ViewBag.dateFrom = From.Value.ToString("MM/dd/yyyy");
            //    AllOrders = AllOrders.Where(p => p.CreatedAt.Date >= From).ToList();
            //}
            //if (To.HasValue)
            //{
            //    ViewBag.dateTo = To.Value.ToString("MM/dd/yyyy");
            //    AllOrders = AllOrders.Where(p => p.CreatedAt.Date <= To).ToList();
            //}
            //#endregion

            var record = new DashboardDTO();

            //#region Today Orders Income
            //record.TodayOrders_Total = TodayOrders.Sum(s => s.Total);
            //record.TodayOrders_Tax = TodayOrders.Where(p => p.StatusId == (int)EnumStatus.Cancelled).Sum(p => p.PaidTax).Value
            //                       + TodayOrders.Where(p => p.StatusId == (int)EnumStatus.Delivered).Sum(p => p.Tax);
            //record.TodayOrders_Subtotal = record.TodayOrders_Total - record.TodayOrders_Tax;
            //record.TodayOrders_Paid = TodayOrders.Where(p => p.StatusId == (int)EnumStatus.Delivered && p.ReturnedAmount.HasValue && p.IsPaid.HasValue).Sum(p => p.ReturnedAmount - p.Tax).Value
            //                        + TodayOrders.Where(p => p.StatusId == (int)EnumStatus.Delivered && !p.ReturnedAmount.HasValue && p.IsPaid.HasValue).Sum(p => p.SubTotal);
            //#endregion
            return Json(record);
        }

        [AuthorizePerRole("Dashboard")]
        public IActionResult AllOrdersCount(DateTime? From, DateTime? To)
        {
            //#region Get Data
            //List<ShipmentDTO> AllOrders, TodayOrders = null;
            //AllOrders = TodayOrders = _context.Shipment
            //    .Where(p => p.StatusId != (int)EnumStatus.Archive)
            //    .Select(p => new ShipmentDTO
            //    {
            //        AdvertisementId = p.AdvertisementId,
            //        CreatedAt = p.ShipmentDate,
            //        LastStatusAt = p.DeliveryDate,
            //        PaidAt = p.OrderDate,
            //        StatusId = p.StatusId,
            //        Freight = p.Freight,
            //        IsFullShipment = p.IsFullShipment,
            //        Tax = p.Tax,
            //        PaidTax = p.PaidTax,
            //        SubTotal = p.SubTotal,
            //        Total = p.Total,
            //        ReturnedAmount = p.ReturnedAmount,
            //        IsAfford = p.IsAfford,
            //        IsPaid = p.IsPaid,
            //        IsFeesPaid = p.IsFeesPaid,
            //        CustomerRefPhone = p.CustomerRefPhone
            //    }).ToList();
            //TodayOrders = TodayOrders.Where(p => p.LastStatusAt.Date == DateTime.Today).ToList();
            //#endregion

            //#region Filter Data
            //if (From.HasValue)
            //{
            //    ViewBag.dateFrom = From.Value.ToString("MM/dd/yyyy");
            //    AllOrders = AllOrders.Where(p => p.CreatedAt.Date >= From).ToList();
            //}
            //if (To.HasValue)
            //{
            //    ViewBag.dateTo = To.Value.ToString("MM/dd/yyyy");
            //    AllOrders = AllOrders.Where(p => p.CreatedAt.Date <= To).ToList();
            //}
            //#endregion

            var record = new DashboardDTO();

            //#region All Orders Count
            //record.AllOrders_Count = AllOrders.Count();
            //record.NewOrders_Count = AllOrders.Where(p => p.StatusId == (int)EnumStatus.Ready_For_Pickup
            //                                           || p.StatusId == (int)EnumStatus.Assigned_For_Pickup
            //                                           || p.StatusId == (int)EnumStatus.Out_For_Delivery
            //                                           || p.StatusId == (int)EnumStatus.Assigned_For_Delivery).Count();
            //record.DeliveredOrders_Count = AllOrders.Where(p => p.StatusId == (int)EnumStatus.Delivered).Count();
            //record.CanceledOrders_Count = AllOrders.Where(p => p.StatusId == (int)EnumStatus.Cancelled).Count();
            //#endregion
            return Json(record);
        }

        public IActionResult AccountDashboardPartial(DateTime? From, DateTime? To, int VendorID)
        {
            int UserID = AuthHelper.GetClaimValue(User, "UserID");
            if (AuthHelper.GetClaimValue(User, "RoleID") == (int)EnumRole.Gamer)
            {
                ViewBag.Accounts = _context.CommonUser.Where(p => p.RoleId == 4 && p.Id == UserID);
                VendorID = UserID;
            }
            else
                ViewBag.Accounts = _context.CommonUser.Where(p => p.RoleId == 4);

            //#region Get Data
            //List<ShipmentDTO> AllOrders, TodayOrders = null;
            //AllOrders = TodayOrders = _context.Shipment
            //    .Include(c => c.Vendor)
            //    .Include(c => c.DeliveryMan)
            //    .Include(c => c.Area)
            //    .Include(c => c.Status)
            //    .Where(p => p.StatusId != (int)EnumStatus.Archive && p.VendorId == VendorID)
            //    .Select(p => new ShipmentDTO
            //    {
            //        AdvertisementId = p.AdvertisementId,
            //        CreatedAt = p.ShipmentDate,
            //        LastStatusAt = p.DeliveryDate,
            //        PaidAt = p.OrderDate,
            //        StatusId = p.StatusId,
            //        Freight = p.Freight,
            //        IsFullShipment = p.IsFullShipment,
            //        Tax = p.Tax,
            //        PaidTax = p.PaidTax,
            //        SubTotal = p.SubTotal,
            //        Total = p.Total,
            //        ReturnedAmount = p.ReturnedAmount,
            //        IsAfford = p.IsAfford,
            //        IsPaid = p.IsPaid,
            //        IsFeesPaid = p.IsFeesPaid,
            //        VendorName = p.Vendor.Name,
            //        VendorId = p.VendorId,
            //        CustomerName = p.CustomerRefName,
            //        CustomerRefPhone = p.CustomerRefPhone,
            //        CustomerRefPhone2 = p.CustomerRefPhone2,
            //        CustomerRefAdress = p.CustomerRefAdress,
            //        DeliveryManName = p.DeliveryMan.Name,
            //        DeliveryManId = p.DeliveryManId,
            //        AreaName = p.Area.CityNameAr,
            //        AreaId = p.AreaId,
            //        StatusName = p.Status.Name
            //    }).ToList();
            //TodayOrders = TodayOrders.Where(p => p.LastStatusAt.Date == DateTime.Today).ToList();
            //#endregion

            //#region Filter Data
            //if (From.HasValue)
            //{
            //    ViewBag.dateFrom = From.Value.ToString("MM/dd/yyyy");
            //    AllOrders = AllOrders.Where(p => p.CreatedAt.Date >= From).ToList();
            //}
            //if (To.HasValue)
            //{
            //    ViewBag.dateTo = To.Value.ToString("MM/dd/yyyy");
            //    AllOrders = AllOrders.Where(p => p.CreatedAt.Date <= To).ToList();
            //}
            //#endregion

            var record = new DashboardDTO();
            //#region Today Orders Income
            //record.TodayOrders_Subtotal = TodayOrders.Where(p => p.StatusId == (int)EnumStatus.Delivered).Sum(p => p.SubTotal);
            //record.TodayOrders_Tax = TodayOrders.Where(p => p.StatusId == (int)EnumStatus.Cancelled).Sum(p => p.PaidTax).Value
            //                       + TodayOrders.Where(p => p.StatusId == (int)EnumStatus.Delivered).Sum(p => p.Tax);
            //record.TodayOrders_Paid = TodayOrders.Where(p => p.StatusId == (int)EnumStatus.Delivered && p.ReturnedAmount.HasValue && p.IsPaid.HasValue).Sum(p => p.ReturnedAmount - p.Tax).Value
            //                        + TodayOrders.Where(p => p.StatusId == (int)EnumStatus.Delivered && !p.ReturnedAmount.HasValue && p.IsPaid.HasValue).Sum(p => p.SubTotal);
            //record.TodayOrders_NotPaid = TodayOrders.Where(p => p.StatusId == (int)EnumStatus.Delivered && p.ReturnedAmount.HasValue && !p.IsPaid.HasValue).Sum(p => p.ReturnedAmount - p.Tax).Value
            //                           + TodayOrders.Where(p => p.StatusId == (int)EnumStatus.Delivered && !p.ReturnedAmount.HasValue && !p.IsPaid.HasValue).Sum(p => p.SubTotal);
            //#endregion
            //#region All Orders Income
            //record.AllOrders_Subtotal = AllOrders.Where(p => p.StatusId == (int)EnumStatus.Delivered).Sum(p => p.SubTotal);
            //record.AllOrders_Paid = AllOrders.Where(p => p.StatusId == (int)EnumStatus.Delivered && p.ReturnedAmount.HasValue && p.IsPaid.HasValue).Sum(p => p.ReturnedAmount - p.Tax).Value
            //                      + AllOrders.Where(p => p.StatusId == (int)EnumStatus.Delivered && !p.ReturnedAmount.HasValue && p.IsPaid.HasValue).Sum(p => p.SubTotal);
            //record.AllOrders_NotPaid = AllOrders.Where(p => p.StatusId == (int)EnumStatus.Delivered && p.ReturnedAmount.HasValue && !p.IsPaid.HasValue).Sum(p => p.ReturnedAmount - p.Tax).Value
            //                         + AllOrders.Where(p => p.StatusId == (int)EnumStatus.Delivered && !p.ReturnedAmount.HasValue && !p.IsPaid.HasValue).Sum(p => p.SubTotal);
            //#endregion
            //#region All Orders Count
            //record.AllOrders_Count = AllOrders.Count();
            //record.NewOrders_Count = AllOrders.Where(p => p.StatusId == (int)EnumStatus.Ready_For_Pickup
            //                                           || p.StatusId == (int)EnumStatus.Assigned_For_Pickup
            //                                           || p.StatusId == (int)EnumStatus.Out_For_Delivery
            //                                           || p.StatusId == (int)EnumStatus.Assigned_For_Delivery).Count();
            //record.DeliveredOrders_Count = AllOrders.Where(p => p.StatusId == (int)EnumStatus.Delivered).Count();
            //record.CanceledOrders_Count = AllOrders.Where(p => p.StatusId == (int)EnumStatus.Cancelled).Count();
            //#endregion
            //#region Charts
            //record.Chart_Year = AllOrders.Where(p => (p.StatusId == (int)EnumStatus.Delivered || p.StatusId == (int)EnumStatus.Cancelled) && p.LastStatusAt.Year == DateTime.Now.Year)
            //    .GroupBy(p => p.LastStatusAt.Year)
            //    .Select(p => new ChartRecord
            //    {
            //        Key = p.Key.ToString(),
            //        Value1 = p.Where(c => c.LastStatusAt.Month == 1 && c.StatusId == (int)EnumStatus.Delivered).Sum(t => t.SubTotal),
            //        Value2 = p.Where(c => c.LastStatusAt.Month == 2 && c.StatusId == (int)EnumStatus.Delivered).Sum(t => t.SubTotal),
            //        Value3 = p.Where(c => c.LastStatusAt.Month == 3 && c.StatusId == (int)EnumStatus.Delivered).Sum(t => t.SubTotal),
            //        Value4 = p.Where(c => c.LastStatusAt.Month == 4 && c.StatusId == (int)EnumStatus.Delivered).Sum(t => t.SubTotal),
            //        Value5 = p.Where(c => c.LastStatusAt.Month == 5 && c.StatusId == (int)EnumStatus.Delivered).Sum(t => t.SubTotal),
            //        Value6 = p.Where(c => c.LastStatusAt.Month == 6 && c.StatusId == (int)EnumStatus.Delivered).Sum(t => t.SubTotal),
            //        Value7 = p.Where(c => c.LastStatusAt.Month == 7 && c.StatusId == (int)EnumStatus.Delivered).Sum(t => t.SubTotal),
            //        Value8 = p.Where(c => c.LastStatusAt.Month == 8 && c.StatusId == (int)EnumStatus.Delivered).Sum(t => t.SubTotal),
            //        Value9 = p.Where(c => c.LastStatusAt.Month == 9 && c.StatusId == (int)EnumStatus.Delivered).Sum(t => t.SubTotal),
            //        Value10 = p.Where(c => c.LastStatusAt.Month == 10 && c.StatusId == (int)EnumStatus.Delivered).Sum(t => t.SubTotal),
            //        Value11 = p.Where(c => c.LastStatusAt.Month == 11 && c.StatusId == (int)EnumStatus.Delivered).Sum(t => t.SubTotal),
            //        Value12 = p.Where(c => c.LastStatusAt.Month == 12 && c.StatusId == (int)EnumStatus.Delivered).Sum(t => t.SubTotal),
            //    }).ToArray();

            //record.Chart_Top10Area = AllOrders.Where(p => p.AreaId != null)
            //    .GroupBy(p => p.AreaName ?? "Other")
            //    .Select(p => new ChartRecord
            //    {
            //        Key = p.Key,
            //        Value1 = p.Where(c => c.StatusId == (int)EnumStatus.Ready_For_Pickup
            //                           || c.StatusId == (int)EnumStatus.Assigned_For_Pickup
            //                           || c.StatusId == (int)EnumStatus.Out_For_Delivery
            //                           || c.StatusId == (int)EnumStatus.Assigned_For_Delivery).Count(),
            //        Value2 = p.Where(c => c.StatusId == (int)EnumStatus.Delivered).Count(),
            //        Value3 = p.Where(c => c.StatusId == (int)EnumStatus.Cancelled).Count()
            //    })
            //    .OrderByDescending(p => p.Value2).Take(10).ToArray();
            //#endregion

            //record.Orders = AllOrders.OrderByDescending(p => p.AdvertisementId).Take(10).ToList();
            return PartialView("_AccountDashboard", record);
        }

        [AuthorizePerRole("Dashboard")]
        public IActionResult Index3(DateTime? dateFrom, DateTime? dateTo, long vendorid)
        {
            //if (dateFrom.HasValue)
            //    ViewBag.dateFrom = dateFrom.Value.ToString("MM/dd/yyyy");
            //if (dateTo.HasValue)
            //    ViewBag.dateTo = dateTo.Value.ToString("MM/dd/yyyy");
            //var orders = _context.Shipment.Where(p => p.StatusId != (int)EnumStatus.Archive);
            //if (dateFrom.HasValue)
            //    orders = orders.Where(p => p.CreatedAt.Date >= dateFrom);
            //if (dateTo.HasValue)
            //    orders = orders.Where(p => p.ShipmentDate.Date <= dateTo);

            var record = new DashboardDTO();
            ////record.Orders = orders.Count();
            ////record.Accounts = orders.GroupBy(p => p.VendorId).Count();
            ////record.New = orders.Where(p => p.StatusId == (int)EnumStatus.Out_For_Delivery).Count();
            ////record.Delivered = orders.Where(p => p.StatusId == (int)EnumStatus.Delivered).Count();
            //record.Chart_Top10Area = orders.Where(p => p.AreaId != null).GroupBy(p => p.Area.CityName ?? "Other")
            //    .Select(p => new ChartRecord
            //    {
            //        Key = p.Key,
            //        Value1 = p.Where(c => c.StatusId == (int)EnumStatus.Out_For_Delivery).Count(),
            //        Value2 = p.Where(c => c.StatusId == (int)EnumStatus.Delivered).Count(),
            //        Value3 = p.Where(c => c.StatusId == (int)EnumStatus.Cancelled).Count()
            //    })
            //    .OrderByDescending(p => p.Value2).Take(5).ToArray();
            //record.Chart_Top10Driver = orders.Where(p => p.DeliveryManId != null)
            //    .GroupBy(p => p.DeliveryMan.Name)
            //    .Select(p => new ChartRecord
            //    {
            //        Key = p.Key,
            //        Value1 = p.Where(c => c.StatusId == (int)EnumStatus.Out_For_Delivery).Count(),
            //        Value2 = p.Where(c => c.StatusId == (int)EnumStatus.Delivered).Count(),
            //        Value3 = p.Where(c => c.StatusId == (int)EnumStatus.Cancelled).Count()
            //    })
            //    .OrderByDescending(p => p.Value2).Take(10).ToArray();
            //record.Chart_Top10Account = orders.Where(p => p.VendorId != null)
            //    .GroupBy(p => p.Vendor.Name)
            //    .Select(p => new ChartRecord
            //    {
            //        Key = p.Key,
            //        Value1 = p.Where(c => c.StatusId == (int)EnumStatus.Out_For_Delivery).Count(),
            //        Value2 = p.Where(c => c.StatusId == (int)EnumStatus.Delivered).Count(),
            //        Value3 = p.Where(c => c.StatusId == (int)EnumStatus.Cancelled).Count()
            //    })
            //    .OrderByDescending(p => p.Value2).Take(10).ToArray();
            //record.AreaChartDate = orders.Where(p => p.ShipmentDate != null && p.ShipmentDate.DateTime.Ticks > 0)
            //    .GroupBy(p => p.ShipmentDate.DateTime.ToString("MM/dd/yyyy"))
            //    .Select(p => new ChartRecord
            //    {
            //        Key = p.Key.ToString(),
            //        Value1 = p.Where(c => c.StatusId == (int)EnumStatus.Out_For_Delivery).Count(),
            //        Value2 = p.Where(c => c.StatusId == (int)EnumStatus.Delivered).Count(),
            //        Value3 = p.Where(c => c.StatusId == (int)EnumStatus.Cancelled).Count()
            //    })
            //    .OrderByDescending(p => p.Value1 + p.Value2).Take(5).ToArray();

            return View(record);
        }

        public IActionResult About()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            var exceptionFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            if (exceptionFeature != null)
            {
                // Get which route the exception occurred at
                string routeWhereExceptionOccurred = exceptionFeature.Path;

                // Get the exception that occurred
                Exception exceptionThatOccurred = exceptionFeature.Error;

                LogHelper.LogException(
                    routeWhereExceptionOccurred + " ** " + exceptionThatOccurred.Message,
                    exceptionThatOccurred.StackTrace
                );
            }
            return View(
                new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
                }
            );
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}

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
            if (roleName == EnumRole.Gamer.ToString())
                return RedirectToAction("Main", "Gamer");

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
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DicomApp.CommonDefinitions.DTO;
using DicomApp.CommonDefinitions.DTO.AdvertisementDTOs;
using DicomApp.CommonDefinitions.DTO.AdvertisementDTOs;
using DicomApp.CommonDefinitions.Requests;
using DicomApp.CommonDefinitions.Responses;
using DicomApp.DAL.DB;
using DicomApp.Helpers;
using Microsoft.EntityFrameworkCore;

namespace DicomApp.BL.Services.Helpers
{
    public class DashboardHelper
    {
        public static DashboardDTO GenerateDashboard(DashboardRequest request)
        {
            var shipmentRequest = new AdvertisementRequest
            {
                RoleID = request.RoleID,
                UserID = request.UserID,
                context = request.context,
                AdsDTO = request.AdsDTO,
                IsDesc = true,
                applyFilter = true,
            };

            var shipmentResponse = BL.Services.AdvertisementService.GetAllAdvertisements(
                shipmentRequest
            );
            //var invoiceRequest = new InvoiceRequest
            //{
            //    context = request.context,
            //    RoleID = request.RoleID,
            //    UserID = request.UserID,
            //    IsDesc = true
            //};
            //   var InvoiceResponse = InvoiceService.ListInvoice(invoiceRequest);

            var DashDTO = new DashboardDTO();

            ShipmentBoxs(ref DashDTO, shipmentResponse.AdsDTOs);

            GetTotalEarning(ref DashDTO, shipmentResponse.AdsDTOs);

            //ChartTotalEarning(ref DashDTO, InvoiceResponse.InvoiceRecords);

            //RevenuesAndExpenses(ref DashDTO, InvoiceResponse.InvoiceRecords);

            #region Charts
            DashDTO.Chart_Year = GetChartYear(shipmentResponse.AdsDTOs);

            DashDTO.Chart_TopAccount = ChartTopAccount(
                shipmentResponse.AdsDTOs,
                request.TopAccount
            );

            DashDTO.Chart_TopDriver = ChartTopDriver(shipmentResponse.AdsDTOs, request.TopDriver);

            DashDTO.Chart_TopArea = ChartTopArea(shipmentResponse.AdsDTOs, request.TopArea);

            DashDTO.Chart_PackagingStock = ChartPackagingStock(
                request.context,
                request.PackagingStock
            );
            #endregion
            return DashDTO;
        }

        public static DashboardDTO GenerateAccountDashboard(DashboardRequest request, int VendorID)
        {
            var shipmentRequest = new AdvertisementRequest
            {
                RoleID = request.RoleID,
                UserID = request.UserID,
                context = request.context,
                AdsDTO = request.AdsDTO,
                IsDesc = true,
                applyFilter = true,
            };
            var shipmentResponse = BL.Services.AdvertisementService.GetAllAdvertisements(
                shipmentRequest
            );

            //var invoiceRequest = new InvoiceRequest
            //{
            //    context = request.context,
            //    RoleID = request.RoleID,
            //    UserID = request.UserID,
            //    IsDesc = true,
            //    applyFilter = true,
            //    InvoiceRecord = new InvoiceDTO { VendorId = VendorID },
            //};
            //var InvoiceResponse = InvoiceService.ListInvoice(invoiceRequest);

            var DashDTO = new DashboardDTO();

            ShipmentBoxs(ref DashDTO, shipmentResponse.AdsDTOs);

            GetTotalEarning(ref DashDTO, shipmentResponse.AdsDTOs);

            ChartTotalEarningVendor(ref DashDTO);

            //AvailableBalance(ref DashDTO, InvoiceResponse.InvoiceRecords);
            #region Charts
            DashDTO.Chart_Year = GetChartYear(shipmentResponse.AdsDTOs);
            DashDTO.Chart_TopArea = ChartTopArea(shipmentResponse.AdsDTOs, request.TopArea);
            #endregion
            return DashDTO;
        }

        private static ChartDTO[] GetChartYear(List<AdsDTO> DataList)
        {
            ChartDTO[] ReturnValue = null;
            //if (DataList != null)
            //{
            //    ReturnValue = DataList
            //        .Where(p =>
            //            (
            //                p.StatusId == (int)EnumStatus.Delivered
            //                || p.StatusId == (int)EnumStatus.Cancelled
            //            )
            //            && p.CreatedAt.Year == DateTime.Now.Year
            //        )
            //        .GroupBy(p => p.CreatedAt.Year)
            //        .Select(p => new ChartDTO
            //        {
            //            Key = p.Key.ToString(),
            //            Value1 =
            //                p.Where(c =>
            //                        c.CreatedAt.Month == 1
            //                        && c.StatusId == (int)EnumStatus.Cancelled
            //                    )
            //                    .Sum(t => t.FeesDetailsDTO.ShippingFeesPaid)
            //                + p.Where(c =>
            //                        c.CreatedAt.Month == 1
            //                        && c.StatusId == (int)EnumStatus.Delivered
            //                    )
            //                    .Sum(t => t.FeesDetailsDTO.ShippingFees),
            //            Value2 =
            //                p.Where(c =>
            //                        c.CreatedAt.Month == 2
            //                        && c.StatusId == (int)EnumStatus.Cancelled
            //                    )
            //                    .Sum(t => t.FeesDetailsDTO.ShippingFeesPaid)
            //                + p.Where(c =>
            //                        c.CreatedAt.Month == 2
            //                        && c.StatusId == (int)EnumStatus.Delivered
            //                    )
            //                    .Sum(t => t.FeesDetailsDTO.ShippingFees),
            //            Value3 =
            //                p.Where(c =>
            //                        c.CreatedAt.Month == 3
            //                        && c.StatusId == (int)EnumStatus.Cancelled
            //                    )
            //                    .Sum(t => t.FeesDetailsDTO.ShippingFeesPaid)
            //                + p.Where(c =>
            //                        c.CreatedAt.Month == 3
            //                        && c.StatusId == (int)EnumStatus.Delivered
            //                    )
            //                    .Sum(t => t.FeesDetailsDTO.ShippingFees),
            //            Value4 =
            //                p.Where(c =>
            //                        c.CreatedAt.Month == 4
            //                        && c.StatusId == (int)EnumStatus.Cancelled
            //                    )
            //                    .Sum(t => t.FeesDetailsDTO.ShippingFeesPaid)
            //                + p.Where(c =>
            //                        c.CreatedAt.Month == 4
            //                        && c.StatusId == (int)EnumStatus.Delivered
            //                    )
            //                    .Sum(t => t.FeesDetailsDTO.ShippingFees),
            //            Value5 =
            //                p.Where(c =>
            //                        c.CreatedAt.Month == 5
            //                        && c.StatusId == (int)EnumStatus.Cancelled
            //                    )
            //                    .Sum(t => t.FeesDetailsDTO.ShippingFeesPaid)
            //                + p.Where(c =>
            //                        c.CreatedAt.Month == 5
            //                        && c.StatusId == (int)EnumStatus.Delivered
            //                    )
            //                    .Sum(t => t.FeesDetailsDTO.ShippingFees),
            //            Value6 =
            //                p.Where(c =>
            //                        c.CreatedAt.Month == 6
            //                        && c.StatusId == (int)EnumStatus.Cancelled
            //                    )
            //                    .Sum(t => t.FeesDetailsDTO.ShippingFeesPaid)
            //                + p.Where(c =>
            //                        c.CreatedAt.Month == 6
            //                        && c.StatusId == (int)EnumStatus.Delivered
            //                    )
            //                    .Sum(t => t.FeesDetailsDTO.ShippingFees),
            //            Value7 =
            //                p.Where(c =>
            //                        c.CreatedAt.Month == 7
            //                        && c.StatusId == (int)EnumStatus.Cancelled
            //                    )
            //                    .Sum(t => t.FeesDetailsDTO.ShippingFeesPaid)
            //                + p.Where(c =>
            //                        c.CreatedAt.Month == 7
            //                        && c.StatusId == (int)EnumStatus.Delivered
            //                    )
            //                    .Sum(t => t.FeesDetailsDTO.ShippingFees),
            //            Value8 =
            //                p.Where(c =>
            //                        c.CreatedAt.Month == 8
            //                        && c.StatusId == (int)EnumStatus.Cancelled
            //                    )
            //                    .Sum(t => t.FeesDetailsDTO.ShippingFeesPaid)
            //                + p.Where(c =>
            //                        c.CreatedAt.Month == 8
            //                        && c.StatusId == (int)EnumStatus.Delivered
            //                    )
            //                    .Sum(t => t.FeesDetailsDTO.ShippingFees),
            //            Value9 =
            //                p.Where(c =>
            //                        c.CreatedAt.Month == 9
            //                        && c.StatusId == (int)EnumStatus.Cancelled
            //                    )
            //                    .Sum(t => t.FeesDetailsDTO.ShippingFeesPaid)
            //                + p.Where(c =>
            //                        c.CreatedAt.Month == 9
            //                        && c.StatusId == (int)EnumStatus.Delivered
            //                    )
            //                    .Sum(t => t.FeesDetailsDTO.ShippingFees),
            //            Value10 =
            //                p.Where(c =>
            //                        c.CreatedAt.Month == 10
            //                        && c.StatusId == (int)EnumStatus.Cancelled
            //                    )
            //                    .Sum(t => t.FeesDetailsDTO.ShippingFeesPaid)
            //                + p.Where(c =>
            //                        c.CreatedAt.Month == 10
            //                        && c.StatusId == (int)EnumStatus.Delivered
            //                    )
            //                    .Sum(t => t.FeesDetailsDTO.ShippingFees),
            //            Value11 =
            //                p.Where(c =>
            //                        c.CreatedAt.Month == 11
            //                        && c.StatusId == (int)EnumStatus.Cancelled
            //                    )
            //                    .Sum(t => t.FeesDetailsDTO.ShippingFeesPaid)
            //                + p.Where(c =>
            //                        c.CreatedAt.Month == 11
            //                        && c.StatusId == (int)EnumStatus.Delivered
            //                    )
            //                    .Sum(t => t.FeesDetailsDTO.ShippingFees),
            //            Value12 =
            //                p.Where(c =>
            //                        c.CreatedAt.Month == 12
            //                        && c.StatusId == (int)EnumStatus.Cancelled
            //                    )
            //                    .Sum(t => t.FeesDetailsDTO.ShippingFeesPaid)
            //                + p.Where(c =>
            //                        c.CreatedAt.Month == 12
            //                        && c.StatusId == (int)EnumStatus.Delivered
            //                    )
            //                    .Sum(t => t.FeesDetailsDTO.ShippingFees),
            //        })
            //        .ToArray();
            //}
            return ReturnValue ?? new ChartDTO[0];
        }

        private static ChartDTO[] ChartTopAccount(List<AdsDTO> DataList, int TopCount)
        {
            ChartDTO[] ReturnData = null;
            //if (DataList != null)
            //{
            //    ReturnData = DataList
            //        .Where(p => p.VendorDetailsDTO.VendorId != 0)
            //        .GroupBy(p => p.VendorDetailsDTO.VendorName)
            //        .Select(p => new ChartDTO
            //        {
            //            Key = p.Key,
            //            Value1 = p.Where(c =>
            //                    c.StatusId == (int)EnumStatus.Ready_For_Pickup
            //                    || c.StatusId == (int)EnumStatus.Assigned_For_Pickup
            //                    || c.StatusId == (int)EnumStatus.Out_For_Delivery
            //                    || c.StatusId == (int)EnumStatus.Assigned_For_Delivery
            //                )
            //                .Count(),
            //            Value2 = p.Where(c => c.StatusId == (int)EnumStatus.Delivered).Count(),
            //            Value3 = p.Where(c => c.StatusId == (int)EnumStatus.Cancelled).Count()
            //        })
            //        .OrderByDescending(p => p.Value2)
            //        .Take(TopCount)
            //        .ToArray();
            //}
            return ReturnData ?? new ChartDTO[0];
        }

        private static ChartDTO[] ChartTopDriver(List<AdsDTO> DataList, int TopCount)
        {
            ChartDTO[] ReturnData = null;
            //if (DataList != null)
            //{
            //    ReturnData = DataList
            //        .Where(p => p.DeliveryManId != null)
            //        .GroupBy(p => p.DeliveryManName)
            //        .Select(p => new ChartDTO
            //        {
            //            Key = p.Key,
            //            Value1 = p.Where(c =>
            //                    c.StatusId == (int)EnumStatus.Assigned_For_Pickup
            //                    || c.StatusId == (int)EnumStatus.Assigned_For_Delivery
            //                )
            //                .Count(),
            //            Value2 = p.Where(c => c.StatusId == (int)EnumStatus.Delivered).Count(),
            //            Value3 = p.Where(c => c.StatusId == (int)EnumStatus.Cancelled).Count()
            //        })
            //        .OrderByDescending(p => p.Value2)
            //        .Take(TopCount)
            //        .ToArray();
            //}
            return ReturnData ?? new ChartDTO[0];
        }

        private static ChartDTO[] ChartTopArea(List<AdsDTO> DataList, int TopCount)
        {
            ChartDTO[] ReturnData = null;
            //if (DataList != null)
            //{
            //    ReturnData = DataList
            //        .Where(p => p.AreaId > 0)
            //        .GroupBy(p => p.AreaName ?? "Other")
            //        .Select(p => new ChartDTO
            //        {
            //            Key = p.Key,
            //            Value1 = p.Where(c => c.StatusId == (int)EnumStatus.Delivered).Count()
            //        })
            //        .OrderByDescending(p => p.Value1)
            //        .Take(TopCount)
            //        .ToArray();
            //}
            return ReturnData ?? new ChartDTO[0];
        }

        private static ChartDTO[] ChartPackagingStock(ShippingDBContext context, int TopCount)
        {
            var Game = GameService.GetGames(new GameRequest() { context = context }).GameDTOs;
            ChartDTO[] ReturnData = null;
            if (Game != null)
            {
                ReturnData = Game.GroupBy(p => p.NameEn ?? "Other")
                    .Select(p => new ChartDTO { Key = p.Key, Value1 = p.Count() })
                    .OrderByDescending(p => p.Value1)
                    .Take(TopCount)
                    .ToArray();
            }
            return ReturnData ?? new ChartDTO[0];
        }

        private static void ShipmentBoxs(ref DashboardDTO DTO, List<AdsDTO> List)
        {
            // #region All Order Box
            //if (List != null)
            //{
            //    DTO.AllOrders_Count = List.Count();
            //    DTO.AllOrders_Total = List.Sum(s => s.FeesDetailsDTO.Total);

            //    double AllOrders_LastManth = List.Where(s =>
            //            new DateTime(DateTime.Now.Year, DateTime.Now.AddMonths(-1).Month, 1)
            //                >= s.CreatedAt
            //            && s.CreatedAt < new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1)
            //        )
            //        .Count();

            //    double AllOrders_PresentManth = List.Where(s =>
            //            s.CreatedAt.Date >= new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1)
            //        )
            //        .Count();

            //    DTO.AllOrders_Percentchanged = BaseHelper.PercentageCalculation(
            //        AllOrders_PresentManth,
            //        AllOrders_LastManth
            //    );

            //    DTO.AllOrders_Arow =
            //        AllOrders_LastManth > AllOrders_PresentManth
            //            ? SystemConstants.ArrowType.Down
            //            : SystemConstants.ArrowType.Up;

            //#endregion

            //    #region Pending Orders Box
            //    DTO.PendingOrders_Count = List.Where(p =>
            //            p.StatusId != (int)EnumStatus.Delivered
            //            && p.StatusId != (int)EnumStatus.Cancelled
            //            && p.StatusId != (int)EnumStatus.Paid_To_Vendor
            //        )
            //        .Count();

            //    DTO.PendingOrders_Total = List.Where(p =>
            //            p.StatusId != (int)EnumStatus.Delivered
            //            && p.StatusId != (int)EnumStatus.Cancelled
            //            && p.StatusId != (int)EnumStatus.Paid_To_Vendor
            //        )
            //        .Sum(s => s.FeesDetailsDTO.Total);

            //    double PendingOrders_LastManth = List.Where(p =>
            //            p.StatusId != (int)EnumStatus.Delivered
            //            && p.StatusId != (int)EnumStatus.Cancelled
            //            && p.StatusId != (int)EnumStatus.Paid_To_Vendor
            //            && new DateTime(DateTime.Now.Year, DateTime.Now.AddMonths(-1).Month, 1)
            //                >= p.CreatedAt
            //            && p.CreatedAt < new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1)
            //        )
            //        .Count();

            //    double PendingOrders_PresentManth = List.Where(p =>
            //            p.StatusId != (int)EnumStatus.Delivered
            //            && p.StatusId != (int)EnumStatus.Cancelled
            //            && p.StatusId != (int)EnumStatus.Paid_To_Vendor
            //            && p.CreatedAt.Date
            //                >= new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1)
            //        )
            //        .Count();

            //    DTO.PendingOrders_Percentchanged = BaseHelper.PercentageCalculation(
            //        PendingOrders_PresentManth,
            //        PendingOrders_LastManth
            //    );

            //    DTO.PendingOrders_Arow =
            //        PendingOrders_LastManth > PendingOrders_PresentManth
            //            ? SystemConstants.ArrowType.Down
            //            : SystemConstants.ArrowType.Up;

            //    #endregion

            //    #region Delivered Orders Box

            //    DTO.DeliveredOrders_Count = List.Where(p => p.StatusId == (int)EnumStatus.Delivered)
            //        .Count();
            //    DTO.DeliveredOrders_Total = List.Where(s => s.StatusId == (int)EnumStatus.Delivered)
            //        .Sum(s => s.FeesDetailsDTO.Total);

            //    double DeliveredOrders_LastManth = List.Where(s =>
            //            s.StatusId == (int)EnumStatus.Delivered
            //            && new DateTime(DateTime.Now.Year, DateTime.Now.AddMonths(-1).Month, 1)
            //                >= s.CreatedAt
            //            && s.CreatedAt < new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1)
            //        )
            //        .Count();

            //    double DeliveredOrders_PresentManth = List.Where(s =>
            //            s.StatusId == (int)EnumStatus.Delivered
            //            && s.CreatedAt.Date
            //                >= new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1)
            //        )
            //        .Count();

            //    DTO.DeliveredOrders_Percentchanged = BaseHelper.PercentageCalculation(
            //        DeliveredOrders_PresentManth,
            //        DeliveredOrders_LastManth
            //    );

            //    DTO.DeliveredOrders_Arow =
            //        DeliveredOrders_LastManth > DeliveredOrders_PresentManth
            //            ? SystemConstants.ArrowType.Down
            //            : SystemConstants.ArrowType.Up;

            //    #endregion

            //    #region Canceled Orders Box
            //    DTO.CanceledOrders_Count = List.Where(p => p.StatusId == (int)EnumStatus.Cancelled)
            //        .Count();
            //    DTO.CanceledOrders_Total = List.Where(s => s.StatusId == (int)EnumStatus.Cancelled)
            //        .Sum(s => s.FeesDetailsDTO.Total);

            //    double CanceledOrders_LastManth = List.Where(s =>
            //            s.StatusId == (int)EnumStatus.Cancelled
            //            && new DateTime(DateTime.Now.Year, DateTime.Now.AddMonths(-1).Month, 1)
            //                >= s.CreatedAt
            //            && s.CreatedAt < new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1)
            //        )
            //        .Count();

            //    double CanceledOrders_PresentManth = List.Where(s =>
            //            s.StatusId == (int)EnumStatus.Cancelled
            //            && s.CreatedAt.Date
            //                >= new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1)
            //        )
            //        .Count();

            //    DTO.CanceledOrders_Percentchanged = BaseHelper.PercentageCalculation(
            //        CanceledOrders_PresentManth,
            //        CanceledOrders_LastManth
            //    );

            //    DTO.CanceledOrders_Arow =
            //        CanceledOrders_LastManth > CanceledOrders_PresentManth
            //            ? SystemConstants.ArrowType.Down
            //            : SystemConstants.ArrowType.Up;

            //    DTO.CanceledOrders_Count = List.Where(p => p.StatusId == (int)EnumStatus.Cancelled)
            //        .Count();
            //    #endregion
            //}
        }

        private static void GetTotalEarning(ref DashboardDTO DTO, List<AdsDTO> List)
        {
            //if (List != null)
            //{
            //    DTO.TotalEarning += (double)
            //        List.Where(s => s.StatusId == (int)EnumStatus.Cancelled)
            //            //&& s.FeesDetailsDTO.ShippingFeesPaid == true && s.FeesDetailsDTO.IsAfford && s.FeesDetailsDTO.IsAfford)
            //            .Sum(t => t.FeesDetailsDTO.ShippingFeesPaid);

            //    DTO.TotalEarning += List.Where(s => s.StatusId == (int)EnumStatus.Cancelled)
            //        //&& s.FeesDetailsDTO.ShippingFeesPaid.HasValue == true && s.FeesDetailsDTO.IsAfford.HasValue
            //        //&& !s.FeesDetailsDTO.IsAfford.Value)
            //        .Sum(t => t.FeesDetailsDTO.ShippingFees);

            //    DTO.TotalEarning += List.Where(s => s.StatusId == (int)EnumStatus.Cancelled)
            //        .Sum(t => t.FeesDetailsDTO.ShippingFees);

            //    double TotalEarning_LastManth =
            //        List.Where(s =>
            //                s.StatusId == (int)EnumStatus.Delivered
            //                && s.CreatedAt.Date
            //                    >= new DateTime(
            //                        SystemConstants.Datetime.Year,
            //                        SystemConstants.Datetime.lastMonth,
            //                        1
            //                    )
            //                && s.CreatedAt <= new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1)
            //            )
            //            .Sum(s => s.FeesDetailsDTO.Total)
            //        - List.Where(s =>
            //                s.StatusId == (int)EnumStatus.Delivered
            //                && s.CreatedAt.Date
            //                    >= new DateTime(
            //                        SystemConstants.Datetime.Year,
            //                        SystemConstants.Datetime.lastMonth,
            //                        1
            //                    )
            //                && s.CreatedAt <= new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1)
            //            )
            //            .Sum(s => s.FeesDetailsDTO.VendorCash);

            //    double TotalEarning_PresentManth =
            //        List.Where(s =>
            //                s.StatusId == (int)EnumStatus.Delivered
            //                && s.CreatedAt.Date
            //                    >= new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1)
            //                && s.CreatedAt <= DateTime.Now
            //            )
            //            .Sum(s => s.FeesDetailsDTO.Total)
            //        - List.Where(s =>
            //                s.StatusId == (int)EnumStatus.Delivered
            //                && s.CreatedAt.Date
            //                    >= new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1)
            //                && s.CreatedAt <= DateTime.Now
            //            )
            //            .Sum(s => s.FeesDetailsDTO.VendorCash);

            //    DTO.TotalEarning_Percentchanged = BaseHelper.PercentageCalculation(
            //        TotalEarning_PresentManth,
            //        TotalEarning_LastManth
            //    );

            //    DTO.TotalEarning_Arow = SystemConstants.ArrowType.Up;
            //    DTO.TotalEarning_Arow =
            //        TotalEarning_LastManth > TotalEarning_PresentManth
            //            ? SystemConstants.ArrowType.Down
            //            : SystemConstants.ArrowType.Up;
            //}
        }

        //private static void ChartTotalEarning(ref DashboardDTO DTO, List<InvoiceDTO> List)
        //{
        //    double packaging = 0;//InvoiceResponse.InvoiceRecords.Where(p => p.InvoiceTypeId == 1).Sum(p => p.GameFees);

        //    DTO.Packaging = DTO.TotalEarning > 0 ? (packaging / DTO.TotalEarning) * 100 : 0;

        //    double Shipping = 0;// InvoiceResponse.InvoiceRecords.Where(p => p.InvoiceTypeId == 1).Sum(p => p.ShippingFees);

        //    Shipping = DTO.TotalEarning - packaging;

        //    DTO.Shipping = DTO.TotalEarning > 0 ? (Shipping / DTO.TotalEarning) * 100 : 0;

        //    DTO.Storage = 0;

        //    DTO.Revenues = DTO.TotalEarning;
        //    DTO.Expenses = 0;
        //}
        public static void ChartTotalEarningVendor(ref DashboardDTO DTO)
        {
            if (DTO.CanceledOrders_Count == 0)
            {
                double ShippingFees = DTO.DeliveredOrders_Total - DTO.DeliveredOrdersSubtotal;
                DTO.Income =
                    DTO.AllOrders_Sum > 0
                        ? (DTO.DeliveredOrdersSubtotal / DTO.DeliveredOrders_Total) * 100
                        : 0;
                DTO.ShippingFees =
                    DTO.AllOrders_Sum > 0 ? (ShippingFees / DTO.DeliveredOrders_Total) * 100 : 0;
                DTO.CancellationFees = 0;
            }
            else
            {
                var Total =
                    DTO.AllOrders_Total > 0 ? DTO.AllOrders_Total - DTO.CanceledOrders_Total : 0;
                DTO.Income = DTO.AllOrders_Sum > 0 ? (Total / DTO.AllOrders_Sum) * 100 : 0;
                DTO.ShippingFees = Total > 0 ? (DTO.DeliveredOrders_Total / Total) * 100 : 0;
                DTO.CancellationFees = Total > 0 ? (DTO.CanceledOrders_Tax / Total) * 100 : 0;
            }
        }

        //private static void AvailableBalance(ref DashboardDTO DTO, List<InvoiceDTO> List)
        //{
        //    DTO.Available_Balance = (List.Where(p => p.InvoiceTypeId == 1)
        //    .Sum(p => p.Total) - List.
        //     Where(p => p.InvoiceTypeId == 1 && p.StatusId == 0)
        //    .Sum(p => p.Total) - List
        //    .Where(p => p.InvoiceTypeId == 2 && p.StatusId == 0).
        //     Sum(p => p.Total));
        //}

        //private static void RevenuesAndExpenses(ref DashboardDTO DTO, List<InvoiceDTO> List)
        //{
        //}
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using DicomApp.CommonDefinitions.DTO;
using DicomApp.CommonDefinitions.DTO.AdvertisementDTOs;
using DicomApp.CommonDefinitions.DTO.CashDTOs;
using DicomApp.CommonDefinitions.Requests;
using DicomApp.CommonDefinitions.Responses;
using DicomApp.DAL.DB;
using DicomApp.Helpers;

namespace DicomApp.BL.Services.Helpers
{
    public class DashboardHelper
    {
        public static DashboardDTO GenerateDashboard(DashboardRequest request)
        {
            var advertisementRequest = new AdvertisementRequest
            {
                RoleID = request.RoleID,
                UserID = request.UserID,
                context = request.context,
                AdsDTO = new AdsDTO
                {
                    DateTo = request.DashboardDTO.DateTo,
                    DateFrom = request.DashboardDTO.DateFrom
                },
                IsDesc = true,
                applyFilter = true,
            };

            var advertisementResponse = BL.Services.AdvertisementService.GetAllAdvertisements(
                advertisementRequest
            );

            var transactionRequest = new TransactionRequest
            {
                context = request.context,
                RoleID = request.RoleID,
                UserID = request.UserID,
                applyFilter = true,
                TransactionDTO = new CommonDefinitions.DTO.CashDTOs.TransactionDTO
                {
                    DateTo = request.DashboardDTO.DateTo,
                    DateFrom = request.DashboardDTO.DateFrom
                }
            };
            var transactionResponse = TransactionService.GetTransactions(transactionRequest);

            var invoiceRequest = new InvoiceRequest
            {
                context = request.context,
                RoleID = request.RoleID,
                UserID = request.UserID,
                applyFilter = true,
                InvoiceDTO = new CommonDefinitions.DTO.InvoiceDTO
                {
                    DateTo = request.DashboardDTO.DateTo,
                    DateFrom = request.DashboardDTO.DateFrom
                }
            };
            var invoiceResponse = InvoiceService.GetAll(invoiceRequest);

            var testimonialRequest = new TestimonialRequest
            {
                context = request.context,
                RoleID = request.RoleID,
                UserID = request.UserID,
            };
            var testimonialResponse = TestimonialService.GetTestimonials(testimonialRequest);

            var userRequest = new UserRequest
            {
                context = request.context,
                RoleID = request.RoleID,
                UserID = request.UserID,
                applyFilter = true,
                UserDTO = new UserDTO { RoleName = SystemConstants.Role.Gamer }
            };
            var userResponse = UserService.GetAllUsers(userRequest);
            var DashDTO = new DashboardDTO();

            AdvertisementBoxes(ref DashDTO, advertisementResponse.AdsDTOs);

            TransactionSource(ref DashDTO, transactionResponse.TransactionDTOs);

            GetTotalEarning(ref DashDTO, invoiceResponse.InvoiceDTOs);

            RatingAverage(testimonialResponse, DashDTO);
            #region Charts
            DashDTO.Chart_Genders = ChartGenders(DashDTO, userResponse.UserDTOs);
            DashDTO.Chart_Year = GetChartYear(advertisementResponse.AdsDTOs);

            DashDTO.Chart_TopBuyer = ChartTopBuyer(advertisementResponse.AdsDTOs, request.TopBuyer);

            DashDTO.Chart_TopGames = ChartTopGames(advertisementResponse.AdsDTOs, request.TopGames);

            DashDTO.Chart_PackagingStock = ChartPackagingStock(
                request.context,
                request.PackagingStock
            );
            #endregion
            return DashDTO;
        }

        private static void RatingAverage(
            TestimonialResponse testimonialResponse,
            DashboardDTO DashDTO
        )
        {
            var total = testimonialResponse?.TestimonialDTOs?.Sum(x => x.Rate) ?? 0;
            var count = testimonialResponse?.TestimonialDTOs?.Count() ?? 0;
            var averageRating = count > 0 ? (double)total / count : 0;
            averageRating = Math.Round(averageRating);
            DashDTO.RatingAverage = averageRating;
        }

        public static ChartDTO[] ChartGenders(DashboardDTO DashDTO, List<UserDTO> dataList)
        {
            ChartDTO[] returnData = null;
            if (dataList != null)
            {
                returnData = dataList
                    .GroupBy(p => p.Gender)
                    .Select(p => new ChartDTO { Key = p.Key.ToString(), Value1 = p.Count() })
                    .ToArray();
            }
            return returnData ?? new ChartDTO[0];
        }

        public static DashboardDTO GenerateAccountDashboard(DashboardRequest request, int VendorID)
        {
            var advertisementRequest = new AdvertisementRequest
            {
                RoleID = request.RoleID,
                UserID = request.UserID,
                context = request.context,
                AdsDTO = new AdsDTO
                {
                    DateTo = request.DashboardDTO.DateTo,
                    DateFrom = request.DashboardDTO.DateFrom
                },
                IsDesc = true,
                applyFilter = true,
            };
            var advertisementResponse = BL.Services.AdvertisementService.GetAllAdvertisements(
                advertisementRequest
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

            AdvertisementBoxes(ref DashDTO, advertisementResponse.AdsDTOs);

            //  GetTotalEarning(ref DashDTO, inv);

            ChartTotalEarningVendor(ref DashDTO);

            //AvailableBalance(ref DashDTO, InvoiceResponse.InvoiceRecords);
            #region Charts
            DashDTO.Chart_Year = GetChartYear(advertisementResponse.AdsDTOs);
            DashDTO.Chart_TopGames = ChartTopGames(advertisementResponse.AdsDTOs, request.TopGames);
            #endregion
            return DashDTO;
        }

        private static ChartDTO[] GetChartYear(List<AdsDTO> DataList)
        {
            ChartDTO[] ReturnValue = null;
            if (DataList != null)
            {
                ReturnValue = DataList
                    .Where(p =>
                        (p.StatusType == (int)StatusTypeEnum.Sold)
                        && p.CreatedAt.Year == DateTime.Now.Year
                    )
                    .GroupBy(p => p.CreatedAt.Year)
                    .Select(p => new ChartDTO
                    {
                        Key = p.Key.ToString(),
                        Value1 = p.Where(c =>
                                c.CreatedAt.Month == 1 && c.StatusType == (int)StatusTypeEnum.Sold
                            )
                            .Sum(t => t.Price),

                        Value2 = p.Where(c =>
                                c.CreatedAt.Month == 2 && c.StatusType == (int)StatusTypeEnum.Sold
                            )
                            .Sum(t => t.Price),
                        Value3 = p.Where(c =>
                                c.CreatedAt.Month == 3 && c.StatusType == (int)StatusTypeEnum.Sold
                            )
                            .Sum(t => t.Price),
                        Value4 = p.Where(c =>
                                c.CreatedAt.Month == 4 && c.StatusType == (int)StatusTypeEnum.Sold
                            )
                            .Sum(t => t.Price),
                        Value5 = p.Where(c =>
                                c.CreatedAt.Month == 5 && c.StatusType == (int)StatusTypeEnum.Sold
                            )
                            .Sum(t => t.Price),
                        Value6 = p.Where(c =>
                                c.CreatedAt.Month == 6 && c.StatusType == (int)StatusTypeEnum.Sold
                            )
                            .Sum(t => t.Price),
                        Value7 = p.Where(c =>
                                c.CreatedAt.Month == 7 && c.StatusType == (int)StatusTypeEnum.Sold
                            )
                            .Sum(t => t.Price),
                        Value8 = p.Where(c =>
                                c.CreatedAt.Month == 8 && c.StatusType == (int)StatusTypeEnum.Sold
                            )
                            .Sum(t => t.Price),
                        Value9 = p.Where(c =>
                                c.CreatedAt.Month == 9 && c.StatusType == (int)StatusTypeEnum.Sold
                            )
                            .Sum(t => t.Price),
                        Value10 = p.Where(c =>
                                c.CreatedAt.Month == 10 && c.StatusType == (int)StatusTypeEnum.Sold
                            )
                            .Sum(t => t.Price),
                        Value11 = p.Where(c =>
                                c.CreatedAt.Month == 11 && c.StatusType == (int)StatusTypeEnum.Sold
                            )
                            .Sum(t => t.Price),
                        Value12 = p.Where(c =>
                                c.CreatedAt.Month == 12 && c.StatusType == (int)StatusTypeEnum.Sold
                            )
                            .Sum(t => t.Price),
                    })
                    .ToArray();
            }
            return ReturnValue ?? new ChartDTO[0];
        }

        private static ChartDTO[] ChartTopBuyer(List<AdsDTO> DataList, int TopCount)
        {
            ChartDTO[] ReturnData = null;
            if (DataList != null)
            {
                ReturnData = DataList
                    .Where(p => p.BuyerId.HasValue)
                    .GroupBy(p => p.Buyer.Name ?? "Other")
                    .Select(p => new ChartDTO
                    {
                        Key = p.Key,
                        Value1 = p.Count(c => c.StatusType == (int)StatusTypeEnum.Sold)
                    })
                    .OrderByDescending(p => p.Value1)
                    .Take(TopCount)
                    .ToArray();
            }
            return ReturnData ?? new ChartDTO[0];
        }

        private static ChartDTO[] ChartTopGames(List<AdsDTO> DataList, int TopCount)
        {
            ChartDTO[] ReturnData = null;
            if (DataList != null)
            {
                ReturnData = DataList
                    .Where(p => p.GameId > 0)
                    .GroupBy(p => p.Game.NameAr ?? "Other")
                    .Select(p => new ChartDTO
                    {
                        Key = p.Key,
                        Value1 = p.Count(c => c.StatusType == (int)StatusTypeEnum.Sold)
                    })
                    .OrderByDescending(p => p.Value1)
                    .Take(TopCount)
                    .ToArray();
            }
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

        private static void AdvertisementBoxes(ref DashboardDTO DTO, List<AdsDTO> List)
        {
            if (List != null)
            {
                DTO.AllAdvertisements_Count = List.Count;
                DTO.AllAdvertisements_Total = List.Sum(s => s.Price);
                var pendingAdvertisements = List.Where(p =>
                        p.StatusType != (int)StatusTypeEnum.Sold
                        && p.StatusType != (int)StatusTypeEnum.Reject
                    )
                    .ToList();

                DTO.PendingAdvertisements_Count = pendingAdvertisements.Count;
                DTO.PendingAdvertisements_Total = pendingAdvertisements.Sum(s => s.Price);

                var soldAdvertisements = List.Where(p => p.StatusType == (int)StatusTypeEnum.Sold)
                    .ToList();

                DTO.SoldAdvertisements_Count = soldAdvertisements.Count;
                DTO.SoldAdvertisements_Total = soldAdvertisements.Sum(s => s.Price);

                var rejectAdvertisements = List.Where(s => s.StatusId == (int)StatusTypeEnum.Reject)
                    .ToList();

                DTO.RejectAdvertisements_Count = rejectAdvertisements.Count;
                DTO.RejectAdvertisements_Total = rejectAdvertisements.Sum(s => s.Price);
            }
        }

        private static void TransactionSource(ref DashboardDTO DTO, List<TransactionDTO> list)
        {
            if (list != null && list.Any())
            {
                DTO.TransactionSource_Success = list.Count(x => x.IsSuccess);
                DTO.TransactionSource_UnSuccess = list.Count(x => !x.IsSuccess);

                double total = list.Count(x =>
                    x.IsSuccess && x.TransactionType != TransactionTypeEnum.None
                );
                if (total != 0)
                {
                    DTO.Account = Math.Round(
                        list.Count(x =>
                            x.IsSuccess && x.TransactionType == TransactionTypeEnum.Account
                        )
                            / total
                            * 100
                    );
                    DTO.Push = Math.Round(
                        list.Count(x =>
                            x.IsSuccess && x.TransactionType == TransactionTypeEnum.Push
                        )
                            / total
                            * 100
                    );
                    DTO.Change = Math.Round(
                        list.Count(x =>
                            x.IsSuccess && x.TransactionType == TransactionTypeEnum.Change
                        )
                            / total
                            * 100
                    );
                }
            }
        }

        private static void GetTotalEarning(ref DashboardDTO DTO, List<InvoiceDTO> list)
        {
            if (list != null)
            {
                DTO.Available_Balance = list.Where(x => !x.IsRefund && !x.IsDeleted)
                    .Sum(x => x.Price);
                DTO.TotalEarning = DTO.Available_Balance;
            }
        }

        public static void ChartTotalEarningVendor(ref DashboardDTO DTO)
        {
            if (DTO.RejectAdvertisements_Count == 0)
            {
                double ShippingFees = DTO.SoldAdvertisements_Total - DTO.SoldAdvertisementsSubtotal;
                DTO.Income =
                    DTO.AllAdvertisements_Sum > 0
                        ? (DTO.SoldAdvertisementsSubtotal / DTO.SoldAdvertisements_Total) * 100
                        : 0;
                DTO.ShippingFees =
                    DTO.AllAdvertisements_Sum > 0
                        ? (ShippingFees / DTO.SoldAdvertisements_Total) * 100
                        : 0;
                DTO.CancellationFees = 0;
            }
            else
            {
                var Total =
                    DTO.AllAdvertisements_Total > 0
                        ? DTO.AllAdvertisements_Total - DTO.RejectAdvertisements_Total
                        : 0;
                DTO.Income =
                    DTO.AllAdvertisements_Sum > 0 ? (Total / DTO.AllAdvertisements_Sum) * 100 : 0;
                DTO.ShippingFees = Total > 0 ? (DTO.SoldAdvertisements_Total / Total) * 100 : 0;
                DTO.CancellationFees = Total > 0 ? (DTO.RejectAdvertisements_Tax / Total) * 100 : 0;
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

using System;
using System.Linq;
using System.Net;
using DicomApp.CommonDefinitions.DTO;
using DicomApp.CommonDefinitions.DTO.CashDTOs;
using DicomApp.CommonDefinitions.Requests;
using DicomApp.CommonDefinitions.Responses;
using DicomApp.DAL.DB;
using DicomApp.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace DicomApp.BL.Services
{
    public class AccountService : BaseService
    {
        public static AccountResponse AddAccount(AccountRequest request)
        {
            var response = new AccountResponse();
            RunBase(
                request,
                response,
                (AccountRequest req) =>
                {
                    try
                    {
                        var account = new Account
                        {
                            Name = request.AccountDTO.Name,
                            RefId = request.AccountDTO.RefId,
                            Balance = 0,
                            UserId = request.AccountDTO.UserId,
                            RoleId = request.AccountDTO.RoleId,
                            Ban = request.AccountDTO.BAN,
                            Iban = request.AccountDTO.IBAN,
                            BankName = request.AccountDTO.BankName,
                            VodafoneCash = request.AccountDTO.VodafoneCash,
                            InstaPay = request.AccountDTO.InstaPay,

                            CreatedAt = DateTime.Now,
                            LastModifiedAt = DateTime.Now,
                            CreatedBy = request.UserID,
                            LastModifiedBy = request.UserID,
                            IsDeleted = false,
                        };

                        request.context.Account.Add(account);

                        request.context.SaveChanges();

                        response.Message = "New account " + account.RefId + " successfully added";
                        response.Success = true;
                        response.StatusCode = HttpStatusCode.OK;
                    }
                    catch (Exception ex)
                    {
                        response.Message = ex.Message;
                        response.Success = false;
                        LogHelper.LogException(ex.Message, ex.StackTrace);
                    }
                    return response;
                }
            );
            return response;
        }

        public static AccountResponse EditAccount(AccountRequest request)
        {
            var response = new AccountResponse();
            RunBase(
                request,
                response,
                (AccountRequest req) =>
                {
                    try
                    {
                        var account = request.context.Account.FirstOrDefault(s =>
                            s.AccountId == request.AccountDTO.AccountId
                        );

                        account.Name = request.AccountDTO.Name;
                        account.RefId = request.AccountDTO.RefId;
                        account.UserId = request.AccountDTO.UserId;
                        account.RoleId = request.AccountDTO.RoleId;
                        account.Ban = request.AccountDTO.BAN;
                        account.Iban = request.AccountDTO.IBAN;
                        account.BankName = request.AccountDTO.BankName;
                        account.VodafoneCash = request.AccountDTO.VodafoneCash;
                        account.InstaPay = request.AccountDTO.InstaPay;

                        account.LastModifiedAt = DateTime.Now;
                        account.LastModifiedBy = request.UserID;

                        request.context.SaveChanges();

                        response.Message = "Account " + account.RefId + " successfully updated";
                        response.Success = true;
                        response.StatusCode = HttpStatusCode.OK;
                    }
                    catch (Exception ex)
                    {
                        response.Message = ex.Message;
                        response.Success = false;
                        LogHelper.LogException(ex.Message, ex.StackTrace);
                    }
                    return response;
                }
            );
            return response;
        }

        public static AccountResponse GetAccount(AccountRequest request)
        {
            var response = new AccountResponse();
            RunBase(
                request,
                response,
                (AccountRequest req) =>
                {
                    try
                    {
                        var query = request
                            .context.Account.Where(s =>
                                (
                                    s.AccountId == request.AccountDTO.AccountId
                                    || s.UserId == request.AccountDTO.UserId
                                ) && !s.IsDeleted
                            )
                            .Select(s => new AccountDTO
                            {
                                AccountId = s.AccountId,
                                Name = s.Name,
                                RefId = s.RefId,
                                Balance = s.Balance,
                                UserId = s.UserId,
                                RoleId = s.RoleId,
                                BAN = s.Ban,
                                IBAN = s.Iban,
                                BankName = s.BankName,
                                VodafoneCash = s.VodafoneCash,
                                InstaPay = s.InstaPay,
                                CreatedAt = s.CreatedAt,
                                LastModifiedAt = s.LastModifiedAt,
                                CreatedBy = s.CreatedBy,
                                LastModifiedBy = s.LastModifiedBy,
                                VendorName = s.User.Name,

                                TransactionDTOs = request.HasTransactionsDTOs
                                    ? s.AccountTransactionReceiver.Select(
                                        t => new AccountTransactionDTO
                                        {
                                            AccountTransactionId = t.AccountTransactionId,
                                            RefId = t.RefId,
                                            TypeId = t.TypeId,
                                            SenderId = t.SenderId,
                                            ReceiverId = t.ReceiverId,
                                            VendorId = t.VendorId,
                                            StatusId = t.StatusId,
                                            StatusName = t.StatusId.HasValue
                                                ? t.Status.NameEN
                                                : null,
                                            AdvertisementId = t.AdvertisementId,
                                            PickupRequestId = t.PickupRequestId,
                                            GameFees = t.GameFees,
                                            WeightFees = t.WeightFees,
                                            SizeFees = t.SizeFees,
                                            PartialDeliveryFees = t.PartialDeliveryFees,
                                            CancelFees = t.CancelFees,
                                            PickupFees = t.PickupFees,
                                            ShippingFees = t.ShippingFees,
                                            ShippingFeesPaid = t.ShippingFeesPaid,
                                            VendorCash = t.VendorCash,
                                            Total = t.Total,
                                            CreatedAt = t.CreatedAt,
                                            CreatedBy = t.CreatedBy
                                        }
                                    )
                                    : null,
                            });

                        response.AccountDTO = query.FirstOrDefault();

                        response.Message = HttpStatusCode.OK.ToString();
                        response.Success = true;
                        response.StatusCode = HttpStatusCode.OK;
                    }
                    catch (Exception ex)
                    {
                        response.Message = ex.Message;
                        response.Success = false;
                        LogHelper.LogException(ex.Message, ex.StackTrace);
                    }
                    return response;
                }
            );
            return response;
        }

        public static AccountResponse GetAllAccounts(AccountRequest request)
        {
            var response = new AccountResponse();
            RunBase(
                request,
                response,
                (AccountRequest req) =>
                {
                    try
                    {
                        var query = request
                            .context.Account.Where(s => !s.IsDeleted)
                            .Select(s => new AccountDTO
                            {
                                AccountId = s.AccountId,
                                Name = s.Name,
                                RefId = s.RefId,
                                Balance = s.Balance,
                                UserId = s.UserId,
                                RoleId = s.RoleId,
                                BAN = s.Ban,
                                IBAN = s.Iban,
                                BankName = s.BankName,
                                VodafoneCash = s.VodafoneCash,
                                InstaPay = s.InstaPay,
                                CreatedAt = s.CreatedAt,
                                LastModifiedAt = s.LastModifiedAt,
                                CreatedBy = s.CreatedBy,
                                LastModifiedBy = s.LastModifiedBy,
                                VendorName = s.User.Name,

                                TransactionDTOs = request.HasTransactionsDTOs
                                    ? s.AccountTransactionVendor.Select(
                                        t => new AccountTransactionDTO
                                        {
                                            AccountTransactionId = t.AccountTransactionId,
                                            RefId = t.RefId,
                                            TypeId = t.TypeId,
                                            SenderId = t.SenderId,
                                            ReceiverId = t.ReceiverId,
                                            VendorId = t.VendorId,
                                            StatusId = t.StatusId,
                                            StatusName = t.StatusId.HasValue
                                                ? t.Status.NameEN
                                                : null,
                                            AdvertisementId = t.AdvertisementId,
                                            PickupRequestId = t.PickupRequestId,
                                            GameFees = t.GameFees,
                                            WeightFees = t.WeightFees,
                                            SizeFees = t.SizeFees,
                                            PartialDeliveryFees = t.PartialDeliveryFees,
                                            CancelFees = t.CancelFees,
                                            PickupFees = t.PickupFees,
                                            ShippingFees = t.ShippingFees,
                                            ShippingFeesPaid = t.ShippingFeesPaid,
                                            VendorCash = t.VendorCash,
                                            Total = t.Total,
                                            CreatedAt = t.CreatedAt,
                                            CreatedBy = t.CreatedBy,
                                        }
                                    )
                                    : null,
                            });

                        if (request.applyFilter)
                            query = ApplyFilter(
                                query,
                                request.AccountDTO,
                                request.RoleID,
                                request.UserID
                            );

                        response.AccountDTOs = query.ToList();

                        response.Message = HttpStatusCode.OK.ToString();
                        response.Success = true;
                        response.StatusCode = HttpStatusCode.OK;
                    }
                    catch (Exception ex)
                    {
                        response.Message = ex.Message;
                        response.Success = false;
                        LogHelper.LogException(ex.Message, ex.StackTrace);
                    }
                    return response;
                }
            );
            return response;
        }

        public static AccountResponse GetForTransferCash(AccountRequest request)
        {
            var response = new AccountResponse();
            RunBase(
                request,
                response,
                (AccountRequest req) =>
                {
                    try
                    {
                        var hasDelvieredShipsIDs = request
                            .context.FollowUp.Where(f => f.StatusId == (int)EnumStatus.Delivered)
                            .Select(f => f.AdvertisementId.Value)
                            .ToList();

                        var nonPaidShips = request
                            .context.Advertisement.Where(s =>
                                s.VendorId == request.AccountDTO.UserId
                                && hasDelvieredShipsIDs.Contains(s.AdvertisementId)
                            )
                            .Select(t => t.AdvertisementId)
                            .ToList();

                        var query = request
                            .context.Account.Where(s =>
                                !s.IsDeleted
                                && s.RoleId == (int)EnumRole.Gamer
                                && s.UserId == request.AccountDTO.UserId
                            )
                            .Select(s => new TreasuryDTO
                            {
                                AccountId = s.AccountId,
                                Balance = s.Balance,
                                VendorName = s.User.Name,

                                Total = s
                                    .AccountTransactionVendor.Where(t =>
                                        t.ReceiverId == s.AccountId
                                        && nonPaidShips.Contains(t.AdvertisementId.Value)
                                    )
                                    .Sum(t => t.Total),

                                TransactionDTOs = s
                                    .AccountTransactionVendor.Where(t =>
                                        t.ReceiverId == s.AccountId
                                        && nonPaidShips.Contains(t.AdvertisementId.Value)
                                    )
                                    .Select(t => new AccountTransactionDTO
                                    {
                                        AccountTransactionId = t.AccountTransactionId,
                                        TypeId = t.TypeId,
                                        SenderId = t.SenderId,
                                        ReceiverId = t.ReceiverId,
                                        VendorId = t.VendorId,
                                        AdvertisementId = t.AdvertisementId,
                                        PickupRequestId = t.PickupRequestId,
                                        GameFees = t.GameFees,
                                        WeightFees = t.WeightFees,
                                        SizeFees = t.SizeFees,
                                        PartialDeliveryFees = t.PartialDeliveryFees,
                                        CancelFees = t.CancelFees,
                                        PickupFees = t.PickupFees,
                                        ShippingFees = t.ShippingFees,
                                        ShippingFeesPaid = t.ShippingFeesPaid,
                                        VendorCash = t.VendorCash,
                                        Total = t.Total,
                                        RefundCash = t.RefundCash,
                                        RefundFees = t.RefundFees,

                                        StatusId = t.StatusId,
                                        StatusName = t.StatusId.HasValue ? t.Status.NameEN : null,
                                        StatusDTO = new StatusDTO
                                        {
                                            Id = t.StatusId.Value,
                                            NameEN = t.Status.NameEN,
                                        },
                                        CreatedAt = t.CreatedAt,
                                    }),
                            });

                        response.TreasuryDTO = query.FirstOrDefault();

                        response.Message = HttpStatusCode.OK.ToString();
                        response.Success = true;
                        response.StatusCode = HttpStatusCode.OK;
                    }
                    catch (Exception ex)
                    {
                        response.Message = ex.Message;
                        response.Success = false;
                        LogHelper.LogException(ex.Message, ex.StackTrace);
                    }
                    return response;
                }
            );
            return response;
        }

        public static AccountResponse GetForTreasury(AccountRequest request)
        {
            var response = new AccountResponse();
            RunBase(
                request,
                response,
                (AccountRequest req) =>
                {
                    try
                    {
                        var query = request
                            .context.Account.Where(s =>
                                !s.IsDeleted && s.RoleId == (int)EnumRole.Gamer
                            )
                            .Select(s => new TreasuryDTO
                            {
                                AccountId = s.AccountId,
                                Balance = s.Balance,
                                VendorName = s.User.Name,

                                TotalDelivered = s
                                    .AccountTransactionVendor.Where(t =>
                                        (t.ReceiverId != (int)EnumAccountId.Main_Treasury)
                                        && t.StatusId == (int)EnumStatus.Delivered
                                        && t.TypeId == (int)EnumTransactionType.Deposit
                                    )
                                    .Sum(t => t.Total),

                                TotalRefunded = s
                                    .AccountTransactionVendor.Where(t =>
                                        (t.ReceiverId == s.AccountId)
                                        && t.StatusId == (int)EnumStatus.Refunded
                                        && t.TypeId == (int)EnumTransactionType.Withdraw
                                    )
                                    .Sum(t => t.Total),

                                TotalOthers = s
                                    .AccountTransactionVendor.Where(t =>
                                        t.ReceiverId == (int)EnumAccountId.RED_Main_Account
                                        && (
                                            t.StatusId != (int)EnumStatus.Delivered
                                            && t.StatusId != (int)EnumStatus.Refunded
                                        )
                                        && t.TypeId == (int)EnumTransactionType.Deposit
                                    )
                                    .Sum(t => t.Total),

                                ShippingFees = s
                                    .AccountTransactionVendor.Where(t =>
                                        t.ReceiverId == (int)EnumAccountId.RED_Main_Account
                                        && t.TypeId == (int)EnumTransactionType.Deposit
                                    )
                                    .Sum(t => t.ShippingFees),

                                GameFees = s
                                    .AccountTransactionVendor.Where(t =>
                                        t.ReceiverId == (int)EnumAccountId.RED_Main_Account
                                        && t.TypeId == (int)EnumTransactionType.Deposit
                                    )
                                    .Sum(t => t.GameFees),

                                WeightFees = s
                                    .AccountTransactionVendor.Where(t =>
                                        t.ReceiverId == (int)EnumAccountId.RED_Main_Account
                                        && t.TypeId == (int)EnumTransactionType.Deposit
                                    )
                                    .Sum(t => t.WeightFees),

                                SizeFees = s
                                    .AccountTransactionVendor.Where(t =>
                                        t.ReceiverId == (int)EnumAccountId.RED_Main_Account
                                        && t.TypeId == (int)EnumTransactionType.Deposit
                                    )
                                    .Sum(t => t.SizeFees),

                                PartialDeliveryFees = s
                                    .AccountTransactionVendor.Where(t =>
                                        t.ReceiverId == (int)EnumAccountId.RED_Main_Account
                                        && t.TypeId == (int)EnumTransactionType.Deposit
                                    )
                                    .Sum(t => t.PartialDeliveryFees),

                                PickupFees = s
                                    .AccountTransactionVendor.Where(t =>
                                        t.ReceiverId == (int)EnumAccountId.RED_Main_Account
                                        && t.TypeId == (int)EnumTransactionType.Deposit
                                    )
                                    .Sum(t => t.PickupFees),

                                CancelFees = s
                                    .AccountTransactionVendor.Where(t =>
                                        t.ReceiverId == s.AccountId
                                        && t.TypeId == (int)EnumTransactionType.Withdraw
                                    )
                                    .Sum(t => t.CancelFees),

                                Withdraw = s
                                    .AccountTransactionVendor.Where(t =>
                                        t.ReceiverId == s.AccountId
                                        && t.TypeId == (int)EnumTransactionType.Withdraw
                                    )
                                    .Sum(t => t.Total),

                                TransactionDTOs = s
                                    .AccountTransactionVendor.Where(t =>
                                        (
                                            t.ReceiverId == s.AccountId
                                            || t.ReceiverId == (int)EnumAccountId.RED_Main_Account
                                        )
                                    )
                                    .Select(t => new AccountTransactionDTO
                                    {
                                        AccountTransactionId = t.AccountTransactionId,
                                        PickupRefId = t.PickupRequestId.HasValue
                                            ? t.PickupRequest.RefId
                                            : null,
                                        CashTransferRefId = t.CashTransferId.HasValue
                                            ? t.CashTransfer.RefId
                                            : null,
                                        CashTransferId = t.CashTransferId,
                                        TypeId = t.TypeId,
                                        SenderId = t.SenderId,
                                        ReceiverId = t.ReceiverId,
                                        VendorId = t.VendorId,
                                        AdvertisementId = t.AdvertisementId,
                                        PickupRequestId = t.PickupRequestId,
                                        GameFees = t.GameFees,
                                        WeightFees = t.WeightFees,
                                        SizeFees = t.SizeFees,
                                        PartialDeliveryFees = t.PartialDeliveryFees,
                                        CancelFees = t.CancelFees,
                                        PickupFees = t.PickupFees,
                                        ShippingFees = t.ShippingFees,
                                        ShippingFeesPaid = t.ShippingFeesPaid,
                                        VendorCash = t.VendorCash,
                                        Total = t.Total,
                                        RefundCash = t.RefundCash,
                                        RefundFees = t.RefundFees,

                                        StatusId = t.StatusId,
                                        StatusName = t.StatusId.HasValue ? t.Status.NameEN : null,
                                        StatusDTO = t.StatusId.HasValue
                                            ? new StatusDTO
                                            {
                                                Id = t.StatusId.Value,
                                                NameEN = t.Status.NameEN,
                                            }
                                            : null,
                                        CreatedAt = t.CreatedAt,
                                    })
                                    .OrderByDescending(t => t.AccountTransactionId),
                            });

                        response.TreasuryDTOs = query.ToList();

                        response.Message = HttpStatusCode.OK.ToString();
                        response.Success = true;
                        response.StatusCode = HttpStatusCode.OK;
                    }
                    catch (Exception ex)
                    {
                        response.Message = ex.Message;
                        response.Success = false;
                        LogHelper.LogException(ex.Message, ex.StackTrace);
                    }
                    return response;
                }
            );
            return response;
        }

        private static IQueryable<AccountDTO> ApplyFilter(
            IQueryable<AccountDTO> query,
            AccountDTO filter,
            int RoleId,
            int UserId
        )
        {
            if (!string.IsNullOrEmpty(filter.Search))
                query = query.Where(c =>
                    c.RefId.Contains(filter.Search) || c.VendorName.Contains(filter.Search)
                );

            if (filter.UserId > 0)
                query = query.Where(p => p.UserId == filter.UserId);

            if (filter.RoleId > 0)
                query = query.Where(p => p.RoleId == filter.RoleId);

            return query;
        }
    }
}

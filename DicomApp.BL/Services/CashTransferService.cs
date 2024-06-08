using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Security.Principal;
using DicomApp.CommonDefinitions.DTO;
using DicomApp.CommonDefinitions.DTO.AdvertisementDTOs;
using DicomApp.CommonDefinitions.DTO.CashDTOs;
using DicomApp.CommonDefinitions.Requests;
using DicomApp.CommonDefinitions.Responses;
using DicomApp.DAL.DB;
using DicomApp.Helpers;
using Microsoft.CodeAnalysis;

namespace DicomApp.BL.Services
{
    public class CashTransferService : BaseService
    {
        public static CashTransferResponse AddCashTransfer(CashTransferRequest request)
        {
            var response = new CashTransferResponse();
            RunBase(
                request,
                response,
                (CashTransferRequest req) =>
                {
                    try
                    {
                        var VendorAccount = request.context.Account.FirstOrDefault(a =>
                            a.UserId == request.CashTransferDTO.VendorId
                        );
                        var MainTreasuryAccount = request.context.Account.FirstOrDefault(a =>
                            a.AccountId == (int)EnumAccountId.Main_Treasury
                        );
                        var selectedTransIDs = request
                            .CashTransferDTO.TransIDs.Split(',')
                            .Select(int.Parse)
                            .ToList();
                        var Transactions = request
                            .context.AccountTransaction.Where(r =>
                                selectedTransIDs.Contains(r.AccountTransactionId)
                            )
                            .ToList();
                        var sumTotal = Transactions.Sum(a => a.Total);

                        #region Add Cash Transfer
                        var cashTransfer = new CashTransfer();
                        cashTransfer.TypeId = request.CashTransferDTO.TypeId;
                        cashTransfer.ReceiverId = VendorAccount.AccountId;
                        cashTransfer.Name = request.CashTransferDTO.Name;
                        cashTransfer.Phone = request.CashTransferDTO.Phone;
                        cashTransfer.Email = request.CashTransferDTO.Email;
                        cashTransfer.Address = request.CashTransferDTO.Address;
                        cashTransfer.Landmark = request.CashTransferDTO.Landmark;
                        cashTransfer.Floor = request.CashTransferDTO.Floor;
                        cashTransfer.Apartment = request.CashTransferDTO.Apartment;
                        cashTransfer.ZoneId = request.CashTransferDTO.ZoneId;
                        cashTransfer.AreaId = request.CashTransferDTO.AreaId;

                        cashTransfer.CreatedAt = DateTime.Now;
                        cashTransfer.LastModifiedAt = DateTime.Now;
                        cashTransfer.CreatedBy = request.UserID;
                        cashTransfer.LastModifiedBy = request.UserID;
                        cashTransfer.IsDeleted = false;
                        cashTransfer.IsReceived = true;

                        request.context.CashTransfer.Add(cashTransfer);
                        #endregion

                        request.context.SaveChanges();

                        cashTransfer.RefId = BaseHelper.GenerateRefId(
                            EnumRefIdType.Cash_Transfer,
                            cashTransfer.CashTransferId
                        );

                        #region Update Shipments
                        foreach (var trans in Transactions)
                        {
                            if (trans.AdvertisementId.HasValue)
                            {
                                var ship = request.context.Advertisement.FirstOrDefault(r =>
                                    r.AdvertisementId == trans.AdvertisementId
                                );
                                ship.StatusId = (int)EnumStatus.Paid_To_Vendor;
                                ship.LastModifiedAt = DateTime.Now;
                                ship.LastModifiedBy = request.UserID;

                                //Add follow up
                                var follow = new FollowUpDTO
                                {
                                    Title = request
                                        .context.Status.FirstOrDefault(s => s.Id == ship.StatusId)
                                        .NameEN,
                                    Comment = request
                                        .context.CommonUser.FirstOrDefault(u =>
                                            u.Id == request.UserID
                                        )
                                        .Name,
                                    CreatedBy = request.UserID,
                                    AdvertisementId = trans.AdvertisementId,
                                    StatusId = trans.StatusId
                                };
                                FollowUpService.Add(follow, request.context);
                            }
                        }
                        #endregion

                        #region Vendor Transaction
                        var VendorTrans = new AccountTransactionDTO();
                        VendorTrans.CreatedBy = request.UserID;
                        VendorTrans.SenderId = (int)EnumAccountId.Main_Treasury;
                        VendorTrans.ReceiverId = VendorAccount.AccountId;
                        VendorTrans.VendorId = VendorAccount.AccountId;
                        VendorTrans.CashTransferId = cashTransfer.CashTransferId;
                        VendorTrans.TypeId = (int)EnumTransactionType.Withdraw;
                        #endregion

                        #region Main Treasury Transaction
                        var TreasuryTrans = new AccountTransactionDTO();
                        TreasuryTrans.CreatedBy = request.UserID;
                        TreasuryTrans.SenderId = (int)EnumAccountId.Main_Treasury;
                        TreasuryTrans.ReceiverId = (int)EnumAccountId.Main_Treasury;
                        TreasuryTrans.VendorId = VendorAccount.AccountId;
                        TreasuryTrans.CashTransferId = cashTransfer.CashTransferId;
                        TreasuryTrans.TypeId = (int)EnumTransactionType.Withdraw;
                        #endregion

                        #region Update Vendor Balance
                        if (VendorAccount.Balance < sumTotal)
                        {
                            cashTransfer.Amount = VendorAccount.Balance;
                            //VendorAccount.Balance = 0;
                            //MainTreasuryAccount.Balance -= cashTransfer.Amount;

                            VendorTrans.Total = cashTransfer.Amount;
                            TreasuryTrans.Total = cashTransfer.Amount;
                        }
                        else
                        {
                            cashTransfer.Amount = sumTotal;
                            //VendorAccount.Balance -= sumTotal;
                            //MainTreasuryAccount.Balance -= sumTotal;

                            VendorTrans.Total = sumTotal;
                            TreasuryTrans.Total = sumTotal;
                        }

                        AccountTransactionService.AddTransaction(request.context, VendorTrans);
                        AccountTransactionService.AddTransaction(request.context, TreasuryTrans);
                        #endregion

                        request.context.SaveChanges();

                        response.Message =
                            "New Cash Transfer #" + cashTransfer.RefId + " successfully added";
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

        private static CashTransferResponse EditCashTransfer(CashTransferRequest request)
        {
            var response = new CashTransferResponse();
            RunBase(
                request,
                response,
                (CashTransferRequest req) =>
                {
                    try
                    {
                        var cashTransfer = request.context.CashTransfer.FirstOrDefault(s =>
                            s.CashTransferId == request.CashTransferDTO.CashTransferId
                        );

                        cashTransfer.TypeId = request.CashTransferDTO.TypeId;
                        cashTransfer.ReceiverId = request.CashTransferDTO.ReceiverId;
                        cashTransfer.Name = request.CashTransferDTO.Name;
                        cashTransfer.ZoneId = request.CashTransferDTO.ZoneId;
                        cashTransfer.AreaId = request.CashTransferDTO.AreaId;
                        cashTransfer.Phone = request.CashTransferDTO.Phone;
                        cashTransfer.Email = request.CashTransferDTO.Email;
                        cashTransfer.Address = request.CashTransferDTO.Address;
                        cashTransfer.Landmark = request.CashTransferDTO.Landmark;
                        cashTransfer.Floor = request.CashTransferDTO.Floor;
                        cashTransfer.Apartment = request.CashTransferDTO.Apartment;
                        cashTransfer.IsReceived = request.CashTransferDTO.IsReceived;
                        cashTransfer.Otp = request.CashTransferDTO.OTP;

                        cashTransfer.LastModifiedAt = DateTime.Now;
                        cashTransfer.LastModifiedBy = request.UserID;

                        request.context.SaveChanges();

                        response.Message = "Cash Transfer successfully updated";
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

        public static CashTransferResponse GetCashTransfer(CashTransferRequest request)
        {
            var response = new CashTransferResponse();
            RunBase(
                request,
                response,
                (CashTransferRequest req) =>
                {
                    try
                    {
                        var query = request
                            .context.CashTransfer.Where(s =>
                                s.CashTransferId == request.CashTransferDTO.CashTransferId
                                && !s.IsDeleted
                            )
                            .Select(s => new CashTransferDTO
                            {
                                TypeId = s.TypeId,
                                ReceiverId = s.ReceiverId,
                                Name = s.Name,
                                ZoneId = s.ZoneId,
                                AreaId = s.AreaId,
                                Phone = s.Phone,
                                Email = s.Email,
                                Address = s.Address,
                                Landmark = s.Landmark,
                                Floor = s.Floor,
                                Apartment = s.Apartment,
                                IsReceived = s.IsReceived,
                                OTP = s.Otp,
                            });

                        response.CashTransferDTO = query.FirstOrDefault();

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

        public static CashTransferResponse GetAllCashTransfers(CashTransferRequest request)
        {
            var response = new CashTransferResponse();
            RunBase(
                request,
                response,
                (CashTransferRequest req) =>
                {
                    try
                    {
                        var currVendorAccountId = 0;
                        if (request.RoleID == (int)EnumRole.Gamer)
                            currVendorAccountId = request
                                .context.Account.FirstOrDefault(a => a.UserId == request.UserID)
                                .AccountId;

                        var result = new InvoiceDTO();

                        result.VendorBalance = request
                            .context.Account.Where(s =>
                                !s.IsDeleted
                                && s.RoleId == (int)EnumRole.Gamer
                                && (
                                    currVendorAccountId > 0
                                        ? s.AccountId == currVendorAccountId
                                        : true
                                )
                            )
                            .Sum(t => t.Balance);

                        result.TreasuryBalance = request
                            .context.Account.FirstOrDefault(s =>
                                s.AccountId == (int)EnumAccountId.Main_Treasury
                            )
                            .Balance;

                        var TotalDelivered = request
                            .context.AccountTransaction.Where(t =>
                                t.ReceiverId != (int)EnumAccountId.Main_Treasury
                                && t.StatusId == (int)EnumStatus.Delivered
                                && t.TypeId == (int)EnumTransactionType.Deposit
                                && (
                                    currVendorAccountId > 0
                                        ? t.VendorId == currVendorAccountId
                                        : true
                                )
                            )
                            .Sum(t => t.Total);

                        var TotalRefunded = request
                            .context.AccountTransaction.Where(t =>
                                (
                                    t.ReceiverId != (int)EnumAccountId.Main_Treasury
                                    && t.ReceiverId != (int)EnumAccountId.RED_Main_Account
                                )
                                && t.StatusId == (int)EnumStatus.Refunded
                                && t.TypeId == (int)EnumTransactionType.Withdraw
                            )
                            .Sum(t => t.Total);

                        var TotalOthers = request
                            .context.AccountTransaction.Where(t =>
                                t.ReceiverId == (int)EnumAccountId.RED_Main_Account
                                && (
                                    t.StatusId != (int)EnumStatus.Delivered
                                    && t.StatusId != (int)EnumStatus.Refunded
                                )
                                && t.TypeId == (int)EnumTransactionType.Deposit
                                && (
                                    currVendorAccountId > 0
                                        ? t.VendorId == currVendorAccountId
                                        : true
                                )
                            )
                            .Sum(t => t.Total);

                        result.Total = TotalDelivered + TotalRefunded + TotalOthers;

                        result.REDFees = request
                            .context.AccountTransaction.Where(t =>
                                t.ReceiverId == (int)EnumAccountId.RED_Main_Account
                                && t.TypeId == (int)EnumTransactionType.Deposit
                                && (
                                    currVendorAccountId > 0
                                        ? t.VendorId == currVendorAccountId
                                        : true
                                )
                            )
                            .Sum(t => t.Total);

                        result.ShippingFees = request
                            .context.AccountTransaction.Where(t =>
                                t.ReceiverId == (int)EnumAccountId.RED_Main_Account
                                && t.TypeId == (int)EnumTransactionType.Deposit
                                && (
                                    currVendorAccountId > 0
                                        ? t.VendorId == currVendorAccountId
                                        : true
                                )
                            )
                            .Sum(t => t.ShippingFees);

                        result.GameFees = request
                            .context.AccountTransaction.Where(t =>
                                t.ReceiverId == (int)EnumAccountId.RED_Main_Account
                                && t.TypeId == (int)EnumTransactionType.Deposit
                                && (
                                    currVendorAccountId > 0
                                        ? t.VendorId == currVendorAccountId
                                        : true
                                )
                            )
                            .Sum(t => t.GameFees);

                        result.WeightFees = request
                            .context.AccountTransaction.Where(t =>
                                t.ReceiverId == (int)EnumAccountId.RED_Main_Account
                                && t.TypeId == (int)EnumTransactionType.Deposit
                                && (
                                    currVendorAccountId > 0
                                        ? t.VendorId == currVendorAccountId
                                        : true
                                )
                            )
                            .Sum(t => t.WeightFees);

                        result.SizeFees = request
                            .context.AccountTransaction.Where(t =>
                                t.ReceiverId == (int)EnumAccountId.RED_Main_Account
                                && t.TypeId == (int)EnumTransactionType.Deposit
                                && (
                                    currVendorAccountId > 0
                                        ? t.VendorId == currVendorAccountId
                                        : true
                                )
                            )
                            .Sum(t => t.SizeFees);

                        result.PartialDeliveryFees = request
                            .context.AccountTransaction.Where(t =>
                                t.ReceiverId == (int)EnumAccountId.RED_Main_Account
                                && t.TypeId == (int)EnumTransactionType.Deposit
                                && (
                                    currVendorAccountId > 0
                                        ? t.VendorId == currVendorAccountId
                                        : true
                                )
                            )
                            .Sum(t => t.PartialDeliveryFees);

                        result.PickupFees = request
                            .context.AccountTransaction.Where(t =>
                                t.ReceiverId == (int)EnumAccountId.RED_Main_Account
                                && t.TypeId == (int)EnumTransactionType.Deposit
                                && (
                                    currVendorAccountId > 0
                                        ? t.VendorId == currVendorAccountId
                                        : true
                                )
                            )
                            .Sum(t => t.PickupFees);

                        result.CancelFees = request
                            .context.AccountTransaction.Where(t =>
                                t.ReceiverId == (int)EnumAccountId.RED_Main_Account
                                && t.TypeId == (int)EnumTransactionType.Deposit
                                && (
                                    currVendorAccountId > 0
                                        ? t.VendorId == currVendorAccountId
                                        : true
                                )
                            )
                            .Sum(t => t.CancelFees);

                        result.Withdraw = request
                            .context.AccountTransaction.Where(t =>
                                t.ReceiverId != (int)EnumAccountId.RED_Main_Account
                                && t.ReceiverId != (int)EnumAccountId.Main_Treasury
                                && t.TypeId == (int)EnumTransactionType.Withdraw
                                && (
                                    currVendorAccountId > 0
                                        ? t.VendorId == currVendorAccountId
                                        : true
                                )
                            )
                            .Sum(t => t.Total);

                        result.CashTransferDTOs = request
                            .context.CashTransfer.Where(s =>
                                !s.IsDeleted
                                && (
                                    currVendorAccountId > 0
                                        ? s.ReceiverId == currVendorAccountId
                                        : true
                                )
                            )
                            .Select(s => new CashTransferDTO
                            {
                                CashTransferId = s.CashTransferId,
                                RefId = s.RefId,
                                Name = s.Name,
                                CreatedAt = s.CreatedAt,
                                Amount = s.Amount,
                                TypeId = s.TypeId,
                                ReceiverId = s.ReceiverId,
                                IsReceived = s.IsReceived,

                                AccountTransactionDTOs = request
                                    .context.AccountTransaction.Where(t =>
                                        t.ReceiverId != (int)EnumAccountId.Main_Treasury
                                        && request
                                            .context.Advertisement.Where(sh =>
                                                sh.CashTransferId == s.CashTransferId
                                            )
                                            .Select(sh => sh.AdvertisementId)
                                            .Contains(t.AdvertisementId.Value)
                                        && t.StatusId == (int)EnumStatus.Delivered
                                        && (
                                            currVendorAccountId > 0
                                                ? t.VendorId == currVendorAccountId
                                                : true
                                        )
                                    )
                                    .Select(t => new AccountTransactionDTO
                                    {
                                        AccountTransactionId = t.AccountTransactionId,
                                        PickupRefId = t.PickupRequestId.HasValue
                                            ? t.PickupRequest.RefId
                                            : null,
                                        CashTransferId = t.CashTransferId,
                                        CashTransferRefId = t.CashTransfer.RefId,
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
                                        RefId = t.RefId,
                                        CreatedAt = t.CreatedAt,

                                        StatusDTO = new StatusDTO
                                        {
                                            Id = t.StatusId.Value,
                                            NameEN = t.Status.NameEN,
                                        },
                                    })
                            })
                            .OrderByDescending(t => t.CashTransferId)
                            .ToList();

                        //if (request.applyFilter)
                        //    query = ApplyFilter(query, request.CashTransferDTO);

                        response.InvoiceDTO = result;

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

        private static IQueryable<CashTransferDTO> ApplyFilter(
            IQueryable<CashTransferDTO> query,
            CashTransferDTO filter
        )
        {
            if (filter.ReceiverId > 0)
                query = query.Where(p => p.ReceiverId == filter.ReceiverId);

            return query;
        }
    }
}

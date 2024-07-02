using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using DicomApp.CommonDefinitions.DTO;
using DicomApp.CommonDefinitions.DTO.AdvertisementDTOs;
using DicomApp.CommonDefinitions.Requests;
using DicomApp.CommonDefinitions.Responses;
using DicomApp.DAL.DB;
using DicomApp.Helpers;
using log4net.Core;
using Microsoft.CodeAnalysis;

namespace DicomApp.BL.Services
{
    public class AdvertisementService : BaseService
    {
        #region Advertisement Common Actions

        public static AdvertisementResponse AddAdvertisement(AdvertisementRequest request)
        {
            var response = new AdvertisementResponse();
            RunBase(
                request,
                response,
                (AdvertisementRequest req) =>
                {
                    try
                    {
                        var status = request.context.Status.FirstOrDefault(x =>
                            x.StatusType == (int)StatusTypeEnum.InProgress
                        );
                        var ship = new Advertisement()
                        {
                            StatusId = status.Id,
                            GamerId = request.AdsDTO.GamerId,
                            GameId = request.AdsDTO.GameId,
                            Description = request.AdsDTO.Description,
                            UserName = request.AdsDTO.UserName,
                            Password = request.AdsDTO.Password,
                            Price = request.AdsDTO.Price,
                            Level = req.AdsDTO.Level,
                            Rank = req.AdsDTO.Rank,
                            IsRefund = false,
                            CreatedAt = DateTime.Now,
                            CreatedBy = request.UserID,
                            IsDeleted = false,
                        };
                        request.context.Advertisement.Add(ship);
                        request.context.SaveChanges();
                        ship.RefId = BaseHelper.GenerateRefId(
                            EnumRefIdType.Advertisement,
                            ship.AdvertisementId
                        );

                        // Add Photos
                        if (request.AdsDTO.Files != null && request.AdsDTO.Files.Any())
                        {
                            foreach (var file in request.AdsDTO.Files)
                            {
                                var url = BaseHelper.UploadImg(file, request.RoutPath);
                                req.context.AdvertisementPhotos.Add(
                                    new AdvertisementPhotos
                                    {
                                        AdvertisementId = ship.AdvertisementId,
                                        Url = url
                                    }
                                );
                            }
                        }
                        //Add follow up
                        var follow = new FollowUpDTO
                        {
                            Title = "Advertisement Added",
                            CreatedBy = request.UserID,
                            AdvertisementId = ship.AdvertisementId,
                            StatusId = ship.StatusId
                        };
                        FollowUpService.Add(follow, request.context);

                        //Add notification
                        request.context.Notification.Add(
                            new Notification
                            {
                                Body = "New Advertisement " + ship.RefId + " added",
                                CreationDate = DateTime.Now,
                                Icon = ship.RefId,
                                SenderId = request.UserID,
                                Title = "New Advertisement",
                                RecipientRoleId = request
                                    .context.Role.FirstOrDefault(x =>
                                        x.Name == SystemConstants.Role.Admin
                                    )
                                    ?.Id,
                                IsDeleted = false,
                                IsSeen = false,
                                RecipientId = 0,
                            }
                        );

                        request.context.SaveChanges();
                        response.Message =
                            "New Advertisement " + ship.RefId + " successfully added";
                        response.Success = true;
                        response.StatusCode = HttpStatusCode.OK;
                    }
                    catch (Exception ex)
                    {
                        response.Message = ex.Message;
                        response.Success = false;
                        response.AdsDTO = request.AdsDTO;
                        LogHelper.LogException(ex.Message, ex.StackTrace);
                    }
                    return response;
                }
            );
            return response;
        }

        public static AdvertisementResponse EditAdvertisement(AdvertisementRequest request)
        {
            var response = new AdvertisementResponse();
            RunBase(
                request,
                response,
                (AdvertisementRequest req) =>
                {
                    try
                    {
                        var ship = request.context.Advertisement.FirstOrDefault(s =>
                            s.AdvertisementId == request.AdsDTO.AdvertisementId
                        );
                        var status = request.context.Status.FirstOrDefault(x =>
                            x.Id == ship.StatusId
                        );
                        if (
                            ship != null
                            && ship.Price != request.AdsDTO.Price
                            && status != null
                            && status.StatusType >= 2
                        )
                        {
                            response.Message = "you Cant Update Price";
                            response.Success = false;
                            response.AdsDTO = request.AdsDTO;
                            return response;
                        }

                        ship.GamerId = request.AdsDTO.GamerId;
                        ship.GameId = request.AdsDTO.GameId;
                        ship.Description = request.AdsDTO.Description;
                        ship.UserName = request.AdsDTO.UserName;
                        ship.Password = request.AdsDTO.Password;
                        ship.Price = request.AdsDTO.Price;
                        ship.Level = request.AdsDTO.Level;
                        ship.Rank = request.AdsDTO.Rank;
                        ship.StatusId =
                            request.AdsDTO.Publish.HasValue && request.AdsDTO.Publish.Value
                                ? request.context.Status.FirstOrDefault(x => x.StatusType == 3)?.Id
                                    ?? ship.StatusId
                                : ship.StatusId;
                        ship.LastModifiedAt = DateTime.Now;
                        ship.LastModifiedBy = request.UserID;
                        request.context.SaveChanges();

                        //Add follow up
                        var follow = new FollowUpDTO
                        {
                            Title = "Advertisement Updated",
                            CreatedBy = request.UserID,
                            AdvertisementId = ship.AdvertisementId,
                            StatusId = ship.StatusId
                        };
                        FollowUpService.Add(follow, request.context);

                        //Add notification
                        request.context.Notification.Add(
                            new Notification
                            {
                                Body = "Advertisement " + ship.RefId + " updated",
                                CreationDate = DateTime.Now,
                                Icon = ship.RefId,
                                SenderId = request.UserID,
                                Title = "Advertisement updated",
                                RecipientRoleId = request
                                    .context.Role.FirstOrDefault(x =>
                                        x.Name == SystemConstants.Role.SuperAdmin
                                    )
                                    ?.Id,
                                IsDeleted = false,
                                IsSeen = false,
                                RecipientId = 0,
                            }
                        );

                        request.context.SaveChanges();
                        response.Message = "Advertisement " + ship.RefId + " successfully updated";
                        response.Success = true;
                        response.AdsDTO = request.AdsDTO;
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

        public static AdvertisementResponse EditBasicAdvertisement(AdvertisementRequest request)
        {
            var response = new AdvertisementResponse();
            RunBase(
                request,
                response,
                (AdvertisementRequest req) =>
                {
                    try
                    {
                        var ship = request.context.Advertisement.FirstOrDefault(s =>
                            s.AdvertisementId == request.AdsDTO.AdvertisementId
                        );
                        if (ship != null)
                        {
                            ship.Description = request.AdsDTO.Description;
                            ship.UserName = request.AdsDTO.UserName;
                            ship.Password = request.AdsDTO.Password;
                            ship.Level = request.AdsDTO.Level;
                            ship.Rank = request.AdsDTO.Rank;

                            ship.LastModifiedAt = DateTime.Now;
                            ship.LastModifiedBy = request.UserID;

                            request.context.SaveChanges();

                            //Add follow up
                            var follow = new FollowUpDTO
                            {
                                Title = "Advertisement Data Update",
                                CreatedBy = request.UserID,
                                AdvertisementId = ship.AdvertisementId,
                                StatusId = ship.StatusId
                            };
                            FollowUpService.Add(follow, request.context);

                            //Add notification
                            request.context.Notification.Add(
                                new Notification
                                {
                                    Body = "Advertisement " + ship.RefId + " updated",
                                    CreationDate = DateTime.Now,
                                    Icon = ship.RefId,
                                    SenderId = request.UserID,
                                    Title = "Advertisement updated",
                                    RecipientRoleId = (int)EnumRole.SuperAdmin,
                                    IsDeleted = false,
                                    IsSeen = false,
                                    RecipientId = 0,
                                }
                            );

                            request.context.SaveChanges();

                            response.Message =
                                "Advertisement " + ship.RefId + " successfully updated";
                            response.Success = true;
                            response.StatusCode = HttpStatusCode.OK;
                        }
                        else
                        {
                            response.Message = "Advertisement Not Found";
                            response.Success = false;
                            response.StatusCode = HttpStatusCode.OK;
                        }
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

        public static AdvertisementResponse RejectAdvertisement(AdvertisementRequest request)
        {
            var response = new AdvertisementResponse();
            RunBase(
                request,
                response,
                (AdvertisementRequest req) =>
                {
                    try
                    {
                        var ship = request.context.Advertisement.FirstOrDefault(s =>
                            s.AdvertisementId == request.AdsDTO.AdvertisementId
                        );
                        var status = request.context.Status.FirstOrDefault(x =>
                            x.StatusType == (int)StatusTypeEnum.Reject
                        );
                        if (status != null && ship != null)
                        {
                            ship.StatusId = status.Id;
                            ship.LastModifiedAt = DateTime.Now;
                            ship.LastModifiedBy = request.UserID;
                            request.context.SaveChanges();

                            //Add follow up
                            var follow = new FollowUpDTO
                            {
                                Title = "Advertisement Data Update",
                                CreatedBy = request.UserID,
                                AdvertisementId = ship.AdvertisementId,
                                StatusId = ship.StatusId
                            };
                            FollowUpService.Add(follow, request.context);

                            //Add notification
                            request.context.Notification.Add(
                                new Notification
                                {
                                    Body = "Advertisement " + ship.RefId + " updated",
                                    CreationDate = DateTime.Now,
                                    Icon = ship.RefId,
                                    SenderId = request.UserID,
                                    Title = "Advertisement updated",
                                    RecipientRoleId = (int)EnumRole.SuperAdmin,
                                    IsDeleted = false,
                                    IsSeen = false,
                                    RecipientId = 0,
                                }
                            );

                            request.context.SaveChanges();

                            response.Message =
                                "Advertisement " + ship.RefId + " successfully updated";
                            response.Success = true;
                            response.StatusCode = HttpStatusCode.OK;
                        }
                        else
                        {
                            response.Message = "Advertisement Not Found";
                            response.Success = false;
                            response.StatusCode = HttpStatusCode.OK;
                        }
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

        public static AdvertisementResponse ChangStatusAdvertisement(AdvertisementRequest request)
        {
            var response = new AdvertisementResponse();
            RunBase(
                request,
                response,
                (AdvertisementRequest req) =>
                {
                    try
                    {
                        var ship = request.context.Advertisement.FirstOrDefault(s =>
                            s.AdvertisementId == request.AdsDTO.AdvertisementId
                        );
                        var status = request.context.Status.FirstOrDefault(x =>
                            x.StatusType == request.AdsDTO.StatusType
                        );
                        if (status != null && ship != null)
                        {
                            ship.StatusId = status.Id;
                            ship.LastModifiedAt = DateTime.Now;
                            ship.LastModifiedBy = request.UserID;
                            request.context.SaveChanges();

                            //Add follow up
                            var follow = new FollowUpDTO
                            {
                                Title = "Advertisement Data Update",
                                CreatedBy = request.UserID,
                                AdvertisementId = ship.AdvertisementId,
                                StatusId = ship.StatusId
                            };
                            FollowUpService.Add(follow, request.context);

                            //Add notification
                            request.context.Notification.Add(
                                new Notification
                                {
                                    Body = "Advertisement " + ship.RefId + " updated",
                                    CreationDate = DateTime.Now,
                                    Icon = ship.RefId,
                                    SenderId = request.UserID,
                                    Title = "Advertisement updated",
                                    RecipientRoleId = request
                                        .context.Role.FirstOrDefault(x =>
                                            x.Name == SystemConstants.Role.SuperAdmin
                                        )
                                        ?.Id,
                                    IsDeleted = false,
                                    IsSeen = false,
                                    RecipientId = 0,
                                }
                            );

                            request.context.SaveChanges();

                            response.Message =
                                "Advertisement " + ship.RefId + " successfully updated";
                            response.Success = true;
                            response.StatusCode = HttpStatusCode.OK;
                        }
                        else
                        {
                            response.Message = "Advertisement Not Found";
                            response.Success = false;
                            response.StatusCode = HttpStatusCode.OK;
                        }
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

        #endregion

        #region Advertisement Change Status

        //public static AdvertisementResponse Cancel(AdvertisementRequest request)
        //{
        //    var response = new AdvertisementResponse();
        //    RunBase(
        //        request,
        //        response,
        //        (AdvertisementRequest req) =>
        //        {
        //            try
        //            {
        //                var ship = request.context.Advertisement.FirstOrDefault(r =>
        //                    r.AdvertisementId == request.AdsDTO.AdvertisementId
        //                );
        //                var shipItems = request.context.AdvertisementItem.Where(i =>
        //                    i.AdvertisementId == ship.AdvertisementId
        //                );
        //                var problem = request.AdsDTO.ProblemDTOs.FirstOrDefault();

        //                ship.CancelComment = request.AdsDTO.CancelComment;
        //                ship.IsTripCompleted = request.AdsDTO.IsTripCompleted;
        //                ship.LastModifiedAt = DateTime.Now;
        //                ship.LastModifiedBy = request.UserID;

        //                if (ship.ShippingFeesPaid == 0)
        //                    ship.IsCashReceived = true;

        //                //*** Add Advertisement problem
        //                request.context.AdvertisementProblem.Add(
        //                    new AdvertisementProblem
        //                    {
        //                        CreatedAt = DateTime.Now,
        //                        CreatedBy = request.UserID,
        //                        ProblemTypeId = problem.ProblemTypeId,
        //                        Note = problem.Note,
        //                        AdvertisementId = ship.AdvertisementId
        //                    }
        //                );

        //                //*** Add cancel follow up
        //                var Problemfollow = new FollowUpDTO
        //                {
        //                    FollowUpTypeId = (int)EnumFollowupType.Problem_Added,
        //                    Title = request
        //                        .context.ProblemType.FirstOrDefault(s =>
        //                            s.ProblemTypeId == problem.ProblemTypeId
        //                        )
        //                        .NameEn,
        //                    Comment = problem.Note,
        //                    CreatedBy = request.UserID,
        //                    AdvertisementId = ship.AdvertisementId,
        //                    StatusId = (int)EnumStatus.Cancelled
        //                };
        //                FollowUpService.Add(Problemfollow, request.context);

        //                //*** Add follow up
        //                var follow = new FollowUpDTO
        //                {
        //                    FollowUpTypeId = (int)EnumFollowupType.Advertisement_Updated,
        //                    Title = request
        //                        .context.Status.FirstOrDefault(s =>
        //                            s.Id == (int)EnumStatus.Cancelled
        //                        )
        //                        .NameEN,
        //                    Comment = request
        //                        .context.CommonUser.FirstOrDefault(u => u.Id == request.UserID)
        //                        .Name,
        //                    CreatedBy = request.UserID,
        //                    AdvertisementId = ship.AdvertisementId,
        //                    StatusId = (int)EnumStatus.Cancelled
        //                };
        //                FollowUpService.Add(follow, request.context);

        //                //*** Check for cancel fees
        //                //*** Note : No cancel fees will applied to below status
        //                var noCancelFeesStatusIDs = new List<int>
        //                {
        //                    (int)EnumStatus.Ready_For_Pickup,
        //                    (int)EnumStatus.Assigned_For_Pickup,
        //                    (int)EnumStatus.Picked_Up,
        //                    (int)EnumStatus.Ready_For_Return,
        //                    (int)EnumStatus.Assigned_For_Return,
        //                    (int)EnumStatus.Out_For_Return
        //                };
        //                if (!noCancelFeesStatusIDs.Contains(ship.StatusId))
        //                {
        //                    #region Add Cancel Fees

        //                    var VendorAccountId = request
        //                        .context.Account.FirstOrDefault(a => a.UserId == ship.VendorId)
        //                        .AccountId;
        //                    #region Vendor Transaction
        //                    var VendorTrans = new AccountTransactionDTO();
        //                    VendorTrans.CreatedBy = request.UserID;
        //                    VendorTrans.ReceiverId = VendorAccountId;
        //                    VendorTrans.VendorId = VendorAccountId;
        //                    VendorTrans.AdvertisementId = ship.AdvertisementId;
        //                    VendorTrans.StatusId = (int)EnumStatus.Cancelled;
        //                    VendorTrans.TypeId = (int)EnumTransactionType.Withdraw;
        //                    #endregion
        //                    #region RED Transaction
        //                    var REDTrans = new AccountTransactionDTO();
        //                    REDTrans.CreatedBy = request.UserID;
        //                    REDTrans.ReceiverId = (int)EnumAccountId.RED_Main_Account;
        //                    REDTrans.VendorId = VendorAccountId;
        //                    REDTrans.AdvertisementId = ship.AdvertisementId;
        //                    REDTrans.StatusId = (int)EnumStatus.Cancelled;
        //                    REDTrans.TypeId = (int)EnumTransactionType.Deposit;
        //                    #endregion

        //                    // Trip is Completed
        //                    if (ship.IsTripCompleted)
        //                    {
        //                        ship.CancelFees =
        //                            ship.ShippingFees
        //                            + ship.GameFees
        //                            + ship.WeightFees
        //                            + ship.SizeFees
        //                            + ship.PartialDeliveryFees
        //                            - ship.ShippingFeesPaid;

        //                        #region Vendor Transaction
        //                        VendorTrans.ShippingFees = ship.ShippingFees;
        //                        VendorTrans.GameFees = ship.GameFees;
        //                        VendorTrans.WeightFees = ship.WeightFees;
        //                        VendorTrans.SizeFees = ship.SizeFees;
        //                        VendorTrans.PartialDeliveryFees = ship.PartialDeliveryFees;
        //                        VendorTrans.ShippingFeesPaid = ship.ShippingFeesPaid;

        //                        VendorTrans.CancelFees = ship.CancelFees;
        //                        VendorTrans.Total = ship.CancelFees;
        //                        AccountTransactionService.AddTransaction(
        //                            request.context,
        //                            VendorTrans
        //                        );
        //                        #endregion
        //                        #region RED Transaction
        //                        REDTrans.ShippingFees = ship.ShippingFees;
        //                        REDTrans.GameFees = ship.GameFees;
        //                        REDTrans.WeightFees = ship.WeightFees;
        //                        REDTrans.SizeFees = ship.SizeFees;
        //                        REDTrans.PartialDeliveryFees = ship.PartialDeliveryFees;
        //                        REDTrans.ShippingFeesPaid = ship.ShippingFeesPaid;
        //                        REDTrans.CancelFees = ship.CancelFees;

        //                        REDTrans.Total =
        //                            ship.ShippingFees
        //                            + ship.GameFees
        //                            + ship.WeightFees
        //                            + ship.SizeFees
        //                            + ship.PartialDeliveryFees;
        //                        AccountTransactionService.AddTransaction(request.context, REDTrans);
        //                        #endregion
        //                    }
        //                    else // Trip is NOT Completed
        //                    {
        //                        ship.CancelFees = BaseHelper.Constants.CancelFees;

        //                        #region Vendor Transaction
        //                        VendorTrans.CancelFees = ship.CancelFees;
        //                        VendorTrans.Total = ship.CancelFees;
        //                        AccountTransactionService.AddTransaction(
        //                            request.context,
        //                            VendorTrans
        //                        );
        //                        #endregion
        //                        #region RED Transaction
        //                        REDTrans.CancelFees = ship.CancelFees;
        //                        REDTrans.Total = ship.CancelFees;
        //                        AccountTransactionService.AddTransaction(request.context, REDTrans);
        //                        #endregion
        //                    }
        //                    #endregion
        //                }

        //                ship.StatusId = (int)EnumStatus.Cancelled;
        //                //*** Change sub items status
        //                foreach (var item in shipItems)
        //                    item.StatusId = (int)EnumStatus.Cancelled;

        //                request.context.SaveChanges();

        //                response.Message = "Advertisement successfully cancelled";
        //                response.Success = true;
        //                response.StatusCode = HttpStatusCode.OK;
        //            }
        //            catch (Exception ex)
        //            {
        //                response.Message = ex.Message;
        //                response.Success = false;
        //                LogHelper.LogException(ex.Message, ex.StackTrace);
        //            }
        //            return response;
        //        }
        //    );
        //    return response;
        //}

        //public static AdvertisementResponse Deliver(AdvertisementRequest request)
        //{
        //    var response = new AdvertisementResponse();
        //    RunBase(
        //        request,
        //        response,
        //        (AdvertisementRequest req) =>
        //        {
        //            try
        //            {
        //                var Advertisements = request.context.Advertisement.Where(r =>
        //                    request.AdsDTO.AdvertisementIDs.Contains(r.AdvertisementId)
        //                );

        //                foreach (var ship in Advertisements)
        //                {
        //                    //*** Deliver Advertisement
        //                    ship.StatusId = (int)EnumStatus.Delivered;
        //                    ship.LastModifiedAt = DateTime.Now;
        //                    ship.LastModifiedBy = request.UserID;

        //                    #region Update Advertisement Items
        //                    if (request.AdsDTO.ShipItemDTOs != null)
        //                    {
        //                        foreach (var item in request.AdsDTO.ShipItemDTOs)
        //                        {
        //                            var shipItem = request.context.AdvertisementItem.FirstOrDefault(
        //                                i => i.AdvertisementItemId == item.AdvertisementItemId
        //                            );

        //                            if (
        //                                item.CourierQuantityDelivered == 0
        //                                || item.CourierQuantityDelivered > shipItem.Quantity
        //                            )
        //                            {
        //                                response.Message =
        //                                    "Delivered quantity is equal to zero or greater than original";
        //                                response.Success = false;
        //                                response.StatusCode = HttpStatusCode.NotImplemented;
        //                                return response;
        //                            }
        //                            else
        //                            {
        //                                shipItem.StatusId = (int)EnumStatus.Delivered;
        //                                shipItem.CourierQuantityDelivered =
        //                                    item.CourierQuantityDelivered;
        //                            }
        //                        }
        //                    }
        //                    else
        //                    {
        //                        var lstShipItems = request.context.AdvertisementItem.Where(i =>
        //                            i.AdvertisementId == ship.AdvertisementId
        //                        );
        //                        foreach (var item in lstShipItems)
        //                        {
        //                            item.StatusId = (int)EnumStatus.Delivered;
        //                            item.CourierQuantityDelivered = item.Quantity;
        //                        }
        //                    }
        //                    #endregion

        //                    #region Add Cash Transactions / Update Accounts Balances

        //                    // *** Check if Advertisement is fully delivered / Customer paid amount
        //                    double CustomerPaidAmount = 0;
        //                    bool takeOriginalCOD = true;
        //                    var VendorAccountId = request
        //                        .context.Account.FirstOrDefault(a => a.UserId == ship.VendorId)
        //                        .AccountId;

        //                    var shipItems = request.context.AdvertisementItem.Where(i =>
        //                        i.AdvertisementId == ship.AdvertisementId
        //                    );
        //                    foreach (var item in shipItems)
        //                    {
        //                        if (
        //                            item.StatusId != (int)EnumStatus.Delivered
        //                            || item.CourierQuantityDelivered < item.Quantity
        //                        )
        //                            takeOriginalCOD = false;

        //                        CustomerPaidAmount += (item.Price * item.CourierQuantityDelivered);
        //                    }

        //                    #region Vendor Transactions
        //                    var VendorTrans = new AccountTransactionDTO();
        //                    VendorTrans.CreatedBy = request.UserID;
        //                    VendorTrans.ReceiverId = VendorAccountId;
        //                    VendorTrans.VendorId = VendorAccountId;
        //                    VendorTrans.AdvertisementId = ship.AdvertisementId;
        //                    VendorTrans.StatusId = (int)EnumStatus.Delivered;

        //                    VendorTrans.VendorCash = takeOriginalCOD
        //                        ? ship.VendorCash
        //                        : (
        //                            CustomerPaidAmount
        //                            - ship.ShippingFees
        //                            - ship.GameFees
        //                            - ship.WeightFees
        //                            - ship.SizeFees
        //                            - ship.PartialDeliveryFees
        //                        );
        //                    VendorTrans.Total = VendorTrans.VendorCash;
        //                    VendorTrans.TypeId = (int)EnumTransactionType.Deposit;
        //                    AccountTransactionService.AddTransaction(request.context, VendorTrans);
        //                    #endregion

        //                    #region RED Transactions
        //                    var REDTrans = new AccountTransactionDTO();
        //                    REDTrans.CreatedBy = request.UserID;
        //                    REDTrans.ReceiverId = (int)EnumAccountId.RED_Main_Account;
        //                    REDTrans.VendorId = VendorAccountId;
        //                    REDTrans.AdvertisementId = ship.AdvertisementId;
        //                    REDTrans.StatusId = (int)EnumStatus.Delivered;

        //                    REDTrans.ShippingFees = ship.ShippingFees;
        //                    REDTrans.GameFees = ship.GameFees;
        //                    REDTrans.WeightFees = ship.WeightFees;
        //                    REDTrans.SizeFees = ship.SizeFees;
        //                    REDTrans.PartialDeliveryFees = ship.PartialDeliveryFees;

        //                    REDTrans.Total =
        //                        ship.ShippingFees
        //                        + ship.GameFees
        //                        + ship.WeightFees
        //                        + ship.SizeFees
        //                        + ship.PartialDeliveryFees;

        //                    REDTrans.TypeId = (int)EnumTransactionType.Deposit;
        //                    AccountTransactionService.AddTransaction(request.context, REDTrans);
        //                    #endregion

        //                    #endregion

        //                    //Add follow up
        //                    var follow = new FollowUpDTO
        //                    {
        //                        FollowUpTypeId = (int)EnumFollowupType.Advertisement_Updated,
        //                        Title = request
        //                            .context.Status.FirstOrDefault(s =>
        //                                s.Id == (int)EnumStatus.Delivered
        //                            )
        //                            .NameEN,
        //                        Comment = request
        //                            .context.CommonUser.FirstOrDefault(u => u.Id == request.UserID)
        //                            .Name,
        //                        CreatedBy = request.UserID,
        //                        AdvertisementId = ship.AdvertisementId,
        //                        StatusId = (int)EnumStatus.Delivered
        //                    };
        //                    FollowUpService.Add(follow, request.context);
        //                }

        //                request.context.SaveChanges();

        //                response.Message = "Advertisement successfully delivered";
        //                response.Success = true;
        //                response.StatusCode = HttpStatusCode.OK;
        //            }
        //            catch (Exception ex)
        //            {
        //                response.Message = ex.Message;
        //                response.Success = false;
        //                LogHelper.LogException(ex.Message, ex.StackTrace);
        //            }
        //            return response;
        //        }
        //    );
        //    return response;
        //}

        //public static AdvertisementResponse Return(AdvertisementRequest request)
        //{
        //    var response = new AdvertisementResponse();
        //    RunBase(
        //        request,
        //        response,
        //        (AdvertisementRequest req) =>
        //        {
        //            try
        //            {
        //                var Advertisements = request.context.Advertisement.Where(r =>
        //                    request.AdsDTO.AdvertisementIDs.Contains(r.AdvertisementId)
        //                );

        //                foreach (var ship in Advertisements)
        //                {
        //                    if (request.AdsDTO.ShipItemDTOs != null)
        //                    {
        //                        foreach (var item in request.AdsDTO.ShipItemDTOs)
        //                        {
        //                            var shipItem = request.context.AdvertisementItem.FirstOrDefault(
        //                                i => i.AdvertisementItemId == item.AdvertisementItemId
        //                            );
        //                            if (
        //                                shipItem.StatusId != (int)EnumStatus.Delivered
        //                                || item.CourierQuantityReturned > 0
        //                            )
        //                                shipItem.StatusId = (int)EnumStatus.Returned;
        //                        }
        //                    }
        //                    else
        //                    {
        //                        var shipItems = request.context.AdvertisementItem.Where(i =>
        //                            i.AdvertisementId == ship.AdvertisementId
        //                        );
        //                        foreach (var item in shipItems)
        //                            if (
        //                                item.StatusId != (int)EnumStatus.Delivered
        //                                || item.CourierQuantityReturned > 0
        //                            )
        //                                item.StatusId = (int)EnumStatus.Returned;
        //                    }

        //                    ship.StatusId = (int)EnumStatus.Returned;
        //                    ship.LastModifiedAt = DateTime.Now;
        //                    ship.LastModifiedBy = request.UserID;

        //                    //Add follow up
        //                    var follow = new FollowUpDTO
        //                    {
        //                        FollowUpTypeId = (int)EnumFollowupType.Advertisement_Updated,
        //                        Title = request
        //                            .context.Status.FirstOrDefault(s =>
        //                                s.Id == (int)EnumStatus.Returned
        //                            )
        //                            .NameEN,
        //                        Comment = request
        //                            .context.CommonUser.FirstOrDefault(u => u.Id == request.UserID)
        //                            .Name,
        //                        CreatedBy = request.UserID,
        //                        AdvertisementId = ship.AdvertisementId,
        //                        StatusId = (int)EnumStatus.Returned
        //                    };
        //                    FollowUpService.Add(follow, request.context);
        //                }

        //                request.context.SaveChanges();

        //                response.Message = "Advertisement successfully returned";
        //                response.Success = true;
        //                response.StatusCode = HttpStatusCode.OK;
        //            }
        //            catch (Exception ex)
        //            {
        //                response.Message = ex.Message;
        //                response.Success = false;
        //                LogHelper.LogException(ex.Message, ex.StackTrace);
        //            }
        //            return response;
        //        }
        //    );
        //    return response;
        //}

        //public static AdvertisementResponse Refund(AdvertisementRequest request)
        //{
        //    var response = new AdvertisementResponse();
        //    RunBase(
        //        request,
        //        response,
        //        (AdvertisementRequest req) =>
        //        {
        //            try
        //            {
        //                var Advertisements = request.context.Advertisement.Where(r =>
        //                    request.AdsDTO.AdvertisementIDs.Contains(r.AdvertisementId)
        //                );

        //                foreach (var ship in Advertisements)
        //                {
        //                    ship.StatusId = (int)EnumStatus.Refunded;
        //                    ship.LastModifiedAt = DateTime.Now;
        //                    ship.LastModifiedBy = request.UserID;

        //                    //Add follow up
        //                    var follow = new FollowUpDTO
        //                    {
        //                        FollowUpTypeId = (int)EnumFollowupType.Advertisement_Updated,
        //                        Title = request
        //                            .context.Status.FirstOrDefault(s => s.Id == ship.StatusId)
        //                            .NameEN,
        //                        Comment = request
        //                            .context.CommonUser.FirstOrDefault(u => u.Id == request.UserID)
        //                            .Name,
        //                        CreatedBy = request.UserID,
        //                        AdvertisementId = ship.AdvertisementId,
        //                        StatusId = ship.StatusId
        //                    };
        //                    FollowUpService.Add(follow, request.context);

        //                    //Add Transactions
        //                    var VendorAccountId = request
        //                        .context.Account.FirstOrDefault(a => a.UserId == ship.VendorId)
        //                        .AccountId;

        //                    #region Vendor Transactions
        //                    var VendorTrans = new AccountTransactionDTO();
        //                    VendorTrans.CreatedBy = request.UserID;
        //                    VendorTrans.SenderId = (int)EnumAccountId.Main_Treasury;
        //                    VendorTrans.ReceiverId = VendorAccountId;
        //                    VendorTrans.VendorId = VendorAccountId;
        //                    VendorTrans.AdvertisementId = ship.AdvertisementId;
        //                    VendorTrans.StatusId = (int)EnumStatus.Refunded;

        //                    VendorTrans.TypeId = (int)EnumTransactionType.Withdraw;
        //                    VendorTrans.RefundCash = ship.RefundCash;
        //                    VendorTrans.RefundFees = ship.RefundFees;
        //                    VendorTrans.ShippingFees = ship.ShippingFees;
        //                    VendorTrans.Total =
        //                        ship.RefundCash.Value + ship.RefundFees.Value + ship.ShippingFees;
        //                    AccountTransactionService.AddTransaction(request.context, VendorTrans);
        //                    #endregion

        //                    #region RED Transactions
        //                    var REDTrans = new AccountTransactionDTO();
        //                    REDTrans.CreatedBy = request.UserID;
        //                    REDTrans.SenderId = (int)EnumAccountId.Main_Treasury;
        //                    REDTrans.ReceiverId = (int)EnumAccountId.RED_Main_Account;
        //                    REDTrans.VendorId = VendorAccountId;
        //                    REDTrans.AdvertisementId = ship.AdvertisementId;
        //                    REDTrans.StatusId = (int)EnumStatus.Refunded;

        //                    REDTrans.TypeId = (int)EnumTransactionType.Deposit;
        //                    REDTrans.RefundFees = ship.RefundFees;
        //                    REDTrans.ShippingFees = ship.ShippingFees;
        //                    REDTrans.Total = ship.RefundFees.Value + ship.ShippingFees;
        //                    AccountTransactionService.AddTransaction(request.context, REDTrans);
        //                    #endregion

        //                    #region Main Treasury Transactions
        //                    var TreasuryTrans = new AccountTransactionDTO();
        //                    TreasuryTrans.CreatedBy = request.UserID;
        //                    TreasuryTrans.SenderId = (int)EnumAccountId.Main_Treasury;
        //                    TreasuryTrans.ReceiverId = (int)EnumAccountId.Main_Treasury;
        //                    TreasuryTrans.VendorId = VendorAccountId;
        //                    TreasuryTrans.AdvertisementId = ship.AdvertisementId;
        //                    TreasuryTrans.StatusId = (int)EnumStatus.Refunded;

        //                    TreasuryTrans.TypeId = (int)EnumTransactionType.Withdraw;
        //                    TreasuryTrans.RefundCash = ship.RefundCash;
        //                    TreasuryTrans.Total = ship.RefundCash.Value;

        //                    AccountTransactionService.AddTransaction(
        //                        request.context,
        //                        TreasuryTrans
        //                    );
        //                    #endregion
        //                }

        //                request.context.SaveChanges();

        //                response.Message = "Advertisement successfully refunded";
        //                response.Success = true;
        //                response.StatusCode = HttpStatusCode.OK;
        //            }
        //            catch (Exception ex)
        //            {
        //                response.Message = ex.Message;
        //                response.Success = false;
        //                LogHelper.LogException(ex.Message, ex.StackTrace);
        //            }
        //            return response;
        //        }
        //    );
        //    return response;
        //}

        //public static AdvertisementResponse Postpone(AdvertisementRequest request)
        //{
        //    var response = new AdvertisementResponse();
        //    RunBase(
        //        request,
        //        response,
        //        (AdvertisementRequest req) =>
        //        {
        //            try
        //            {
        //                var ship = request.context.Advertisement.FirstOrDefault(s =>
        //                    s.AdvertisementId == request.AdsDTO.AdvertisementId
        //                );

        //                ship.LastModifiedAt = DateTime.Now;
        //                ship.LastModifiedBy = request.UserID;
        //                ship.StatusId =
        //                    request.AdsDTO.CustomerFollowUpDTO.NextCallOn.Value.Day
        //                    == DateTime.Now.Day
        //                        ? (int)EnumStatus.Ready_For_Delivery
        //                        : (int)EnumStatus.Postponed;
        //                //ship.StatusId = request.AdsDTO.CustomerFollowUpDTO.NextCallOn.Value.Day == DateTime.Now.Day ?
        //                //                (int)EnumStatus.Ready_For_Delivery : (int)EnumStatus.At_Warehouse;

        //                var shipItems = request.context.AdvertisementItem.Where(i =>
        //                    i.AdvertisementId == ship.AdvertisementId
        //                );
        //                foreach (var item in shipItems)
        //                    item.StatusId = ship.StatusId;

        //                var shipFollow = new AdvertisementCustomerFollowUp
        //                {
        //                    AdvertisementId = ship.AdvertisementId,
        //                    NextCallOn = request.AdsDTO.CustomerFollowUpDTO.NextCallOn,
        //                    NextCallTimeFrom = request.AdsDTO.CustomerFollowUpDTO.NextCallTimeFrom,
        //                    NextCallTimeTo = request.AdsDTO.CustomerFollowUpDTO.NextCallTimeTo,
        //                    IsConfirmed = request.AdsDTO.CustomerFollowUpDTO.IsConfirmed,
        //                    Note = request.AdsDTO.CustomerFollowUpDTO.Note,

        //                    CreatedAt = DateTime.Now,
        //                    CreatedBy = request.UserID
        //                };
        //                request.context.AdvertisementCustomerFollowUp.Add(shipFollow);

        //                request.context.SaveChanges();

        //                //Add follow up
        //                var follow = new FollowUpDTO
        //                {
        //                    FollowUpTypeId = (int)EnumFollowupType.Advertisement_Updated,
        //                    Title = "Advertisement Postponed",
        //                    CreatedBy = request.UserID,
        //                    AdvertisementId = ship.AdvertisementId,
        //                    StatusId = ship.StatusId
        //                };
        //                FollowUpService.Add(follow, request.context);

        //                request.context.SaveChanges();

        //                response.Message =
        //                    "Advertisement " + ship.RefId + " successfully postponed";
        //                response.Success = true;
        //                response.StatusCode = HttpStatusCode.OK;
        //            }
        //            catch (Exception ex)
        //            {
        //                response.Message = ex.Message;
        //                response.Success = false;
        //                LogHelper.LogException(ex.Message, ex.StackTrace);
        //            }
        //            return response;
        //        }
        //    );
        //    return response;
        //}

        //public static AdvertisementResponse Assign(AdvertisementRequest request)
        //{
        //    var response = new AdvertisementResponse();
        //    RunBase(
        //        request,
        //        response,
        //        (AdvertisementRequest req) =>
        //        {
        //            try
        //            {
        //                var Advertisements = request.context.Advertisement.Where(r =>
        //                    request.AdsDTO.AdvertisementIDs.Contains(r.AdvertisementId)
        //                );
        //                foreach (var ship in Advertisements)
        //                {
        //                    ship.DeliveryManId = request.AdsDTO.DeliveryManId;

        //                    if (ship.StatusId == (int)EnumStatus.Ready_For_Delivery)
        //                        ship.StatusId = (int)EnumStatus.Assigned_For_Delivery;
        //                    else if (ship.StatusId == (int)EnumStatus.Ready_For_Return)
        //                        ship.StatusId = (int)EnumStatus.Assigned_For_Return;
        //                    else if (ship.StatusId == (int)EnumStatus.Ready_For_Refund)
        //                        ship.StatusId = (int)EnumStatus.Assigned_For_Refund;

        //                    ship.LastModifiedAt = DateTime.Now;
        //                    ship.LastModifiedBy = request.UserID;

        //                    var shipItems = request.context.AdvertisementItem.Where(i =>
        //                        i.AdvertisementId == ship.AdvertisementId
        //                    );
        //                    foreach (var item in shipItems)
        //                    {
        //                        if (item.StatusId != (int)EnumStatus.Delivered)
        //                        {
        //                            if (item.StatusId == (int)EnumStatus.Ready_For_Delivery)
        //                                item.StatusId = (int)EnumStatus.Assigned_For_Delivery;
        //                            else if (item.StatusId == (int)EnumStatus.Ready_For_Return)
        //                                item.StatusId = (int)EnumStatus.Assigned_For_Return;
        //                            else if (item.StatusId == (int)EnumStatus.Ready_For_Refund)
        //                                item.StatusId = (int)EnumStatus.Assigned_For_Refund;
        //                        }
        //                    }

        //                    //Add follow up
        //                    var follow = new FollowUpDTO
        //                    {
        //                        FollowUpTypeId = (int)EnumFollowupType.Advertisement_Updated,
        //                        Title = request
        //                            .context.Status.FirstOrDefault(s => s.Id == ship.StatusId)
        //                            .NameEN,
        //                        Comment = request
        //                            .context.CommonUser.FirstOrDefault(u =>
        //                                u.Id == request.AdsDTO.DeliveryManId
        //                            )
        //                            .Name,
        //                        CreatedBy = request.UserID,
        //                        AdvertisementId = ship.AdvertisementId,
        //                        StatusId = ship.StatusId
        //                    };

        //                    FollowUpService.Add(follow, request.context);
        //                }

        //                request.context.SaveChanges();

        //                response.Message = "Advertisement(s) successfully assigned";
        //                response.Success = true;
        //                response.StatusCode = HttpStatusCode.OK;
        //            }
        //            catch (Exception ex)
        //            {
        //                response.Message = ex.Message;
        //                response.Success = false;
        //                LogHelper.LogException(ex.Message, ex.StackTrace);
        //            }
        //            return response;
        //        }
        //    );
        //    return response;
        //}

        //public static AdvertisementResponse Unassigned(AdvertisementRequest request)
        //{
        //    var response = new AdvertisementResponse();
        //    RunBase(
        //        request,
        //        response,
        //        (AdvertisementRequest req) =>
        //        {
        //            try
        //            {
        //                var Advertisement = request.context.Advertisement.FirstOrDefault(s =>
        //                    request.AdsDTO.AdvertisementId == s.AdvertisementId
        //                );

        //                Advertisement.DeliveryManId = null;
        //                Advertisement.LastModifiedAt = DateTime.Now;
        //                Advertisement.LastModifiedBy = request.UserID;

        //                if (Advertisement.StatusId == (int)EnumStatus.Assigned_For_Delivery)
        //                    Advertisement.StatusId = (int)EnumStatus.Ready_For_Delivery;
        //                else if (Advertisement.StatusId == (int)EnumStatus.Assigned_For_Return)
        //                    Advertisement.StatusId = (int)EnumStatus.Ready_For_Return;
        //                else if (Advertisement.StatusId == (int)EnumStatus.Assigned_For_Refund)
        //                    Advertisement.StatusId = (int)EnumStatus.Ready_For_Refund;

        //                var shipItems = request.context.AdvertisementItem.Where(i =>
        //                    i.AdvertisementId == Advertisement.AdvertisementId
        //                );
        //                foreach (var item in shipItems)
        //                {
        //                    if (item.StatusId != (int)EnumStatus.Delivered)
        //                    {
        //                        if (item.StatusId == (int)EnumStatus.Assigned_For_Delivery)
        //                            item.StatusId = (int)EnumStatus.Ready_For_Delivery;
        //                        else if (item.StatusId == (int)EnumStatus.Assigned_For_Return)
        //                            item.StatusId = (int)EnumStatus.Ready_For_Return;
        //                        else if (item.StatusId == (int)EnumStatus.Assigned_For_Refund)
        //                            item.StatusId = (int)EnumStatus.Ready_For_Refund;
        //                    }
        //                }

        //                request.context.SaveChanges();

        //                //Add follow up
        //                var follow = new FollowUpDTO
        //                {
        //                    FollowUpTypeId = (int)EnumFollowupType.Advertisement_Updated,
        //                    Title = request
        //                        .context.Status.FirstOrDefault(s => s.Id == Advertisement.StatusId)
        //                        .NameEN,
        //                    //Comment = request.context.CommonUser.FirstOrDefault(u => u.Id == request.AdsDTO.DeliveryManId).Name,
        //                    Comment = "Advertisement UnAssigned",
        //                    CreatedBy = request.UserID,
        //                    CreatedAt = DateTime.Now,
        //                    AdvertisementId = Advertisement.AdvertisementId,
        //                    StatusId = Advertisement.StatusId
        //                };
        //                FollowUpService.Add(follow, request.context);

        //                request.context.SaveChanges();

        //                response.Message = "Advertisement(s) successfully Unassigned";
        //                response.Success = true;
        //                response.StatusCode = HttpStatusCode.OK;
        //            }
        //            catch (Exception ex)
        //            {
        //                response.Message = ex.Message;
        //                response.Success = false;
        //                LogHelper.LogException(ex.Message, ex.StackTrace);
        //            }
        //            return response;
        //        }
        //    );
        //    return response;
        //}

        //public static AdvertisementResponse ReceiveReturns(AdvertisementRequest request)
        //{
        //    var response = new AdvertisementResponse();
        //    RunBase(
        //        request,
        //        response,
        //        (AdvertisementRequest req) =>
        //        {
        //            try
        //            {
        //                var ships = request.context.Advertisement.Where(r =>
        //                    request.AdsDTO.AdvertisementIDs.Contains(r.AdvertisementId)
        //                );
        //                foreach (var ship in ships)
        //                {
        //                    // *** Update Advertisement ***
        //                    if (ship.StatusId == (int)EnumStatus.Out_For_Delivery) // *** Out_For_Delivery
        //                    {
        //                        ship.StatusId = (int)EnumStatus.At_Warehouse;
        //                        ship.ReturnCount += 1;
        //                        ship.LastModifiedAt = DateTime.Now;
        //                        ship.LastModifiedBy = request.UserID;
        //                        ship.IsCourierReturned = true;

        //                        var shipItems = request.context.AdvertisementItem.Where(i =>
        //                            i.AdvertisementId == ship.AdvertisementId
        //                        );
        //                        foreach (var item in shipItems)
        //                        {
        //                            item.StatusId = (int)EnumStatus.At_Warehouse;
        //                            item.CourierQuantityReturned =
        //                                item.Quantity - item.CourierQuantityDelivered;

        //                            if (item.VendorProductId.HasValue) // Is Stock
        //                            {
        //                                var product = request.context.VendorProduct.FirstOrDefault(
        //                                    p => p.VendorProductId == item.VendorProductId
        //                                );
        //                                product.AvailableStock += item.CourierQuantityReturned;
        //                            }
        //                        }
        //                    }
        //                    else if (ship.StatusId == (int)EnumStatus.Delivered) // *** Delivered
        //                    {
        //                        ship.ReturnCount += 1;
        //                        ship.LastModifiedAt = DateTime.Now;
        //                        ship.LastModifiedBy = request.UserID;
        //                        ship.IsCourierReturned = true;

        //                        var shipItems = request.context.AdvertisementItem.Where(i =>
        //                            i.AdvertisementId == ship.AdvertisementId
        //                        );
        //                        foreach (var item in shipItems)
        //                        {
        //                            item.CourierQuantityReturned =
        //                                item.Quantity - item.CourierQuantityDelivered;

        //                            if (item.VendorProductId.HasValue) // Is Stock
        //                            {
        //                                var product = request.context.VendorProduct.FirstOrDefault(
        //                                    p => p.VendorProductId == item.VendorProductId
        //                                );
        //                                product.AvailableStock += item.CourierQuantityReturned;
        //                            }
        //                        }
        //                    }
        //                    else if (ship.StatusId == (int)EnumStatus.Cancelled) // *** Cancelled
        //                    {
        //                        ship.ReturnCount += 1;
        //                        ship.LastModifiedAt = DateTime.Now;
        //                        ship.LastModifiedBy = request.UserID;
        //                        ship.IsCourierReturned = true;

        //                        var shipItems = request.context.AdvertisementItem.Where(i =>
        //                            i.AdvertisementId == ship.AdvertisementId
        //                        );
        //                        foreach (var item in shipItems)
        //                        {
        //                            item.CourierQuantityReturned =
        //                                item.Quantity - item.CourierQuantityDelivered;

        //                            if (item.VendorProductId.HasValue) // Is Stock
        //                            {
        //                                var product = request.context.VendorProduct.FirstOrDefault(
        //                                    p => p.VendorProductId == item.VendorProductId
        //                                );
        //                                product.AvailableStock += item.CourierQuantityReturned;
        //                            }
        //                        }
        //                    }
        //                    else if (ship.StatusId == (int)EnumStatus.Refunded) // *** Refunded
        //                    {
        //                        var oldShip = request.context.Advertisement.FirstOrDefault(f =>
        //                            f.AdvertisementId == ship.ShipToRefundId
        //                        );

        //                        oldShip.StatusId = (int)EnumStatus.Ready_For_Return;
        //                        oldShip.ReturnCount += 1;
        //                        oldShip.LastModifiedAt = DateTime.Now;
        //                        oldShip.LastModifiedBy = request.UserID;
        //                        oldShip.IsCourierReturned = true;

        //                        ship.ReturnCount += 1;
        //                        ship.LastModifiedAt = DateTime.Now;
        //                        ship.LastModifiedBy = request.UserID;
        //                        ship.IsCourierReturned = true;

        //                        var oldShipItems = request.context.AdvertisementItem.Where(i =>
        //                            i.AdvertisementId == oldShip.AdvertisementId
        //                        );
        //                        foreach (var item in oldShipItems)
        //                        {
        //                            item.StatusId = (int)EnumStatus.Ready_For_Return;
        //                        }
        //                    }

        //                    //Add follow up
        //                    var follow = new FollowUpDTO
        //                    {
        //                        FollowUpTypeId = (int)EnumFollowupType.Advertisement_Updated,
        //                        Title = "Courier Return",
        //                        Comment = request
        //                            .context.CommonUser.FirstOrDefault(u =>
        //                                u.Id == ship.DeliveryManId
        //                            )
        //                            .Name,
        //                        CreatedBy = request.UserID,
        //                        AdvertisementId = ship.AdvertisementId,
        //                        StatusId = ship.StatusId
        //                    };
        //                    FollowUpService.Add(follow, request.context);
        //                }

        //                request.context.SaveChanges();

        //                response.Message = "Advertisement(s) successfully received";
        //                response.Success = true;
        //                response.StatusCode = HttpStatusCode.OK;
        //            }
        //            catch (Exception ex)
        //            {
        //                response.Message = ex.Message;
        //                response.Success = false;
        //                LogHelper.LogException(ex.Message, ex.StackTrace);
        //            }
        //            return response;
        //        }
        //    );
        //    return response;
        //}

        //public static AdvertisementResponse ReceiveCash(AdvertisementRequest request)
        //{
        //    var response = new AdvertisementResponse();
        //    RunBase(
        //        request,
        //        response,
        //        (AdvertisementRequest req) =>
        //        {
        //            try
        //            {
        //                var ships = request.context.Advertisement.Where(r =>
        //                    r.DeliveryManId == request.AdsDTO.DeliveryManId
        //                    && r.IsCashReceived == false
        //                    && (
        //                        r.StatusId == (int)EnumStatus.Delivered
        //                        || r.StatusId == (int)EnumStatus.Cancelled
        //                        || r.StatusId == (int)EnumStatus.Paid_To_Vendor
        //                    )
        //                );
        //                foreach (var ship in ships)
        //                {
        //                    ship.IsCashReceived = true;
        //                    //ship.DeliveryManId = null;
        //                    ship.LastModifiedAt = DateTime.Now;
        //                    ship.LastModifiedBy = request.UserID;

        //                    // *** Check if Advertisement is fully delivered
        //                    double CustomerPaidAmount = 0;
        //                    bool takeOriginalCOD = true;
        //                    if (
        //                        ship.StatusId == (int)EnumStatus.Delivered
        //                        || ship.StatusId == (int)EnumStatus.Paid_To_Vendor
        //                    )
        //                    {
        //                        var shipItems = request.context.AdvertisementItem.Where(i =>
        //                            i.AdvertisementId == ship.AdvertisementId
        //                        );
        //                        foreach (var item in shipItems)
        //                        {
        //                            if (
        //                                (
        //                                    item.StatusId != (int)EnumStatus.Delivered
        //                                    && item.StatusId != (int)EnumStatus.Paid_To_Vendor
        //                                )
        //                                || item.CourierQuantityDelivered < item.Quantity
        //                            )
        //                                takeOriginalCOD = false;

        //                            CustomerPaidAmount += (
        //                                item.Price * item.CourierQuantityDelivered
        //                            );
        //                        }
        //                    }

        //                    #region Add Cash Transactions / Update Accounts Balances

        //                    var CourierAccountId = request
        //                        .context.Account.FirstOrDefault(a =>
        //                            a.UserId == request.AdsDTO.DeliveryManId
        //                        )
        //                        .AccountId;
        //                    var VendorAccountId = request
        //                        .context.Account.FirstOrDefault(a => a.UserId == ship.VendorId)
        //                        .AccountId;

        //                    #region Main Treasury Transactions
        //                    var TreasuryTrans = new AccountTransactionDTO();
        //                    TreasuryTrans.CreatedBy = request.UserID;
        //                    TreasuryTrans.TypeId = (int)EnumTransactionType.Deposit;
        //                    TreasuryTrans.SenderId = CourierAccountId;
        //                    TreasuryTrans.ReceiverId = (int)EnumAccountId.Main_Treasury;
        //                    TreasuryTrans.VendorId = VendorAccountId;
        //                    TreasuryTrans.AdvertisementId = ship.AdvertisementId;
        //                    TreasuryTrans.StatusId = ship.StatusId;

        //                    if (
        //                        ship.StatusId == (int)EnumStatus.Delivered
        //                        || ship.StatusId == (int)EnumStatus.Paid_To_Vendor
        //                    )
        //                    {
        //                        TreasuryTrans.ShippingFees = ship.ShippingFees;
        //                        TreasuryTrans.GameFees = ship.GameFees;
        //                        TreasuryTrans.WeightFees = ship.WeightFees;
        //                        TreasuryTrans.SizeFees = ship.SizeFees;
        //                        TreasuryTrans.PartialDeliveryFees = ship.PartialDeliveryFees;
        //                        TreasuryTrans.VendorCash = takeOriginalCOD
        //                            ? ship.VendorCash
        //                            : (
        //                                CustomerPaidAmount
        //                                - ship.ShippingFees
        //                                - ship.GameFees
        //                                - ship.WeightFees
        //                                - ship.SizeFees
        //                                - ship.PartialDeliveryFees
        //                            );
        //                        TreasuryTrans.Total =
        //                            TreasuryTrans.VendorCash
        //                            + ship.ShippingFees
        //                            + ship.GameFees
        //                            + ship.WeightFees
        //                            + ship.SizeFees
        //                            + ship.PartialDeliveryFees;

        //                        AccountTransactionService.AddTransaction(
        //                            request.context,
        //                            TreasuryTrans
        //                        );
        //                    }
        //                    else if (ship.StatusId == (int)EnumStatus.Cancelled)
        //                    {
        //                        if (ship.ShippingFeesPaid > 0)
        //                        {
        //                            TreasuryTrans.ShippingFeesPaid = ship.ShippingFeesPaid;
        //                            TreasuryTrans.Total = ship.ShippingFeesPaid;

        //                            AccountTransactionService.AddTransaction(
        //                                request.context,
        //                                TreasuryTrans
        //                            );
        //                        }
        //                    }
        //                    #endregion

        //                    #endregion

        //                    //Add follow up
        //                    var follow = new FollowUpDTO
        //                    {
        //                        FollowUpTypeId = (int)EnumFollowupType.Advertisement_Updated,
        //                        Title = "Cash Received",
        //                        Comment = request
        //                            .context.CommonUser.FirstOrDefault(u => u.Id == request.UserID)
        //                            .Name,
        //                        CreatedBy = request.UserID,
        //                        AdvertisementId = ship.AdvertisementId,
        //                        StatusId = ship.StatusId
        //                    };
        //                    FollowUpService.Add(follow, request.context);
        //                }

        //                request.context.SaveChanges();

        //                response.Message = "Cash successfully received";
        //                response.Success = true;
        //                response.StatusCode = HttpStatusCode.OK;
        //            }
        //            catch (Exception ex)
        //            {
        //                response.Message = ex.Message;
        //                response.Success = false;
        //                LogHelper.LogException(ex.Message, ex.StackTrace);
        //            }
        //            return response;
        //        }
        //    );
        //    return response;
        //}

        #endregion

        #region Advertisement Select Queries

        public static AdvertisementResponse GetAdvertisement(AdvertisementRequest request)
        {
            var response = new AdvertisementResponse();
            RunBase(
                request,
                response,
                (AdvertisementRequest req) =>
                {
                    try
                    {
                        IQueryable<Advertisement> ship;
                        if (string.IsNullOrEmpty(request.AdsDTO.RefId))
                            ship = request.context.Advertisement.Where(s =>
                                s.AdvertisementId == request.AdsDTO.AdvertisementId && !s.IsDeleted
                            );
                        else
                            ship = request.context.Advertisement.Where(s =>
                                s.RefId == request.AdsDTO.RefId && !s.IsDeleted
                            );

                        var query = ship.Select(s => new AdsDTO
                        {
                            AdvertisementId = s.AdvertisementId,
                            AdvertisementPhotos = s.AdvertisementPhotos.Select(p => p.Url).ToList(),
                            Description = s.Description,
                            UserName = s.UserName,
                            Password = s.Password,
                            IsDeleted = s.IsDeleted,
                            CreatedAt = s.CreatedAt,
                            CreatedBy = s.CreatedBy,
                            Price = s.Price,
                            Level = s.Level,
                            Rank = s.Rank,
                            RefId = s.RefId,
                            GameId = s.GameId,
                            BuyerId = s.BuyerId,
                            StatusId = s.StatusId,
                            GamerId = s.GamerId,
                            LastModifiedAt = s.LastModifiedAt,
                            LastModifiedBy = s.LastModifiedBy,
                            Status = new StatusDTO
                            {
                                Id = s.Status.Id,
                                StatusType = s.Status.StatusType,
                                NameAR = s.Status.NameAR,
                                NameEN = s.Game.NameEn
                            },
                            FollowUp = s
                                .FollowUp.Select(f => new FollowUpDTO
                                {
                                    Comment = f.Comment,
                                    CreatedAt = f.CreatedAt,
                                    Title = f.Title,
                                    CreatedBy = f.CreatedBy,
                                    CreatedByName = f.CreatedByNavigation.Name,
                                })
                                .ToList(),
                            Game = new GameDTO
                            {
                                Id = s.Game.Id,
                                ImgUrl = s.Game.ImgUrl,
                                NameAr = s.Game.NameAr,
                                NameEn = s.Game.NameEn,
                                CategoryName = s.Game.Category.NameEn,
                            },
                            Buyer =
                                s.Buyer != null
                                    ? new UserDTO
                                    {
                                        Id = s.Buyer.Id,
                                        ImgUrl = s.Buyer.ImgUrl,
                                        Name = s.Buyer.Name,
                                    }
                                    : null,
                            Gamer = new UserDTO
                            {
                                Id = s.Gamer.Id,
                                ImgUrl = s.Gamer.ImgUrl,
                                Name = s.Gamer.Name,
                            },
                        });
                        response.AdsDTO = query.FirstOrDefault();
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

        public static AdvertisementResponse GetAllAdvertisements(AdvertisementRequest request)
        {
            var response = new AdvertisementResponse();
            RunBase(
                request,
                response,
                (AdvertisementRequest req) =>
                {
                    try
                    {
                        IQueryable<Advertisement> ship;
                        if (
                            request.AdsDTO.AdvertisementIds != null
                            && request.AdsDTO.AdvertisementIds.Any()
                        )
                            ship = request.context.Advertisement.Where(s =>
                                request.AdsDTO.AdvertisementIds.Contains(s.AdvertisementId)
                                && !s.IsDeleted
                            );
                        else if (!string.IsNullOrEmpty(request.AdsDTO.RefId))
                            ship = request.context.Advertisement.Where(s =>
                                s.AdvertisementId == request.AdsDTO.AdvertisementId && !s.IsDeleted
                            );
                        else
                            ship = request.context.Advertisement.Where(s => !s.IsDeleted);

                        if (request.PageSize > 0)
                            ship = ApplyPaging(ship, request.PageSize, request.PageIndex);

                        if (request.applyFilter)
                            ship = ApplyFilter(ship, request.AdsDTO, req.RoleID, req.UserID);

                        if (!string.IsNullOrEmpty(request.OrderByColumn))
                            ship = OrderByDynamic(ship, request.OrderByColumn, request.IsDesc);

                        var query = ship.Select(s => new AdsDTO
                        {
                            AdvertisementId = s.AdvertisementId,
                            AdvertisementPhotos = s.AdvertisementPhotos.Select(p => p.Url).ToList(),
                            Description = s.Description,
                            UserName = s.UserName,
                            Password = s.Password,
                            IsDeleted = s.IsDeleted,
                            CreatedAt = s.CreatedAt,
                            CreatedBy = s.CreatedBy,
                            Level = s.Level,
                            Rank = s.Rank,
                            Price = s.Price,
                            RefId = s.RefId,
                            GameId = s.GameId,
                            BuyerId = s.BuyerId,
                            StatusId = s.StatusId,
                            GamerId = s.GamerId,
                            LastModifiedAt = s.LastModifiedAt,
                            LastModifiedBy = s.LastModifiedBy,
                            Status = new StatusDTO
                            {
                                Id = s.Status.Id,
                                StatusType = s.Status.StatusType,
                                NameAR = s.Status.NameAR,
                                NameEN = s.Status.NameEN
                            },
                            FollowUp = s
                                .FollowUp.Select(f => new FollowUpDTO
                                {
                                    Comment = f.Comment,
                                    CreatedAt = f.CreatedAt,
                                    Title = f.Title,
                                    CreatedBy = f.CreatedBy,
                                    CreatedByName = f.CreatedByNavigation.Name,
                                })
                                .ToList(),
                            Game = new GameDTO
                            {
                                Id = s.Game.Id,
                                ImgUrl = s.Game.ImgUrl,
                                NameAr = s.Game.NameAr,
                                NameEn = s.Game.NameEn,
                                CategoryName = s.Game.Category.NameEn,
                            },
                            Buyer =
                                s.Buyer != null
                                    ? new UserDTO
                                    {
                                        Id = s.Buyer.Id,
                                        ImgUrl = s.Buyer.ImgUrl,
                                        Name = s.Buyer.Name,
                                    }
                                    : null,
                            Gamer = new UserDTO
                            {
                                Id = s.Gamer.Id,
                                ImgUrl = s.Gamer.ImgUrl,
                                Name = s.Gamer.Name,
                            },
                        });
                        response.AdsDTOs = query.ToList();
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

        #endregion

        #region Filters

        private static IQueryable<Advertisement> ApplyFilter(
            IQueryable<Advertisement> query,
            AdsDTO filter,
            int RoleId,
            int UserId
        )
        {
            if (!string.IsNullOrEmpty(filter.Search))
            {
                filter.Search = filter.Search.ToLower();
                query = query.Where(c =>
                    c.RefId.ToLower().Contains(filter.Search)
                    || c.UserName.ToLower().Contains(filter.Search)
                    || c.Description.Contains(filter.Search)
                    || c.Password.Contains(filter.Search)
                    || c.Rank.Contains(filter.Search)
                );
            }

            if (filter.AdvertisementId > 0)
                query = query.Where(c => c.AdvertisementId == filter.AdvertisementId);

            if (!string.IsNullOrEmpty(filter.RefId))
                query = query.Where(c => c.RefId == filter.RefId);

            if (filter.AdvertisementId != 0)
                query = query.Where(s => filter.AdvertisementId == s.AdvertisementId);

            if (filter.StatusType != 0)
                query = query.Where(p => p.Status.StatusType == filter.StatusType);

            if (filter.GamerId != 0)
                query = query.Where(c => c.GamerId == filter.GamerId);

            if (filter.GameId != 0)
                query = query.Where(c => c.GameId == filter.GameId);

            if (filter.BuyerId != 0)
                query = query.Where(c => c.BuyerId == filter.BuyerId);

            if (filter.Price != 0)
                query = query.Where(c => c.Price > filter.Price);

            if (filter.LessLevel != 0)
                query = query.Where(c => c.Level < filter.LessLevel);

            if (filter.GreeterLevel != 0)
                query = query.Where(c => c.Level > filter.GreeterLevel);

            if (filter.LessPrice != 0)
                query = query.Where(c => c.Price < filter.LessPrice);

            if (filter.GreeterPrice != 0)
                query = query.Where(c => c.Price > filter.GreeterPrice);

            if (filter.DateFrom.HasValue)
                query = query.Where(p => p.CreatedAt.Date >= filter.DateFrom);

            if (filter.DateTo.HasValue)
                query = query.Where(p => p.CreatedAt.Date <= filter.DateTo);

            return query;
        }

        #endregion

        #region Helper Methods

        public static List<int> GetStatusGroup(int groupType)
        {
            var result = new List<int>();

            switch (groupType)
            {
                case (int)EnumStatus.All:
                    result = new List<int>
                    {
                        (int)EnumStatus.Ready_For_Delivery,
                        (int)EnumStatus.Assigned_For_Delivery,
                        (int)EnumStatus.Out_For_Delivery,
                        (int)EnumStatus.Ready_For_Pickup,
                        (int)EnumStatus.Assigned_For_Pickup,
                        (int)EnumStatus.Picked_Up,
                        (int)EnumStatus.At_Warehouse,
                        (int)EnumStatus.Postponed,
                        (int)EnumStatus.Ready_For_Return,
                        (int)EnumStatus.Assigned_For_Return,
                        (int)EnumStatus.Out_For_Return,
                        (int)EnumStatus.Ready_For_Refund,
                        (int)EnumStatus.Assigned_For_Refund,
                        (int)EnumStatus.Out_For_Refund,
                        (int)EnumStatus.Delivered,
                        (int)EnumStatus.Paid_To_Vendor,
                        (int)EnumStatus.Refunded,
                        (int)EnumStatus.Returned,
                        (int)EnumStatus.Cancelled,
                        (int)EnumStatus.Cancelled_Pickup,
                        (int)EnumStatus.Cancelled_Refund,
                        (int)EnumStatus.Not_Delivered,
                        (int)EnumStatus.Not_Received,
                        (int)EnumStatus.Archive
                    };
                    break;
                case (int)EnumStatus.Current:
                    result = new List<int>
                    {
                        (int)EnumStatus.Ready_For_Delivery,
                        (int)EnumStatus.Assigned_For_Delivery,
                        (int)EnumStatus.Out_For_Delivery,
                        (int)EnumStatus.Ready_For_Pickup,
                        (int)EnumStatus.Assigned_For_Pickup,
                        (int)EnumStatus.Picked_Up,
                        (int)EnumStatus.At_Warehouse,
                        (int)EnumStatus.Postponed,
                        (int)EnumStatus.Ready_For_Return,
                        (int)EnumStatus.Assigned_For_Return,
                        (int)EnumStatus.Out_For_Return,
                        (int)EnumStatus.Ready_For_Refund,
                        (int)EnumStatus.Assigned_For_Refund,
                        (int)EnumStatus.Out_For_Refund
                    };
                    break;
                case (int)EnumStatus.Done:
                    result = new List<int>
                    {
                        (int)EnumStatus.Delivered,
                        (int)EnumStatus.Paid_To_Vendor,
                        (int)EnumStatus.Refunded,
                        (int)EnumStatus.Returned
                    };
                    break;
                case (int)EnumStatus.Cancelled:
                    result = new List<int>
                    {
                        (int)EnumStatus.Cancelled,
                        (int)EnumStatus.Cancelled_Pickup,
                        (int)EnumStatus.Cancelled_Refund,
                        (int)EnumStatus.Not_Delivered,
                        (int)EnumStatus.Not_Received,
                        (int)EnumStatus.Archive
                    };
                    break;
            }

            return result;
        }

        public static bool CanReturnToVendor(Advertisement ship, List<FollowUp> lstShipFollowup)
        {
            var result = true;
            var canNotReturnStatus = new List<int>
            {
                (int)EnumStatus.Ready_For_Return,
                (int)EnumStatus.Assigned_For_Return,
                (int)EnumStatus.Out_For_Return,
                (int)EnumStatus.Returned,
                (int)EnumStatus.Ready_For_Refund,
                (int)EnumStatus.Assigned_For_Refund,
                (int)EnumStatus.Out_For_Refund,
                (int)EnumStatus.Refunded
            };

            #region check can return
            if (canNotReturnStatus.Contains(ship.StatusId))
                result = false;

            if (lstShipFollowup.Any(f => f.StatusId == (int)EnumStatus.Returned))
                result = false;

            #endregion

            return result;
        }

        #endregion
    }
}

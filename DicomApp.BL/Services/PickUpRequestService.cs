using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using DicomApp.CommonDefinitions.DTO;
using DicomApp.CommonDefinitions.DTO.CashDTOs;
using DicomApp.CommonDefinitions.DTO.PickupDTOs;
using DicomApp.CommonDefinitions.Requests;
using DicomApp.CommonDefinitions.Responses;
using DicomApp.DAL.DB;
using DicomApp.Helpers;

namespace DicomApp.BL.Services
{
    public class PickUpRequestService : BaseService
    {
        //public static PickUpRequestResponse AddPickUpRequest(PickUpRequestRequest request)
        //{
        //    var response = new PickUpRequestResponse();
        //    RunBase(
        //        request,
        //        response,
        //        (PickUpRequestRequest req) =>
        //        {
        //            try
        //            {
        //                var newPickupRequest = new PickupRequest
        //                {
        //                    PickupRequestTypeId = request.PickupDTO.PickupRequestTypeId,
        //                    PickupFees = request.PickupDTO.PickupFees,
        //                    StatusId = (int)EnumStatus.Ready_For_Pickup,
        //                    CreatedAt = DateTime.Now,
        //                    CreatedBy = request.UserID,
        //                    IsDeleted = false,
        //                    VendorId = request.PickupDTO.VendorId,
        //                    VendorName = request.PickupDTO.VendorName,
        //                    VendorPhone = request.PickupDTO.VendorPhone,
        //                    VendorAddress = request.PickupDTO.VendorAddress,
        //                    VendorLocation = request.PickupDTO.VendorLocation,
        //                    VendorLandmark = request.PickupDTO.VendorLandmark,
        //                    VendorFloor = request.PickupDTO.VendorFloor,
        //                    VendorApartment = request.PickupDTO.VendorApartment,
        //                    ZoneId = request.PickupDTO.ZoneId,
        //                    AreaId = request.PickupDTO.AreaId,
        //                    PickupDate = request.PickupDTO.PickupDate,
        //                    TimeFrom = request.PickupDTO.TimeFrom,
        //                    TimeTo = request.PickupDTO.TimeTo,
        //                    Notes = request.PickupDTO.Notes,
        //                };

        //                request.context.PickupRequest.Add(newPickupRequest);
        //                request.context.SaveChanges();

        //                // Save stock pickup items
        //                if (
        //                    request.PickupDTO.PickupRequestTypeId
        //                    == (int)EnumPickupRequestType.StockPickup
        //                )
        //                {
        //                    newPickupRequest.RefId = BaseHelper.GenerateRefId(
        //                        EnumRefIdType.Stock_Pickup,
        //                        newPickupRequest.PickupRequestId
        //                    );
        //                    newPickupRequest.PickupFees = 0; // No fees for stock pickup

        //                    foreach (var item in request.PickupDTO.PickupItems)
        //                    {
        //                        request.context.PickupRequestItem.Add(
        //                            new PickupRequestItem()
        //                            {
        //                                PickupRequestId = newPickupRequest.PickupRequestId,
        //                                VendorProductId = item.VendorProductId,
        //                                Quantity = item.Quantity,
        //                                Price = item.Price,
        //                                CreatedAt = DateTime.Now,
        //                                CreatedBy = request.UserID,
        //                                IsDeleted = false,
        //                                StatusId = (int)EnumStatus.Ready_For_Pickup
        //                            }
        //                        );
        //                    }
        //                }
        //                else // Save delivery pickup Advertisements
        //                {
        //                    newPickupRequest.RefId = BaseHelper.GenerateRefId(
        //                        EnumRefIdType.Delivery_Pickup,
        //                        newPickupRequest.PickupRequestId
        //                    );
        //                    newPickupRequest.PickupFees = BaseHelper.CalcPickupFees(
        //                        request.PickupDTO.PickupItems.Count()
        //                    );

        //                    foreach (var item in request.PickupDTO.PickupItems)
        //                    {
        //                        request.context.PickupRequestItem.Add(
        //                            new PickupRequestItem()
        //                            {
        //                                PickupRequestId = newPickupRequest.PickupRequestId,
        //                                AdvertisementId = item.AdvertisementId,
        //                                CreatedAt = DateTime.Now,
        //                                CreatedBy = request.UserID,
        //                                IsDeleted = false,
        //                                StatusId = (int)EnumStatus.Ready_For_Pickup
        //                            }
        //                        );
        //                        request
        //                            .context.Advertisement.FirstOrDefault(s =>
        //                                s.AdvertisementId == item.AdvertisementId
        //                            )
        //                            .AdvertisementRequestId =
        //                            newPickupRequest.AdvertisementRequestId;
        //                    }
        //                }

        //                // Include return Advertisements with pickup
        //                if (request.PickupDTO.IncludeReturns)
        //                {
        //                    var ships = request.context.Advertisement.Where(s =>
        //                        s.StatusId == (int)EnumStatus.Ready_For_Return
        //                        && s.AdvertisementRequestId == null
        //                    );
        //                    foreach (var ship in ships)
        //                    {
        //                        request.context.PickupRequestItem.Add(
        //                            new PickupRequestItem()
        //                            {
        //                                PickupRequestId = newPickupRequest.PickupRequestId,
        //                                AdvertisementId = ship.AdvertisementId,
        //                                CreatedAt = DateTime.Now,
        //                                CreatedBy = request.UserID,
        //                                IsDeleted = false,
        //                                StatusId = (int)EnumStatus.Ready_For_Return
        //                            }
        //                        );
        //                        ship.AdvertisementRequestId = newPickupRequest.PickupRequestId;
        //                    }
        //                }

        //                //Add notification
        //                request.context.Notification.Add(
        //                    new Notification
        //                    {
        //                        Body = "New Pickup Request " + newPickupRequest.RefId + " Added",
        //                        CreationDate = DateTime.Now,
        //                        Icon = newPickupRequest.RefId,
        //                        SenderId = request.UserID,
        //                        Title = "New Pickup Request",
        //                        RecipientRoleId = (int)EnumRole.BranchManger,
        //                        IsDeleted = false,
        //                        IsSeen = false,
        //                        RecipientId = 0,
        //                    }
        //                );

        //                request.context.SaveChanges();

        //                response.Message =
        //                    "New Pickup Request " + newPickupRequest.RefId + " Added";
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

        //public static PickUpRequestResponse EditPickUpRequest(PickUpRequestRequest request)
        //{
        //    var response = new PickUpRequestResponse();
        //    RunBase(
        //        request,
        //        response,
        //        (PickUpRequestRequest req) =>
        //        {
        //            try
        //            {
        //                var model = request.context.PickupRequest.Find(
        //                    request.PickupDTO.PickupRequestId
        //                );
        //                if (model != null)
        //                {
        //                    model.VendorAddress = request.PickupDTO.VendorAddress;
        //                    model.ZoneId = request
        //                        .context.City.FirstOrDefault(c => c.Id == request.PickupDTO.AreaId)
        //                        .ZoneId.Value;
        //                    model.AreaId = request.PickupDTO.AreaId;
        //                    model.PickupDate = request.PickupDTO.PickupDate;
        //                    model.TimeFrom = request.PickupDTO.TimeFrom;
        //                    model.TimeTo = request.PickupDTO.TimeTo;

        //                    //Add notification
        //                    request.context.Notification.Add(
        //                        new Notification
        //                        {
        //                            Body = "Pickup request " + request.PickupDTO.RefId + " updated",
        //                            CreationDate = DateTime.Now,
        //                            Icon = request.PickupDTO.RefId,
        //                            SenderId = request.UserID,
        //                            Title = "Pickup request updated",
        //                            RecipientRoleId = (int)EnumRole.BranchManger,
        //                            IsDeleted = false,
        //                            IsSeen = false,
        //                            RecipientId = 0,
        //                        }
        //                    );

        //                    request.context.SaveChanges();

        //                    response.Message =
        //                        "Pickup request " + request.PickupDTO.RefId + " updated";
        //                    response.Success = true;
        //                    response.StatusCode = HttpStatusCode.OK;
        //                }
        //                else
        //                {
        //                    response.Message = "Invalid";
        //                    response.Success = false;
        //                }
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

        //public static PickUpRequestResponse CancelPickupRequest(PickUpRequestRequest request)
        //{
        //    var response = new PickUpRequestResponse();
        //    RunBase(
        //        request,
        //        response,
        //        (PickUpRequestRequest req) =>
        //        {
        //            try
        //            {
        //                var pickup = request.context.PickupRequest.FirstOrDefault(r =>
        //                    r.PickupRequestId == request.PickupDTO.PickupRequestId
        //                );

        //                pickup.StatusId = (int)EnumStatus.Cancelled_Pickup;

        //                var pickItems = request.context.PickupRequestItem.Where(i =>
        //                    i.PickupRequestId == pickup.PickupRequestId
        //                );
        //                foreach (var item in pickItems)
        //                {
        //                    if (
        //                        item.StatusId == (int)EnumStatus.Ready_For_Return
        //                        || item.StatusId == (int)EnumStatus.Assigned_For_Return
        //                        || item.StatusId == (int)EnumStatus.Out_For_Return
        //                    )
        //                        item.StatusId = (int)EnumStatus.Ready_For_Return;
        //                    else
        //                        item.StatusId = (int)EnumStatus.Cancelled_Pickup;
        //                }

        //                var Advertisements = request.context.Advertisement.Where(s =>
        //                    s.AdvertisementRequestId == pickup.AdvertisementRequestId
        //                );
        //                foreach (var ship in Advertisements)
        //                {
        //                    if (
        //                        ship.StatusId == (int)EnumStatus.Ready_For_Return
        //                        || ship.StatusId == (int)EnumStatus.Assigned_For_Return
        //                        || ship.StatusId == (int)EnumStatus.Out_For_Return
        //                    )
        //                        ship.StatusId = (int)EnumStatus.Ready_For_Return;
        //                    else
        //                        ship.StatusId = (int)EnumStatus.Cancelled_Pickup;

        //                    //Add follow up
        //                    var follow = new FollowUpDTO
        //                    {
        //                        FollowUpTypeId = (int)EnumFollowupType.Advertisement_Updated,
        //                        Title = request
        //                            .context.Status.FirstOrDefault(s =>
        //                                s.Id == (int)EnumStatus.Cancelled_Pickup
        //                            )
        //                            .NameEN,
        //                        Comment = "Pickup request cancelled",
        //                        CreatedBy = request.UserID,
        //                        AdvertisementId = ship.AdvertisementId,
        //                        StatusId = (int)EnumStatus.Cancelled_Pickup
        //                    };
        //                    FollowUpService.Add(follow, request.context);
        //                }

        //                request.context.SaveChanges();

        //                response.Message = "Pickup request cancelled";
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

        //public static PickUpRequestResponse WarehouseReceive(PickUpRequestRequest request)
        //{
        //    var response = new PickUpRequestResponse();
        //    RunBase(
        //        request,
        //        response,
        //        (PickUpRequestRequest req) =>
        //        {
        //            try
        //            {
        //                var pickups = request.context.PickupRequest.Where(r =>
        //                    request.PickupDTO.PickupIDs.Contains(r.PickupRequestId)
        //                );

        //                foreach (var pickup in pickups)
        //                {
        //                    pickup.StatusId = request.PickupDTO.StatusId; //(int)EnumStatus.At_Warehouse

        //                    // ** Update Pickup request items
        //                    IQueryable<PickupRequestItem> pickupItems;
        //                    if (request.PickupDTO.PickupItemsIDs != null)
        //                        pickupItems = request.context.PickupRequestItem.Where(r =>
        //                            request.PickupDTO.PickupItemsIDs.Contains(r.PickupRequestItemId)
        //                        );
        //                    else
        //                        pickupItems = request.context.PickupRequestItem.Where(i =>
        //                            i.PickupRequestId == pickup.PickupRequestId
        //                        );

        //                    foreach (var pickupItem in pickupItems)
        //                    {
        //                        if (pickupItem.StatusId == (int)EnumStatus.Picked_Up)
        //                            pickupItem.StatusId = (int)EnumStatus.At_Warehouse;
        //                        else
        //                            pickupItem.StatusId = (int)EnumStatus.Not_Received;

        //                        if (pickupItem.AdvertisementId != 0) // Advertisement
        //                        {
        //                            var ship = request.context.Advertisement.FirstOrDefault(r =>
        //                                r.AdvertisementId == pickupItem.AdvertisementId
        //                            );

        //                            if (ship.StatusId == (int)EnumStatus.Picked_Up)
        //                                ship.StatusId = (int)EnumStatus.At_Warehouse;
        //                            else
        //                                ship.StatusId = (int)EnumStatus.Not_Received;

        //                            ship.LastModifiedAt = DateTime.Now;
        //                            ship.LastModifiedBy = request.UserID;

        //                            //Add follow up
        //                            var follow = new FollowUpDTO
        //                            {
        //                                FollowUpTypeId = (int)
        //                                    EnumFollowupType.Advertisement_Updated,
        //                                Title = request
        //                                    .context.Status.FirstOrDefault(s =>
        //                                        s.Id == ship.StatusId
        //                                    )
        //                                    .NameEN,
        //                                Comment = request
        //                                    .context.CommonUser.FirstOrDefault(u =>
        //                                        u.Id == request.UserID
        //                                    )
        //                                    .Name,
        //                                CreatedBy = request.UserID,
        //                                AdvertisementId = ship.AdvertisementId,
        //                                StatusId = ship.StatusId
        //                            };
        //                            FollowUpService.Add(follow, request.context);
        //                        }
        //                        else // Stock Product
        //                        {
        //                            if (
        //                                request.PickupDTO.StatusId == (int)EnumStatus.At_Warehouse
        //                                && pickupItem.StatusId == (int)EnumStatus.At_Warehouse
        //                            )
        //                            {
        //                                var product = request.context.VendorProduct.FirstOrDefault(
        //                                    p => p.VendorProductId == pickupItem.VendorProductId
        //                                );

        //                                // Update available stock
        //                                product.AvailableStock += pickupItem.Quantity.Value;
        //                                product.StockPrice = pickupItem.Price.Value;
        //                            }
        //                        }
        //                    }
        //                }

        //                request.context.SaveChanges();

        //                response.Message = "Pickup request(s) successfully received";
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

        //public static PickUpRequestResponse CourierReceive(PickUpRequestRequest request)
        //{
        //    var response = new PickUpRequestResponse();
        //    RunBase(
        //        request,
        //        response,
        //        (PickUpRequestRequest req) =>
        //        {
        //            try
        //            {
        //                var pickup = request.context.PickupRequest.FirstOrDefault(r =>
        //                    r.PickupRequestId == request.PickupDTO.PickupRequestId
        //                );

        //                int PickedUpCount = 0;
        //                int VendorId = pickup.VendorId;
        //                int PickupRequestId = pickup.PickupRequestId;

        //                pickup.StatusId = request.PickupDTO.StatusId; // (int)EnumStatus.Picked_Up

        //                // ** Update Pickup request items
        //                IQueryable<PickupRequestItem> pickupItems;
        //                if (request.PickupDTO.PickupItemsIDs != null)
        //                    pickupItems = request.context.PickupRequestItem.Where(r =>
        //                        request.PickupDTO.PickupItemsIDs.Contains(r.PickupRequestItemId)
        //                    );
        //                else
        //                    pickupItems = request.context.PickupRequestItem.Where(i =>
        //                        i.PickupRequestId == pickup.PickupRequestId
        //                    );

        //                foreach (var pickupItem in pickupItems)
        //                {
        //                    if (
        //                        pickupItem.StatusId == (int)EnumStatus.Ready_For_Return
        //                        || pickupItem.StatusId == (int)EnumStatus.Assigned_For_Return
        //                        || pickupItem.StatusId == (int)EnumStatus.Out_For_Return
        //                    )
        //                        pickupItem.StatusId = (int)EnumStatus.Returned;
        //                    else
        //                    {
        //                        pickupItem.StatusId = (int)EnumStatus.Picked_Up;
        //                        PickedUpCount++;
        //                    }

        //                    if (pickupItem.AdvertisementId != 0) // Advertisement
        //                    {
        //                        var ship = request.context.Advertisement.FirstOrDefault(r =>
        //                            r.AdvertisementId == pickupItem.AdvertisementId
        //                        );

        //                        if (
        //                            ship.StatusId == (int)EnumStatus.Ready_For_Return
        //                            || ship.StatusId == (int)EnumStatus.Assigned_For_Return
        //                            || ship.StatusId == (int)EnumStatus.Out_For_Return
        //                        )
        //                            ship.StatusId = (int)EnumStatus.Returned;
        //                        else
        //                            ship.StatusId = (int)EnumStatus.Picked_Up;

        //                        //Update Advertisement items
        //                        var shipItems = request.context.AdvertisementItem.Where(i =>
        //                            i.AdvertisementId == ship.AdvertisementId
        //                        );
        //                        foreach (var item in shipItems)
        //                        {
        //                            if (
        //                                item.StatusId == (int)EnumStatus.Ready_For_Return
        //                                || item.StatusId == (int)EnumStatus.Assigned_For_Return
        //                                || item.StatusId == (int)EnumStatus.Out_For_Return
        //                            )
        //                                item.StatusId = (int)EnumStatus.Returned;
        //                            else
        //                                item.StatusId = (int)EnumStatus.Picked_Up;
        //                        }

        //                        ship.LastModifiedAt = DateTime.Now;
        //                        ship.LastModifiedBy = request.UserID;

        //                        //Add follow up
        //                        var follow = new FollowUpDTO
        //                        {
        //                            FollowUpTypeId = (int)EnumFollowupType.Advertisement_Updated,
        //                            Title = request
        //                                .context.Status.FirstOrDefault(s => s.Id == ship.StatusId)
        //                                .NameEN,
        //                            Comment = request
        //                                .context.CommonUser.FirstOrDefault(u =>
        //                                    u.Id == request.UserID
        //                                )
        //                                .Name,
        //                            CreatedBy = request.UserID,
        //                            AdvertisementId = ship.AdvertisementId,
        //                            StatusId = ship.StatusId
        //                        };
        //                        FollowUpService.Add(follow, request.context);
        //                    }
        //                }

        //                #region Add pickup fees
        //                if (
        //                    pickup.PickupRequestTypeId == (int)EnumPickupRequestType.DeliveryPickup
        //                    && PickedUpCount < BaseHelper.Constants.MinFreeShipCount
        //                )
        //                {
        //                    var VendorAccountId = request
        //                        .context.Account.FirstOrDefault(a => a.UserId == VendorId)
        //                        .AccountId;

        //                    #region Vendor Transactions
        //                    var VendorTrans = new AccountTransactionDTO
        //                    {
        //                        CreatedBy = request.UserID,
        //                        ReceiverId = VendorAccountId,
        //                        VendorId = VendorAccountId,
        //                        PickupRequestId = PickupRequestId,
        //                        StatusId = (int)EnumStatus.Picked_Up,

        //                        PickupFees = BaseHelper.Constants.PickupFees,
        //                        Total = BaseHelper.Constants.PickupFees,

        //                        TypeId = (int)EnumTransactionType.Withdraw,
        //                    };
        //                    AccountTransactionService.AddTransaction(request.context, VendorTrans);
        //                    #endregion

        //                    #region RED Transactions
        //                    var REDTrans = new AccountTransactionDTO
        //                    {
        //                        CreatedBy = request.UserID,
        //                        ReceiverId = (int)EnumAccountId.RED_Main_Account,
        //                        VendorId = VendorAccountId,
        //                        PickupRequestId = PickupRequestId,
        //                        StatusId = (int)EnumStatus.Picked_Up,

        //                        PickupFees = BaseHelper.Constants.PickupFees,
        //                        Total = BaseHelper.Constants.PickupFees,

        //                        TypeId = (int)EnumTransactionType.Deposit,
        //                    };

        //                    AccountTransactionService.AddTransaction(request.context, REDTrans);
        //                    #endregion
        //                }

        //                #endregion

        //                request.context.SaveChanges();

        //                response.Message = "Pickup request(s) successfully picked up";
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

        //public static PickUpRequestResponse GetAllPickUpRequests(PickUpRequestRequest request)
        //{
        //    var res = new PickUpRequestResponse();
        //    RunBase(
        //        request,
        //        res,
        //        (PickUpRequestRequest req) =>
        //        {
        //            try
        //            {
        //                var query = request.context.PickupRequest.Select(p => new PickupDTO
        //                {
        //                    PickupRequestId = p.PickupRequestId,
        //                    PickupRequestTypeId = p.PickupRequestTypeId,
        //                    DeliveryManId = p.DeliveryManId,
        //                    DeliveryManName = p.DeliveryManId.HasValue ? p.DeliveryMan.Name : null,
        //                    PickupFees = p.PickupFees,
        //                    StatusId = p.StatusId,
        //                    StatusName = p.Status.NameEN,
        //                    CreatedAt = p.CreatedAt,
        //                    CreatedBy = p.CreatedBy,
        //                    IsDeleted = p.IsDeleted,
        //                    VendorId = p.VendorId,
        //                    VendorName = p.VendorName,
        //                    VendorPhone = p.VendorPhone,
        //                    VendorAddress = p.VendorAddress,
        //                    VendorLocation = p.VendorLocation,
        //                    VendorLandmark = p.VendorLandmark,
        //                    VendorFloor = p.VendorFloor,
        //                    VendorApartment = p.VendorApartment,
        //                    ZoneId = p.ZoneId,
        //                    ZoneName = p.Zone.NameEn,
        //                    AreaId = p.AreaId,
        //                    AreaName = p.Area.CityNameAr,
        //                    PickupDate = p.PickupDate,
        //                    TimeFrom = p.TimeFrom,
        //                    TimeTo = p.TimeTo,
        //                    RefId = p.RefId,
        //                    Notes = p.Notes,

        //                    PickupItems = request.HasPickupItemDTO
        //                        ? p.PickupRequestItem.Select(s => new PickupItemDTO
        //                        {
        //                            PickupRequestId = s.PickupRequestId,
        //                            PickupRequestItemId = s.PickupRequestItemId,
        //                            VendorProductId = s.VendorProductId,
        //                            VendorProductName = s.VendorProduct.Name,
        //                            AdvertisementId = s.AdvertisementId,
        //                            RefId = s.AdvertisementId.HasValue
        //                                ? s.Advertisement.RefId
        //                                : null,
        //                            Quantity = s.Quantity,
        //                            Price = s.Price,
        //                            CreatedAt = s.CreatedAt,
        //                            CreatedBy = s.CreatedBy,
        //                            StatusId = s.StatusId,
        //                        })
        //                        : null,
        //                });

        //                if (request.applyFilter)
        //                    query = ApplyFilter(query, request.PickupDTO);

        //                //if (string.IsNullOrEmpty(request.OrderByColumn))
        //                //    request.OrderByColumn = "PickupRequestId";

        //                //query = OrderByDynamic(query, request.OrderByColumn, request.IsDesc);

        //                if (request.PageSize > 0)
        //                    query = ApplyPaging(query, request.PageSize, request.PageIndex);

        //                res.PickupDTOs = query.OrderByDescending(s => s.PickupRequestId).ToList();

        //                res.Message = HttpStatusCode.OK.ToString();
        //                res.Success = true;
        //                res.StatusCode = HttpStatusCode.OK;
        //            }
        //            catch (Exception ex)
        //            {
        //                res.Message = ex.Message;
        //                res.Success = false;
        //                LogHelper.LogException(ex.Message, ex.StackTrace);
        //            }
        //            return res;
        //        }
        //    );
        //    return res;
        //}

        //public static PickUpRequestResponse GetPickupStockItems(PickUpRequestRequest request)
        //{
        //    var response = new PickUpRequestResponse();
        //    RunBase(
        //        request,
        //        response,
        //        (PickUpRequestRequest req) =>
        //        {
        //            try
        //            {
        //                var query = request
        //                    .context.PickupRequestItem.Where(p =>
        //                        p.PickupRequestId == request.PickupItemDTO.PickupRequestId
        //                    )
        //                    .Select(p => new PickupItemDTO
        //                    {
        //                        PickupRequestItemId = p.PickupRequestItemId,
        //                        PickupRequestId = p.PickupRequestId,
        //                        VendorProductName = p.AdvertisementId.HasValue
        //                            ? p.Advertisement.RefId
        //                            : p.VendorProduct.Name,
        //                        Quantity = p.Quantity,
        //                        Price = p.Price,
        //                        IsDeleted = p.IsDeleted,
        //                        StatusId = p.StatusId,
        //                        StatusName = p.Status.NameEN
        //                    });

        //                if (request.PageSize > 0)
        //                    query = ApplyPaging(query, request.PageSize, request.PageIndex);

        //                response.PickupItemDTOs = query.ToList();

        //                response.Message = HttpStatusCode.OK.ToString();
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

        //private static IQueryable<PickupDTO> ApplyFilter(
        //    IQueryable<PickupDTO> query,
        //    PickupDTO filter
        //)
        //{
        //    if (!string.IsNullOrEmpty(filter.Search))
        //        query = query.Where(c =>
        //            c.RefId.Contains(filter.Search)
        //            || c.VendorName.Contains(filter.Search)
        //            || c.VendorPhone.Contains(filter.Search)
        //            || c.StatusName.Contains(filter.Search)
        //        );

        //    if (filter.PickupRequestId > 0)
        //        query = query.Where(p => p.PickupRequestId == filter.PickupRequestId);

        //    if (filter.PickupRequestTypeId > 0)
        //        query = query.Where(p => p.PickupRequestTypeId == filter.PickupRequestTypeId);

        //    if (filter.DeliveryManId > 0)
        //        query = query.Where(p => p.DeliveryManId == filter.DeliveryManId);

        //    if (filter.StatusId > 0)
        //        query = query.Where(p => p.StatusId == filter.StatusId);

        //    if (filter.StatusIDs != null)
        //        if (filter.StatusIDs.Count > 0)
        //            query = query.Where(s => filter.StatusIDs.Contains(s.StatusId));

        //    if (filter.VendorId > 0)
        //        query = query.Where(p => p.VendorId == filter.VendorId);

        //    if (filter.ZoneId > 0)
        //        query = query.Where(p => p.ZoneId == filter.ZoneId);

        //    if (filter.AreaId > 0)
        //        query = query.Where(p => p.AreaId == filter.AreaId);

        //    return query;
        //}
    }
}

﻿using System;
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
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

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
                            OldPrice = request.AdsDTO.OldPrice,
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
                            var files = new List<string>();
                            foreach (var file in request.AdsDTO.Files)
                            {
                                if (!files.Contains(file.FileName))
                                {
                                    files.Add(file.FileName);
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
                        }
                        //Add follow up
                        var follow = new HistoryDTO
                        {
                            Message = request.Localizer["newAdvertisement", ship.RefId],
                            CreatedBy = request.UserID,
                            AdvertisementId = ship.AdvertisementId,
                            CreatedAt = DateTime.Now,
                            ActionType = ActionTypeEnum.Add,
                            EntityType = EntityTypeEnum.Advertisement
                        };
                        FollowUpService.Add(follow, request.context);
                        request.context.SaveChanges();
                        response.Message = request.Localizer["newAdvertisement", ship.RefId];
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
                            response.Message = request.Localizer[
                                SystemConstants.Message.CantUpdatePrice
                            ];
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
                        ship.OldPrice = request.AdsDTO.OldPrice;
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
                        var follow = new HistoryDTO
                        {
                            Message = request.Localizer["updateAdvertisement", ship.RefId],
                            CreatedBy = request.UserID,
                            AdvertisementId = ship.AdvertisementId,
                            CreatedAt = DateTime.Now,
                            ActionType = ActionTypeEnum.Edit,
                            EntityType = EntityTypeEnum.Advertisement
                        };
                        FollowUpService.Add(follow, request.context);

                        request.context.SaveChanges();
                        response.Message = request.Localizer["updateAdvertisement", ship.RefId];
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
                            var follow = new HistoryDTO
                            {
                                Message = request.Localizer["updateAdvertisement", ship.RefId],
                                CreatedBy = request.UserID,
                                AdvertisementId = ship.AdvertisementId,
                                CreatedAt = DateTime.Now,
                                ActionType = ActionTypeEnum.Edit,
                                EntityType = EntityTypeEnum.Advertisement
                            };
                            FollowUpService.Add(follow, request.context);

                            request.context.SaveChanges();
                            response.Message = request.Localizer["updateAdvertisement", ship.RefId];
                            response.Success = true;
                            response.StatusCode = HttpStatusCode.OK;
                        }
                        else
                        {
                            response.Message = request.Localizer[SystemConstants.Message.NotFound];
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
                            var follow = new HistoryDTO
                            {
                                Message = request.Localizer["rejectAdvertisement", ship.RefId],
                                CreatedBy = request.UserID,
                                AdvertisementId = ship.AdvertisementId,
                                CreatedAt = DateTime.Now,
                                ActionType = ActionTypeEnum.ChangeStatus,
                                EntityType = EntityTypeEnum.Advertisement
                            };
                            FollowUpService.Add(follow, request.context);

                            request.context.SaveChanges();
                            response.Message = request.Localizer["rejectAdvertisement", ship.RefId];
                            response.Success = true;
                            response.StatusCode = HttpStatusCode.OK;
                        }
                        else
                        {
                            response.Message = request.Localizer[SystemConstants.Message.NotFound];
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
                        var ship = request
                            .context.Advertisement.Include(x => x.Gamer)
                            .FirstOrDefault(s =>
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
                            var follow = new HistoryDTO
                            {
                                Message = request.Localizer[
                                    "changeStatusAdvertisement",
                                    ship.RefId
                                ],
                                CreatedBy = request.UserID,
                                AdvertisementId = ship.AdvertisementId,
                                CreatedAt = DateTime.Now,
                                ActionType = ActionTypeEnum.ChangeStatus,
                                EntityType = EntityTypeEnum.Advertisement
                            };
                            FollowUpService.Add(follow, request.context);

                            request.context.SaveChanges();
                            response.AdsDTO = new AdsDTO
                            {
                                AdvertisementId = ship.AdvertisementId,
                                UserName = ship.UserName,
                                Password = ship.Password,
                                Gamer = new UserDTO { Email = ship.Gamer.Email }
                            };
                            response.Message = request.Localizer[
                                "changeStatusAdvertisement",
                                ship.RefId
                            ];
                            response.Success = true;
                            response.StatusCode = HttpStatusCode.OK;
                        }
                        else
                        {
                            response.Message = request.Localizer[SystemConstants.Message.NotFound];
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
                            OldPrice = s.OldPrice,
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

                        if (request.applyFilter)
                            ship = ApplyFilter(ship, request.AdsDTO, req.RoleID, req.UserID);

                        if (!string.IsNullOrEmpty(request.OrderByColumn))
                            ship = OrderByDynamic(ship, request.OrderByColumn, request.IsDesc);

                        if (request.PageSize > 0)
                            ship = ApplyPaging(ship, request.PageSize, request.PageIndex);

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
                            OldPrice = s.OldPrice,
                            RefId = s.RefId,
                            GameId = s.GameId,
                            BuyerId = s.BuyerId,
                            StatusId = s.StatusId,
                            GamerId = s.GamerId,
                            LastModifiedAt = s.LastModifiedAt,
                            LastModifiedBy = s.LastModifiedBy,
                            StatusType = s.Status.StatusType,
                            Status = new StatusDTO
                            {
                                Id = s.Status.Id,
                                StatusType = s.Status.StatusType,
                                NameAR = s.Status.NameAR,
                                NameEN = s.Status.NameEN
                            },
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

        public static AdvertisementResponse GeAdvertisementIds(AdvertisementRequest request)
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
                        ship = request.context.Advertisement.Where(s => !s.IsDeleted);

                        if (request.applyFilter)
                            ship = ApplyFilter(ship, request.AdsDTO, req.RoleID, req.UserID);

                        ship = OrderByDynamic(ship, nameof(Advertisement.RefId), request.IsDesc);

                        var query = ship.Select(s => new SelectOptionDTO
                        {
                            Key = s.RefId,
                            Value = s.AdvertisementId,
                        });
                        response.SelectOption = query.ToList();
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

            if (filter.StatusTypes != null && filter.StatusTypes.Any())
                query = query.Where(p => filter.StatusTypes.Contains(p.Status.StatusType));

            if (filter.GamerId != 0)
                query = query.Where(c => c.GamerId == filter.GamerId);

            if (filter.GameId != 0)
                query = query.Where(c => c.GameId == filter.GameId);

            if (filter.BuyerId.HasValue && filter.BuyerId != 0)
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

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using DicomApp.CommonDefinitions.DTO;
using DicomApp.CommonDefinitions.Requests;
using DicomApp.CommonDefinitions.Responses;
using DicomApp.DAL.DB;
using DicomApp.Helpers;
using Microsoft.CodeAnalysis;

namespace DicomApp.BL.Services
{
    public class GamerServiceService : BaseService
    {
        #region GamerService Common Actions

        public static GamerServiceResponse AddGamerService(GamerServiceRequest request)
        {
            var response = new GamerServiceResponse();
            RunBase(
                request,
                response,
                (GamerServiceRequest req) =>
                {
                    try
                    {
                        var statusType = request.ServiceDTO.GameChargeId.HasValue
                            ? (int)StatusTypeEnum.Accept
                            : (int)StatusTypeEnum.InProgress;

                        var status = request.context.Status.FirstOrDefault(x =>
                            x.StatusType == statusType
                        );
                        var gameCharge = request.context.GameCharges.FirstOrDefault(x =>
                            x.Id == request.ServiceDTO.GameChargeId
                        );

                        var serv = new GamerService()
                        {
                            StatusId = status?.Id ?? 0,
                            CurrentLevel = request.ServiceDTO.CurrentLevel,
                            GamerId = request.ServiceDTO.GamerId,
                            GameId = request.ServiceDTO.GameId,
                            GameServiceType = request.ServiceDTO.GameServiceType,
                            GameChargeId = request.ServiceDTO.GameChargeId,
                            Description = request.ServiceDTO.Description,
                            UserName = request.ServiceDTO.UserName,
                            Password = request.ServiceDTO.Password,
                            Price =
                                gameCharge != null
                                    ? (gameCharge.Price - gameCharge.Discount)
                                    : request.ServiceDTO.Price,
                            Level = request.ServiceDTO.Level,
                            CreatedAt = DateTime.Now,
                            CreatedBy = request.UserID,
                            IsDeleted = false,
                        };
                        request.context.GamerService.Add(serv);
                        request.context.SaveChanges();
                        serv.RefId = BaseHelper.GenerateRefId(
                            EnumRefIdType.GamerService,
                            serv.GamerServiceId
                        );
                        //Add follow up
                        var follow = new FollowUpDTO
                        {
                            Title = "Gamer Service Added",
                            CreatedBy = request.UserID,
                            GameServiceId = serv.GamerServiceId,
                            StatusId = serv.StatusId,
                        };
                        FollowUpService.Add(follow, request.context);

                        //Add notification
                        request.context.Notification.Add(
                            new Notification
                            {
                                Body = "New Gamer Service " + serv.RefId + " added",
                                CreationDate = DateTime.Now,
                                Icon = serv.RefId,
                                SenderId = request.UserID,
                                Title = "New GamerService",
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
                        response.ServiceDTO = new ServiceDTO
                        {
                            Price = serv.Price,
                            GamerServiceId = serv.GamerServiceId,
                            GameServiceType = serv.GameServiceType,
                        };
                        response.Message = "New GamerService " + serv.RefId + " successfully added";
                        response.Success = true;
                        response.StatusCode = HttpStatusCode.OK;
                    }
                    catch (Exception ex)
                    {
                        response.Message = ex.Message;
                        response.Success = false;
                        response.ServiceDTO = request.ServiceDTO;
                        LogHelper.LogException(ex.Message, ex.StackTrace);
                    }
                    return response;
                }
            );
            return response;
        }

        public static GamerServiceResponse EditGamerService(GamerServiceRequest request)
        {
            var response = new GamerServiceResponse();
            RunBase(
                request,
                response,
                (GamerServiceRequest req) =>
                {
                    try
                    {
                        var serv = request.context.GamerService.FirstOrDefault(s =>
                            s.GamerServiceId == request.ServiceDTO.GamerServiceId
                        );
                        var status = request.context.Status.FirstOrDefault(x =>
                            x.Id == serv.StatusId
                        );
                        if (
                            serv != null
                            && serv.Price != request.ServiceDTO.Price
                            && status != null
                            && status.StatusType >= 2
                        )
                        {
                            response.Message = "you Cant Update Price";
                            response.Success = false;
                            response.ServiceDTO = request.ServiceDTO;
                            return response;
                        }

                        serv.GameServiceType = request.ServiceDTO.GameServiceType;
                        serv.GamerId = request.ServiceDTO.GamerId;
                        serv.GameId = request.ServiceDTO.GameId;
                        serv.Description = request.ServiceDTO.Description;
                        serv.UserName = request.ServiceDTO.UserName;
                        serv.Password = request.ServiceDTO.Password;
                        serv.Price = request.ServiceDTO.Price;
                        serv.Level = request.ServiceDTO.Level;
                        serv.CurrentLevel = request.ServiceDTO.CurrentLevel;
                        serv.StatusId = request.ServiceDTO.StatusId;
                        serv.LastModifiedAt = DateTime.Now;
                        serv.LastModifiedBy = request.UserID;
                        request.context.SaveChanges();

                        //Add follow up
                        var follow = new FollowUpDTO
                        {
                            Title = "Gamer Service Updated",
                            CreatedBy = request.UserID,
                            GameServiceId = serv.GamerServiceId,
                            StatusId = serv.StatusId
                        };
                        FollowUpService.Add(follow, request.context);

                        //Add notification
                        request.context.Notification.Add(
                            new Notification
                            {
                                Body = "Gamer Service " + serv.RefId + " updated",
                                CreationDate = DateTime.Now,
                                Icon = serv.RefId,
                                SenderId = request.UserID,
                                Title = "Gamer Service updated",
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
                        response.Message = "GamerService " + serv.RefId + " successfully updated";
                        response.Success = true;
                        response.ServiceDTO = request.ServiceDTO;
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

        public static GamerServiceResponse EditBasicGamerService(GamerServiceRequest request)
        {
            var response = new GamerServiceResponse();
            RunBase(
                request,
                response,
                (GamerServiceRequest req) =>
                {
                    try
                    {
                        var serv = request.context.GamerService.FirstOrDefault(s =>
                            s.GamerServiceId == request.ServiceDTO.GamerServiceId
                        );
                        if (serv != null)
                        {
                            serv.Description = request.ServiceDTO.Description;
                            serv.UserName = request.ServiceDTO.UserName;
                            serv.Password = request.ServiceDTO.Password;
                            serv.Level = request.ServiceDTO.Level;
                            serv.CurrentLevel = request.ServiceDTO.CurrentLevel;

                            serv.LastModifiedAt = DateTime.Now;
                            serv.LastModifiedBy = request.UserID;

                            request.context.SaveChanges();

                            //Add follow up
                            var follow = new FollowUpDTO
                            {
                                Title = "Gamer Service Data Update",
                                CreatedBy = request.UserID,
                                GameServiceId = serv.GamerServiceId,
                                StatusId = serv.StatusId
                            };
                            FollowUpService.Add(follow, request.context);

                            //Add notification
                            request.context.Notification.Add(
                                new Notification
                                {
                                    Body = "Gamer Service " + serv.RefId + " updated",
                                    CreationDate = DateTime.Now,
                                    Icon = serv.RefId,
                                    SenderId = request.UserID,
                                    Title = "GamerService updated",
                                    RecipientRoleId = (int)EnumRole.SuperAdmin,
                                    IsDeleted = false,
                                    IsSeen = false,
                                    RecipientId = 0,
                                }
                            );

                            request.context.SaveChanges();

                            response.Message =
                                "GamerService " + serv.RefId + " successfully updated";
                            response.Success = true;
                            response.StatusCode = HttpStatusCode.OK;
                        }
                        else
                        {
                            response.Message = "Gamer Service Not Found";
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

        public static GamerServiceResponse RejectGamerService(GamerServiceRequest request)
        {
            var response = new GamerServiceResponse();
            RunBase(
                request,
                response,
                (GamerServiceRequest req) =>
                {
                    try
                    {
                        var serv = request.context.GamerService.FirstOrDefault(s =>
                            s.GamerServiceId == request.ServiceDTO.GamerServiceId
                        );
                        var status = request.context.Status.FirstOrDefault(x =>
                            x.StatusType == (int)StatusTypeEnum.Reject
                        );
                        if (status != null && serv != null)
                        {
                            serv.StatusId = status.Id;
                            serv.LastModifiedAt = DateTime.Now;
                            serv.LastModifiedBy = request.UserID;
                            request.context.SaveChanges();

                            //Add follow up
                            var follow = new FollowUpDTO
                            {
                                Title = "GamerService Data Update",
                                CreatedBy = request.UserID,
                                GameServiceId = serv.GamerServiceId,
                                StatusId = serv.StatusId
                            };
                            FollowUpService.Add(follow, request.context);

                            //Add notification
                            request.context.Notification.Add(
                                new Notification
                                {
                                    Body = "GamerService " + serv.RefId + " updated",
                                    CreationDate = DateTime.Now,
                                    Icon = serv.RefId,
                                    SenderId = request.UserID,
                                    Title = "GamerService updated",
                                    RecipientRoleId = (int)EnumRole.SuperAdmin,
                                    IsDeleted = false,
                                    IsSeen = false,
                                    RecipientId = 0,
                                }
                            );

                            request.context.SaveChanges();

                            response.Message =
                                "GamerService " + serv.RefId + " successfully updated";
                            response.Success = true;
                            response.StatusCode = HttpStatusCode.OK;
                        }
                        else
                        {
                            response.Message = "GamerService Not Found";
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

        public static GamerServiceResponse ChangStatusGamerService(GamerServiceRequest request)
        {
            var response = new GamerServiceResponse();
            RunBase(
                request,
                response,
                (GamerServiceRequest req) =>
                {
                    try
                    {
                        var serv = request.context.GamerService.FirstOrDefault(s =>
                            s.GamerServiceId == request.ServiceDTO.GamerServiceId
                        );
                        var status = request.context.Status.FirstOrDefault(x =>
                            x.StatusType == request.ServiceDTO.StatusType
                        );
                        if (status != null && serv != null)
                        {
                            serv.StatusId = status.Id;
                            if (
                                request.ServiceDTO.Price > 0
                                && request.ServiceDTO.GameServiceType == GameServiceType.Push
                            )
                                serv.Price = request.ServiceDTO.Price;

                            serv.LastModifiedAt = DateTime.Now;
                            serv.LastModifiedBy = request.UserID;
                            request.context.SaveChanges();

                            //Add follow up
                            var follow = new FollowUpDTO
                            {
                                Title = "Gamer Service Data Update",
                                CreatedBy = request.UserID,
                                GameServiceId = serv.GamerServiceId,
                                StatusId = serv.StatusId
                            };
                            FollowUpService.Add(follow, request.context);

                            //Add notification
                            request.context.Notification.Add(
                                new Notification
                                {
                                    Body = "Gamer Service " + serv.RefId + " updated",
                                    CreationDate = DateTime.Now,
                                    Icon = serv.RefId,
                                    SenderId = request.UserID,
                                    Title = "GamerService updated",
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
                            response.ServiceDTO = new ServiceDTO
                            {
                                GamerServiceId = serv.GamerServiceId,
                                GameChargeId = serv.GameChargeId,
                                Price = serv.Price,
                                GameServiceType = serv.GameServiceType,
                                StatusType = status.StatusType,
                                GamerId = serv.GamerId,
                            };
                            response.Message =
                                "Gamer Service " + serv.RefId + " successfully updated";
                            response.Success = true;
                            response.StatusCode = HttpStatusCode.OK;
                        }
                        else
                        {
                            response.Message = "Gamer Service Not Found";
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

        #region GamerService Select Queries

        public static GamerServiceResponse GetGamerService(GamerServiceRequest request)
        {
            var response = new GamerServiceResponse();
            RunBase(
                request,
                response,
                (GamerServiceRequest req) =>
                {
                    try
                    {
                        IQueryable<GamerService> serv;
                        if (string.IsNullOrEmpty(request.ServiceDTO.RefId))
                            serv = request.context.GamerService.Where(s =>
                                s.GamerServiceId == request.ServiceDTO.GamerServiceId
                                && !s.IsDeleted
                            );
                        else
                            serv = request.context.GamerService.Where(s =>
                                s.RefId == request.ServiceDTO.RefId && !s.IsDeleted
                            );

                        var query = serv.Select(s => new ServiceDTO
                        {
                            GamerServiceId = s.GamerServiceId,
                            GameServiceType = s.GameServiceType,
                            Description = s.Description,
                            UserName = s.UserName,
                            Password = s.Password,
                            IsDeleted = s.IsDeleted,
                            CreatedAt = s.CreatedAt,
                            CreatedBy = s.CreatedBy,
                            Price = s.Price,
                            Level = s.Level,
                            CurrentLevel = s.CurrentLevel,
                            RefId = s.RefId,
                            GameId = s.GameId,
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
                            Gamer = new UserDTO
                            {
                                Id = s.Gamer.Id,
                                ImgUrl = s.Gamer.ImgUrl,
                                Name = s.Gamer.Name,
                            },
                        });
                        response.ServiceDTO = query.FirstOrDefault();
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

        public static GamerServiceResponse GetAllGamerServices(GamerServiceRequest request)
        {
            var response = new GamerServiceResponse();
            RunBase(
                request,
                response,
                (GamerServiceRequest req) =>
                {
                    try
                    {
                        IQueryable<GamerService> serv;
                        if (
                            request.ServiceDTO.GamerServiceIds != null
                            && request.ServiceDTO.GamerServiceIds.Any()
                        )
                            serv = request.context.GamerService.Where(s =>
                                request.ServiceDTO.GamerServiceIds.Contains(s.GamerServiceId)
                                && !s.IsDeleted
                            );
                        else
                            serv = request.context.GamerService.Where(s => !s.IsDeleted);

                        if (request.PageSize > 0)
                            serv = ApplyPaging(serv, request.PageSize, request.PageIndex);

                        if (request.applyFilter)
                            serv = ApplyFilter(serv, request.ServiceDTO, req.RoleID, req.UserID);

                        if (!string.IsNullOrEmpty(request.OrderByColumn))
                            serv = OrderByDynamic(serv, request.OrderByColumn, request.IsDesc);

                        var query = serv.Select(s => new ServiceDTO
                        {
                            GamerServiceId = s.GamerServiceId,
                            GameServiceType = s.GameServiceType,
                            Description = s.Description,
                            UserName = s.UserName,
                            Password = s.Password,
                            IsDeleted = s.IsDeleted,
                            CreatedAt = s.CreatedAt,
                            CreatedBy = s.CreatedBy,
                            Level = s.Level,
                            CurrentLevel = s.CurrentLevel,
                            Price = s.Price,
                            RefId = s.RefId,
                            GameId = s.GameId,
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
                            Gamer = new UserDTO
                            {
                                Id = s.Gamer.Id,
                                ImgUrl = s.Gamer.ImgUrl,
                                Name = s.Gamer.Name,
                            },
                        });
                        response.ServiceDTOs = query.ToList();
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

        public static GamerServiceResponse GetGamerServicesIds(GamerServiceRequest request)
        {
            var response = new GamerServiceResponse();
            RunBase(
                request,
                response,
                (GamerServiceRequest req) =>
                {
                    {
                        try
                        {
                            IQueryable<GamerService> ship;
                            ship = request.context.GamerService.Where(s => !s.IsDeleted);

                            if (request.applyFilter)
                                ship = ApplyFilter(
                                    ship,
                                    request.ServiceDTO,
                                    req.RoleID,
                                    req.UserID
                                );

                            ship = OrderByDynamic(ship, nameof(GamerService.RefId), request.IsDesc);

                            var query = ship.Select(s => new SelectOptionDTO
                            {
                                Key = s.RefId,
                                Value = s.GamerServiceId,
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
                }
            );
            return response;
        }

        #endregion

        #region Filters

        private static IQueryable<GamerService> ApplyFilter(
            IQueryable<GamerService> query,
            ServiceDTO filter,
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
                    || c.Level.Contains(filter.Search)
                    || c.CurrentLevel.Contains(filter.Search)
                );
            }

            if (filter.GamerServiceId > 0)
                query = query.Where(c => c.GamerServiceId == filter.GamerServiceId);

            if (!string.IsNullOrEmpty(filter.RefId))
                query = query.Where(c => c.RefId == filter.RefId);

            if (filter.GameServiceType != 0)
                query = query.Where(p => p.GameServiceType == filter.GameServiceType);

            if (filter.StatusType != 0)
                query = query.Where(p => p.Status.StatusType == filter.StatusType);

            if (filter.GamerId != 0)
                query = query.Where(c => c.GamerId == filter.GamerId);

            if (filter.GameId != 0)
                query = query.Where(c => c.GameId == filter.GameId);

            if (filter.Price != 0)
                query = query.Where(c => c.Price > filter.Price);

            //if (filter.LessLevel != 0)
            //    query = query.Where(c => c.Level < filter.LessLevel);

            //if (filter.GreeterLevel != 0)
            //    query = query.Where(c => c.Level > filter.GreeterLevel);

            if (filter.LessPrice != 0)
                query = query.Where(c => c.Price < filter.LessPrice);

            if (filter.GreeterPrice != 0)
                query = query.Where(c => c.Price > filter.GreeterPrice);

            if (filter.GameServiceType != 0)
                query = query.Where(c => c.GameServiceType == filter.GameServiceType);

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

        public static bool CanReturnToVendor(GamerService serv, List<FollowUp> lstShipFollowup)
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
            if (canNotReturnStatus.Contains(serv.StatusId))
                result = false;

            if (lstShipFollowup.Any(f => f.StatusId == (int)EnumStatus.Returned))
                result = false;

            #endregion

            return result;
        }

        #endregion
    }
}

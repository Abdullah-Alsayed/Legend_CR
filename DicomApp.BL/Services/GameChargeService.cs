using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using DicomApp.CommonDefinitions.DTO;
using DicomApp.CommonDefinitions.Requests;
using DicomApp.CommonDefinitions.Responses;
using DicomApp.DAL.DB;
using DicomApp.Helpers;

namespace DicomApp.BL.Services
{
    public class GameChargeService : BaseService
    {
        public static GameChargeResponse GetAllGameCharges(GameChargeRequest request)
        {
            var res = new GameChargeResponse();
            RunBase(
                request,
                res,
                (GameChargeRequest req) =>
                {
                    try
                    {
                        var query = request
                            .context.GameCharges.Where(x => !x.IsDeleted)
                            .Select(p => new GameChargeDTO
                            {
                                Id = p.Id,
                                CreatedAt = p.CreatedAt,
                                CreatedBy = p.CreatedBy,
                                Img = p.Img,
                                GameId = p.GameId,
                                Price = p.Price,
                                Count = p.Count,
                                Discount = p.Discount,
                                Game = new GameDTO
                                {
                                    NameAr = p.Game.NameAr,
                                    NameEn = p.Game.NameEn,
                                    ImgUrl = p.Game.ImgUrl
                                }
                            });

                        if (request.GameChargeDTO != null)
                            query = ApplyFilter(query, request.GameChargeDTO);

                        res.TotalCount = query.Count();

                        query = OrderByDynamic(
                            query,
                            request.OrderByColumn ?? "Id",
                            request.IsDesc
                        );

                        if (request.PageSize > 0)
                            query = ApplyPaging(query, request.PageSize, request.PageIndex);

                        res.GameChargeDTOs = query.ToList();
                        res.Message = HttpStatusCode.OK.ToString();
                        res.Success = true;
                        res.StatusCode = HttpStatusCode.OK;
                    }
                    catch (Exception ex)
                    {
                        res.Message = ex.Message;
                        res.Success = false;
                        LogHelper.LogException(ex.Message, ex.StackTrace);
                    }
                    return res;
                }
            );
            return res;
        }

        public static GameChargeResponse GetGameCharge(GameChargeRequest request)
        {
            var res = new GameChargeResponse();
            RunBase(
                request,
                res,
                (GameChargeRequest req) =>
                {
                    try
                    {
                        var query = request
                            .context.GameCharges.Where(p => p.Id == request.GameChargeDTO.Id)
                            .Select(p => new GameChargeDTO
                            {
                                Id = p.Id,
                                CreatedAt = p.CreatedAt,
                                CreatedBy = p.CreatedBy,
                                Img = p.Img,
                                Price = p.Price,
                                Count = p.Count,
                                Discount = p.Discount,
                                GameId = p.GameId,
                                Game = new GameDTO
                                {
                                    NameEn = p.Game.NameEn,
                                    NameAr = p.Game.NameAr,
                                    ImgUrl = p.Game.ImgUrl
                                }
                            });

                        res.GameChargeDTO = query.FirstOrDefault();
                        res.Message = HttpStatusCode.OK.ToString();
                        res.Success = true;
                        res.StatusCode = HttpStatusCode.OK;
                    }
                    catch (Exception ex)
                    {
                        res.Message = ex.Message;
                        res.Success = false;
                        LogHelper.LogException(ex.Message, ex.StackTrace);
                    }
                    return res;
                }
            );
            return res;
        }

        public static GameChargeResponse EditGameCharge(GameChargeRequest request)
        {
            var res = new GameChargeResponse();
            RunBase(
                request,
                res,
                (GameChargeRequest req) =>
                {
                    try
                    {
                        var model = request.GameChargeDTO;
                        int GameChargeID = model.Id;
                        var GameCharge = request.context.GameCharges.Find(GameChargeID);
                        var game = request.context.Game.Find(model.GameId);
                        if (GameCharge != null)
                        {
                            GameCharge = AddOrEditGameCharge(
                                request.UserID,
                                request.GameChargeDTO,
                                GameCharge
                            );
                            var url = BaseHelper.UploadImg(
                                request.GameChargeDTO.File,
                                request.RoutPath,
                                GameCharge.Img
                            );
                            GameCharge.Img = url;
                            request.context.SaveChanges();
                            model.Game = new GameDTO
                            {
                                ImgUrl = game?.ImgUrl,
                                NameEn = game?.NameEn,
                                NameAr = game?.NameAr
                            };
                            res.GameChargeDTO = model;
                            res.Message = HttpStatusCode.OK.ToString();
                            res.Success = true;
                            res.StatusCode = HttpStatusCode.OK;
                        }
                        else
                        {
                            res.Message = "Invalid";
                            res.Success = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        res.Message = ex.Message;
                        res.Success = false;
                        LogHelper.LogException(ex.Message, ex.StackTrace);
                    }
                    return res;
                }
            );
            return res;
        }

        public static GameChargeResponse AddGameCharge(GameChargeRequest request)
        {
            var res = new GameChargeResponse();
            RunBase(
                request,
                res,
                (GameChargeRequest req) =>
                {
                    try
                    {
                        var game = request
                            .context.Game.Select(x => new GameDTO
                            {
                                NameEn = x.NameEn,
                                NameAr = x.NameAr,
                                ImgUrl = x.ImgUrl
                            })
                            .FirstOrDefault(x => x.Id == request.GameChargeDTO.GameId);
                        var GameCharge = AddOrEditGameCharge(request.UserID, request.GameChargeDTO);
                        var url = BaseHelper.UploadImg(
                            request.GameChargeDTO.File,
                            request.RoutPath
                        );
                        GameCharge.Img = url;
                        request.context.GameCharges.Add(GameCharge);
                        request.context.SaveChanges();
                        request.GameChargeDTO.Game = game;
                        res.GameChargeDTOs.Add(request.GameChargeDTO);
                        res.Message = "Added Successfully";
                        res.Success = true;
                        res.StatusCode = HttpStatusCode.OK;
                    }
                    catch (Exception ex)
                    {
                        res.Message = ex.Message;
                        res.Success = false;
                        LogHelper.LogException(ex.Message, ex.StackTrace);
                    }
                    return res;
                }
            );
            return res;
        }

        private static GameCharge AddOrEditGameCharge(
            int createdBy,
            GameChargeDTO record,
            GameCharge oldGameCharge = null
        )
        {
            if (oldGameCharge == null) //new GameCharge
            {
                oldGameCharge = new GameCharge();
                oldGameCharge.CreatedAt = DateTime.Now;
                oldGameCharge.CreatedBy = createdBy;
            }
            else
            {
                oldGameCharge.LastModifiedBy = createdBy;
                oldGameCharge.LastModifiedAt = DateTime.Now;
            }
            oldGameCharge.Img = record.Img;
            oldGameCharge.CreatedBy = record.CreatedBy;
            oldGameCharge.Id = record.Id;
            oldGameCharge.Price = record.Price;
            oldGameCharge.Count = record.Count;
            oldGameCharge.Discount = record.Discount;

            oldGameCharge.GameId = record.GameId;
            return oldGameCharge;
        }

        private static IQueryable<GameChargeDTO> ApplyFilter(
            IQueryable<GameChargeDTO> query,
            GameChargeDTO GameChargeDTO
        )
        {
            if (GameChargeDTO.GameId > 0)
                query = query.Where(p => p.GameId == GameChargeDTO.GameId);

            if (GameChargeDTO.DateFrom.HasValue)
                query = query.Where(p => p.CreatedAt.Date >= GameChargeDTO.DateFrom);

            if (GameChargeDTO.DateTo.HasValue)
                query = query.Where(p => p.CreatedAt.Date <= GameChargeDTO.DateTo);

            return query;
        }

        public static GameChargeResponse DeleteGameCharge(GameChargeRequest request)
        {
            var res = new GameChargeResponse();
            RunBase(
                request,
                res,
                (GameChargeRequest req) =>
                {
                    try
                    {
                        var model = request.GameChargeDTO;
                        var GameCharge = request.context.GameCharges.FirstOrDefault(c =>
                            c.Id == model.Id
                        );
                        if (GameCharge != null)
                        {
                            GameCharge.IsDeleted = true;
                            GameCharge.DeletedOn = DateTime.UtcNow;
                            GameCharge.DeleteBy = request.UserID;
                            request.context.SaveChanges();
                            res.Message = SystemEnums.DeletedSuccessfully.ToString();
                            res.Success = true;
                            res.StatusCode = HttpStatusCode.OK;
                        }
                        else
                        {
                            res.Message = SystemEnums.InvalidData.ToString();
                            res.Success = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        res.Message = ex.Message;
                        res.Success = false;
                        LogHelper.LogException(ex.Message, ex.StackTrace);
                    }
                    return res;
                }
            );
            return res;
        }
    }
}

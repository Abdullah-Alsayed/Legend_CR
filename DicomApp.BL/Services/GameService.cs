using System;
using System.Linq;
using System.Net;
using DicomApp.CommonDefinitions.DTO;
using DicomApp.CommonDefinitions.DTO.ShipmentDTOs;
using DicomApp.CommonDefinitions.Requests;
using DicomApp.CommonDefinitions.Responses;
using DicomApp.DAL.DB;
using DicomApp.Helpers;

namespace DicomApp.BL.Services
{
    public class GameService : BaseService
    {
        public static GameResponse GetGames(GameRequest request)
        {
            var res = new GameResponse();
            RunBase(
                request,
                res,
                (GameRequest req) =>
                {
                    try
                    {
                        var query = request.context.Game.Select(p => new GameDTO
                        {
                            Id = p.Id,
                            CreatedAt = p.CreatedAt,
                            CreatedBy = p.CreatedBy,
                            ImgUrl = p.ImgUrl,
                            NameAr = p.NameAr,
                            NameEn = p.NameEn,
                            CategoryId = p.CategoryId,
                            CategoryName = p.Category.NameEn,
                            Description = p.Description,
                            ShipDTOs = p.ShipmentWarehouseGame.Select(s => new ShipDTO
                            {
                                ShipmentId = s.ShipmentId
                            })
                        });

                        if (request.GameDTO != null)
                            query = ApplyFilter(query, request.GameDTO);

                        res.TotalCount = query.Count();

                        query = OrderByDynamic(
                            query,
                            request.OrderByColumn ?? "Id",
                            request.IsDesc
                        );

                        if (request.PageSize > 0)
                            query = ApplyPaging(query, request.PageSize, request.PageIndex);

                        res.GameDTOs = query.ToList();
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

        public static GameResponse GetLastGame(GameRequest request)
        {
            var res = new GameResponse();
            RunBase(
                request,
                res,
                (GameRequest req) =>
                {
                    try
                    {
                        var query = request
                            .context.Game.Select(p => new GameDTO
                            {
                                Id = p.Id,
                                CreatedAt = p.CreatedAt,
                                CreatedBy = p.CreatedBy,
                                ImgUrl = p.ImgUrl,
                                NameAr = p.NameAr,
                                NameEn = p.NameEn,
                                CategoryId = p.CategoryId,
                                CategoryName = p.Category.NameEn,
                                Description = p.Description,
                            })
                            .LastOrDefault();

                        res.TotalCount = 1;

                        res.GameDTO = query;
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

        public static GameResponse GetGame(GameRequest request)
        {
            var res = new GameResponse();
            RunBase(
                request,
                res,
                (GameRequest req) =>
                {
                    try
                    {
                        var query = request
                            .context.Game.Where(p => p.Id == request.GameDTO.Id)
                            .Select(p => new GameDTO
                            {
                                Id = p.Id,
                                CreatedAt = p.CreatedAt,
                                CreatedBy = p.CreatedBy,
                                ImgUrl = p.ImgUrl,
                                NameAr = p.NameAr,
                                NameEn = p.NameEn,
                                CategoryId = p.CategoryId,
                                CategoryName = p.Category.NameEn,
                                Description = p.Description,
                            });

                        res.TotalCount = 1;
                        res.GameDTO = query.FirstOrDefault();
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

        public static GameResponse EditGame(GameRequest request)
        {
            var res = new GameResponse();
            RunBase(
                request,
                res,
                (GameRequest req) =>
                {
                    try
                    {
                        var model = request.GameDTO;
                        int GameID = model.Id;
                        var Game = request.context.Game.Find(GameID);
                        if (Game != null)
                        {
                            //update whole Game
                            Game = AddOrEditGame(request.UserID, request.GameDTO, Game);
                            request.context.SaveChanges();

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

        public static GameResponse AddGame(GameRequest request)
        {
            var res = new GameResponse();
            RunBase(
                request,
                res,
                (GameRequest req) =>
                {
                    try
                    {
                        var Game = AddOrEditGame(request.UserID, request.GameDTO);
                        request.context.Game.Add(Game);
                        request.context.SaveChanges();

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

        private static Game AddOrEditGame(int createdBy, GameDTO record, Game oldGame = null)
        {
            if (oldGame == null) //new Game
            {
                oldGame = new Game();
                oldGame.CreatedAt = DateTime.Now;
                oldGame.CreatedBy = createdBy;
            }
            oldGame.ImgUrl = record.ImgUrl;
            oldGame.CreatedBy = record.CreatedBy;
            oldGame.Id = record.Id;
            oldGame.Description = record.Description;
            oldGame.LastModifiedBy = createdBy;
            oldGame.LastModifiedAt = DateTime.Now;
            oldGame.CategoryId = record.CategoryId;
            oldGame.NameAr = record.NameAr;
            oldGame.NameEn = record.NameEn;
            return oldGame;
        }

        private static IQueryable<GameDTO> ApplyFilter(IQueryable<GameDTO> query, GameDTO GameDTO)
        {
            if (!string.IsNullOrEmpty(GameDTO.search))
                query = query.Where(c =>
                    (!string.IsNullOrEmpty(c.NameEn) && c.NameEn.Contains(GameDTO.search))
                    || (!string.IsNullOrEmpty(c.NameAr) && c.NameAr.Contains(GameDTO.search))
                );

            if (GameDTO.CategoryId > 0)
                query = query.Where(p => p.CategoryId == GameDTO.CategoryId);

            if (GameDTO.DateFrom.HasValue)
                query = query.Where(p => p.CreatedAt.Date >= GameDTO.DateFrom);

            if (GameDTO.DateTo.HasValue)
                query = query.Where(p => p.CreatedAt.Date <= GameDTO.DateTo);

            return query;
        }
    }
}

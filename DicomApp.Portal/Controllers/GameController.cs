﻿using System;
using System.Collections.Generic;
using System.Linq;
using DicomApp.BL.Services;
using DicomApp.CommonDefinitions.DTO;
using DicomApp.CommonDefinitions.Requests;
using DicomApp.CommonDefinitions.Responses;
using DicomApp.DAL.DB;
using DicomApp.Helpers;
using DicomApp.Portal.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace DicomApp.Portal.Controllers
{
    [Authorize]
    public class GameController : Controller
    {
        private readonly ShippingDBContext _context;
        private readonly IHostingEnvironment hosting;

        public GameController(ShippingDBContext context, IHostingEnvironment hosting)
        {
            _context = context;
            this.hosting = hosting;
        }

        [HttpPost]
        [AuthorizePerRole(SystemConstants.Permission.AddGame)]
        public IActionResult AddGame(GameDTO model)
        {
            model.ImgUrl = BaseHelper.UploadImg(model.File, hosting.WebRootPath, model.ImgUrl);
            var GameRequest = new GameRequest
            {
                RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
                UserID = AuthHelper.GetClaimValue(User, "UserID"),
                context = _context,
                GameDTO = model
            };
            var GameResponse = GameService.AddGame(GameRequest);
            var GetGameResponse = GameService.GetLastGame(GameRequest);
            if (GameResponse.Success)
                return PartialView("_GameList", new List<GameDTO> { GetGameResponse.GameDTO });
            else
                return Json(new { message = GameResponse, success = false });
        }

        [HttpPost]
        [AuthorizePerRole(SystemConstants.Permission.EditGame)]
        public IActionResult EditGame(GameDTO model)
        {
            try
            {
                model.ImgUrl = BaseHelper.UploadImg(model.File, hosting.WebRootPath, model.ImgUrl);
                var GameRequest = new GameRequest
                {
                    RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
                    UserID = AuthHelper.GetClaimValue(User, "UserID"),
                    context = _context,
                    GameDTO = model
                };
                var userResponse = GameService.EditGame(GameRequest);
                if (userResponse.Success)
                    return Json(
                        new
                        {
                            message = "Edit Successfully",
                            success = true,
                            model = model
                        }
                    );
                else
                    return Json(new { message = userResponse.Message, success = false });
            }
            catch (Exception ex)
            {
                return Json(new { message = ex.Message, success = false });
            }
        }

        [HttpGet]
        [AuthorizePerRole(SystemConstants.Permission.EditGame)]
        public IActionResult EditGame(int id)
        {
            var GameRequest = new GameRequest
            {
                RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
                UserID = AuthHelper.GetClaimValue(User, "UserID"),
                context = _context,
                GameDTO = new GameDTO { Id = id }
            };
            var GameResponse = GameService.GetGame(GameRequest);

            if (GameResponse.Success)
                return Json(GameResponse.GameDTO);
            else
                return Json(false);
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var GameRequest = new GameRequest
            {
                RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
                UserID = AuthHelper.GetClaimValue(User, "UserID"),
                context = _context,
                GameDTO = new GameDTO { Id = id }
            };
            var GameResponse = GameService.GetGame(GameRequest);
            return View(GameResponse.GameDTO);
        }

        [AuthorizePerRole(SystemConstants.Permission.ListGame)]
        public IActionResult GameList(
            DateTime? From,
            int? Quantity,
            int CategoryId,
            string Search,
            string OrderByColumn,
            bool? IsDesc,
            int PageIndex,
            string ActionType = null
        )
        {
            var ViewData = new ViewModel<GameDTO>();
            var filter = new GameDTO();
            filter.search = Search;
            filter.CategoryId = CategoryId;

            if (From.HasValue)
                filter.CreatedAt = From.Value;

            var GameRequest = new GameRequest
            {
                RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
                UserID = AuthHelper.GetClaimValue(User, "UserID"),
                context = _context,
                GameDTO = filter,
                PageSize = BaseHelper.Constants.PageSize,
                PageIndex = PageIndex,
                IsDesc = IsDesc ?? true,
                OrderByColumn = OrderByColumn ?? "Id",
                applyFilter = true
            };
            var GameResponse = GameService.GetGames(GameRequest);
            ViewData.ObjDTOs = GameResponse.GameDTOs;

            if (
                ActionType != SystemConstants.ActionType.Table
                && ActionType != SystemConstants.ActionType.Print
            )
                ViewData.Lookup = BaseHelper.GetLookup(
                    new List<byte> { (byte)EnumSelectListType.Category },
                    _context
                );

            if (ActionType == SystemConstants.ActionType.PartialView)
                return PartialView(ViewData);

            if (ActionType == SystemConstants.ActionType.Table)
                return PartialView("_GameList", GameResponse.GameDTOs);
            else if (ActionType == SystemConstants.ActionType.Print)
                return BaseHelper.GeneratePDF<List<GameDTO>>(
                    "GameManagementPDF",
                    GameResponse.GameDTOs
                );
            else
                return View(ViewData);
        }

        [AuthorizePerRole(SystemConstants.Permission.ListGame)]
        public IActionResult GetGameList(int ID)
        {
            var GameRequest = new GameRequest
            {
                RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
                UserID = AuthHelper.GetClaimValue(User, "UserID"),
                context = _context,
                GameDTO = new GameDTO { CategoryId = ID },
                IsDesc = true,
                OrderByColumn = "Id",
                applyFilter = true
            };
            var gameResponse = GameService.GetGames(GameRequest);
            return Json(gameResponse.GameDTOs);
        }

        [AuthorizePerRole(SystemConstants.Permission.GameDelete)]
        public IActionResult DeleteGame(int Id)
        {
            var Filter = new GameDTO { Id = Id };
            var GameRequest = new GameRequest();
            var GameResponse = new GameResponse();
            try
            {
                GameRequest.RoleID = AuthHelper.GetClaimValue(User, "RoleID");
                GameRequest.UserID = AuthHelper.GetClaimValue(User, "UserID");
                GameRequest.context = _context;
                GameRequest.GameDTO = Filter;

                GameResponse = GameService.DeleteGame(GameRequest);
                if (GameResponse.Success)
                    return Json(
                        "Game-" + GameResponse.GameDTOs.FirstOrDefault().NameEn + " Is Deleted"
                    );
                else
                    return Json(false);
            }
            catch (Exception ex)
            {
                GameResponse.Message = ex.Message;
            }
            return Json(GameResponse.Message);
        }
    }
}

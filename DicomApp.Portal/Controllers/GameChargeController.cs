using System;
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
    public class GameChargeController : Controller
    {
        private readonly ShippingDBContext _context;
        private readonly IHostingEnvironment hosting;

        public GameChargeController(ShippingDBContext context, IHostingEnvironment hosting)
        {
            _context = context;
            this.hosting = hosting;
        }

        [HttpPost]
        [AuthorizePerRole(SystemConstants.Permission.AddGameCharge)]
        public IActionResult AddGameCharge(GameChargeDTO model)
        {
            var GameChargeRequest = new GameChargeRequest
            {
                RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
                UserID = AuthHelper.GetClaimValue(User, "UserID"),
                context = _context,
                GameChargeDTO = model,
                RoutPath = hosting.WebRootPath
            };
            var GameChargeResponse = GameChargeService.AddGameCharge(GameChargeRequest);
            if (GameChargeResponse.Success)
                return PartialView("_GameChargeList", GameChargeResponse.GameChargeDTOs);
            else
                return Json(new { message = GameChargeResponse, success = false });
        }

        [HttpPost]
        [AuthorizePerRole(SystemConstants.Permission.EditGameCharge)]
        public IActionResult EditGameCharge(GameChargeDTO model)
        {
            try
            {
                var GameChargeRequest = new GameChargeRequest
                {
                    RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
                    UserID = AuthHelper.GetClaimValue(User, "UserID"),
                    context = _context,
                    GameChargeDTO = model,
                    RoutPath = hosting.WebRootPath
                };
                var userResponse = GameChargeService.EditGameCharge(GameChargeRequest);
                if (userResponse.Success)
                    return Json(
                        new
                        {
                            message = "Edit Successfully",
                            success = true,
                            model = userResponse.GameChargeDTO
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
        [AuthorizePerRole(SystemConstants.Permission.EditGameCharge)]
        public IActionResult EditGameCharge(int id)
        {
            var GameChargeRequest = new GameChargeRequest
            {
                RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
                UserID = AuthHelper.GetClaimValue(User, "UserID"),
                context = _context,
                GameChargeDTO = new GameChargeDTO { Id = id }
            };
            var GameChargeResponse = GameChargeService.GetGameCharge(GameChargeRequest);

            if (GameChargeResponse.Success)
                return Json(GameChargeResponse.GameChargeDTO);
            else
                return Json(false);
        }

        public IActionResult GameChargeList(
            DateTime? From,
            int? Quantity,
            int GameId,
            string Search,
            string OrderByColumn,
            bool? IsDesc,
            int PageIndex,
            string ActionType = null
        )
        {
            var ViewData = new ViewModel<GameChargeDTO>();
            var filter = new GameChargeDTO();
            filter.Search = Search;
            filter.GameId = GameId;

            if (From.HasValue)
                filter.CreatedAt = From.Value;

            var GameChargeRequest = new GameChargeRequest
            {
                RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
                UserID = AuthHelper.GetClaimValue(User, "UserID"),
                context = _context,
                GameChargeDTO = filter,
                PageSize = BaseHelper.Constants.PageSize,
                PageIndex = PageIndex,
                IsDesc = IsDesc ?? true,
                OrderByColumn = OrderByColumn ?? "Id",
                applyFilter = true
            };
            var GameChargeResponse = GameChargeService.GetAllGameCharges(GameChargeRequest);
            ViewData.ObjDTOs = GameChargeResponse.GameChargeDTOs;

            if (
                ActionType != SystemConstants.ActionType.Table
                && ActionType != SystemConstants.ActionType.Print
            )
                ViewData.Lookup = BaseHelper.GetLookup(
                    new List<byte> { (byte)EnumSelectListType.Game },
                    _context
                );

            if (ActionType == SystemConstants.ActionType.PartialView)
                return PartialView(ViewData);

            if (ActionType == SystemConstants.ActionType.Table)
                return PartialView("_GameChargeList", GameChargeResponse.GameChargeDTOs);
            else if (ActionType == SystemConstants.ActionType.Print)
                return BaseHelper.GeneratePDF<List<GameChargeDTO>>(
                    "GameChargeManagementPDF",
                    GameChargeResponse.GameChargeDTOs
                );
            else
                return View(ViewData);
        }

        public IActionResult GetGameChargeList(int ID)
        {
            var GameChargeRequest = new GameChargeRequest
            {
                RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
                UserID = AuthHelper.GetClaimValue(User, "UserID"),
                context = _context,
                GameChargeDTO = new GameChargeDTO { GameId = ID },
                IsDesc = true,
                OrderByColumn = nameof(GameCharge.CreatedAt),
                applyFilter = true
            };
            var GameChargeResponse = GameChargeService.GetAllGameCharges(GameChargeRequest);
            return Json(GameChargeResponse.GameChargeDTOs);
        }

        [AuthorizePerRole(SystemConstants.Permission.GameChargeDelete)]
        public IActionResult DeleteGameCharge(int Id)
        {
            var Filter = new GameChargeDTO { Id = Id };
            var GameChargeRequest = new GameChargeRequest();
            var GameChargeResponse = new GameChargeResponse();
            try
            {
                GameChargeRequest.RoleID = AuthHelper.GetClaimValue(User, "RoleID");
                GameChargeRequest.UserID = AuthHelper.GetClaimValue(User, "UserID");
                GameChargeRequest.context = _context;
                GameChargeRequest.GameChargeDTO = Filter;

                GameChargeResponse = GameChargeService.DeleteGameCharge(GameChargeRequest);
                if (GameChargeResponse.Success)
                    return Json("GameCharge Is Deleted");
                else
                    return Json(false);
            }
            catch (Exception ex)
            {
                GameChargeResponse.Message = ex.Message;
            }
            return Json(GameChargeResponse.Message);
        }
    }
}

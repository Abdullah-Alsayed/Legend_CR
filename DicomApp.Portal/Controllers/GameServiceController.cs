using System;
using System.Collections.Generic;
using System.Linq;
using DicomApp.BL.Services;
using DicomApp.CommonDefinitions.DTO;
using DicomApp.CommonDefinitions.Requests;
using DicomApp.DAL.DB;
using DicomApp.Helpers;
using DicomApp.Portal.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Rotativa.AspNetCore;
using Rotativa.AspNetCore.Options;

namespace DicomApp.Portal.Controllers
{
    public class GamerServiceController : Controller
    {
        private readonly ShippingDBContext _context;
        private readonly IHostingEnvironment _hosting;

        public GamerServiceController(ShippingDBContext context, IHostingEnvironment hosting)
        {
            _context = context;
            _hosting = hosting;
        }

        #region GamerService actions

        [AuthorizePerRole(SystemConstants.Permission.AddGamerService)]
        [HttpPost]
        public IActionResult Add(ServiceDTO model)
        {
            var request = new GamerServiceRequest();
            request.context = _context;
            request.RoleID = AuthHelper.GetClaimValue(User, "RoleID");
            request.UserID = AuthHelper.GetClaimValue(User, "UserID");
            request.ServiceDTO = model;
            request.RoutPath = _hosting.WebRootPath;

            if (request.RoleID == (int)EnumRole.Gamer)
                model.GamerId = request.UserID;

            var response = BL.Services.GamerServiceService.AddGamerService(request);
            //request.PageIndex = 0;
            //request.PageSize = BaseHelper.Constants.PageSize;
            //request.IsDesc = true;
            //request.OrderByColumn = nameof(GamerService.CreatedAt);
            //var Getresponse = BL.Services.GamerServiceService.GetAllGamerServices(request);
            return Json(response);
        }

        [AuthorizePerRole(SystemConstants.Permission.AddGamerService)]
        public IActionResult Add(string ActionType = null)
        {
            ViewBag.GameList = GameService
                .GetGames(new GameRequest { context = _context })
                .GameDTOs;

            ViewBag.Gamers = GeneralHelper.GetUsers(SystemConstants.Role.Gamer, _context);

            if (ActionType == SystemConstants.ActionType.PartialView)
                return PartialView("/Views/GamerService/_AddOrder.cshtml");
            else
                return View();
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult GetTotalPrice(int AreaID, int GameID)
        {
            double ShippingFees = 0;
            double GameFees = 0;
            string ZoneName = "";

            if (AreaID > 0)
            {
                var Area = _context.City.FirstOrDefault(z => z.Id == AreaID);
                ZoneTax zonetax = _context.ZoneTax.FirstOrDefault(z => z.ZoneId == Area.ZoneId);
                var zone = _context.Zone.FirstOrDefault(z => z.Id == zonetax.ZoneId);

                ShippingFees = zonetax.Tax;
                ZoneName = zone.NameEn;
            }

            if (GameID > 0)
            {
                var Game = _context.Game.FirstOrDefault(p => p.Id == GameID);
            }

            return Json(
                new
                {
                    ShippingFees = ShippingFees,
                    ZoneName = ZoneName,
                    GameFees = GameFees
                }
            );
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult GetAreaFees(int AreaID)
        {
            double ShippingFees = 0;
            string ZoneName = "";

            if (AreaID > 0)
            {
                var Area = _context.City.FirstOrDefault(z => z.Id == AreaID);
                ZoneTax zonetax = _context.ZoneTax.FirstOrDefault(z => z.ZoneId == Area.ZoneId);
                var zone = _context.Zone.FirstOrDefault(z => z.Id == zonetax.ZoneId);

                ShippingFees = zonetax.Tax;
                ZoneName = zone.NameEn;
            }

            return Json(new { ShippingFees = ShippingFees, ZoneName = ZoneName });
        }

        [AuthorizePerRole(SystemConstants.Permission.EditGamerService)]
        public IActionResult Edit(int adsID, string ActionType = null)
        {
            ViewBag.GameList = GameService
                .GetGames(new GameRequest { context = _context })
                .GameDTOs;

            ViewBag.Gamers = GeneralHelper.GetUsers(SystemConstants.Role.Gamer, _context);
            var model = new ServiceDTO();
            model.GamerServiceId = adsID;
            var request = new GamerServiceRequest
            {
                RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
                UserID = AuthHelper.GetClaimValue(User, "UserID"),
                context = _context,
                ServiceDTO = model
            };
            var response = DicomApp.BL.Services.GamerServiceService.GetGamerService(request);
            if (ActionType == SystemConstants.ActionType.PartialView)
                return PartialView("_Edit", response.ServiceDTO);
            else
                return View(response.ServiceDTO);
        }

        [AuthorizePerRole(SystemConstants.Permission.EditGamerService)]
        [HttpPost]
        public ActionResult Edit(ServiceDTO model)
        {
            var request = new GamerServiceRequest();
            request.context = _context;
            request.RoleID = AuthHelper.GetClaimValue(User, "RoleID");
            request.UserID = AuthHelper.GetClaimValue(User, "UserID");
            request.ServiceDTO = model;
            var response = BL.Services.GamerServiceService.EditGamerService(request);

            return Json(response.Message);
        }

        [AuthorizePerRole(SystemConstants.Permission.EditGamerService)]
        [HttpPost]
        public ActionResult EditAndPublish(ServiceDTO model)
        {
            var request = new GamerServiceRequest();
            request.context = _context;
            request.RoleID = AuthHelper.GetClaimValue(User, "RoleID");
            request.UserID = AuthHelper.GetClaimValue(User, "UserID");
            request.ServiceDTO = model;
            var response = BL.Services.GamerServiceService.EditGamerService(request);

            return Json(response.Message);
        }

        [AuthorizePerRole(SystemConstants.Permission.ChangStatusGamerService)]
        [HttpPut]
        public ActionResult ChangStatusGamerService(int ID, int status)
        {
            var request = new GamerServiceRequest();
            request.context = _context;
            request.RoleID = AuthHelper.GetClaimValue(User, "RoleID");
            request.UserID = AuthHelper.GetClaimValue(User, "UserID");
            request.ServiceDTO.GamerServiceId = ID;
            request.ServiceDTO.StatusType = status;
            var response = BL.Services.GamerServiceService.ChangStatusGamerService(request);
            return Json(response.Message);
        }

        [AuthorizePerRole(SystemConstants.Permission.RejectGamerService)]
        [HttpPut]
        public ActionResult RejectGamerService(int ID)
        {
            var request = new GamerServiceRequest();
            request.context = _context;
            request.RoleID = AuthHelper.GetClaimValue(User, "RoleID");
            request.UserID = AuthHelper.GetClaimValue(User, "UserID");
            request.ServiceDTO.GamerServiceId = ID;
            var response = BL.Services.GamerServiceService.RejectGamerService(request);
            return Json(response.Message);
        }

        [AuthorizePerRole(SystemConstants.Permission.EditBasicGamerService)]
        [HttpPost]
        public IActionResult EditBasicGamerService(
            int adsID,
            string Description,
            string UserName,
            string Password,
            int Level,
            string Rank
        )
        {
            var request = new GamerServiceRequest
            {
                context = _context,
                RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
                UserID = AuthHelper.GetClaimValue(User, "UserID"),
                ServiceDTO = new ServiceDTO
                {
                    GamerServiceId = adsID,
                    Description = Description,
                    UserName = UserName,
                    Password = Password,
                    Level = Level,
                    Rank = Rank
                }
            };

            var response = BL.Services.GamerServiceService.EditBasicGamerService(request);

            return Json(new { success = response.Success, message = response.Message });
        }

        [AuthorizePerRole(SystemConstants.Permission.PrintGamerService)]
        public IActionResult Print(int GamerServiceId)
        {
            var request = new GamerServiceRequest
            {
                RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
                UserID = AuthHelper.GetClaimValue(User, "UserID"),
                context = _context,
                ServiceDTO = new ServiceDTO { GamerServiceId = GamerServiceId }
            };

            var response = BL.Services.GamerServiceService.GetGamerService(request);

            string footer =
                "--no-stop-slow-scripts --javascript-delay 10000  --footer-right \"Date: [date] [time]\" "
                + "--footer-center \"Page: [page] of [toPage]\" --footer-line --footer-font-size \"9\" --footer-spacing 5 --footer-font-name \"calibri light\"";
            var pdfFile = new ViewAsPdf("~/Views/GamerService/Print.cshtml", response.ServiceDTO)
            {
                CustomSwitches = footer,
                PageMargins = new Margins(2, 2, 2, 2),
                PageOrientation = Orientation.Portrait,
                PageSize = Size.A4
            };
            return pdfFile;
        }

        [AuthorizePerRole(SystemConstants.Permission.PrintGamerService)]
        public IActionResult PrintAll(string GamerServicesIds)
        {
            List<int> GamerServicesList = GamerServicesIds
                .Split(',')
                .Select<string, int>(int.Parse)
                .ToList();
            var request = new GamerServiceRequest
            {
                RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
                UserID = AuthHelper.GetClaimValue(User, "UserID"),
                context = _context,
                applyFilter = true,
                ServiceDTO = new ServiceDTO { GamerServiceIds = GamerServicesList }
            };
            var response = BL.Services.GamerServiceService.GetAllGamerServices(request);

            string footer =
                "--no-stop-slow-scripts --javascript-delay 10000  --footer-right \"Date: [date] [time]\" "
                + "--footer-center \"Page: [page] of [toPage]\" --footer-line --footer-font-size \"9\" --footer-spacing 5 --footer-font-name \"calibri light\"";
            var pdfFile = new ViewAsPdf(
                "~/Views/GamerService/PrintAll.cshtml",
                response.ServiceDTOs
            )
            {
                CustomSwitches = footer,
                PageMargins = new Margins(2, 2, 2, 2),
                PageOrientation = Orientation.Portrait,
                PageSize = Size.A4
            };
            return pdfFile;
        }

        #endregion

        #region GamerService list

        [AuthorizePerRole(SystemConstants.Permission.ListGamerService)]
        public IActionResult All(
            string Search,
            string OrderByColumn,
            bool? IsDesc,
            int PageIndex,
            DateTime? From,
            DateTime? To,
            int ZoneId,
            int StatusType,
            int GamerId,
            int GameId,
            int LessPrice,
            int GreeterPrice,
            int LessLevel,
            int GreeterLevel,
            GameServiceType GameServiceType,
            string ActionType = null
        )
        {
            ViewModel<ServiceDTO> ViewData = new ViewModel<ServiceDTO>();
            var filter = new ServiceDTO()
            {
                StatusType = StatusType,
                GamerId = GamerId,
                GameId = GameId,
                Search = Search,
                LessPrice = LessPrice,
                GreeterPrice = GreeterPrice,
                LessLevel = LessLevel,
                GreeterLevel = GreeterLevel,
                GameServiceType = GameServiceType
            };
            if (ActionType != SystemConstants.ActionType.Table)
            {
                ViewData.Lookup = BaseHelper.GetLookup(
                    new List<byte>
                    {
                        (byte)EnumSelectListType.Game,
                        (byte)EnumSelectListType.Gamer,
                    },
                    _context
                );
            }

            if (From.HasValue)
                filter.DateFrom = From.Value;
            if (To.HasValue)
                filter.DateTo = To.Value;

            var request = new GamerServiceRequest
            {
                RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
                UserID = AuthHelper.GetClaimValue(User, "UserID"),
                context = _context,
                IsDesc = IsDesc ?? true,
                PageIndex = PageIndex,
                PageSize = BaseHelper.Constants.PageSize,
                OrderByColumn = OrderByColumn ?? nameof(GamerService.CreatedAt),
                applyFilter = true,
                ServiceDTO = filter
            };

            var response = BL.Services.GamerServiceService.GetAllGamerServices(request);

            ViewData.ObjDTOs = response.ServiceDTOs;

            if (ActionType == SystemConstants.ActionType.PartialView)
                return PartialView(ViewData);
            else if (ActionType == SystemConstants.ActionType.Table)
            {
                if (StatusType == (int)StatusTypeEnum.All)
                    return PartialView("_All", response.ServiceDTOs);
                else if (StatusType == (int)StatusTypeEnum.InProgress)
                    return PartialView("_Current", response.ServiceDTOs);
                else if (StatusType == (int)StatusTypeEnum.Accept)
                    return PartialView("_Current", response.ServiceDTOs);
                else if (StatusType == (int)StatusTypeEnum.Published)
                    return PartialView("_Current", response.ServiceDTOs);
                else if (StatusType == (int)StatusTypeEnum.Reject)
                    return PartialView("_Current", response.ServiceDTOs);
                else if (StatusType == (int)StatusTypeEnum.Sold)
                    return PartialView("_Current", response.ServiceDTOs);
                else
                    return PartialView("_All", response.ServiceDTOs);
            }
            else
                return View(ViewData);
        }

        #endregion
    }
}

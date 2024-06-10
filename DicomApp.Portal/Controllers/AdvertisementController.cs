using System;
using System.Collections.Generic;
using System.Linq;
using DicomApp.BL.Services;
using DicomApp.CommonDefinitions.DTO;
using DicomApp.CommonDefinitions.DTO.AdvertisementDTOs;
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
    public class AdvertisementController : Controller
    {
        private readonly ShippingDBContext _context;
        private readonly IHostingEnvironment _hosting;

        public AdvertisementController(ShippingDBContext context, IHostingEnvironment hosting)
        {
            _context = context;
            _hosting = hosting;
        }

        #region Advertisement details popup

        [AuthorizePerRole(SystemConstants.Permission.TrackAdvertisement)]
        public ActionResult TrackAdvertisement(string TrackAdvertisement)
        {
            var RoleID = AuthHelper.GetClaimValue(User, "RoleID");
            var UserID = AuthHelper.GetClaimValue(User, "UserID");

            var model = new AdsDTO();
            model.RefId = TrackAdvertisement;
            if (RoleID == (int)EnumRole.Gamer)
                model.GamerId = UserID;

            var request = new AdvertisementRequest
            {
                RoleID = RoleID,
                UserID = UserID,
                context = _context,
            };

            var response = DicomApp.BL.Services.AdvertisementService.GetAdvertisement(request);
            return PartialView("~/Views/Shared/Advertisement/_MainDetails.cshtml", response.AdsDTO);
        }

        [AllowAnonymous]
        public IActionResult AdvertisementDetails(int id)
        {
            var model = new AdsDTO();
            model.AdvertisementId = id;
            var request = new AdvertisementRequest
            {
                RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
                UserID = AuthHelper.GetClaimValue(User, "UserID"),
                context = _context,
            };

            var response = DicomApp.BL.Services.AdvertisementService.GetAdvertisement(request);

            return PartialView("~/Views/Shared/Advertisement/_MainDetails.cshtml", response.AdsDTO);
        }
        #endregion

        #region Advertisement actions

        [AuthorizePerRole(SystemConstants.Permission.AddAdvertisement)]
        [HttpPost]
        public IActionResult Add(AdsDTO model)
        {
            var request = new AdvertisementRequest();
            request.context = _context;
            request.RoleID = AuthHelper.GetClaimValue(User, "RoleID");
            request.UserID = AuthHelper.GetClaimValue(User, "UserID");
            request.AdsDTO = model;
            request.RoutPath = _hosting.WebRootPath;

            if (request.RoleID == (int)EnumRole.Gamer)
                model.GamerId = request.UserID;

            var response = BL.Services.AdvertisementService.AddAdvertisement(request);

            ViewBag.Vendors = GeneralHelper.GetUsers(SystemConstants.Role.Gamer, _context);
            ViewBag.branch = _context.Branch.ToList();
            ViewBag.areas = _context.City.ToList();

            return Json(response.Message);
        }

        [AuthorizePerRole(SystemConstants.Permission.AddAdvertisement)]
        public IActionResult Add(string ActionType = null)
        {
            ViewBag.GameList = GameService
                .GetGames(new GameRequest { context = _context })
                .GameDTOs;

            ViewBag.Gamers = GeneralHelper.GetUsers(SystemConstants.Role.Gamer, _context);

            if (ActionType == SystemConstants.ActionType.PartialView)
                return PartialView("/Views/Shared/Advertisement/_AddOrder.cshtml");
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

        [AuthorizePerRole(SystemConstants.Permission.EditAdvertisement)]
        public IActionResult Edit(int adsID)
        {
            ViewBag.GameList = GameService
                .GetGames(new GameRequest { context = _context })
                .GameDTOs;

            ViewBag.Gamers = GeneralHelper.GetUsers(SystemConstants.Role.Gamer, _context);

            var model = new AdsDTO();
            model.AdvertisementId = adsID;
            var request = new AdvertisementRequest
            {
                RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
                UserID = AuthHelper.GetClaimValue(User, "UserID"),
                context = _context,
                AdsDTO = model
            };

            var response = DicomApp.BL.Services.AdvertisementService.GetAdvertisement(request);

            return PartialView("_Edit", response.AdsDTO);
        }

        [AuthorizePerRole(SystemConstants.Permission.EditAdvertisement)]
        [HttpPost]
        public ActionResult Edit(AdsDTO model)
        {
            var request = new AdvertisementRequest();
            request.context = _context;
            request.RoleID = AuthHelper.GetClaimValue(User, "RoleID");
            request.UserID = AuthHelper.GetClaimValue(User, "UserID");
            var response = BL.Services.AdvertisementService.EditAdvertisement(request);
            ViewBag.Vendors = GeneralHelper.GetUsers(SystemConstants.Role.Gamer, _context);
            ViewBag.branch = _context.Branch.ToList();
            ViewBag.areas = _context.City.ToList();

            return Json(response.Message);
        }

        [AuthorizePerRole(SystemConstants.Permission.EditBasicAdvertisement)]
        [HttpPost]
        public IActionResult EditBasicAdvertisement(
            int adsID,
            string Description,
            string UserName,
            string Password,
            string Level
        )
        {
            var request = new AdvertisementRequest
            {
                context = _context,
                RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
                UserID = AuthHelper.GetClaimValue(User, "UserID"),
                AdsDTO = new AdsDTO
                {
                    AdvertisementId = adsID,
                    Description = Description,
                    UserName = UserName,
                    Password = Password,
                    Level = Level
                }
            };

            var response = BL.Services.AdvertisementService.EditBasicAdvertisement(request);

            return Json(new { success = response.Success, message = response.Message });
        }

        [AuthorizePerRole(SystemConstants.Permission.PrintAdvertisement)]
        public IActionResult Print(int AdvertisementId)
        {
            var request = new AdvertisementRequest
            {
                RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
                UserID = AuthHelper.GetClaimValue(User, "UserID"),
                context = _context,
                AdsDTO = new AdsDTO { AdvertisementId = AdvertisementId }
            };

            var response = BL.Services.AdvertisementService.GetAdvertisement(request);

            string footer =
                "--no-stop-slow-scripts --javascript-delay 10000  --footer-right \"Date: [date] [time]\" "
                + "--footer-center \"Page: [page] of [toPage]\" --footer-line --footer-font-size \"9\" --footer-spacing 5 --footer-font-name \"calibri light\"";
            var pdfFile = new ViewAsPdf("~/Views/Advertisement/Print.cshtml", response.AdsDTO)
            {
                CustomSwitches = footer,
                PageMargins = new Margins(2, 2, 2, 2),
                PageOrientation = Orientation.Portrait,
                PageSize = Size.A4
            };
            return pdfFile;
        }

        [AuthorizePerRole(SystemConstants.Permission.PrintAdvertisement)]
        public IActionResult PrintAll(string AdvertisementsIds)
        {
            List<int> AdvertisementsList = AdvertisementsIds
                .Split(',')
                .Select<string, int>(int.Parse)
                .ToList();
            var request = new AdvertisementRequest
            {
                RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
                UserID = AuthHelper.GetClaimValue(User, "UserID"),
                context = _context,
                applyFilter = true,
                AdsDTO = new AdsDTO { AdvertisementIds = AdvertisementsList }
            };
            var response = BL.Services.AdvertisementService.GetAllAdvertisements(request);

            string footer =
                "--no-stop-slow-scripts --javascript-delay 10000  --footer-right \"Date: [date] [time]\" "
                + "--footer-center \"Page: [page] of [toPage]\" --footer-line --footer-font-size \"9\" --footer-spacing 5 --footer-font-name \"calibri light\"";
            var pdfFile = new ViewAsPdf("~/Views/Advertisement/PrintAll.cshtml", response.AdsDTOs)
            {
                CustomSwitches = footer,
                PageMargins = new Margins(2, 2, 2, 2),
                PageOrientation = Orientation.Portrait,
                PageSize = Size.A4
            };
            return pdfFile;
        }

        #endregion

        #region Advertisement list

        [AuthorizePerRole(SystemConstants.Permission.ListAdvertisement)]
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
            int AreaId,
            string ActionType = null
        )
        {
            ViewModel<AdsDTO> ViewData = new ViewModel<AdsDTO>();
            var filter = new AdsDTO()
            {
                StatusType = StatusType,
                GamerId = GamerId,
                Search = Search
            };
            if (ActionType != SystemConstants.ActionType.Table)
            {
                ViewData.Lookup = BaseHelper.GetLookup(
                    new List<byte>
                    {
                        (byte)EnumSelectListType.Zone,
                        (byte)EnumSelectListType.Vendor,
                        (byte)EnumSelectListType.Area,
                        (byte)EnumSelectListType.Status
                    },
                    _context
                );
            }

            if (From.HasValue)
                filter.DateFrom = From.Value;
            if (To.HasValue)
                filter.DateTo = To.Value;

            var request = new AdvertisementRequest
            {
                RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
                UserID = AuthHelper.GetClaimValue(User, "UserID"),
                context = _context,
                IsDesc = IsDesc ?? true,
                PageIndex = PageIndex,
                PageSize = BaseHelper.Constants.PageSize,
                OrderByColumn = OrderByColumn,
                applyFilter = true,
                AdsDTO = filter
            };

            var response = BL.Services.AdvertisementService.GetAllAdvertisements(request);

            ViewData.ObjDTOs = response.AdsDTOs;

            if (ActionType == SystemConstants.ActionType.PartialView)
                return PartialView(ViewData);
            else if (ActionType == SystemConstants.ActionType.Table)
            {
                if (StatusType == (int)StatusTypeEnum.All)
                    return PartialView("_All", response.AdsDTOs);
                else if (StatusType == (int)StatusTypeEnum.InProgress)
                    return PartialView("_Current", response.AdsDTOs);
                else if (StatusType == (int)StatusTypeEnum.Accept)
                    return PartialView("_Current", response.AdsDTOs);
                else if (StatusType == (int)StatusTypeEnum.Published)
                    return PartialView("_Current", response.AdsDTOs);
                else if (StatusType == (int)StatusTypeEnum.Reject)
                    return PartialView("_Cancelled", response.AdsDTOs);
                else if (StatusType == (int)StatusTypeEnum.Sold)
                    return PartialView("_Done", response.AdsDTOs);
                else
                    return PartialView("_All", response.AdsDTOs);
            }
            else
                return View(ViewData);
        }

        #endregion
    }
}

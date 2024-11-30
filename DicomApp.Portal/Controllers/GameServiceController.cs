using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DicomApp.BL.Services;
using DicomApp.CommonDefinitions.DTO;
using DicomApp.CommonDefinitions.DTO.CashDTOs;
using DicomApp.CommonDefinitions.Requests;
using DicomApp.CommonDefinitions.Responses;
using DicomApp.DAL.DB;
using DicomApp.DAL.Migrations;
using DicomApp.Helpers;
using DicomApp.Helpers.Services.PayPal;
using DicomApp.Helpers.Services.Telegram;
using DicomApp.Portal.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using PayPal;
using Rotativa.AspNetCore;
using Rotativa.AspNetCore.Options;
using Telegram.Bot.Types;
using static DicomApp.Helpers.SystemConstants;

namespace DicomApp.Portal.Controllers
{
    public class GamerServiceController : Controller
    {
        private readonly ShippingDBContext _context;
        private readonly IHostingEnvironment _hosting;
        private readonly IPayPalService _payPalService;
        private readonly ITelegramService _telegramService;
        readonly IConfiguration _configuration;

        public GamerServiceController(
            ShippingDBContext context,
            IHostingEnvironment hosting,
            IPayPalService payPalService,
            ITelegramService telegramService,
            IConfiguration configuration
        )
        {
            _context = context;
            _hosting = hosting;
            _configuration = configuration;
            _payPalService = payPalService;
            _telegramService = telegramService;
        }

        #region GamerService actions

        [AuthorizePerRole(SystemConstants.Permission.AddGamerService)]
        [HttpPost]
        public async Task<IActionResult> Add(ServiceDTO model)
        {
            var response = new GamerServiceResponse();
            try
            {
                var roleName = AuthHelper.GetRoleName(User, SystemConstants.Claims.RoleName);
                var request = new GamerServiceRequest();
                var roleID = AuthHelper.GetClaimValue(User, SystemConstants.Claims.RoleID);
                var userID = AuthHelper.GetClaimValue(User, SystemConstants.Claims.UserID);
                request.context = _context;
                request.RoleID = roleID;
                request.UserID = userID;
                request.ServiceDTO = model;
                request.RoutPath = _hosting.WebRootPath;

                if (roleName == SystemConstants.Role.Gamer)
                    model.GamerId = request.UserID;

                response = BL.Services.GamerServiceService.AddGamerService(request);
                if (model.GameChargeId.HasValue)
                    await AddPayment(userID, roleID, response);

                //request.PageIndex = 0;
                //request.PageSize = BaseHelper.Constants.PageSize;
                //request.IsDesc = true;
                //request.OrderByColumn = nameof(GamerService.CreatedAt);
                //var Getresponse = BL.Services.GamerServiceService.GetAllGamerServices(request);
                return Json(response);
            }
            catch (Exception)
            {
                return Json(response);
            }
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
        public async Task<ActionResult> ChangStatusGamerService(int ID, int status, int price)
        {
            var userID = AuthHelper.GetClaimValue(User, "UserID");
            var roleID = AuthHelper.GetClaimValue(User, "RoleID");
            var request = new GamerServiceRequest();
            request.context = _context;
            request.RoleID = roleID;
            request.UserID = userID;
            request.ServiceDTO.GamerServiceId = ID;
            request.ServiceDTO.StatusType = status;
            request.ServiceDTO.Price = price;
            var response = BL.Services.GamerServiceService.ChangStatusGamerService(request);
            if (
                response.Success
                && response.ServiceDTO.GameServiceType == GameServiceType.Push
                && response.ServiceDTO.StatusType == (int)StatusTypeEnum.Accept
            )
                await AddPayment(userID, roleID, response);

            return Json(response.Message);
        }

        private async Task AddPayment(int userID, int roleID, GamerServiceResponse response)
        {
            var returnUrl =
                $"{_configuration["AppDomain"]}/Payment/SuccessPay?ServiceId={response.ServiceDTO.GamerServiceId}";
            var cancelUrl = $"{_configuration["AppDomain"]}/Payment/CancelPay";
            var createPayment = await _payPalService.PurchaseAccountAsync(
                response.ServiceDTO.Price,
                returnUrl,
                cancelUrl
            );
            var approverUrl = createPayment
                ?.links?.Find(x => x.rel.ToLower() == "approval_url")
                ?.href;
            if (!string.IsNullOrEmpty(approverUrl))
            {
                TransactionService.AddTransaction(
                    new TransactionRequest
                    {
                        UserID = userID,
                        RoleID = roleID,
                        context = _context,
                        TransactionDTO = new TransactionDTO
                        {
                            ServiceId = response.ServiceDTO.GamerServiceId,
                            PaymentId = createPayment.id,
                            Amount = response.ServiceDTO.GamerServiceId,
                            BuyerId = response.ServiceDTO.GamerId,
                            TransactionSource = TransactionSourceEnum.Paypal,
                            TransactionType =
                                response.ServiceDTO.GameServiceType == GameServiceType.Charge
                                    ? TransactionTypeEnum.Change
                                    : TransactionTypeEnum.Push
                        }
                    }
                );
                var userResponse = UserService.GetUser(
                    new UserRequest
                    {
                        context = _context,
                        UserDTO = new UserDTO { Id = response.ServiceDTO.GamerId }
                    }
                );
                if (userResponse.Success)
                {
                    var messageResult = await _telegramService.SendMessage(
                        userResponse.UserDTO.TelegramUserName,
                        $"Pay on this Link {approverUrl}"
                    );
                }
            }
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
            string Level,
            string CurrentLevel
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
                    CurrentLevel = CurrentLevel
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

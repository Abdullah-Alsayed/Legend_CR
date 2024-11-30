using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Threading.Tasks;
using DicomApp.BL.Services;
using DicomApp.CommonDefinitions.DTO;
using DicomApp.CommonDefinitions.DTO.AdvertisementDTOs;
using DicomApp.CommonDefinitions.DTO.CashDTOs;
using DicomApp.CommonDefinitions.DTO.VendorProductDTOs;
using DicomApp.CommonDefinitions.Requests;
using DicomApp.CommonDefinitions.Responses;
using DicomApp.DAL.DB;
using DicomApp.Helpers;
using DicomApp.Helpers.Services.PayPal;
using DicomApp.Portal.Helpers;
using ECommerce.Core.Services.MailServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using PayPal;
using static DicomApp.Helpers.SystemConstants;

namespace DicomApp.Portal.Controllers
{
    [Authorize]
    public class GamerController : Controller
    {
        private readonly ShippingDBContext _context;
        readonly IPayPalService _payPalService;
        readonly IConfiguration _configuration;
        readonly IMailServices _mailServices;
        readonly IStringLocalizer<GamerController> _stringLocalizer;

        public GamerController(
            ShippingDBContext context,
            IPayPalService payPalService,
            IConfiguration configuration,
            IMailServices mailServices,
            IStringLocalizer<GamerController> stringLocalizer
        )
        {
            _context = context;
            _payPalService = payPalService;
            _configuration = configuration;
            _mailServices = mailServices;
            _stringLocalizer = stringLocalizer;
        }

        [AllowAnonymous]
        public IActionResult Main(
            string Search,
            int PageIndex,
            DateTime? From,
            DateTime? To,
            string ActionType = null
        )
        {
            var mainResponse = new MainResponse();
            var userID = AuthHelper.GetClaimValue(User, "UserID");
            var roleID = AuthHelper.GetClaimValue(User, "RoleID");
            var model = new AdsDTO();
            //model.Search = Search;

            //if (From.HasValue)
            //    model.DateFrom = From.Value;

            //if (To.HasValue)
            //    model.DateTo = To.Value;

            var PageSize =
                ActionType == SystemConstants.ActionType.Print ? 0 : BaseHelper.Constants.PageSize;

            model.StatusTypes = new List<int>
            {
                (int)StatusTypeEnum.Pending,
                (int)StatusTypeEnum.Published
            };
            var advertisementRequest = new AdvertisementRequest
            {
                RoleID = roleID,
                UserID = userID,
                context = _context,
                AdsDTO = model,
                applyFilter = true,
                PageIndex = PageIndex,
                PageSize = 20,
                OrderByColumn = nameof(Advertisement.Price),
            };
            ViewBag.GameList = GameService
                .GetGames(new GameRequest { context = _context })
                .GameDTOs;
            mainResponse.TopAdvertisements = DicomApp
                .BL.Services.AdvertisementService.GetAllAdvertisements(advertisementRequest)
                .AdsDTOs;

            mainResponse.Games = GameService
                .GetGames(new GameRequest { context = _context })
                .GameDTOs;

            mainResponse.Testimonials = TestimonialService
                .GetTestimonials(
                    new TestimonialRequest
                    {
                        context = _context,
                        IsDesc = true,
                        OrderByColumn = nameof(Testimonial.CreatedAt),
                        PageSize = PageSize
                    }
                )
                .TestimonialDTOs;

            mainResponse.CanAddFeedBack = TestimonialService
                .CanAddTestimonials(
                    new TestimonialRequest
                    {
                        context = _context,
                        UserID = userID,
                        RoleID = roleID
                    }
                )
                .Success;

            advertisementRequest.PageSize = 30;
            advertisementRequest.OrderByColumn = nameof(Advertisement.CreatedAt);
            mainResponse.AllAdvertisements = DicomApp
                .BL.Services.AdvertisementService.GetAllAdvertisements(advertisementRequest)
                .AdsDTOs;

            if (ActionType == SystemConstants.ActionType.PartialView)
                return PartialView("_advertisementList", mainResponse);
            else if (ActionType == SystemConstants.ActionType.Table)
                return PartialView("_advertisementListTable", mainResponse);
            else
                return View(mainResponse);
        }

        public IActionResult All(
            string Search,
            string OrderByColumn,
            bool? IsDesc,
            int PageIndex,
            DateTime? From,
            DateTime? To,
            int StatusType,
            int GameId,
            int LessPrice,
            int GreeterPrice,
            int LessLevel,
            int GreeterLevel,
            string ActionType = null
        )
        {
            ViewModel<AdsDTO> ViewData = new ViewModel<AdsDTO>();
            var filter = new AdsDTO()
            {
                StatusType = StatusType,
                GameId = GameId,
                Search = Search,
                LessPrice = LessPrice,
                GreeterPrice = GreeterPrice,
                LessLevel = LessLevel,
                GreeterLevel = GreeterLevel,

                StatusTypes = new List<int>
                {
                    (int)StatusTypeEnum.Pending,
                    (int)StatusTypeEnum.Published
                }
            };

            if (ActionType != SystemConstants.ActionType.PartialView)
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

            var request = new AdvertisementRequest
            {
                RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
                UserID = AuthHelper.GetClaimValue(User, "UserID"),
                context = _context,
                IsDesc = IsDesc ?? true,
                PageIndex = PageIndex,
                PageSize = BaseHelper.Constants.PageSize,
                OrderByColumn = OrderByColumn ?? nameof(Advertisement.CreatedAt),
                applyFilter = true,
                AdsDTO = filter
            };
            var response = BL.Services.AdvertisementService.GetAllAdvertisements(request);
            ViewData.ObjDTOs = response.AdsDTOs;
            if (ActionType == SystemConstants.ActionType.PartialView)
                return PartialView("_All", response.AdsDTOs);
            else
                return View(ViewData);
        }

        [AuthorizePerRole("Vendor_ContactUs")]
        public IActionResult ContactUs(string ActionType = null)
        {
            if (ActionType == SystemConstants.ActionType.PartialView)
                return PartialView("_ContactUs");
            else
                return View();
        }

        [AuthorizePerRole("Vendor_ContactUs")]
        [HttpPost]
        public IActionResult ContactUs(ContactUsDTO model)
        {
            var ContactUsRequest = new ContactUsRequest
            {
                RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
                UserID = AuthHelper.GetClaimValue(User, "UserID"),
                context = _context,
                ContactUsDTO = model
            };
            var ContactUsRespons = ContactUsService.AddContactUs(ContactUsRequest);
            if (ContactUsRespons.Success)
                return Json(ContactUsRespons.Message);
            else
                return Json(false);
        }

        public IActionResult MyAccount(string ActionType = null)
        {
            ViewModel<UserDTO> ViewModel = new ViewModel<UserDTO>();
            var userRequest = new UserRequest
            {
                RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
                UserID = AuthHelper.GetClaimValue(User, "UserID"),
                context = _context,
                UserDTO = new UserDTO { Id = AuthHelper.GetClaimValue(User, "UserID") }
            };
            var userResponse = UserService.GetUser(userRequest);
            ViewModel.ObjDTO = userResponse.UserDTO;
            if (userResponse.Success)
            {
                ViewModel.Lookup = BaseHelper.GetLookup(
                    new List<byte> { (byte)EnumSelectListType.Area, (byte)EnumSelectListType.Zone },
                    _context
                );

                if (ActionType == SystemConstants.ActionType.PartialView)
                    return PartialView("_MyAccount", ViewModel);
                else
                    return View(ViewModel);
            }
            else
                return RedirectToAction("AccountDashboard", "Home");
        }

        public IActionResult advertisementDetails(int id)
        {
            //var model = new advertisementDTO();
            //model.IsDeleted = false;
            //model.AdvertisementId = id;
            //model.VendorId = AuthHelper.GetClaimValue(User, "UserID");
            //var advertisementRequest = new advertisementRequest
            //{
            //    RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
            //    UserID = AuthHelper.GetClaimValue(User, "UserID"),
            //    context = _context,
            //    advertisementDTO = model,
            //    IsDetails = true,
            //    OrderByColumn = "AdvertisementId"
            //};
            //var userResponse = DicomApp.BL.Services.advertisementService.Listadvertisement(advertisementRequest);
            //return PartialView("_advertisementDetails", userResponse.AdvertisementDTOs.FirstOrDefault());
            return View();
        }

        [HttpPost]
        public IActionResult ChangePassword(
            string ConfirmPassword,
            string NewPassword,
            string Password
        )
        {
            int UserID = AuthHelper.GetClaimValue(User, "UserID");
            CommonUser user = _context.CommonUser.Find(UserID);

            if (user.Password == Password)
            {
                if (NewPassword == ConfirmPassword)
                {
                    user.Password = NewPassword;
                    _context.SaveChanges();

                    return Json("Password has been changed successfully");
                }
                else
                    return Json("New password does not match");
            }
            else
                return Json("Old password is incorrect");
        }

        [HttpPost]
        public ActionResult EditUser(UserDTO model)
        {
            var userRequest = new UserRequest
            {
                RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
                UserID = AuthHelper.GetClaimValue(User, "UserID"),
                context = _context,
                UserDTO = model
            };
            var userResponse = UserService.EditUser(userRequest);
            if (userResponse.Success)
            {
                return Json(userResponse.Message);
            }
            return Json(userResponse.Message);
        }
    }
}

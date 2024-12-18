﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using DicomApp.BL.Services;
using DicomApp.BLL;
using DicomApp.CommonDefinitions.DTO;
using DicomApp.CommonDefinitions.Requests;
using DicomApp.CommonDefinitions.Responses;
using DicomApp.DAL.DB;
using DicomApp.Helpers;
using DicomApp.Helpers.Services.GenrateAvatar;
using DicomApp.Helpers.Services.GetCounter;
using DicomApp.Portal.Helpers;
using ECommerce.Core.Services.MailServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Telegram.Bot.Types;

namespace DicomApp.Portal.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly ShippingDBContext _context;
        private readonly AvatarService _avatarService;
        private readonly IApiCountryService _countryService;
        private readonly IHostingEnvironment hosting;
        private readonly IMailServices _emailSender;
        private readonly IStringLocalizer<UserController> _stringLocalizer;

        public UserController(
            ShippingDBContext context,
            IHostingEnvironment hosting,
            AvatarService avatarService,
            IApiCountryService countryService,
            IMailServices mailServices,
            IStringLocalizer<UserController> stringLocalizer
        )
        {
            _context = context;
            _stringLocalizer = stringLocalizer;
            _emailSender = mailServices;
            this.hosting = hosting;
            _avatarService = avatarService;
            _countryService = countryService;
        }

        private string GetUserIp()
        {
            var ipAddress = HttpContext.Connection.RemoteIpAddress;

            if (Request.Headers.ContainsKey("X-Forwarded-For"))
                ipAddress = IPAddress.Parse(Request.Headers["X-Forwarded-For"]);

            return ipAddress?.ToString();
        }

        private void GetViewBags()
        {
            var roleRequest = new RoleRequest
            {
                RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
                UserID = AuthHelper.GetClaimValue(User, "UserID"),
                context = _context
            };
            var roleResponse = RoleService.ListRole(roleRequest);
            if (roleResponse.Success)
                ViewBag.roles = roleResponse.RoleDTOs.ToList();
            else
                ViewBag.error = roleResponse.Message;

            //var sectionRequest = new SectionRequest
            //{
            //    RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
            //    CreatedBy = AuthHelper.GetClaimValue(User, "UserID"),
            //    _context = _context
            //};
            //var sectionResponse = SectionService.ListSection(sectionRequest);
            //ViewBag.sections = sectionResponse.SectionRecords;
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult Create()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult LoginPatient()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(UserDTO model)
        {
            var loginRequest = new UserRequest { UserDTO = model, context = _context };
            var loginResponse = UserService.Login(loginRequest);
            if (loginResponse.Success && loginResponse.UserDTOs.Count > 0)
            {
                var user = loginResponse.UserDTOs[0];
                var claims = new List<Claim>
                {
                    new Claim("UserID", user.Id.ToString()),
                    new Claim("Name", user.Name ?? ""),
                    new Claim("Email", user.Email ?? ""),
                    new Claim("RoleID", user.RoleID.ToString()),
                    new Claim("RoleName", user.RoleName),
                    new Claim("Language", user.Language ?? SystemConstants.Languages.Arabic)
                };

                var claimsIdentity = new ClaimsIdentity(
                    claims,
                    CookieAuthenticationDefaults.AuthenticationScheme
                );

                var authProperties = new AuthenticationProperties { AllowRefresh = true, };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties
                );
                if (user.RoleName == SystemConstants.Role.Gamer)
                    return RedirectToAction("Main", "Gamer");
                else
                    return RedirectToAction("main", "Dashboard");
            }
            ViewBag.error = loginResponse.Message;
            return View(model);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> Register(UserDTO model)
        {
            try
            {
                UserResponse response;
                PasswordHasher<object> hasher = new PasswordHasher<object>();
                model.HashedPassword = hasher.HashPassword(model, model.Password);
                var role = RoleService
                    .GetRole(
                        new RoleRequest
                        {
                            context = _context,
                            applyFilter = true,
                            RoleDTO = new RoleDTO { Name = SystemConstants.Role.Gamer }
                        }
                    )
                    .RoleDTO;
                model.RoleID = role.Id;
                var userRequest = new UserRequest
                {
                    RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
                    UserID = AuthHelper.GetClaimValue(User, "UserID"),
                    context = _context,
                    UserDTO = model,
                    WebRootPath = hosting.WebRootPath,
                    avatarService = _avatarService,
                    CountryService = _countryService,
                    UserIP = GetUserIp()
                };

                response = await UserService.AddUser(userRequest);
                if (response.Success)
                {
                    var claims = new List<Claim>
                    {
                        new Claim("UserID", response.UserDTO.Id.ToString()),
                        new Claim("Name", response.UserDTO.Name ?? ""),
                        new Claim("Email", response.UserDTO.Email ?? ""),
                        new Claim("RoleID", response.UserDTO.RoleID.ToString()),
                        new Claim("RoleName", role.Name)
                    };

                    var claimsIdentity = new ClaimsIdentity(
                        claims,
                        CookieAuthenticationDefaults.AuthenticationScheme
                    );
                    var authProperties = new AuthenticationProperties { AllowRefresh = true, };
                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        authProperties
                    );
                }
                return Json(response);
            }
            catch (Exception ex)
            {
                return Json(new UserResponse { Success = false, Message = ex.Message });
            }
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Main", "Gamer");
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordDTO model)
        {
            if (ModelState.IsValid)
            {
                var userResponse = UserService.GetUser(
                    new UserRequest
                    {
                        context = _context,
                        UserDTO = new UserDTO { Email = model.Email }
                    }
                );
                if (userResponse.UserDTO == null)
                    return RedirectToAction("ForgotPasswordConfirmation");

                // Generate the token using a custom method
                var token = GeneratePasswordResetToken();

                // Build the reset URL
                var callbackUrl = Url.Action(
                    "ResetPassword",
                    "User",
                    new { token, email = userResponse.UserDTO.Email },
                    protocol: Request.Scheme
                );

                // Send the email
                var result = await _emailSender.SendAsync(
                    new EmailDto
                    {
                        Body =
                            $"{_stringLocalizer["resetPasswordMessage"]} <a href='{callbackUrl}'>{_stringLocalizer["here"]}</a>.",
                        Email = model.Email,
                        Subject = $"{_stringLocalizer["resetPassword"]}"
                    }
                );

                return RedirectToAction("ForgotPasswordConfirmation");
            }

            return View(model);
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string token = null)
        {
            if (token == null)
            {
                return BadRequest("A token must be supplied for password reset.");
            }
            var model = new ResetPasswordDTO { Token = token };
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult ResetPassword(ResetPasswordDTO model)
        {
            var userRespons = UserService.GetUser(
                new UserRequest
                {
                    context = _context,
                    UserDTO = new UserDTO { Email = model.EmailUser }
                }
            );
            if (userRespons.UserDTO == null)
                return RedirectToAction("ResetPasswordConfirmation");

            // Update the user's password
            var userRequest = new UserRequest
            {
                RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
                UserID = AuthHelper.GetClaimValue(User, "UserID"),
                context = _context,
                UserDTO = new UserDTO
                {
                    Id = userRespons.UserDTO.Id,
                    ConfirmPassword = model.Password,
                    NewPassword = model.Password
                },
            };

            var userResponse = UserService.ChangePassword(userRequest);
            return RedirectToAction("ResetPasswordConfirmation");
        }

        [AllowAnonymous]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        public IActionResult NotFound()
        {
            return PartialView();
        }

        [AuthorizePerRole(SystemConstants.Permission.ListStaff)]
        public ActionResult ListUser(
            int RoleId,
            string Search,
            bool IsDesc,
            int PageIndex,
            string ActionType = null
        )
        {
            var ViewData = new ViewModel<UserDTO>();

            ViewData.Lookup = BaseHelper.GetLookup(
                new List<byte> { (byte)EnumSelectListType.Role },
                _context
            );
            var currRoleName = AuthHelper.GetClaimStringValue(User, "RoleName");
            if (
                currRoleName != SystemConstants.Role.SuperAdmin
                || currRoleName != SystemConstants.Role.Admin
            )
                ViewData.Lookup.RoleDTOs = ViewData
                    .Lookup.RoleDTOs.Where(r =>
                        r.Name != SystemConstants.Role.Gamer
                        && r.Name != SystemConstants.Role.SuperAdmin
                    )
                    .ToList();

            var userRequest = new UserRequest
            {
                context = _context,
                PageSize = BaseHelper.Constants.PageSize,
                RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
                UserID = AuthHelper.GetClaimValue(User, "UserID"),
                UserDTO = new UserDTO()
                {
                    RoleID = RoleId,
                    Search = Search,
                    StaffOnly = true
                },
                PageIndex = PageIndex,
                IsDesc = IsDesc,
                applyFilter = true,
            };
            var userResponse = UserService.GetAllUsers(userRequest);

            ViewData.ObjDTOs = userResponse.UserDTOs;
            if (ActionType == SystemConstants.ActionType.PartialView)
                return PartialView(ViewData);
            else if (ActionType == SystemConstants.ActionType.Table)
                return PartialView("_ListUser", userResponse.UserDTOs);
            else if (ActionType == SystemConstants.ActionType.Print)
                return BaseHelper.GeneratePDF<List<UserDTO>>(
                    "StaffReportPDF",
                    userResponse.UserDTOs
                );
            else
                return View(ViewData);
        }

        [AuthorizePerRole(SystemConstants.Permission.ListGamer)]
        public ActionResult ListGamer(
            string Search,
            bool IsDesc,
            int GamerID,
            int PageIndex,
            string ActionType = null
        )
        {
            var ViewData = new ViewModel<UserDTO>();

            ViewData.Lookup = BaseHelper.GetLookup(
                new List<byte> { (byte)EnumSelectListType.Countries, },
                _context
            );
            var userRequest = new UserRequest
            {
                context = _context,
                PageSize = BaseHelper.Constants.PageSize,
                RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
                UserID = AuthHelper.GetClaimValue(User, "UserID"),
                UserDTO = new UserDTO()
                {
                    RoleName = SystemConstants.Role.Gamer,
                    Search = Search,
                    Id = GamerID
                },
                PageIndex = PageIndex,
                IsDesc = IsDesc,
                applyFilter = true,
            };
            var userResponse = UserService.GetAllUsers(userRequest);

            ViewData.ObjDTOs = userResponse.UserDTOs.OrderByDescending(u => u.Id).ToList();
            if (ActionType == SystemConstants.ActionType.PartialView)
                return PartialView(ViewData);
            else if (ActionType == SystemConstants.ActionType.Table)
                return PartialView("_ListGamer", userResponse.UserDTOs);
            else if (ActionType == SystemConstants.ActionType.Print)
                return BaseHelper.GeneratePDF<List<UserDTO>>(
                    "AccountReportPDF",
                    userResponse.UserDTOs
                );
            else
                return View(ViewData);
        }

        [AuthorizePerRole(SystemConstants.Permission.AddGamer)]
        [HttpGet]
        public IActionResult GamerDetails(int ID = 0, string ActionType = null)
        {
            var ViewData = new ViewModel<UserDTO>();
            ViewData.ObjDTO = new UserDTO();
            ViewData.Lookup = BaseHelper.GetLookup(
                new List<byte> { (byte)EnumSelectListType.Countries, },
                _context
            );

            if (ID > 0)
            {
                var userRequest = new UserRequest
                {
                    RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
                    UserID = AuthHelper.GetClaimValue(User, "UserID"),
                    context = _context,
                    UserDTO = new UserDTO { Id = ID },
                };
                var userResponse = UserService.GetUser(userRequest);
                ViewData.ObjDTO = userResponse.UserDTO;
                if (ActionType == SystemConstants.ActionType.PartialView)
                    return PartialView(ViewData);
                else
                    return View(ViewData);
            }
            else
            {
                if (ActionType == SystemConstants.ActionType.PartialView)
                    return PartialView(ViewData);
                else
                    return View(ViewData);
            }
        }

        [AuthorizePerRole(SystemConstants.Permission.ListStaff)]
        [HttpPost]
        public async Task<ActionResult> SaveGamer(UserDTO model)
        {
            try
            {
                UserResponse response;
                PasswordHasher<object> hasher = new PasswordHasher<object>();
                model.HashedPassword = hasher.HashPassword(model, model.Password);

                var roleId = RoleService
                    .GetRole(
                        new RoleRequest
                        {
                            context = _context,
                            applyFilter = true,
                            RoleDTO = new RoleDTO { Name = SystemConstants.Role.Gamer }
                        }
                    )
                    .RoleDTO.Id;
                model.RoleID = roleId;
                var userRequest = new UserRequest
                {
                    RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
                    UserID = AuthHelper.GetClaimValue(User, "UserID"),
                    context = _context,
                    UserDTO = model,
                    WebRootPath = hosting.WebRootPath,
                    avatarService = _avatarService
                };
                if (model.Id > 0)
                    response = UserService.EditUser(userRequest);
                else
                    response = await UserService.AddUser(userRequest);

                return Json(response);
            }
            catch (Exception ex)
            {
                return Json(new UserResponse { Success = false, Message = ex.Message });
            }
        }

        [AuthorizePerRole(SystemConstants.Permission.ListStaff)]
        public ActionResult LoadUser(
            string orderByColumn = null,
            string searchVal = null,
            int pageIndex = 0,
            int roleID = 0,
            bool staffOnly = false,
            string Search = null
        )
        {
            return ViewComponent(
                "UserVComponent",
                new
                {
                    orderByColumn = orderByColumn,
                    searchVal = searchVal,
                    pageIndex = pageIndex,
                    roleID,
                    staffOnly,
                    Search
                }
            );
        }

        [AuthorizePerRole(SystemConstants.Permission.DeleteStaff)]
        public IActionResult DeleteUser(int ID)
        {
            var userRequest = new UserRequest
            {
                context = _context,
                UserDTO = new UserDTO { Id = ID },
                RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
                UserID = AuthHelper.GetClaimValue(User, "UserID"),
            };

            var userResponse = UserService.DeleteUser(userRequest);

            if (userResponse.Success)
                TempData["SuccessMsg"] = userResponse.Message;
            else
                TempData["ErrorMsg"] = userResponse.Message;

            return RedirectToAction("ListUser");
        }

        [AuthorizePerRole(SystemConstants.Permission.AddStaff)]
        public ActionResult AddUser(long roleid)
        {
            GetViewBags();
            ViewBag.roleID = roleid;
            return View();
        }

        [AuthorizePerRole(SystemConstants.Permission.AddStaff)]
        [HttpPost]
        public async Task<ActionResult> AddUser(UserDTO model)
        {
            model.ImgUrl = BaseHelper.UploadImg(model.File, hosting.WebRootPath, model.ImgUrl);

            PasswordHasher<object> hasher = new PasswordHasher<object>();
            model.HashedPassword = hasher.HashPassword(model, model.Password);

            var userRequest = new UserRequest
            {
                RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
                UserID = AuthHelper.GetClaimValue(User, "UserID"),
                context = _context,
                UserDTO = model,
                avatarService = _avatarService,
                WebRootPath = hosting.WebRootPath
            };

            var userResponse = await UserService.AddUser(userRequest);

            if (userResponse.Success)
                TempData["SuccessMsg"] = userResponse.Message;
            else
                TempData["ErrorMsg"] = userResponse.Message;

            return RedirectToAction("ListUser");
        }

        [AuthorizePerRole(SystemConstants.Permission.GetUserData)]
        public IActionResult GetUserData(int Id)
        {
            var userRequest = new UserRequest
            {
                RoleID = (AuthHelper.GetClaimValue(User, "RoleID")),
                UserID = (AuthHelper.GetClaimValue(User, "UserID")),
                context = _context,
                UserDTO = new UserDTO() { Id = Id },
                applyFilter = true,
            };
            var UserData = UserService.GetUser(userRequest);

            if (UserData.Success)
                return Json(UserData.UserDTO);
            else
                return Json(false);
        }

        [AuthorizePerRole(SystemConstants.Permission.EditStaff)]
        public ActionResult EditUser(string ActionType = null)
        {
            var userRequest = new UserRequest
            {
                RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
                UserID = AuthHelper.GetClaimValue(User, "UserID"),
                context = _context,
                UserDTO = new UserDTO { Id = AuthHelper.GetClaimValue(User, "UserID") },
                applyFilter = true
            };
            var userResponse = UserService.GetAllUsers(userRequest);

            if (userResponse.Success)
            {
                if (ActionType == SystemConstants.ActionType.PartialView)
                    return PartialView("_EditUser", userResponse.UserDTOs[0]);
                else
                    return View(userResponse.UserDTOs[0]);
            }
            else
                return View();
        }

        [AuthorizePerRole(SystemConstants.Permission.EditStaff)]
        [HttpPost]
        public ActionResult EditUser(UserDTO model)
        {
            model.ImgUrl = BaseHelper.UploadImg(model.File, hosting.WebRootPath, model.ImgUrl);
            var userRequest = new UserRequest
            {
                RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
                UserID = AuthHelper.GetClaimValue(User, "UserID"),
                context = _context,
                UserDTO = model
            };
            var userResponse = UserService.EditUser(userRequest);

            if (userResponse.Success)
                TempData["SuccessMsg"] = userResponse.Message;
            else
                TempData["ErrorMsg"] = userResponse.Message;

            return RedirectToAction("ListUser");
        }

        [AuthorizePerRole(SystemConstants.Permission.EditStaff)]
        [HttpPost]
        public async Task<ActionResult> EditUserInfo(UserDTO model)
        {
            model.ImgUrl = BaseHelper.UploadImg(model.File, hosting.WebRootPath, model.ImgUrl);
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
                var currentUser = HttpContext.User;
                var claimsIdentity = currentUser.Identity as ClaimsIdentity;

                if (claimsIdentity != null)
                {
                    claimsIdentity.RemoveClaim(
                        claimsIdentity.FindFirst(SystemConstants.Claims.Name)
                    );
                    claimsIdentity.RemoveClaim(
                        claimsIdentity.FindFirst(SystemConstants.Claims.Email)
                    );

                    // Add updated claims
                    claimsIdentity.AddClaim(
                        new Claim(SystemConstants.Claims.Name, userResponse.UserDTO.Name ?? "")
                    );
                    claimsIdentity.AddClaim(
                        new Claim(SystemConstants.Claims.Email, userResponse.UserDTO.Email ?? "")
                    );

                    // Update the authentication cookie
                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        new AuthenticationProperties { AllowRefresh = true }
                    );
                }
            }

            return Json(userResponse);
        }

        [AllowAnonymous]
        public async Task<ActionResult> EditLanguage(string language, string navigate)
        {
            var userRequest = new UserRequest
            {
                RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
                UserID = AuthHelper.GetClaimValue(User, "UserID"),
                context = _context,
                UserDTO = new UserDTO { Language = language }
            };
            var userResponse = UserService.EditLanguage(userRequest);
            if (userResponse.Success)
            {
                var currentUser = HttpContext.User;
                var claimsIdentity = currentUser.Identity as ClaimsIdentity;

                if (claimsIdentity != null)
                {
                    // Remove existing claims
                    claimsIdentity.RemoveClaim(
                        claimsIdentity.FindFirst(SystemConstants.Claims.Language)
                    );

                    // Add updated claims
                    claimsIdentity.AddClaim(
                        new Claim(
                            SystemConstants.Claims.Language,
                            userResponse.UserDTO.Language ?? ""
                        )
                    );

                    // Update the authentication cookie
                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        new AuthenticationProperties { AllowRefresh = true }
                    );
                }
            }
            Response.Cookies.Append(
                "PreferredLanguage",
                language,
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );
            if (navigate == SystemConstants.Layout.Dashboard)
                return RedirectToAction("Main", "Dashboard");
            else
                return RedirectToAction("Main", "Gamer");
        }

        [AllowAnonymous]
        public IActionResult ForgetPass()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult ForgetPass(string Email)
        {
            var user = _context.CommonUser.FirstOrDefault(p => p.Email == Email);
            if (user != null)
            {
                var body =
                    @"<style type='text/css'> html{ background-color: #E1E1E1; margin: 0; padding: 0; } body, #bodyTable, #bodyCell, #bodyCell { height: 100% !important; margin: 0; padding: 0; width: 100% !important; font-family: Helvetica, Arial, 'Lucida Grande', sans-serif; } table { border-collapse: collapse; } table[id=bodyTable] { width: 100% !important; margin: auto; max-width: 500px !important; color: #7A7A7A; font-weight: normal; } img, a img { border: 0; outline: none; text-decoration: none; height: auto; line-height: 100%; } a { text-decoration: none !important; border-bottom: 1px solid; } h1, h2, h3, h4, h5, h6 { color: #5F5F5F; font-weight: normal; font-family: Helvetica; font-size: 20px; line-height: 125%; text-align: Left; letter-spacing: normal; margin-top: 0; margin-right: 0; margin-bottom: 10px; margin-left: 0; padding-top: 0; padding-bottom: 0; padding-left: 0; padding-right: 0; } /* CLIENT-SPECIFIC STYLES */ .ReadMsgBody { width: 100%; } .ExternalClass { width: 100%; } /* Force Hotmail/Outlook.com to display emails at full width. */ .ExternalClass, .ExternalClass p, .ExternalClass span, .ExternalClass font, .ExternalClass td, .ExternalClass div { line-height: 100%; } /* Force Hotmail/Outlook.com to display line heights normally. */ table, td { mso-table-lspace: 0pt; mso-table-rspace: 0pt; } /* Remove spacing between tables in Outlook 2007 and up. */ #outlook a { padding: 0; } /* Force Outlook 2007 and up to provide a 'view in browser' message. */ img { -ms-interpolation-mode: bicubic; display: block; outline: none; text-decoration: none; } /* Force IE to smoothly render resized images. */ body, table, td, p, a, li, blockquote { -ms-text-size-adjust: 100%; -webkit-text-size-adjust: 100%; font-weight: normal !important; } /* Prevent Windows- and Webkit-based mobile platforms from changing declared text sizes. */ .ExternalClass td[class='ecxflexibleContainerBox'] h3 { padding-top: 10px !important; } /* Force hotmail to push 2-grid sub headers down */ /* /\/\/\/\/\/\/\/\/ TEMPLATE STYLES /\/\/\/\/\/\/\/\/ */ /* ========== Page Styles ========== */ h1 { display: block; font-size: 26px; font-style: normal; font-weight: normal; line-height: 100%; } h2 { display: block; font-size: 20px; font-style: normal; font-weight: normal; line-height: 120%; } h3 { display: block; font-size: 17px; font-style: normal; font-weight: normal; line-height: 110%; } h4 { display: block; font-size: 18px; font-style: italic; font-weight: normal; line-height: 100%; } .flexibleImage { height: auto; } .linkRemoveBorder { border-bottom: 0 !important; } table[class=flexibleContainerCellDivider] { padding-bottom: 0 !important; padding-top: 0 !important; } body, #bodyTable { background-color: #E1E1E1; } #emailHeader { background-color: #E1E1E1; } #emailBody { background-color: #FFFFFF; } #emailFooter { background-color: #E1E1E1; } .nestedContainer { background-color: #F8F8F8; border: 1px solid #CCCCCC; } .emailButton { background-color: #205478; border-collapse: separate; } .buttonContent { color: #FFFFFF; font-family: Helvetica; font-size: 18px; font-weight: bold; line-height: 100%; padding: 15px; text-align: center; } .buttonContent a { color: #FFFFFF; display: block; text-decoration: none !important; border: 0 !important; } .emailCalendar { background-color: #FFFFFF; border: 1px solid #CCCCCC; } .emailCalendarMonth { background-color: #205478; color: #FFFFFF; font-family: Helvetica, Arial, sans-serif; font-size: 16px; font-weight: bold; padding-top: 10px; padding-bottom: 10px; text-align: center; } .emailCalendarDay { color: #205478; font-family: Helvetica, Arial, sans-serif; font-size: 60px; font-weight: bold; line-height: 100%; padding-top: 20px; padding-bottom: 20px; text-align: center; } .imageContentText { margin-top: 10px; line-height: 0; } .imageContentText a { line-height: 0; } #invisibleIntroduction { display: none !important; } /* Removing the introduction text from the view */ /*FRAMEWORK HACKS & OVERRIDES */ span[class=ios-color-hack] a { color: #275100 !important; text-decoration: none !important; } /* Remove all link colors in IOS (below are duplicates based on the color preference) */ span[class=ios-color-hack2] a { color: #205478 !important; text-decoration: none !important; } span[class=ios-color-hack3] a { color: #8B8B8B !important; text-decoration: none !important; } /* A nice and clean way to target phone numbers you want clickable and avoid a mobile phone from linking other numbers that look like, but are not phone numbers. Use these two blocks of code to 'unstyle' any numbers that may be linked. The second block gives you a class to apply with a span tag to the numbers you would like linked and styled. Inspired by Campaign Monitor's article on using phone numbers in email: http://www.campaignmonitor.com/blog/post/3571/using-phone-numbers-in-html-email/. */ .a[href^='tel'], a[href^='sms'] { text-decoration: none !important; color: #606060 !important; pointer-events: none !important; cursor: default !important; } .mobile_link a[href^='tel'], .mobile_link a[href^='sms'] { text-decoration: none !important; color: #606060 !important; pointer-events: auto !important; cursor: default !important; } /* MOBILE STYLES */</style>"
                    + "<center style='background-color:#E1E1E1;'><table border='0' cellpadding='0' cellspacing='0' height='100%' width='100%' id='bodyTable' style='table-layout: fixed;max-width:100% !important;width: 100% !important;min-width: 100% !important;'> <tr> <td align='center' valign='top' id='bodyCell'> <table bgcolor='#E1E1E1' border='0' cellpadding='0' cellspacing='0' width='500' id='emailHeader'> <tr> <td align='center' valign='top'> <table border='0' cellpadding='0' cellspacing='0' width='100%'> <tr> <td align='center' valign='top'> <table border='0' cellpadding='10' cellspacing='0' width='500' class='flexibleContainer'> <tr> <td valign='top' width='500' class='flexibleContainerCell'> <table align='left' border='0' cellpadding='0' cellspacing='0' width='100%'> <tr> <td align='left' valign='middle' id='invisibleIntroduction' class='flexibleContainerBox' style='display:none !important; mso-hide:all;'> <table border='0' cellpadding='0' cellspacing='0' width='100%' style='max-width:100%;'> <tr> <td align='left' class='textContent'> <div style='font-family:Helvetica,Arial,sans-serif;font-size:13px;color:#828282;text-align:center;line-height:120%;'> emalaf </div> </td> </tr> </table> </td> <td align='right' valign='middle' class='flexibleContainerBox'> <table border='0' cellpadding='0' cellspacing='0' width='100%' style='max-width:100%;'> <tr> <td align='left' class='textContent'> <div style='font-family:Helvetica,Arial,sans-serif;font-size:13px;color:#828282;text-align:center;line-height:120%;'> If you can't see this message, <a href='#' target='_blank' style='text-decoration:none;border-bottom:1px solid #828282;color:#828282;'><span style='color:#828282;'>view&nbsp;it&nbsp;in&nbsp;your&nbsp;browser</span></a>. </div> </td> </tr> </table> </td> </tr> </table> </td> </tr> </table> </td> </tr> </table> </td> </tr> </table>"
                    + "<table bgcolor='#FFFFFF' border='0' cellpadding='0' cellspacing='0' width='500' id='emailBody'> <tr> <td align='center' valign='top'> <table border='0' cellpadding='0' cellspacing='0' width='100%' style='color:#FFFFFF;' bgcolor='#3498db'> <tr> <td align='center' valign='top'> <table border='0' cellpadding='0' cellspacing='0' width='500' class='flexibleContainer'> <tr> <td align='center' valign='top' width='500' class='flexibleContainerCell'> <table border='0' cellpadding='30' cellspacing='0' width='100%'> <tr> <td align='center' valign='top' class='textContent'> <div><img src='http://95.216.142.45/dicom/front/img/logo.png' height='40px' /></div> <h1 style='color:#FFFFFF;line-height:100%;font-family:Helvetica,Arial,sans-serif;font-size:35px;font-weight:normal;margin-bottom:5px;text-align:center;'>emalaf</h1> </td> </tr> </table> </td> </tr> </table> </td> </tr> </table> </td> </tr> <tr> <td align='center' valign='top'>"
                    + "<table border='0' cellpadding='0' cellspacing='0' width='100%' bgcolor='#F8F8F8'> <tr> <td align='center' valign='top'> <table border='0' cellpadding='0' cellspacing='0' width='500' class='flexibleContainer'> <tr> <td align='center' valign='top' width='500' class='flexibleContainerCell'> <table border='0' cellpadding='30' cellspacing='0' width='100%'> <tr> <td align='center' valign='top'> <table border='0' cellpadding='0' cellspacing='0' width='100%'> <tr> <td valign='top' class='textContent'> <h3 mc:edit='header' style='color:#5F5F5F;line-height:125%;font-family:Helvetica,Arial,sans-serif;font-size:15px;font-weight:normal;margin-top:0;margin-bottom:3px;text-align:left;'>"
                    + "Dear "
                    + user.Name
                    + ", <br /> <span>your account information to access our portal and services, we will be in touch soon.</span> <br /> Username: "
                    + "<b>"
                    + user.Email
                    + "</b>"
                    + "<br />Password:"
                    + "<b>"
                    + user.Password
                    + "</b><br /><br /> Thanks "
                    + "<br /><br /><br /> contacts: (+200) 11111111,&nbsp;<span> (+200) 111 1112 4554 <br /></span> Support@emalaf.net<br />' </h3> <div mc:edit='body' style='text-align:left;font-family:Helvetica,Arial,sans-serif;font-size:15px;margin-bottom:0;font-weight:bold;color:#5F5F5F;line-height:135%;'></div> <h3 mc:edit='header' style='color:#5F5F5F;line-height:125%;font-family:Helvetica,Arial,sans-serif;font-size:15px;font-weight:normal;margin-top:0;margin-bottom:3px;text-align:left;'> we will be in touch soon</h3> </td> </tr> </table> </td> </tr> </table> </td> </tr> </table> </td> </tr> </table> </td> </tr> </table> <table bgcolor='#E1E1E1' border='0' cellpadding='0' cellspacing='0' width='500' id='emailFooter'> <tr> <td align='center' valign='top'> <table border='0' cellpadding='0' cellspacing='0' width='100%'> <tr> <td align='center' valign='top'> <table border='0' cellpadding='0' cellspacing='0' width='500' class='flexibleContainer'> <tr> <td align='center' valign='top' width='500' class='flexibleContainerCell'> <table border='0' cellpadding='30' cellspacing='0' width='100%'> <tr> <td valign='top' bgcolor='#E1E1E1'> <div style='font-family:Helvetica,Arial,sans-serif;font-size:13px;color:#828282;text-align:center;line-height:120%;'> <div>Copyright &#169; 2021 <a href='#' target='_blank' style='text-decoration:none;color:#828282;'><span style='color:#828282;'>GT</span></a>. All&nbsp;rights&nbsp;reserved.</div> <div>Support@Barmagah.net</div> <br /> (+202) 010 108 448 80 <br /> </div> </td> </tr> </table> </td> </tr> </table> </td> </tr> </table> </td> </tr> </table> </td> </tr> </table></center>";
                var mailSender = new MailSender();
                mailSender.SendMail(user.Email, "Forget Password | emalaf", body, true);
                TempData["SuccessMsg"] = "Account information sent successfully to your email";
            }
            else
            {
                TempData["ErrorMsg"] =
                    "Sorry, We have not any information about this email address";
            }
            return View();
        }

        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ChangePassword(string confirmPass, string newPass, int id)
        {
            var userRequest = new UserRequest
            {
                RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
                UserID = AuthHelper.GetClaimValue(User, "UserID"),
                context = _context,
                UserDTO = new UserDTO
                {
                    Id = id,
                    ConfirmPassword = confirmPass,
                    NewPassword = newPass
                },
            };

            var userResponse = UserService.ChangePassword(userRequest);

            return Json(userResponse);
        }

        [AllowAnonymous]
        public IActionResult Authorization()
        {
            return View();
        }

        public string GeneratePasswordResetToken()
        {
            // Generate a random token (you can use more secure methods like JWT if needed)
            using (var rng = new RNGCryptoServiceProvider())
            {
                byte[] tokenData = new byte[32];
                rng.GetBytes(tokenData);
                return Convert.ToBase64String(tokenData);
            }
        }
    }
}

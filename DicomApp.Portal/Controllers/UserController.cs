using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DicomApp.BL.Services;
using DicomApp.BLL;
using DicomApp.CommonDefinitions.DTO;
using DicomApp.CommonDefinitions.DTO.ShipmentDTOs;
using DicomApp.CommonDefinitions.Requests;
using DicomApp.CommonDefinitions.Responses;
using DicomApp.DAL.DB;
using DicomApp.Helpers;
using DicomApp.Portal.Helpers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;

namespace DicomApp.Portal.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly ShippingDBContext _context;
        private readonly IHostingEnvironment hosting;

        public UserController(ShippingDBContext context, IHostingEnvironment hosting)
        {
            _context = context;
            this.hosting = hosting;
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

            ViewBag.areas = _context.City.ToList();

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
                    new Claim("RoleName", user.RoleName)
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
                switch (user.RoleID)
                {
                    case (int)EnumRole.Vendor:
                        return RedirectToAction("AccountDashboard", "Home");
                    case (int)EnumRole.AccountManager:
                        return RedirectToAction("AccountDashboard", "Home");
                    case (int)EnumRole.Accounting:
                        return RedirectToAction("Invoices", "Invoice");
                    case (int)EnumRole.BranchManger:
                        return RedirectToAction("shipmentsReport", "User");
                    case (int)EnumRole.WarehouseManager:
                        return RedirectToAction("Shipments", "Warehouse");
                    case (int)EnumRole.DataEntry:
                        return RedirectToAction("CustomerFollowup", "User");
                    default:
                        return RedirectToAction("Index", "Home");
                }
            }
            ViewBag.error = loginResponse.Message;
            //IndicatorAutoFill();
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Login");
        }

        public IActionResult NotFound()
        {
            return PartialView();
        }

        [AuthorizePerRole("StaffList")]
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
            var currRoleID = AuthHelper.GetClaimValue(User, "RoleID");
            if (currRoleID == (int)EnumRole.SuperAdmin)
                ViewData.Lookup.RoleDTOs = ViewData
                    .Lookup.RoleDTOs.Where(r => r.Id != (int)EnumRole.Vendor)
                    .ToList();
            else if (currRoleID == (int)EnumRole.Admin)
                ViewData.Lookup.RoleDTOs = ViewData
                    .Lookup.RoleDTOs.Where(r =>
                        r.Id != (int)EnumRole.SuperAdmin && r.Id != (int)EnumRole.Vendor
                    )
                    .ToList();
            else
                ViewData.Lookup.RoleDTOs = ViewData
                    .Lookup.RoleDTOs.Where(r =>
                        r.Id != (int)EnumRole.SuperAdmin
                        && r.Id != (int)EnumRole.Admin
                        && r.Id != (int)EnumRole.Vendor
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

        [AuthorizePerRole("VendorsList")]
        public ActionResult ListAccount(
            string Search,
            bool IsDesc,
            int VendorID,
            int PageIndex,
            string ActionType = null
        )
        {
            var ViewData = new ViewModel<UserDTO>();

            ViewData.Lookup = BaseHelper.GetLookup(
                new List<byte>
                {
                    (byte)EnumSelectListType.Vendor,
                    (byte)EnumSelectListType.Zone,
                    (byte)EnumSelectListType.Area
                },
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
                    RoleID = (int)EnumRole.Vendor,
                    Search = Search,
                    Id = VendorID
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
                return PartialView("_ListAccount", userResponse.UserDTOs);
            else if (ActionType == SystemConstants.ActionType.Print)
                return BaseHelper.GeneratePDF<List<UserDTO>>(
                    "AccountReportPDF",
                    userResponse.UserDTOs
                );
            else
                return View(ViewData);
        }

        [AuthorizePerRole("VendorsAdd")]
        [HttpGet]
        public IActionResult VendorDetails(int ID = 0)
        {
            ViewBag.ZoneList = ZoneService
                .GetZones(new ZoneRequest { context = _context })
                .ZoneDTOs;

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

                return View(userResponse.UserDTO);
            }
            else
                return View();
        }

        [AuthorizePerRole("VendorsAdd")]
        [HttpPost]
        public ActionResult SaveVendor(UserDTO model)
        {
            model.ImgUrl = BaseHelper.UploadImg(model.File, hosting.WebRootPath, model.ImgUrl);

            PasswordHasher<object> hasher = new PasswordHasher<object>();
            model.HashedPassword = hasher.HashPassword(model, model.Password);
            model.RoleID = (int)EnumRole.Vendor;

            var userRequest = new UserRequest
            {
                RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
                UserID = AuthHelper.GetClaimValue(User, "UserID"),
                context = _context,
                UserDTO = model
            };

            UserResponse response;
            if (model.Id > 0)
                response = UserService.EditUser(userRequest);
            else
                response = UserService.AddUser(userRequest);

            if (response.Success)
            {
                TempData["SuccessMsg"] = "Saved";
                return RedirectToAction("ListAccount");
            }
            else
            {
                TempData["ErrorMsg"] = response.Message;
                return RedirectToAction("VendorDetails", new { ID = 0 });
            }
        }

        [AuthorizePerRole("Staff")]
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

        [AuthorizePerRole("StaffDelete")]
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

        [AuthorizePerRole("StaffAdd")]
        public ActionResult AddUser(long roleid)
        {
            GetViewBags();
            ViewBag.roleID = roleid;
            return View();
        }

        [AuthorizePerRole("StaffAdd")]
        [HttpPost]
        public ActionResult AddUser(UserDTO model)
        {
            model.ImgUrl = BaseHelper.UploadImg(model.File, hosting.WebRootPath, model.ImgUrl);

            PasswordHasher<object> hasher = new PasswordHasher<object>();
            model.HashedPassword = hasher.HashPassword(model, model.Password);

            var userRequest = new UserRequest
            {
                RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
                UserID = AuthHelper.GetClaimValue(User, "UserID"),
                context = _context,
                UserDTO = model
            };

            var userResponse = UserService.AddUser(userRequest);

            if (userResponse.Success)
                TempData["SuccessMsg"] = userResponse.Message;
            else
                TempData["ErrorMsg"] = userResponse.Message;

            return RedirectToAction("ListUser");
        }

        [AuthorizePerRole("GetUserData")]
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
            var UserData = UserService.GetAllUsers(userRequest);

            if (UserData.Success)
                return Json(UserData.UserDTOs.FirstOrDefault());
            else
                return Json(false);
        }

        [AuthorizePerRole("StaffEdit")]
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
                ViewBag.areas = _context.City.ToList();
                ViewBag.Zone = _context.Zone.ToList();

                if (ActionType == SystemConstants.ActionType.PartialView)
                    return PartialView("_EditUser", userResponse.UserDTOs[0]);
                else
                    return View(userResponse.UserDTOs[0]);
            }
            else
                return View();
        }

        [AuthorizePerRole("StaffEdit")]
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
        public IActionResult ChangePassword(UserDTO model)
        {
            if (model.NewPassword != model.ConfirmPassword)
            {
                TempData["ErrorMsg"] = "Password didn't match";
                return RedirectToAction("ListUser");
            }

            var userRequest = new UserRequest
            {
                RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
                UserID = AuthHelper.GetClaimValue(User, "UserID"),
                context = _context,
                UserDTO = model,
            };

            var userResponse = UserService.ChangePassword(userRequest);

            if (userResponse.Success)
                TempData["SuccessMsg"] = userResponse.Message;
            else
                TempData["ErrorMsg"] = userResponse.Message;

            return RedirectToAction("ListUser");
        }
    }
}

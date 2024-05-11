//using DicomDB.CommonDefinitions.Requests;
//using LegendCR.BL.Services;
//using LegendCR.CommonDefinitions.DTO;
//using LegendCR.CommonDefinitions.Records;
//using LegendCR.CommonDefinitions.Requests;
//using LegendCR.DAL.DB;
//using LegendCR.Helpers;
//using LegendCR.Portal.Helpers;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;

//namespace DicomDB.Portal.Controllers
//{
//    public class AdminController : Controller
//    {
//        private readonly ApplicationDBContext _context;

//        public AdminController(ApplicationDBContext context)
//        {
//            _context = context;
//        }

//        #region Zone Manager

//        [AuthorizePerRole("ZoneList")]
//        public ActionResult ZoneList(string ActionType, string Search)
//        {
//            ViewModel<ZoneDTO> ViewData = new ViewModel<ZoneDTO>();
//            var ZoneRequest = new ZoneRequest
//            {
//                RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
//                UserID = AuthHelper.GetClaimValue(User, "UserID"),
//                context = _context,
//                ZoneDTO = new ZoneDTO { Search = Search },
//                HasDetails = true
//            };
//            var ZoneRespons = ZoneService.GetZones(ZoneRequest);
//            ViewData.ObjDTOs = ZoneRespons.ZoneDTOs;
//            ViewData.Lookup = BaseHelper.GetLookup(new List<byte> {
//                (byte)EnumSelectListType.Area},
//                _context);

//            if (ActionType == Constants.ActionType.PartialView)
//                return PartialView(ViewData);

//            else if (ActionType == Constants.ActionType.Table)
//                return PartialView("_ZoneList", ViewData);

//            else if (ActionType == Constants.ActionType.Print)
//                return BaseHelper.GeneratePDF<List<ZoneDTO>>("ZoneReportPDF", ViewData.ObjDTOs);

//            else
//                return View(ViewData);
//        }

//        [HttpPost]
//        [AuthorizePerRole("ZoneAdd")]
//        public ActionResult AddZone(ZoneDTO model)
//        {
//            if (model.AreaList is null || model.AreaList.Count == 0)
//            {
//                TempData["ErrorMsg"] = "Please select area(s)";
//                return RedirectToAction("ZoneList");
//            }

//            var ZoneRequest = new ZoneRequest
//            {
//                RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
//                UserID = AuthHelper.GetClaimValue(User, "UserID"),
//                context = _context,
//                ZoneDTO = model
//            };

//            var ZoneRespons = ZoneService.AddZone(ZoneRequest);

//            //return Json(new { success = ZoneRespons.Success, message = ZoneRespons.Message });

//            if (ZoneRespons.Success)
//                TempData["SuccessMsg"] = "New route Added successfully";
//            else
//                TempData["ErrorMsg"] = "Faild route added";

//            return RedirectToAction("ZoneList");
//        }

//        [HttpPost]
//        [AuthorizePerRole("ZoneEdit")]
//        public ActionResult EditZone(ZoneDTO model)
//        {
//            if (model.AreaList is null || model.AreaList.Count == 0)
//            {
//                TempData["ErrorMsg"] = "Please select area(s)";
//                return RedirectToAction("ZoneList");
//            }

//            var ZoneRequest = new ZoneRequest
//            {
//                RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
//                UserID = AuthHelper.GetClaimValue(User, "UserID"),
//                context = _context,
//                ZoneDTO = model
//            };

//            var ZoneRespons = ZoneService.EditZone(ZoneRequest);

//            if (ZoneRespons.Success)
//                TempData["SuccessMsg"] = "route updated successfully";
//            else
//                TempData["ErrorMsg"] = "Faild route Updated";

//            return RedirectToAction("ZoneList");
//        }

//        [HttpGet]
//        [AuthorizePerRole("ZoneDelete")]
//        public ActionResult DeleteZone(int ID)
//        {
//            var ZoneRequest = new ZoneRequest
//            {
//                RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
//                UserID = AuthHelper.GetClaimValue(User, "UserID"),
//                context = _context,
//                ZoneDTO = new ZoneDTO { Id = ID }
//            };

//            var ZoneRespons = ZoneService.DeleteZone(ZoneRequest);

//            if (ZoneRespons.Success)
//                TempData["SuccessMsg"] = "route deleted successfully";
//            else
//                TempData["ErrorMsg"] = "Faild to delete route";

//            return RedirectToAction("ZoneList");
//        }

//        [AuthorizePerRole("ZoneList")]
//        public IActionResult RouteList(string ActionType)
//        {
//            if (ActionType == Constants.ActionType.PartialView)
//                return PartialView();
//            else
//                return View();
//        }

//        #endregion

//        #region Area Manager

//        [AuthorizePerRole("AreaList")]
//        public ActionResult AreaList(string ActionType, string Search, int ZoneId)
//        {
//            ViewModel<CityDTO> ViewData = new ViewModel<CityDTO>();
//            var CityRequest = new CityRequest
//            {
//                RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
//                UserID = (AuthHelper.GetClaimValue(User, "UserID")),
//                context = _context,
//                CityDTO = new CityDTO() { ZoneId = ZoneId, Search = Search },
//            };
//            ViewData.Lookup = BaseHelper.GetLookup(new List<byte> {
//                             (byte)EnumSelectListType.Zone }, _context);

//            var CityResponse = CityService.GetCity(CityRequest);
//            ViewData.ObjDTOs = CityResponse.CityDTOs;

//            if (ActionType == Constants.ActionType.PartialView)
//                return PartialView(ViewData);

//            else if (ActionType == Constants.ActionType.Table)
//                return PartialView("_AreaList", ViewData);

//            else if (ActionType == Constants.ActionType.Print)
//                return BaseHelper.GeneratePDF<List<CityDTO>>("AreaReportPDF", CityResponse.CityDTOs);

//            else
//                return View(ViewData);
//        }

//        [HttpPost]
//        [AuthorizePerRole("AreaAdd")]
//        public ActionResult AddArea(CityDTO model)
//        {
//            if (model.ZoneId is null || model.ZoneId == 0)
//            {
//                TempData["ErrorMsg"] = "Please Select Route (Governorate)";
//                return RedirectToAction("AreaList");
//            }

//            var CityRequest = new CityRequest
//            {
//                RoleID = (AuthHelper.GetClaimValue(User, "RoleID")),
//                UserID = (AuthHelper.GetClaimValue(User, "UserID")),
//                context = _context,
//                CityDTO = model,
//            };

//            var CityResponse = CityService.AddCity(CityRequest);

//            if (CityResponse.Success)
//                TempData["SuccessMsg"] = "New area added successfully";
//            else
//                TempData["ErrorMsg"] = "Faild area added";

//            return RedirectToAction("AreaList");
//        }

//        [AuthorizePerRole("AreaEdit")]
//        [HttpPost]
//        public ActionResult EditArea(CityDTO model)
//        {
//            if (model.ZoneId is null || model.ZoneId == 0)
//            {
//                TempData["ErrorMsg"] = "Please Select Route (Governorate)";
//                return RedirectToAction("AreaList");
//            }

//            int UserID = (AuthHelper.GetClaimValue(User, "UserID"));
//            model.LastModifiedAt = DateTime.Now;
//            model.LastModifiedBy = UserID.ToString();

//            var CityRequest = new CityRequest
//            {
//                RoleID = (AuthHelper.GetClaimValue(User, "RoleID")),
//                UserID = UserID,
//                context = _context,
//                CityDTO = model,
//            };

//            var CityResponse = CityService.EditCity(CityRequest);

//            if (CityResponse.Success)
//                TempData["SuccessMsg"] = "Area updated successfully";
//            else
//                TempData["ErrorMsg"] = "Faild Area Updated";

//            return RedirectToAction("AreaList");
//        }

//        [AuthorizePerRole("AreaDelete")]
//        public ActionResult DeletArea(int ID)
//        {
//            var CityRequest = new CityRequest
//            {
//                RoleID = (AuthHelper.GetClaimValue(User, "RoleID")),
//                UserID = (AuthHelper.GetClaimValue(User, "UserID")),
//                context = _context,
//                CityDTO = new CityDTO { Id = ID },
//            };

//            var CityResponse = CityService.DeleteCity(CityRequest);

//            if (CityResponse.Success)
//                TempData["SuccessMsg"] = "Area deleted successfully";
//            else
//                TempData["ErrorMsg"] = "Faild to delete area";

//            return RedirectToAction("AreaList");
//        }

//        public ActionResult AssignAreas(int ZoneId, string AreasId)
//        {
//            var AreaList = AreasId.Split(',').Select<string, int>(int.Parse).ToList();
//            City Area = new City();
//            foreach (var area in AreaList)
//            {
//                Area = _context.City.FirstOrDefault(a => a.Id == area);
//                if (Area != null)
//                {
//                    Area.ZoneId = ZoneId;
//                    _context.SaveChanges();
//                }
//                else
//                    return Json(false);
//            }
//            return Json(true);
//        }

//        public ActionResult GetAreas(int ID)
//        {
//            ViewBag.ZoneID = ID;
//            return PartialView("_GetAreas", _context.City.ToList());
//        }

//        [HttpGet]
//        public ActionResult GetAreasByZone(int ID)
//        {
//            return Json(_context.City.Where(a => a.ZoneId == ID).ToList());
//        }

//        #endregion

//        #region Zone Tax Manager

//        [AuthorizePerRole("ZoneTaxList")]
//        public ActionResult ZoneTax(string ActionType)
//        {
//            var ZoneTax = _context.ZoneTax.Include(c => c.Zone).OrderByDescending(c => c.Id).ToList();
//            if (ActionType == Constants.ActionType.PartialView)
//                return PartialView(ZoneTax);
//            else
//                return View(ZoneTax);
//        }

//        [AuthorizePerRole("ZoneTaxEdit")]
//        [HttpPost]
//        public ActionResult EditZoneTax(ZoneTax model, int ZoneTaxID)
//        {
//            ZoneTax zonetax = _context.ZoneTax.Find(ZoneTaxID);
//            if (zonetax == null)
//            {
//                return RedirectToAction("ZoneTax");
//            }

//            int UserID = AuthHelper.GetClaimValue(User, "UserID");
//            zonetax.Tax = model.Tax;
//            zonetax.LastModifiedAt = DateTime.Now;
//            zonetax.LastModifiedBy = UserID;
//            _context.SaveChanges();

//            TempData["SuccessMsg"] = "Zone fees updated successfully";
//            return RedirectToAction("ZoneTax");
//        }

//        #endregion
//    }
//}
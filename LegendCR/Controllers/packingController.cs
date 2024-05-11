
//using LegendCR.BL.Services;
//using LegendCR.CommonDefinitions.DTO;
//using LegendCR.CommonDefinitions.Requests;
//using LegendCR.CommonDefinitions.Responses;
//using LegendCR.DAL.DB;
//using LegendCR.Helpers;
//using LegendCR.Portal.Helpers;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

//namespace LegendCR.Portal.Controllers
//{
//    [Authorize]
//    public class PackingController : Controller
//    {
//        private readonly ApplicationDBContext _context;
//        private readonly IHostingEnvironment hosting;

//        public PackingController(ApplicationDBContext context, IHostingEnvironment hosting)
//        {
//            _context = context;
//            this.hosting = hosting;
//        }

//        [HttpPost]
//        [AuthorizePerRole("PackingAdd")]
//        public IActionResult Addpacking(PackingDTO model)
//        {
//            model.ImgUrl = BaseHelper.UploadImg(model.File, hosting.WebRootPath, model.ImgUrl);
//            var packingRequest = new PackingRequest
//            {
//                RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
//                UserID = AuthHelper.GetClaimValue(User, "UserID"),
//                context = _context,
//                PackingDTO = model
//            };
//            var PackingResponse = PackingService.AddPacking(packingRequest);
//            var GetPackingResponse = PackingService.GetLastPacking(packingRequest);
//            if (PackingResponse.Success)
//                return PartialView("_PackingList", new List<PackingDTO> { GetPackingResponse.PackingDTO });
//            else
//                return Json(new { message = PackingResponse, success = false });
//        }

//        [HttpPost]
//        [AuthorizePerRole("PackingEdit")]
//        public IActionResult EditPacking(PackingDTO model)
//        {
//            model.ImgUrl = BaseHelper.UploadImg(model.File, hosting.WebRootPath, model.ImgUrl);
//            var packingRequest = new PackingRequest
//            {
//                RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
//                UserID = AuthHelper.GetClaimValue(User, "UserID"),
//                context = _context,
//                PackingDTO = model
//            };
//            var userResponse = PackingService.EditPacking(packingRequest);
//            if (userResponse.Success)
//                return Json(new { message = "Edit Successfully", success = true, model = model });
//            else
//                return Json(new { message = userResponse.Message, success = false });
//        }

//        [HttpGet]
//        [AuthorizePerRole("PackingEdit")]
//        public IActionResult EditPacking(int id)
//        {
//            var packingRequest = new PackingRequest
//            {
//                RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
//                UserID = AuthHelper.GetClaimValue(User, "UserID"),
//                context = _context,
//                PackingDTO = new PackingDTO { id = id }
//            };
//            var PackingResponse = PackingService.GetPacking(packingRequest);

//            if (PackingResponse.Success)
//                return Json(PackingResponse.PackingDTO);
//            else
//                return Json(false);
//        }

//        [AuthorizePerRole("PackingList")]
//        public IActionResult PackingList(DateTime? From, int? Quantity, int PackingTypeId, string Search,
//                                        string OrderByColumn, bool? IsDesc, int PageIndex, string ActionType = null)
//        {
//            var ViewData = new ViewModel<PackingDTO>();
//            var filter = new PackingDTO();
//            filter.search = Search;
//            filter.PackingTypeId = PackingTypeId;
//            filter.Count = Quantity.HasValue ? Quantity.Value : 0;

//            if (From.HasValue)
//                filter.CreatedAt = From.Value;

//            var packingRequest = new PackingRequest
//            {
//                RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
//                UserID = AuthHelper.GetClaimValue(User, "UserID"),
//                context = _context,
//                PackingDTO = filter,
//                PageSize = BaseHelper.Constants.PageSize,
//                PageIndex = PageIndex,
//                IsDesc = IsDesc ?? true,
//                OrderByColumn = OrderByColumn ?? "id",
//                applyFilter = true
//            };
//            var packingResponse = PackingService.GetPackings(packingRequest);
//            ViewData.ObjDTOs = packingResponse.PackingDTOs;

//            if (ActionType != Constants.ActionType.Table && ActionType != Constants.ActionType.Print)
//                ViewData.Lookup = BaseHelper.GetLookup(new List<byte> {
//                     (byte)EnumSelectListType.PackingType }, _context);

//            if (ActionType == Constants.ActionType.PartialView)
//                return PartialView(ViewData);

//            if (ActionType == Constants.ActionType.Table)
//                return PartialView("_PackingList", packingResponse.PackingDTOs);

//            else if (ActionType == Constants.ActionType.Print)
//                return BaseHelper.GeneratePDF<List<PackingDTO>>("PackingManagementPDF", packingResponse.PackingDTOs);

//            else
//                return View(ViewData);
//        }

//        [AuthorizePerRole("PackingTypeList")]
//        public IActionResult PackingTypeList(string ActionType = null)
//        {
//            var PackingTypeList = _context.PackingType.ToList();
//            if (ActionType == Constants.ActionType.PartialView)
//                return PartialView(PackingTypeList);
//            else
//                return View(PackingTypeList);
//        }

//        [HttpPost]
//        [AuthorizePerRole("AddPackingType")]
//        public async Task<IActionResult> AddpackingType(PackingType model)
//        {
//            if (ModelState.IsValid)
//            {
//                model.CreatedBy = AuthHelper.GetClaimValue(User, "UserID");
//                model.CreatedAt = DateTime.Now;
//                await _context.PackingType.AddAsync(model);
//                await _context.SaveChangesAsync();
//                TempData["SuccessMsg"] = "New Packing Type successfully added";
//                return RedirectToAction(nameof(PackingTypeList));
//            }
//            else
//            {
//                TempData["ErrorMsg"] = "Update Packing Type Failed";
//                return View(nameof(PackingTypeList), model);
//            }

//        }

//        [HttpPost]
//        [AuthorizePerRole("EditPackingType")]
//        public async Task<IActionResult> EditpackingType(PackingType model)
//        {
//            var PackingType = _context.PackingType.Find(model.Id);
//            if (ModelState.IsValid)
//            {
//                if (PackingType != null)
//                {
//                    PackingType.LastModifiedBy = AuthHelper.GetClaimValue(User, "UserID");
//                    PackingType.LastModifiedAt = DateTime.Now;
//                    PackingType.NameAr = model.NameAr;
//                    PackingType.NameEn = model.NameEn;
//                    await _context.SaveChangesAsync();
//                }
//                TempData["SuccessMsg"] = "Update Packing Type successfully";
//                return RedirectToAction(nameof(PackingTypeList));
//            }
//            else
//            {
//                TempData["ErrorMsg"] = "Update Packing Type Failed";
//                return View(nameof(PackingTypeList), model);
//            }

//        }

//        [AuthorizePerRole("GetPackingList")]
//        public IActionResult GetPackingList(int ID)
//        {
//            var Packing = _context.Packing.Where(p => p.PackingTypeId == ID && p.Count > 0).ToList();
//            return Json(Packing);
//        }
//    }
//}
//using LegendCR.BL.Services;
//using LegendCR.CommonDefinitions.DTO;
//using LegendCR.CommonDefinitions.DTO.VendorProductDTOs;
//using LegendCR.CommonDefinitions.Requests;
//using LegendCR.CommonDefinitions.Responses;
//using LegendCR.DAL.DB;
//using LegendCR.Helpers;
//using LegendCR.Portal.Helpers;
//using Microsoft.AspNetCore.Mvc;
//using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

//namespace LegendCR.Portal.Controllers
//{
//    public class ProductController : Controller
//    {
//        private readonly ApplicationDBContext _context;

//        private readonly IHostingEnvironment hosting;

//        public ProductController(ApplicationDBContext context, IHostingEnvironment hosting)
//        {
//            _context = context;
//            this.hosting = hosting;
//        }

//        [AuthorizePerRole("ProductList")]
//        public IActionResult ProductList(string ActionType, int VendorID, string Search)
//        {
//            var ViewData = new ViewModel<VendorProductDTO>();

//            var Role = AuthHelper.GetClaimValue(User, "RoleID");
//            var user = AuthHelper.GetClaimValue(User, "UserID");
//            var filter = new VendorProductDTO();
//            if (VendorID == 0)
//                filter.VendorId = Role == (int)EnumRole.Admin ? 0 : user;
//            else
//                filter.VendorId = VendorID;

//            filter.Search = Search;

//            var VendorProductRequest = new VendorProductRequest
//            {
//                RoleID = Role,
//                UserID = user,
//                context = _context,
//                VendorProductDTO = filter,
//                IsDesc = true,
//                applyFilter = true
//            };
//            var ProductResponse = VendorProductService.GetAllProducts(VendorProductRequest);
//            ViewData.ObjDTOs = ProductResponse.VendorProductDTOs;

//            if (ActionType != Constants.ActionType.Table && ActionType != Constants.ActionType.Print)
//                ViewData.Lookup = BaseHelper.GetLookup(new List<byte> {
//                        (byte)EnumSelectListType.Vendor,
//                    }, _context);

//            if (ActionType == Constants.ActionType.PartialView)
//                return PartialView("_ProductList", ViewData);

//            else if (ActionType == Constants.ActionType.Print)
//                return BaseHelper.GeneratePDF<List<VendorProductDTO>>("/Views/Product/ProductReportPDF.cshtml", ViewData.ObjDTOs);

//            else if (ActionType == Constants.ActionType.Table)
//                return PartialView("_ProductTable", ViewData.ObjDTOs);

//            else
//                return View(ViewData);
//        }

//        [AuthorizePerRole("AddProduct")]
//        public IActionResult AddProduct(string ActionType = null)
//        {
//            if (ActionType == Constants.ActionType.PartialView)
//                return PartialView("ProductForm");
//            else
//                return View("ProductForm");
//        }

//        [AuthorizePerRole("AddProduct")]
//        [HttpPost]
//        public IActionResult AddProduct(VendorProductDTO model)
//        {
//            if (ModelState.IsValid)
//            {
//                model.ImageUrl = BaseHelper.UploadImg(model.File, hosting.WebRootPath, model.ImageUrl);
//                if (model.VendorId == 0)
//                    model.VendorId = AuthHelper.GetClaimValue(User, "UserID");
//                try
//                {
//                    var VendorProductRequest = new VendorProductRequest
//                    {
//                        RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
//                        UserID = AuthHelper.GetClaimValue(User, "UserID"),
//                        context = _context,
//                        VendorProductDTO = model,
//                    };
//                    var productResponse = VendorProductService.AddProduct(VendorProductRequest);
//                    var productData = VendorProductService.GetLastProduct(VendorProductRequest);
//                    if (productResponse.Success)
//                        return PartialView("_ProductTable", new List<VendorProductDTO>() { productData.VendorProductDTO });
//                    else
//                        return Json(false);
//                }
//                catch (Exception ex)
//                {
//                    return Json(false);
//                }
//            }
//            else
//                return Json(false);
//        }

//        [AuthorizePerRole("EditProduct")]
//        public async Task<IActionResult> EditProduct(int ID, string ActionType = null)
//        {
//            var model = await _context.VendorProduct.FindAsync(ID);
//            var VendorProductDTO = new VendorProductDTO
//            {
//                VendorId = model.VendorId,
//                OriginalPrice = model.OriginalPrice,
//                VendorProductId = model.VendorProductId,
//                Name = model.Name,
//                Description = model.Description,
//                ImageUrl = model.ImageUrl
//            };
//            if (ActionType == Constants.ActionType.PartialView)
//                return PartialView("ProductForm", VendorProductDTO);
//            else
//                return View("ProductForm", VendorProductDTO);
//        }

//        [AuthorizePerRole("EditProduct")]
//        [HttpPost]
//        public IActionResult EditProduct(VendorProductDTO model)
//        {
//            var UserID = AuthHelper.GetClaimValue(User, "UserID");
//            model.ImageUrl = BaseHelper.UploadImg(model.File, hosting.WebRootPath, model.ImageUrl);
//            if (model.VendorId == 0)
//                model.VendorId = UserID;

//            var vendorProductRequest = new VendorProductRequest
//            {
//                RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
//                UserID = UserID,
//                context = _context,
//                VendorProductDTO = model
//            };
//            var vendorProductResponse = VendorProductService.EditProduct(vendorProductRequest);
//            if (vendorProductResponse.Success)
//                return Json(model.VendorProductId);
//            else
//                return Json(false);
//        }

//        [AuthorizePerRole("DeletProduct")]
//        public IActionResult DeletProduct(int Id)
//        {
//            var Filter = new VendorProductDTO { VendorProductId = Id };
//            var VendorProductRequest = new VendorProductRequest();
//            var VendorProductResponse = new VendorProductResponse();
//            try
//            {
//                VendorProductRequest.RoleID = AuthHelper.GetClaimValue(User, "RoleID");
//                VendorProductRequest.UserID = AuthHelper.GetClaimValue(User, "UserID");
//                VendorProductRequest.context = _context;
//                VendorProductRequest.VendorProductDTO = Filter;

//                VendorProductResponse = VendorProductService.DeleteProduct(VendorProductRequest);

//                return Json("Product-" + Id + " Is Deleted");
//            }
//            catch (Exception ex)
//            {
//                VendorProductResponse.Message = ex.Message;
//            }
//            return Json(VendorProductResponse.Message);
//        }

//        public IActionResult GetProductList(int VendorID)
//        {
//            var Filter = new VendorProductDTO();
//            Filter.VendorId = VendorID == 0 ? AuthHelper.GetClaimValue(User, "UserID") : VendorID;
//            var VendorProductRequest = new VendorProductRequest
//            {
//                RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
//                UserID = AuthHelper.GetClaimValue(User, "UserID"),
//                context = _context,
//                VendorProductDTO = Filter,
//                applyFilter = true
//            };
//            var vendorProductResponse = VendorProductService.GetAllProducts(VendorProductRequest);

//            return PartialView("_StockModal", vendorProductResponse.VendorProductDTOs);
//        }

//        public IActionResult GetProduct(int ProductId)
//        {
//            var ProductRequest = new VendorProductRequest
//            {
//                RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
//                UserID = AuthHelper.GetClaimValue(User, "UserID"),
//                context = _context,
//                VendorProductDTO = new VendorProductDTO { VendorProductId = ProductId },
//                IsDesc = true,
//                applyFilter = true
//            };
//            var ProductResponse = VendorProductService.GetAllProducts(ProductRequest);
//            return Json(ProductResponse.VendorProductDTOs.FirstOrDefault());
//        }
//    }
//}

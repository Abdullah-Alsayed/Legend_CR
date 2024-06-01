using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DicomApp.BL.Services;
using DicomApp.CommonDefinitions.DTO;
using DicomApp.CommonDefinitions.Requests;
using DicomApp.CommonDefinitions.Responses;
using DicomApp.DAL.DB;
using DicomApp.Helpers;
using DicomApp.Portal.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;

namespace DicomApp.Portal.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ShippingDBContext _context;
        private readonly IHostingEnvironment hosting;

        public CategoryController(ShippingDBContext context, IHostingEnvironment hosting)
        {
            _context = context;
            this.hosting = hosting;
        }

        [AuthorizePerRole(SystemConstants.Permission.CategoryList)]
        public IActionResult CategoryList(
            DateTime? From,
            string Search,
            string OrderByColumn,
            bool? IsDesc,
            int PageIndex,
            string ActionType = null
        )
        {
            var ViewData = new ViewModel<CategoryDTO>();
            var filter = new CategoryDTO();
            filter.search = Search;

            if (From.HasValue)
                filter.CreatedAt = From.Value;

            var CategoryRequest = new CategoryRequest
            {
                RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
                UserID = AuthHelper.GetClaimValue(User, "UserID"),
                context = _context,
                CategoryDTO = filter,
                PageSize = BaseHelper.Constants.PageSize,
                PageIndex = PageIndex,
                IsDesc = IsDesc ?? true,
                OrderByColumn = OrderByColumn ?? "Id",
                applyFilter = true
            };
            var CategoryResponse = CategoryService.GetCategorys(CategoryRequest);
            ViewData.ObjDTOs = CategoryResponse.CategoryDTOs;

            if (ActionType == SystemConstants.ActionType.PartialView)
                return PartialView(ViewData);

            if (ActionType == SystemConstants.ActionType.Table)
                return PartialView("_CategoryList", CategoryResponse.CategoryDTOs);
            else if (ActionType == SystemConstants.ActionType.Print)
                return BaseHelper.GeneratePDF<List<CategoryDTO>>(
                    "CategoryPDF",
                    CategoryResponse.CategoryDTOs
                );
            else
                return View(ViewData);
        }

        [HttpPost]
        [AuthorizePerRole(SystemConstants.Permission.AddCategory)]
        public async Task<IActionResult> AddCategory(CategoryDTO model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var CategoryRequest = new CategoryRequest
                    {
                        RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
                        UserID = AuthHelper.GetClaimValue(User, "UserID"),
                        context = _context,
                        CategoryDTO = model,
                    };
                    var categoryResponse = CategoryService.AddCategory(CategoryRequest);
                    if (categoryResponse.Success)
                        return PartialView(
                            "_CategoryList",
                            new List<CategoryDTO>()
                            {
                                categoryResponse.CategoryDTOs.FirstOrDefault()
                            }
                        );
                    else
                        return Json(false);
                }
                catch (Exception ex)
                {
                    return Json(false);
                }
            }
            else
                return Json(false);
        }

        [HttpPost]
        [AuthorizePerRole(SystemConstants.Permission.EditCategory)]
        public async Task<IActionResult> EditCategory(CategoryDTO model)
        {
            var UserID = AuthHelper.GetClaimValue(User, "UserID");
            model.LastModifiedAt = DateTime.Now;
            model.LastModifiedBy = UserID;
            var CategoryRequest = new CategoryRequest
            {
                RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
                UserID = UserID,
                context = _context,
                CategoryDTO = model
            };
            var CategoryResponse = CategoryService.EditCategory(CategoryRequest);
            if (CategoryResponse.Success)
                return Json(model.Id);
            else
                return Json(false);
        }

        [AuthorizePerRole(SystemConstants.Permission.DeleteCategory)]
        public IActionResult DeleteCategory(int Id)
        {
            var Filter = new CategoryDTO { Id = Id };
            var CategoryRequest = new CategoryRequest();
            var CategoryResponse = new CategoryResponse();
            try
            {
                CategoryRequest.RoleID = AuthHelper.GetClaimValue(User, "RoleID");
                CategoryRequest.UserID = AuthHelper.GetClaimValue(User, "UserID");
                CategoryRequest.context = _context;
                CategoryRequest.CategoryDTO = Filter;

                CategoryResponse = CategoryService.DeleteCategory(CategoryRequest);
                if (CategoryResponse.Success)
                    return Json(
                        "Category-"
                            + CategoryResponse.CategoryDTOs.FirstOrDefault().NameEn
                            + " Is Deleted"
                    );
                else
                    return Json(false);
            }
            catch (Exception ex)
            {
                CategoryResponse.Message = ex.Message;
            }
            return Json(CategoryResponse.Message);
        }
    }
}

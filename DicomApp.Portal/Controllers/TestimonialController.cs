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
using Microsoft.AspNetCore.Mvc;

namespace DicomApp.Portal.Controllers
{
    [Authorize]
    public class TestimonialController : Controller
    {
        private readonly ShippingDBContext _context;

        public TestimonialController(ShippingDBContext context) => _context = context;

        [HttpPost]
        public IActionResult Create(string Comment, int Rating)
        {
            var request = new TestimonialRequest();
            request.context = _context;
            request.RoleID = AuthHelper.GetClaimValue(User, "RoleID");
            request.UserID = AuthHelper.GetClaimValue(User, "UserID");
            request.TestimonialDTO = new TestimonialDTO { Comment = Comment, Rate = Rating };
            var response = BL.Services.TestimonialService.AddTestimonial(request);
            return Json(response);
        }

        [HttpPut]
        public IActionResult Delete(int ID)
        {
            var request = new TestimonialRequest();
            request.context = _context;
            request.RoleID = AuthHelper.GetClaimValue(User, "RoleID");
            request.UserID = AuthHelper.GetClaimValue(User, "UserID");
            request.TestimonialDTO = new TestimonialDTO { TestimonialId = ID };
            var response = BL.Services.TestimonialService.DeleteTestimonial(request);
            return Json(response);
        }

        [AuthorizePerRole(SystemConstants.Permission.ListTestimonial)]
        public IActionResult All(
            string Search,
            string OrderByColumn,
            bool? IsDesc,
            int PageIndex,
            DateTime? From,
            DateTime? To,
            string ActionType = null
        )
        {
            var filter = new TestimonialDTO();
            filter.Search = Search;
            if (From.HasValue)
                filter.DateFrom = From.Value;
            if (To.HasValue)
                filter.DateTo = To.Value;

            var request = new TestimonialRequest
            {
                RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
                UserID = AuthHelper.GetClaimValue(User, "UserID"),
                context = _context,
                IsDesc = IsDesc ?? true,
                PageIndex = PageIndex,
                PageSize = BaseHelper.Constants.PageSize,
                OrderByColumn = OrderByColumn ?? nameof(Advertisement.CreatedAt),
                applyFilter = true,
                TestimonialDTO = filter
            };

            var response = BL.Services.TestimonialService.GetTestimonials(request);

            if (ActionType == SystemConstants.ActionType.PartialView)
                return PartialView(response.TestimonialDTOs);
            else if (ActionType == SystemConstants.ActionType.Table)
                return PartialView("_Table", response.TestimonialDTOs);
            else
                return View(response.TestimonialDTOs);
        }
    }
}

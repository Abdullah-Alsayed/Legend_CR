using System;
using System.Collections.Generic;
using System.Linq;
using DicomApp.BL.Services;
using DicomApp.CommonDefinitions.DTO;
using DicomApp.CommonDefinitions.DTO.AdvertisementDTOs;
using DicomApp.CommonDefinitions.DTO.AdvertisementDTOs;
using DicomApp.CommonDefinitions.DTO.PickupDTOs;
using DicomApp.CommonDefinitions.DTO.VendorProductDTOs;
using DicomApp.CommonDefinitions.Requests;
using DicomApp.DAL.DB;
using DicomApp.Helpers;
using DicomApp.Helpers;
using DicomApp.Portal.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml.Style;
using StackExchange.Redis;

namespace DicomApp.Portal.Controllers
{
    public class HistoryController : Controller
    {
        private readonly ShippingDBContext _context;

        public HistoryController(ShippingDBContext context)
        {
            _context = context;
        }

        [AuthorizePerRole(SystemConstants.Permission.History)]
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
            var filter = new HistoryDTO();
            filter.Search = Search;
            if (From.HasValue)
                filter.DateFrom = From.Value;
            if (To.HasValue)
                filter.DateTo = To.Value;

            var request = new FollowUpRequest
            {
                RoleID = AuthHelper.GetClaimValue(User, SystemConstants.Claims.RoleID),
                UserID = AuthHelper.GetClaimValue(User, SystemConstants.Claims.UserID),
                context = _context,
                IsDesc = IsDesc ?? true,
                PageIndex = PageIndex,
                PageSize = BaseHelper.Constants.PageSize,
                OrderByColumn = OrderByColumn ?? nameof(Advertisement.CreatedAt),
                applyFilter = true,
                FollowUpDTO = filter
            };

            var response = BL.Services.FollowUpService.GetAll(request);

            if (ActionType == SystemConstants.ActionType.PartialView)
                return PartialView(response.FollowUpDTOs);
            else if (ActionType == SystemConstants.ActionType.Table)
                return PartialView("_Table", response.FollowUpDTOs);
            else
                return View(response.FollowUpDTOs);
        }
    }
}

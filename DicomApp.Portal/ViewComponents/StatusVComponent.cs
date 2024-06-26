﻿using DicomApp.BL.Services;
using DicomApp.CommonDefinitions.Requests;
using DicomApp.DAL.DB;
using DicomApp.Helpers;
using DicomApp.Portal.Helpers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DicomApp.Portal.ViewComponents
{

    public class StatusVComponent : ViewComponent
    {
        private readonly ShippingDBContext _context;

        public StatusVComponent(ShippingDBContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke(string orderByColumn = null, string searchVal = null, int pageIndex = 0)
        {
            ViewBag.pageIndex = pageIndex;
            var statusRequest = new StatusRequest
            {
                context = _context,
                OrderByColumn = orderByColumn,
                PageSize = BaseRequest.DefaultPageSize,
                PageIndex = pageIndex,
                RoleID = AuthHelper.GetClaimValue(UserClaimsPrincipal, "RoleID"),
                UserID = AuthHelper.GetClaimValue(UserClaimsPrincipal, "UserID"),
            };
            var statusResponse = StatusService.GetStatus(statusRequest);
            if (statusResponse.StatusDTOs != null)
            {
                ViewBag.noOfPages = Math.Ceiling((double)statusResponse.TotalCount / statusRequest.PageSize);
                ViewBag.hasMore = statusResponse.TotalCount > (statusResponse.StatusDTOs.Count + (statusRequest.PageIndex * statusRequest.PageSize));
                return View(statusResponse.StatusDTOs);
            }
            ViewBag.error = statusResponse.Message;
            return View();
        }

    }
}

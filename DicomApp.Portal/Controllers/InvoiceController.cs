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
using Microsoft.AspNetCore.Mvc;

namespace DicomApp.Portal.Controllers
{
    public class InvoiceController : Controller
    {
        private readonly ShippingDBContext _context;

        public InvoiceController(ShippingDBContext context) => _context = context;

        public IActionResult GetItemIds(int invoiceType)
        {
            var response = new List<SelectOptionDTO>();
            switch (invoiceType)
            {
                case (int)InvoiceTypeEnum.Advertisement:
                    response = AdvertisementService
                        .GeAdvertisementIds(
                            new AdvertisementRequest
                            {
                                AdsDTO = new AdsDTO { StatusType = (int)StatusTypeEnum.Published },
                                context = _context,
                                applyFilter = true
                            }
                        )
                        .SelectOption;
                    break;
                case (int)InvoiceTypeEnum.GamerService:
                    response = GamerServiceService
                        .GetGamerServicesIds(
                            new GamerServiceRequest
                            {
                                context = _context,
                                ServiceDTO = new ServiceDTO
                                {
                                    StatusType = (int)StatusTypeEnum.Accept
                                },
                                applyFilter = true
                            }
                        )
                        .SelectOption;
                    break;
                default:
                    response = new List<SelectOptionDTO>();
                    break;
            }
            return Json(response);
        }

        [AuthorizePerRole(SystemConstants.Permission.ListInvoice)]
        public IActionResult All(
            string Search,
            string OrderByColumn,
            bool? IsDesc,
            int PageIndex,
            DateTime? From,
            DateTime? To,
            int InvoiceType,
            int LessPrice,
            int GreeterPrice,
            string ActionType = null
        )
        {
            ViewModel<InvoiceDTO> ViewData = new ViewModel<InvoiceDTO>();
            var filter = new InvoiceDTO()
            {
                InvoiceType = InvoiceType,
                Search = Search,
                LessPrice = LessPrice,
                GreeterPrice = GreeterPrice,
            };
            if (ActionType != SystemConstants.ActionType.Table)
            {
                ViewData.Lookup = BaseHelper.GetLookup(
                    new List<byte>
                    {
                        (byte)EnumSelectListType.AcceptAdvertisement,
                        (byte)EnumSelectListType.AcceptGamerService,
                    },
                    _context
                );
            }

            if (From.HasValue)
                filter.DateFrom = From.Value;
            if (To.HasValue)
                filter.DateTo = To.Value;

            var request = new InvoiceRequest
            {
                RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
                UserID = AuthHelper.GetClaimValue(User, "UserID"),
                context = _context,
                IsDesc = IsDesc ?? true,
                PageIndex = PageIndex,
                PageSize = BaseHelper.Constants.PageSize,
                OrderByColumn = OrderByColumn ?? nameof(Invoice.CreatedAt),
                applyFilter = true,
                InvoiceDTO = filter
            };

            var response = BL.Services.InvoiceService.GetAll(request);
            ViewData.ObjDTOs = response.InvoiceDTOs;
            if (ActionType == SystemConstants.ActionType.PartialView)
                return PartialView(ViewData);
            else if (ActionType == SystemConstants.ActionType.Table)
            {
                switch (InvoiceType)
                {
                    case (int)InvoiceTypeEnum.Advertisement:
                        return PartialView("_Advertisement", response.InvoiceDTOs);
                    case (int)InvoiceTypeEnum.GamerService:
                        return PartialView("_GamerService", response.InvoiceDTOs);
                    default:
                        return PartialView("_Advertisement", response.InvoiceDTOs);
                }
            }
            else
                return View(ViewData);
        }
    }
}

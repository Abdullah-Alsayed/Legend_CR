//using System;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using DicomApp.BL.Services;
//using DicomApp.CommonDefinitions.DTO;
//using DicomApp.CommonDefinitions.DTO.AdvertisementDTOs;
//using DicomApp.CommonDefinitions.DTO.PickupDTOs;
//using DicomApp.CommonDefinitions.DTO.AdvertisementDTOs;
//using DicomApp.CommonDefinitions.DTO.VendorProductDTOs;
//using DicomApp.CommonDefinitions.Requests;
//using DicomApp.CommonDefinitions.Responses;
//using DicomApp.DAL.DB;
//using DicomApp.Helpers;
//using DicomApp.Portal.Helpers;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Rotativa.AspNetCore;
//using Rotativa.AspNetCore.Options;

//namespace DicomApp.Portal.Controllers
//{
//    public class PickUpRequestController : Controller
//    {
//        private readonly ShippingDBContext _context;

//        public PickUpRequestController(ShippingDBContext context)
//        {
//            _context = context;
//        }

//        #region Delivery pickup request

//        [HttpGet]
//        [AuthorizePerRole("PickupDeliveryList")]
//        public IActionResult Delivery(
//            DateTime? From,
//            DateTime? To,
//            int VendorId,
//            string Search,
//            bool? IsDesc,
//            string ActionType = null
//        )
//        {
//            ViewModel<PickupDTO> viewModel = new ViewModel<PickupDTO>();
//            var filter = new AdsDTO();
//            filter.Search = Search;
//            filter.StatusId = (int)EnumStatus.Ready_For_Pickup;
//            filter.VendorDetailsDTO = new VendorDetailsDTO { VendorId = VendorId };
//            filter.HasPickup = false;
//            if (From.HasValue)
//                filter.DateFrom = From.Value;
//            if (To.HasValue)
//                filter.DateTo = To.Value;

//            var request = new AdvertisementRequest()
//            {
//                RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
//                UserID = AuthHelper.GetClaimValue(User, "UserID"),
//                context = _context,
//                ShipDTO = filter,
//                IsDesc = IsDesc ?? true,
//                applyFilter = true,
//                HasCustomerDetailsDTO = true,
//                HasVendorDetailsDTO = true,
//                HasSettingDTO = true
//            };

//            if (
//                ActionType != SystemConstants.ActionType.Table
//                && ActionType != SystemConstants.ActionType.Print
//            )
//            {
//                viewModel.Lookup = BaseHelper.GetLookup(
//                    new List<byte>
//                    {
//                        (byte)EnumSelectListType.Zone,
//                        (byte)EnumSelectListType.Vendor,
//                        (byte)EnumSelectListType.Area
//                    },
//                    _context
//                );
//            }
//            var response = BL.Services.AdvertisementService.GetAllShipments(request);

//            viewModel.ObjDTO = new PickupDTO { Shipments = response.ShipDTOs };

//            if (ActionType == SystemConstants.ActionType.PartialView)
//                return PartialView(viewModel);
//            else if (ActionType == SystemConstants.ActionType.Table)
//                return PartialView("_Delivery", viewModel.ObjDTO);
//            else
//                return View(viewModel);
//        }

//        [HttpPost]
//        [AuthorizePerRole("PickupDeliveryAdd")]
//        public IActionResult AddDeliveryPickup(PickupDTO model)
//        {
//            var request = new PickUpRequestRequest();
//            List<PickupItemDTO> items = new List<PickupItemDTO>();

//            request.context = _context;
//            request.RoleID = AuthHelper.GetClaimValue(User, "RoleID");
//            request.UserID = AuthHelper.GetClaimValue(User, "UserID");
//            if (model.VendorId == 0)
//                model.VendorId = request.UserID;

//            var ShipmentIDs = model.ShipmentIDs.Split(',').Select(int.Parse).ToList();

//            for (int i = 0; i < ShipmentIDs.Count; i++)
//                items.Add(new PickupItemDTO { ShipmentId = ShipmentIDs[i] });

//            request.PickupDTO = new PickupDTO()
//            {
//                PickupRequestTypeId = (int)EnumPickupRequestType.DeliveryPickup,
//                VendorId = model.VendorId,
//                VendorName = model.VendorName,
//                VendorPhone = model.VendorPhone,
//                VendorAddress = model.VendorAddress,
//                VendorLocation = model.VendorLocation,
//                VendorLandmark = model.VendorLandmark,
//                VendorFloor = model.VendorFloor,
//                VendorApartment = model.VendorApartment,
//                ZoneId = model.ZoneId,
//                AreaId = model.AreaId,
//                PickupDate = model.PickupDate,
//                TimeFrom = model.TimeFrom,
//                TimeTo = model.TimeTo,
//                PickupItems = items,
//                Notes = model.Notes,
//                IncludeReturns = model.IncludeReturns
//            };

//            var response = PickUpRequestService.AddPickUpRequest(request);

//            return Json(response.Message);
//        }

//        #endregion

//        #region Stock pickup request

//        [AuthorizePerRole("PickupStockList")]
//        public IActionResult Stock(string ActionType)
//        {
//            ViewModel<PickupDTO> viewModel = new ViewModel<PickupDTO>();
//            if (
//                ActionType != SystemConstants.ActionType.Table
//                && ActionType != SystemConstants.ActionType.Print
//            )
//            {
//                viewModel.Lookup = BaseHelper.GetLookup(
//                    new List<byte>
//                    {
//                        (byte)EnumSelectListType.Zone,
//                        (byte)EnumSelectListType.Area,
//                        (byte)EnumSelectListType.Vendor,
//                    },
//                    _context
//                );
//            }

//            if (ActionType == SystemConstants.ActionType.PartialView)
//                return PartialView(viewModel);
//            else
//                return View(viewModel);
//        }

//        [AuthorizePerRole("PickupStockList")]
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

//            return PartialView("_StockItems", vendorProductResponse.VendorProductDTOs);
//        }

//        [AuthorizePerRole("PickupStockAdd")]
//        [HttpPost]
//        public IActionResult AddStockPickup(PickupDTO model)
//        {
//            var request = new PickUpRequestRequest();
//            List<PickupItemDTO> items = new List<PickupItemDTO>();

//            request.context = _context;
//            request.RoleID = AuthHelper.GetClaimValue(User, "RoleID");
//            request.UserID = AuthHelper.GetClaimValue(User, "UserID");

//            var ProductIDs = model.ProductIDs.Split(',').Select(int.Parse).ToList();
//            var ProductsQuantity = model.ProductsQuantity.Split(',').Select(int.Parse).ToList();
//            var ProductsPrice = model.ProductsPrice.Split(',').Select(int.Parse).ToList();

//            for (int i = 0; i < ProductIDs.Count(); i++)
//            {
//                items.Add(
//                    new PickupItemDTO
//                    {
//                        VendorProductId = ProductIDs[i],
//                        Quantity = ProductsQuantity[i],
//                        Price = ProductsPrice[i]
//                    }
//                );
//            }
//            request.PickupDTO = new PickupDTO()
//            {
//                PickupRequestTypeId = (int)EnumPickupRequestType.StockPickup,
//                VendorId = model.VendorId,
//                VendorName = model.VendorName,
//                VendorPhone = model.VendorPhone,
//                VendorAddress = model.VendorAddress,
//                VendorLocation = model.VendorLocation,
//                VendorLandmark = model.VendorLandmark,
//                VendorFloor = model.VendorFloor,
//                VendorApartment = model.VendorApartment,
//                ZoneId = model.ZoneId,
//                AreaId = model.AreaId,
//                PickupDate = model.PickupDate,
//                TimeFrom = model.TimeFrom,
//                TimeTo = model.TimeTo,
//                PickupItems = items,
//                IncludeReturns = model.IncludeReturns
//            };

//            var response = PickUpRequestService.AddPickUpRequest(request);

//            return Json(response.Message);
//        }

//        #endregion

//        #region Pickup request list

//        [AuthorizePerRole("PickupsList")]
//        public IActionResult All(
//            DateTime? From,
//            DateTime? To,
//            int VendorId,
//            int StatusId,
//            int ZoneID,
//            string Search,
//            string OrderByColumn,
//            bool? IsDesc,
//            int PageIndex,
//            string ActionType = null
//        )
//        {
//            ViewModel<PickupDTO> viewModel = new ViewModel<PickupDTO>();
//            viewModel.Lookup = new LookupDTO();
//            var filter = new PickupDTO();
//            filter.Search = Search;
//            filter.VendorId = VendorId;
//            filter.ZoneId = ZoneID;
//            filter.StatusId = StatusId == (int)EnumStatus.All ? 0 : StatusId;

//            var PageSize =
//                ActionType == SystemConstants.ActionType.Print ? 0 : BaseHelper.Constants.PageSize;

//            var request = new PickUpRequestRequest
//            {
//                RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
//                UserID = AuthHelper.GetClaimValue(User, "UserID"),
//                context = _context,
//                PickupDTO = filter,
//                PageIndex = PageIndex,
//                PageSize = PageSize,
//                IsDesc = IsDesc ?? true,
//                applyFilter = true,
//                HasPickupItemDTO = true
//            };

//            var response = PickUpRequestService.GetAllPickUpRequests(request);
//            viewModel.ObjDTOs = response.PickupDTOs;

//            if (
//                ActionType != SystemConstants.ActionType.Table
//                && ActionType != SystemConstants.ActionType.Print
//            )
//            {
//                viewModel.Lookup = BaseHelper.GetLookup(
//                    new List<byte>
//                    {
//                        (byte)EnumSelectListType.Vendor,
//                        (byte)EnumSelectListType.Zone,
//                        (byte)EnumSelectListType.Status
//                    },
//                    _context
//                );
//            }

//            viewModel.Lookup.AreaDTOs = BaseHelper
//                .GetLookup(new List<byte> { (byte)EnumSelectListType.Area }, _context)
//                .AreaDTOs;

//            if (ActionType == SystemConstants.ActionType.PartialView)
//                return PartialView(viewModel);
//            else if (ActionType == SystemConstants.ActionType.Table)
//                return PartialView("_All", viewModel);
//            else if (ActionType == SystemConstants.ActionType.Print)
//                return BaseHelper.GeneratePDF<List<PickupDTO>>(
//                    "PickupReportsPDF",
//                    viewModel.ObjDTOs
//                );

//            return View(viewModel);
//        }

//        [AllowAnonymous]
//        public IActionResult GetDeliveryPickups(int id)
//        {
//            var RoleID = AuthHelper.GetClaimValue(User, "RoleID");
//            var UserID = AuthHelper.GetClaimValue(User, "UserID");

//            var filter = new AdsDTO();
//            filter.PickupRequestId = id;
//            if (RoleID == (int)EnumRole.Vendor)
//                filter.VendorDetailsDTO = new VendorDetailsDTO { VendorId = UserID };

//            var request = new AdvertisementRequest
//            {
//                RoleID = RoleID,
//                UserID = UserID,
//                context = _context,
//                ShipDTO = filter,
//                IsDesc = true,
//                applyFilter = true,

//                HasFeesDetailsDTO = true,
//                HasCustomerDetailsDTO = true,
//                HasVendorDetailsDTO = true,
//                HasSettingDTO = true,
//            };

//            var response = BL.Services.AdvertisementService.GetAllShipments(request);

//            return PartialView("_Shipments", response.ShipDTOs);
//        }

//        [AllowAnonymous]
//        public ActionResult GetStockPickups(int id)
//        {
//            var RoleID = AuthHelper.GetClaimValue(User, "RoleID");
//            var UserID = AuthHelper.GetClaimValue(User, "UserID");

//            var filter = new PickupItemDTO();
//            filter.PickupRequestId = id;

//            var request = new PickUpRequestRequest
//            {
//                RoleID = RoleID,
//                UserID = UserID,
//                context = _context,
//                PickupItemDTO = filter,
//                IsDesc = true,
//            };

//            var response = PickUpRequestService.GetPickupStockItems(request);

//            return PartialView("_Products", response.PickupItemDTOs);
//        }

//        [AllowAnonymous]
//        public IActionResult GetReadyForReturn(int VendorID = 0)
//        {
//            if (VendorID <= 0)
//                return PartialView("_Shipments");

//            var RoleID = AuthHelper.GetClaimValue(User, "RoleID");
//            var UserID = AuthHelper.GetClaimValue(User, "UserID");

//            var filter = new AdsDTO();
//            filter.StatusId = (int)EnumStatus.Ready_For_Return;
//            if (RoleID == (int)EnumRole.Vendor)
//                filter.VendorDetailsDTO = new VendorDetailsDTO { VendorId = UserID };
//            else
//                filter.VendorDetailsDTO = new VendorDetailsDTO { VendorId = VendorID };

//            var request = new AdvertisementRequest
//            {
//                RoleID = RoleID,
//                UserID = UserID,
//                context = _context,
//                ShipDTO = filter,
//                IsDesc = true,
//                applyFilter = true,

//                HasFeesDetailsDTO = true,
//                HasCustomerDetailsDTO = true,
//                HasVendorDetailsDTO = true,
//                HasSettingDTO = true
//            };

//            var response = BL.Services.AdvertisementService.GetAllShipments(request);

//            return PartialView("_Shipments", response.ShipDTOs);
//        }

//        [AuthorizePerRole("PickupEdit")]
//        [HttpPost]
//        public IActionResult UpdatePickupRequst(
//            int id,
//            int AreaId,
//            string Address,
//            DateTime PickupDate,
//            DateTime ReadyTime,
//            DateTime LastTimeAvailable
//        )
//        {
//            var PickupDTO = new PickupDTO
//            {
//                PickupRequestId = id,
//                VendorAddress = Address,
//                AreaId = AreaId,
//                PickupDate = PickupDate,
//                TimeFrom = ReadyTime,
//                TimeTo = LastTimeAvailable
//            };

//            var request = new PickUpRequestRequest
//            {
//                RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
//                UserID = AuthHelper.GetClaimValue(User, "UserID"),
//                context = _context,
//                PickupDTO = PickupDTO
//            };

//            var response = PickUpRequestService.EditPickUpRequest(request);

//            return Json(response.Success);
//        }

//        [AuthorizePerRole("PickupCancel")]
//        [HttpPost]
//        public ActionResult Cancel(int pickupID)
//        {
//            var request = new PickUpRequestRequest
//            {
//                context = _context,
//                RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
//                UserID = AuthHelper.GetClaimValue(User, "UserID"),
//                PickupDTO = new PickupDTO { PickupRequestId = pickupID }
//            };

//            var response = DicomApp.BL.Services.PickUpRequestService.CancelPickupRequest(request);

//            return Json(new { success = response.Success, message = response.Message });
//        }

//        [AuthorizePerRole("PickupUnAssign")]
//        [HttpPost]
//        public IActionResult Unassigned(int PickupID)
//        {
//            var request = new PickUpRequestRequest
//            {
//                RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
//                UserID = AuthHelper.GetClaimValue(User, "UserID"),
//                context = _context,
//                PickupDTO = new PickupDTO { PickupRequestId = PickupID }
//            };

//            var response = BL.Services.PickUpRequestService.UnAssignPickupRequests(request);

//            return Json(response);
//        }

//        #endregion

//        #region Assign pickup requests

//        [AuthorizePerRole("AssignPickupList")]
//        public IActionResult Assign(
//            DateTime? From,
//            DateTime? To,
//            int VendorId,
//            int ZoneID,
//            bool? IsDesc,
//            string OrderByColumn,
//            string Search = null,
//            string accountsIDs = null,
//            string ActionType = null
//        )
//        {
//            ViewModel<PickupDTO> ViewData = new ViewModel<PickupDTO>();

//            ViewData.Lookup = BaseHelper.GetLookup(
//                new List<byte>
//                {
//                    (byte)EnumSelectListType.Vendor,
//                    (byte)EnumSelectListType.Zone,
//                    (byte)EnumSelectListType.Courier,
//                },
//                _context
//            );

//            var filter = new PickupDTO();
//            filter.Search = Search;
//            filter.ZoneId = ZoneID;
//            filter.VendorId = VendorId;
//            filter.StatusId = (int)EnumStatus.Ready_For_Pickup;

//            var request = new PickUpRequestRequest
//            {
//                RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
//                UserID = AuthHelper.GetClaimValue(User, "UserID"),
//                context = _context,
//                PickupDTO = filter,
//                IsDesc = true,
//                applyFilter = true,
//                HasPickupItemDTO = true,
//            };

//            var response = PickUpRequestService.GetAllPickUpRequests(request);
//            ViewData.ObjDTOs = response.PickupDTOs;
//            if (ActionType == SystemConstants.ActionType.PartialView)
//                return PartialView(ViewData);
//            else if (ActionType == SystemConstants.ActionType.Table)
//                return PartialView("_Assign", ViewData);
//            else
//                return View(ViewData);
//        }

//        [AuthorizePerRole("PickupAssign")]
//        [HttpPost]
//        public IActionResult Assign(List<int> ShipmentIDs, int ddldriver)
//        {
//            var request = new PickUpRequestRequest
//            {
//                RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
//                UserID = AuthHelper.GetClaimValue(User, "UserID"),
//                context = _context,
//                PickupDTO = new PickupDTO { PickupIDs = ShipmentIDs, DeliveryManId = ddldriver }
//            };

//            var response = PickUpRequestService.AssignPickupRequests(request);

//            return RedirectToAction("Assign");
//        }

//        #endregion
//    }
//}

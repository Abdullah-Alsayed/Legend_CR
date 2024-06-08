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
//using DicomApp.DAL.DB;
//using DicomApp.Helpers;
//using DicomApp.Helpers;
//using DicomApp.Portal.Helpers;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.CodeAnalysis;
//using Microsoft.EntityFrameworkCore;
//using OfficeOpenXml.Style;
//using StackExchange.Redis;

//namespace DicomApp.Portal.Controllers
//{
//    public class WarehouseController : Controller
//    {
//        private readonly ShippingDBContext _context;

//        public WarehouseController(ShippingDBContext context)
//        {
//            _context = context;
//        }

//        #region Receive Delivery/Stock Pickups

//        [AuthorizePerRole("ReceiveDeliveryPickups")]
//        public IActionResult ReceivePickups(string ActionType)
//        {
//            ViewBag.Couriers = GeneralHelper.GetUsers((int)EnumRole.DeliveryMan, _context);
//            ViewBag.Vendors = GeneralHelper.GetUsers((int)EnumRole.Vendor, _context);

//            if (ActionType == SystemConstants.ActionType.PartialView)
//                return PartialView();
//            else
//                return View();
//        }

//        public IActionResult GetPickupCodes(int deliveryManID = 0, int vendorID = 0)
//        {
//            var request = new PickUpRequestRequest
//            {
//                context = _context,
//                RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
//                UserID = AuthHelper.GetClaimValue(User, "UserID"),
//                applyFilter = true,
//                IsDesc = true,
//                PickupDTO = new PickupDTO
//                {
//                    StatusId = (int)EnumStatus.Picked_Up,
//                    PickupRequestTypeId = (int)EnumPickupRequestType.DeliveryPickup,
//                    DeliveryManId = deliveryManID,
//                    VendorId = vendorID
//                }
//            };
//            var response = DicomApp.BL.Services.PickUpRequestService.GetAllPickUpRequests(request);

//            return PartialView("_PickupCodes", response.PickupDTOs);
//        }

//        [AuthorizePerRole("ReceiveStockPickups")]
//        public IActionResult ReceiveStock(string ActionType)
//        {
//            ViewBag.Couriers = GeneralHelper.GetUsers((int)EnumRole.DeliveryMan, _context);
//            ViewBag.Vendors = GeneralHelper.GetUsers((int)EnumRole.Vendor, _context);
//            if (ActionType == SystemConstants.ActionType.PartialView)
//                return PartialView();
//            else
//                return View();
//        }

//        public IActionResult GetStockCodes(int deliveryManID = 0, int vendorID = 0)
//        {
//            var request = new PickUpRequestRequest
//            {
//                context = _context,
//                RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
//                UserID = AuthHelper.GetClaimValue(User, "UserID"),
//                applyFilter = true,
//                IsDesc = true,
//                PickupDTO = new PickupDTO
//                {
//                    StatusId = (int)EnumStatus.Picked_Up,
//                    PickupRequestTypeId = (int)EnumPickupRequestType.StockPickup,
//                    DeliveryManId = deliveryManID,
//                    VendorId = vendorID
//                }
//            };
//            var response = DicomApp.BL.Services.PickUpRequestService.GetAllPickUpRequests(request);

//            return PartialView("_StockCodes", response.PickupDTOs);
//        }

//        [AuthorizePerRole("ConfirmReceivePickups")]
//        [HttpPost]
//        public IActionResult ReveivePickup(string pickUpRequestIDs)
//        {
//            if (string.IsNullOrEmpty(pickUpRequestIDs))
//                return View();

//            var ListIDs = pickUpRequestIDs.Split(',').Select<string, int>(int.Parse).ToList();
//            var request = new PickUpRequestRequest
//            {
//                context = _context,
//                RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
//                UserID = AuthHelper.GetClaimValue(User, "UserID"),
//                applyFilter = true,
//                IsDesc = true,
//                PickupDTO = new PickupDTO
//                {
//                    PickupIDs = ListIDs,
//                    StatusId = (int)EnumStatus.At_Warehouse
//                }
//            };

//            var response = DicomApp.BL.Services.PickUpRequestService.WarehouseReceive(request);

//            return Json(new { success = response.Success, message = response.Message });
//        }

//        #endregion

//        #region Receive Returns

//        [AuthorizePerRole("ReceiveReturns")]
//        public IActionResult ReceiveReturns(string ActionType)
//        {
//            ViewBag.Couriers = GeneralHelper.GetUsers((int)EnumRole.DeliveryMan, _context);
//            if (ActionType == SystemConstants.ActionType.PartialView)
//                return PartialView();
//            else
//                return View();
//        }

//        public IActionResult GetShipmentCodes(int deliveryManID = 0)
//        {
//            var request = new AdvertisementRequest
//            {
//                context = _context,
//                RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
//                UserID = AuthHelper.GetClaimValue(User, "UserID"),
//                applyFilter = true,
//                IsDesc = true,
//                HasShipItemDTOs = true,
//                HasSettingDTO = true,
//                ShipDTO = new AdsDTO
//                {
//                    StatusIDs = new List<int>
//                    {
//                        (int)EnumStatus.Postponed,
//                        (int)EnumStatus.Cancelled,
//                        (int)EnumStatus.Out_For_Delivery,
//                        (int)EnumStatus.Delivered,
//                        (int)EnumStatus.Refunded
//                    },
//                    DeliveryManId = deliveryManID,
//                    IsForReceiveReturns = true
//                }
//            };
//            var response = DicomApp.BL.Services.AdvertisementService.GetAllShipments(request);

//            return PartialView("_ShipCodes", response.ShipDTOs);
//        }

//        [AuthorizePerRole("ConfirmReceiveReturn")]
//        [HttpPost]
//        public IActionResult ReceiveReturn(string AdvertisementIds)
//        {
//            var ListIDs = AdvertisementIds.Split(',').Select<string, int>(int.Parse).ToList();
//            var request = new AdvertisementRequest
//            {
//                context = _context,
//                RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
//                UserID = AuthHelper.GetClaimValue(User, "UserID"),
//                ShipDTO = new AdsDTO { AdvertisementIds = ListIDs }
//            };

//            var response = DicomApp.BL.Services.AdvertisementService.ReceiveReturns(request);

//            return Json(new { success = response.Success, message = response.Message });
//        }

//        #endregion

//        #region Shipments List and Actions

//        [AuthorizePerRole("WarehouseShipments")]
//        public IActionResult Shipments(
//            DateTime? From,
//            DateTime? To,
//            string Search,
//            int VendorId,
//            int DeliveryManId,
//            string ActionType,
//            int StatusId = (int)EnumStatus.At_Warehouse
//        )
//        {
//            if (ActionType != SystemConstants.ActionType.Print)
//            {
//                ViewBag.Vendor = GeneralHelper.GetUsers((int)EnumRole.Vendor, _context);
//                ViewBag.Couriers = GeneralHelper.GetUsers((int)EnumRole.DeliveryMan, _context);
//                ViewBag.Game = _context.Game.ToList();
//                ViewBag.Category = _context.Category.ToList();
//            }

//            var filter = new AdsDTO();
//            filter.StatusId = StatusId;
//            filter.Search = "";
//            filter.VendorDetailsDTO = new VendorDetailsDTO { VendorId = VendorId };
//            filter.DeliveryManId = DeliveryManId;

//            //if (From.HasValue)
//            //    filter.DateFrom = From.Value;

//            //if (To.HasValue)
//            //    filter.DateTo = To.Value;

//            var request = new AdvertisementRequest
//            {
//                RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
//                UserID = AuthHelper.GetClaimValue(User, "UserID"),
//                context = _context,
//                ShipDTO = filter,
//                IsDesc = true,

//                applyFilter = true,

//                HasCustomerDetailsDTO = true,
//                HasVendorDetailsDTO = true,
//                HasSettingDTO = true,
//            };
//            var response = DicomApp.BL.Services.AdvertisementService.GetAllShipments(request);

//            if (ActionType == SystemConstants.ActionType.PartialView)
//                return PartialView(response.ShipDTOs);
//            else if (ActionType == SystemConstants.ActionType.Table)
//            {
//                if (StatusId == (int)EnumStatus.At_Warehouse)
//                    return PartialView("_Pending", response.ShipDTOs);
//                else if (StatusId == (int)EnumStatus.Ready_For_Delivery)
//                    return PartialView("_Ready", response.ShipDTOs);
//                else if (StatusId == (int)EnumStatus.Cancelled)
//                    return PartialView("_Cancelled", response.ShipDTOs);
//            }
//            else
//                return View(response.ShipDTOs);

//            return View();
//        }

//        [AuthorizePerRole("WarehouseChangeToReady")]
//        public IActionResult ChangeToReady(int shipId = 0)
//        {
//            var request = new AdvertisementRequest
//            {
//                context = _context,
//                RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
//                UserID = AuthHelper.GetClaimValue(User, "UserID"),
//                ShipDTO = new AdsDTO
//                {
//                    AdvertisementId = shipId,
//                    StatusId = (int)EnumStatus.Ready_For_Delivery
//                }
//            };

//            var response = DicomApp.BL.Services.AdvertisementService.ChangeStatus(request);

//            return Json(new { success = response.Success, message = response.Message });
//        }

//        [AuthorizePerRole("WarehouseEditGame")]
//        public IActionResult EditGame(AdsDTO model)
//        {
//            var request = new AdvertisementRequest
//            {
//                context = _context,
//                RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
//                UserID = AuthHelper.GetClaimValue(User, "UserID"),
//                AdsDTO = model
//            };

//            var response = DicomApp.BL.Services.AdvertisementService.EditGame(request);

//            return Json(new { success = response.Success, message = response.Message });
//        }

//        [AuthorizePerRole("WarehouseBack")]
//        public IActionResult BackToWarehouse(int shipId = 0)
//        {
//            var request = new AdvertisementRequest
//            {
//                context = _context,
//                RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
//                UserID = AuthHelper.GetClaimValue(User, "UserID"),
//                ShipDTO = new AdsDTO
//                {
//                    AdvertisementId = shipId,
//                    StatusId = (int)EnumStatus.At_Warehouse
//                }
//            };

//            var response = DicomApp.BL.Services.AdvertisementService.ChangeStatus(request);

//            return Json(new { success = response.Success, message = response.Message });
//        }

//        #endregion

//        #region Stock Report

//        [AuthorizePerRole("WarehouseStock")]
//        public IActionResult StockReport(string ActionType, int pageIndex = 0)
//        {
//            var request = new VendorProductRequest
//            {
//                RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
//                UserID = AuthHelper.GetClaimValue(User, "UserID"),
//                context = _context,
//            };
//            var response = VendorProductService.GetAllProducts(request);

//            if (ActionType == SystemConstants.ActionType.PartialView)
//                return PartialView(response.VendorProductDTOs);
//            else
//                return View(response.VendorProductDTOs);
//        }

//        #endregion

//        #region Courier's Daily Order

//        [AuthorizePerRole("WarehouseCouriersShipments")]
//        public IActionResult Courier(string Search, string ActionType = null, int DeliveryManId = 0)
//        {
//            if (ActionType != SystemConstants.ActionType.Table)
//                ViewBag.Couriers = GeneralHelper.GetUsers((int)EnumRole.DeliveryMan, _context);

//            var request = new UserRequest
//            {
//                RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
//                UserID = AuthHelper.GetClaimValue(User, "UserID"),
//                context = _context,
//                HasCourierShipments = true,
//                applyFilter = true,

//                UserDTO = new UserDTO
//                {
//                    RoleID = (int)EnumRole.DeliveryMan,
//                    HasCourierShipments = true
//                }
//            };
//            var response = UserService.GetAllUsers(request);

//            if (ActionType == SystemConstants.ActionType.PartialView)
//                return PartialView(response.UserDTOs);
//            else if (ActionType == SystemConstants.ActionType.Table)
//                return PartialView("_Courier", response.UserDTOs);
//            else if (ActionType == SystemConstants.ActionType.Print)
//                return BaseHelper.GeneratePDF<List<UserDTO>>(
//                    "~/Views/Warehouse/CourierPDF.cshtml",
//                    response.UserDTOs
//                );
//            else
//                return View(response.UserDTOs);
//        }

//        #endregion
//    }
//}

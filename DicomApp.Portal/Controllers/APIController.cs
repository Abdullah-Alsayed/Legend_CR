//using System;
//using System.Collections.Generic;
//using System.IdentityModel.Tokens.Jwt;
//using System.Linq;
//using System.Net;
//using System.Security.Claims;
//using System.Text;
//using DicomApp.BL.Services;
//using DicomApp.CommonDefinitions.DTO;
//using DicomApp.CommonDefinitions.DTO;
//using DicomApp.CommonDefinitions.DTO.AdvertisementDTOs;
//using DicomApp.CommonDefinitions.DTO.PickupDTOs;
//using DicomApp.CommonDefinitions.DTO.AdvertisementDTOs;
//using DicomApp.CommonDefinitions.Requests;
//using DicomApp.CommonDefinitions.Responses;
//using DicomApp.DAL.DB;
//using DicomApp.Helpers;
//using Microsoft.AspNetCore.Authentication.JwtBearer;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Cors;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Logging;
//using Microsoft.IdentityModel.Tokens;

//namespace DicomApp.API.Controllers
//{
//    [EnableCors("MyPolicy")]
//    [Route("api/[controller]")]
//    public class APIsController : Controller
//    {
//        private readonly ShippingDBContext _context;
//        private readonly ILogger<APIsController> _log;

//        public APIsController(ShippingDBContext context, ILogger<APIsController> log)
//        {
//            _log = log;
//            _context = context;
//        }

//        [HttpPost]
//        [Route("Login")]
//        public IActionResult Login([FromBody] UserDTO model)
//        {
//            var loginRequest = new UserRequest { UserDTO = model, context = _context };
//            var loginResponse = UserService.Login(loginRequest);
//            if (loginResponse.Success && loginResponse.UserDTOs.Count > 0)
//            {
//                var user = loginResponse.UserDTOs[0];
//                var claims = new[]
//                {
//                    new Claim(JwtRegisteredClaimNames.Sub, user.Name),
//                    new Claim("UserID", user.Id.ToString()),
//                    new Claim("RoleID", user.RoleID.ToString())
//                };
//                var signingKey = new SymmetricSecurityKey(
//                    Encoding.UTF8.GetBytes("GTSportsSecurityKey")
//                );
//                var token = new JwtSecurityToken(
//                    issuer: "GT",
//                    audience: "GT",
//                    expires: DateTime.UtcNow.AddDays(7),
//                    claims: claims,
//                    signingCredentials: new SigningCredentials(
//                        signingKey,
//                        SecurityAlgorithms.HmacSha256
//                    )
//                );
//                var tokenHandler = new JwtSecurityTokenHandler().WriteToken(token);
//                user.Token = tokenHandler;
//                return Ok(
//                    new
//                    {
//                        token = tokenHandler,
//                        expiration = token.ValidTo,
//                        userId = user.Id,
//                        roleId = user.RoleID,
//                        roleName = user.RoleName,
//                        userName = user.Name,
//                        email = user.Email
//                    }
//                );
//            }
//            //loginResponse.Message = "User Not Exist";
//            return Ok(loginResponse);
//        }

//        [Route("ListShips")]
//        [HttpPost]
//        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
//        public ActionResult ListShips([FromBody] AdvertisementRequest request)
//        {
//            try
//            {
//                var model = new AdsDTO()
//                {
//                    StatusIDs = new List<int>
//                    {
//                        (int)EnumStatus.Postponed,
//                        (int)EnumStatus.Out_For_Delivery,
//                        (int)EnumStatus.Out_For_Return,
//                        (int)EnumStatus.Out_For_Refund,
//                        (int)EnumStatus.Cancelled,
//                        (int)EnumStatus.Delivered,
//                        (int)EnumStatus.Refunded
//                    },
//                    DeliveryManId = request.ShipDTO_Filter.DeliveryManId,
//                    AdvertisementId = request.ShipDTO_Filter.AdvertisementId,
//                };
//                request.ShipDTO = model;
//                request.context = _context;
//                request.RoleID = AuthHelper.GetClaimValue(User, "RoleID");
//                request.UserID = AuthHelper.GetClaimValue(User, "UserID");
//                request.IsDesc = true;
//                request.applyFilter = true;
//                request.HasProblemDTOs = true;

//                var userResponse = BL.Services.AdvertisementService.GetAllShipments(request);

//                return Ok(new { userResponse.ShipDTOs });
//            }
//            catch
//            {
//                return BadRequest();
//            }
//        }

//        [Route("ListPickups")]
//        [HttpPost]
//        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
//        public ActionResult ListPickups([FromBody] PickUpRequestRequest request)
//        {
//            try
//            {
//                var model = new PickupDTO()
//                {
//                    StatusIDs = new List<int>
//                    {
//                        (int)EnumStatus.Assigned_For_Pickup,
//                        (int)EnumStatus.Cancelled_Pickup,
//                        (int)EnumStatus.Picked_Up
//                    },
//                    DeliveryManId = request.PickupDTO.DeliveryManId,
//                    PickupRequestId = request.PickupDTO.PickupRequestId,
//                };
//                request.PickupDTO = model;
//                request.HasPickupItemDTO = true;
//                request.context = _context;
//                request.RoleID = AuthHelper.GetClaimValue(User, "RoleID");
//                request.UserID = AuthHelper.GetClaimValue(User, "UserID");
//                request.IsDesc = true;
//                request.applyFilter = true;

//                var userResponse = PickUpRequestService.GetAllPickUpRequests(request);

//                return Ok(new { userResponse.PickupDTOs });
//            }
//            catch
//            {
//                return BadRequest();
//            }
//        }

//        [Route("ListRecieve")]
//        [HttpPost]
//        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
//        public ActionResult ListRecieve([FromBody] AdvertisementRequest request)
//        {
//            try
//            {
//                var model = new AdsDTO()
//                {
//                    StatusIDs = new List<int>
//                    {
//                        (int)EnumStatus.Assigned_For_Delivery,
//                        (int)EnumStatus.Assigned_For_Return,
//                        (int)EnumStatus.Assigned_For_Refund
//                    },
//                    DeliveryManId = request.ShipDTO_Filter.DeliveryManId,
//                    AdvertisementId = request.ShipDTO_Filter.AdvertisementId,
//                };
//                request.ShipDTO = model;
//                request.context = _context;
//                request.RoleID = AuthHelper.GetClaimValue(User, "RoleID");
//                request.UserID = AuthHelper.GetClaimValue(User, "UserID");
//                request.IsDesc = true;
//                request.applyFilter = true;

//                var userResponse = BL.Services.AdvertisementService.GetAllShipments(request);

//                return Ok(new { userResponse.ShipDTOs });
//            }
//            catch
//            {
//                return BadRequest();
//            }
//        }

//        [Route("ChangeStatus")]
//        [HttpPost]
//        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
//        public ActionResult ChangeStatus([FromBody] AdvertisementRequest request)
//        {
//            try
//            {
//                var response = new AdvertisementResponse();

//                //*** (1) START : Receive Pickups From Vendor
//                if (
//                    request.ShipDTO_Filter.PickupRequestId > 0
//                    && request.ShipDTO_Filter.StatusId == (int)EnumStatus.Picked_Up
//                )
//                {
//                    var PickupRequest = new PickUpRequestRequest();
//                    PickupRequest.context = _context;
//                    PickupRequest.RoleID = AuthHelper.GetClaimValue(User, "RoleID");
//                    PickupRequest.UserID = AuthHelper.GetClaimValue(User, "UserID");
//                    PickupRequest.PickupDTO = new PickupDTO
//                    {
//                        PickupRequestId = (int)request.ShipDTO_Filter.PickupRequestId,
//                        PickupItemsIDs =
//                            request.ShipDTO_Filter.PickupItemsIDs != null
//                                ? request.ShipDTO_Filter.PickupItemsIDs
//                                : null,
//                        StatusId = request.ShipDTO_Filter.StatusId,
//                    };

//                    var PickupResponse = BL.Services.PickUpRequestService.CourierReceive(
//                        PickupRequest
//                    );
//                    response.Message = PickupResponse.Message;
//                    response.Success = PickupResponse.Success;
//                    response.StatusCode = PickupResponse.StatusCode;
//                }
//                //*** (1) END : Receive Pickups From Vendor


//                //*** (2) START : Receive Shipments From Warehouse
//                else if (
//                    request.ShipDTO_Filter.AdvertisementIds != null
//                    && (
//                        request.ShipDTO_Filter.StatusId == (int)EnumStatus.Out_For_Delivery
//                        || request.ShipDTO_Filter.StatusId == (int)EnumStatus.Out_For_Return
//                        || request.ShipDTO_Filter.StatusId == (int)EnumStatus.Out_For_Refund
//                    )
//                )
//                {
//                    request.context = _context;
//                    request.RoleID = AuthHelper.GetClaimValue(User, "RoleID");
//                    request.UserID = AuthHelper.GetClaimValue(User, "UserID");
//                    request.ShipDTO = new AdsDTO
//                    {
//                        AdvertisementIds = request.ShipDTO_Filter.AdvertisementIds,
//                        StatusId = request.ShipDTO_Filter.StatusId,
//                    };

//                    response = BL.Services.AdvertisementService.ChangeStatus(request);
//                }
//                //*** (2) END : Receive Shipments From Warehouse


//                //*** (3) START : Deliver/Return/Refund Shipments To Customer/Vendor
//                else if (
//                    request.ShipDTO_Filter.AdvertisementIds != null
//                    && (
//                        request.ShipDTO_Filter.StatusId == (int)EnumStatus.Delivered
//                        || request.ShipDTO_Filter.StatusId == (int)EnumStatus.Returned
//                        || request.ShipDTO_Filter.StatusId == (int)EnumStatus.Refunded
//                    )
//                )
//                {
//                    request.context = _context;
//                    request.RoleID = AuthHelper.GetClaimValue(User, "RoleID");
//                    request.UserID = AuthHelper.GetClaimValue(User, "UserID");
//                    request.ShipDTO = new AdsDTO
//                    {
//                        AdvertisementIds = request.ShipDTO_Filter.AdvertisementIds,
//                        ShipItemDTOs =
//                            request.ShipDTO_Filter.ShipItemDTOs != null
//                                ? request.ShipDTO_Filter.ShipItemDTOs
//                                : null,
//                        StatusId = request.ShipDTO_Filter.StatusId,
//                    };

//                    if (request.ShipDTO_Filter.StatusId == (int)EnumStatus.Delivered)
//                        response = BL.Services.AdvertisementService.Deliver(request);
//                    else if (request.ShipDTO_Filter.StatusId == (int)EnumStatus.Returned)
//                        response = BL.Services.AdvertisementService.Return(request);
//                    else if (request.ShipDTO_Filter.StatusId == (int)EnumStatus.Refunded)
//                        response = BL.Services.AdvertisementService.Refund(request);
//                }
//                //*** (3) END : Deliver/Return/Refund Shipments To Customer/Vendor

//                else
//                {
//                    response.Message = "Shipment ID or status Empty";
//                    response.Success = false;
//                }

//                return Ok(new { response });
//            }
//            catch (Exception ex)
//            {
//                LogHelper.LogException(ex.Message, ex.StackTrace);
//                return BadRequest();
//            }
//        }

//        [Route("ReportProblem")]
//        [HttpPost]
//        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
//        public ActionResult ReportProblem([FromBody] AdvertisementRequest request)
//        {
//            try
//            {
//                request.context = _context;
//                request.RoleID = AuthHelper.GetClaimValue(User, "RoleID");
//                request.UserID = AuthHelper.GetClaimValue(User, "UserID");
//                request.ShipDTO = new AdsDTO()
//                {
//                    AdvertisementId = request.ShipDTO_Filter.AdvertisementId,
//                    FeesDetailsDTO = request.ShipDTO_Filter.ShippingFeesPaid.HasValue
//                        ? new FeesDetailsDTO
//                        {
//                            ShippingFeesPaid = request.ShipDTO_Filter.ShippingFeesPaid.Value
//                        }
//                        : null,
//                    ProblemDTOs = new List<ShipProblemDTO>
//                    {
//                        new ShipProblemDTO
//                        {
//                            ProblemTypeId = request.ShipDTO_Filter.ProblemTypeID,
//                            Note = request.ShipDTO_Filter.Note,
//                            IsCourierProblem = true
//                        }
//                    }
//                };

//                var response = BL.Services.AdvertisementService.AddProblem(request);

//                return Ok(new { response });
//            }
//            catch (Exception ex)
//            {
//                LogHelper.LogException(ex.Message, ex.StackTrace);
//                return BadRequest();
//            }
//        }

//        [Route("UpdateUserLocation")]
//        [HttpPost]
//        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
//        public ActionResult UpdateUserLocation([FromBody] UserLocation request)
//        {
//            try
//            {
//                _context.UserLocation.Add(
//                    new UserLocation()
//                    {
//                        PlaceId = "10",
//                        ApplicationId = 1,
//                        CreationDateTime = DateTime.Now,
//                        SubmitDateTime = DateTime.Now,
//                        Latitude = request.Latitude,
//                        Longitude = request.Longitude,
//                        UserId = AuthHelper.GetClaimValue(User, "UserID"),
//                        ChannelId = 1,
//                        IsOnline = true,
//                        Title = request.Title
//                    }
//                );
//                _context.SaveChanges();

//                return Ok();
//            }
//            catch (Exception ex)
//            {
//                LogHelper.LogException(ex.Message, ex.StackTrace);
//                return BadRequest();
//            }
//        }

//        [Route("StatusList")]
//        [HttpPost]
//        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
//        public ActionResult StatusList()
//        {
//            var request = new StatusRequest
//            {
//                RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
//                UserID = AuthHelper.GetClaimValue(User, "UserID"),
//                context = _context,
//            };

//            var response = StatusService.GetStatus(request);

//            return Ok(new { response });
//        }

//        [Route("ShipmentTypeList")]
//        [HttpPost]
//        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
//        public ActionResult ShipmentType()
//        {
//            var response = _context.ShipmentType.ToList();

//            return Ok(new { response });
//        }

//        [Route("ProblemTypeList")]
//        [HttpPost]
//        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
//        public ActionResult ProblemTypeList()
//        {
//            var request = new ProblemTypeRequest
//            {
//                RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
//                UserID = AuthHelper.GetClaimValue(User, "UserID"),
//                context = _context,
//                ProblemTypeDTO = new ProblemTypeDTO()
//            };

//            var response = ProblemTypeService.ListProblemType(request);

//            return Ok(new { response });
//        }
//    }
//}

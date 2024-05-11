//using LegendCR.CommonDefinitions.DTO;
//using LegendCR.CommonDefinitions.DTO.CashDTOs;
//using LegendCR.CommonDefinitions.DTO.ShipmentDTOs;
//using LegendCR.CommonDefinitions.Requests;
//using LegendCR.CommonDefinitions.Responses;
//using LegendCR.DAL.DB;
//using LegendCR.Helpers;
//using Microsoft.EntityFrameworkCore;
//using System.Net;
//using System.Text.Json;

//namespace LegendCR.BL.Services
//{
//    public class ShipmentService : BaseService
//    {
//        #region Shipment Common Actions

//        public static ShipmentResponse AddShipment(ShipmentRequest request)
//        {
//            var response = new ShipmentResponse();
//            RunBase(request, response, (ShipmentRequest req) =>
//            {
//                try
//                {
//                    var ship = new Shipment();

//                    ship.ShipmentTypeId = request.ShipDTO.SettingDTO.IsStock ? (int)EnumShipmentType.StockDelivery : (int)EnumShipmentType.PickupDelivery;
//                    ship.ShipmentServiceId = request.ShipDTO.ShipmentServiceId;
//                    ship.StatusId = request.ShipDTO.SettingDTO.IsStock ? (int)EnumStatus.At_Warehouse : (int)EnumStatus.Ready_For_Pickup;
//                    ship.BranchId = 2; // request.ShipDTO.BranchId;
//                    ship.CustomerName = request.ShipDTO.CustomerDetailsDTO.CustomerName;
//                    ship.CustomerPhone = request.ShipDTO.CustomerDetailsDTO.CustomerPhone;
//                    ship.CustomerPhone2 = request.ShipDTO.CustomerDetailsDTO.CustomerPhone2;
//                    ship.CustomerAddress = request.ShipDTO.CustomerDetailsDTO.CustomerAddress;
//                    ship.VendorId = request.ShipDTO.VendorDetailsDTO.VendorId;
//                    ship.ZoneId = request.ShipDTO.ZoneId;
//                    ship.AreaId = request.ShipDTO.AreaId;
//                    ship.Floor = request.ShipDTO.CustomerDetailsDTO.Floor;
//                    ship.Apartment = request.ShipDTO.CustomerDetailsDTO.Apartment;
//                    ship.Landmark = request.ShipDTO.CustomerDetailsDTO.Landmark;
//                    ship.Location = request.ShipDTO.CustomerDetailsDTO.Location;
//                    ship.Description = request.ShipDTO.Description;
//                    ship.IsOpenPackage = request.ShipDTO.SettingDTO.IsOpenPackage;
//                    ship.IsFragilePackage = request.ShipDTO.SettingDTO.IsFragilePackage;
//                    ship.IsPartialDelivery = request.ShipDTO.SettingDTO.IsPartialDelivery;
//                    ship.IsStock = request.ShipDTO.SettingDTO.IsStock;
//                    ship.NoOfItems = request.ShipDTO.NoOfItems;

//                    ship.CreatedAt = DateTime.Now;
//                    ship.LastModifiedAt = DateTime.Now;
//                    ship.CreatedBy = request.UserID;
//                    ship.LastModifiedBy = request.UserID;
//                    ship.IsDeleted = false;

//                    ship.Total = request.ShipDTO.FeesDetailsDTO.Total;
//                    ship.VendorCash = request.ShipDTO.FeesDetailsDTO.Total;

//                    ship.Weight = request.ShipDTO.SettingDTO.Weight;
//                    ship.WarehouseWeight = request.ShipDTO.SettingDTO.Weight;
//                    ship.Size = request.ShipDTO.SettingDTO.Size;
//                    ship.WarehouseSize = request.ShipDTO.SettingDTO.Size;

//                    CommonUser user = request.context.CommonUser.FirstOrDefault(u => u.Id == request.ShipDTO.VendorDetailsDTO.VendorId);
//                    ship.VendorName = user.Name;
//                    ship.VendorPhone = user.PhoneNumber;

//                    // Check packing fees
//                    if (request.ShipDTO.SettingDTO.PackingId.HasValue)
//                    {
//                        Packing pack = request.context.Packing.Include(p => p.PackingType).FirstOrDefault(u => u.Id == request.ShipDTO.SettingDTO.PackingId);
//                        ship.PackingId = pack.Id;
//                        ship.WarehousePackingId = pack.Id;
//                        ship.PackingFees = pack.Price;
//                        ship.VendorCash -= pack.Price;
//                        pack.Count--;
//                    }

//                    // Check partial delivery fees
//                    if (ship.IsPartialDelivery)
//                    {
//                        ship.PartialDeliveryFees = BaseHelper.Constants.PartialDeliveryFees;
//                        ship.VendorCash -= BaseHelper.Constants.PartialDeliveryFees;
//                    }

//                    // Check weight fees
//                    ship.WeightFees = BaseHelper.CalcWeightFees(request.ShipDTO.SettingDTO.Weight);
//                    ship.VendorCash -= BaseHelper.CalcWeightFees(request.ShipDTO.SettingDTO.Weight);

//                    // Check shipping fees
//                    ship.ShippingFees = request.context.ZoneTax.FirstOrDefault(z => z.ZoneId == ship.ZoneId).Tax;
//                    ship.VendorCash -= ship.ShippingFees;

//                    if (ship.VendorCash < 0 || ship.Total <= 0)
//                    {
//                        response.Message = "Vendor cash or total is equal or less than to zero";
//                        response.Success = false;
//                        response.StatusCode = HttpStatusCode.NotImplemented;
//                        return response;
//                    }

//                    request.context.Shipment.Add(ship);
//                    request.context.SaveChanges();

//                    ship.RefId = BaseHelper.GenerateRefId(EnumRefIdType.Shipment, ship.ShipmentId);

//                    // Save stock items if exist
//                    if (ship.IsStock)
//                    {
//                        var selectedStockIDs = request.ShipDTO.ProductIDs.Split(',').Select(int.Parse).ToList();
//                        var selectedStockQuantities = request.ShipDTO.ProductsQuantity.Split(',').Select(int.Parse).ToList();
//                        var selectedStockPrices = request.ShipDTO.ProductsPrice.Split(',').Select(int.Parse).ToList();

//                        for (int i = 0; i < selectedStockIDs.Count(); i++)
//                        {
//                            if (selectedStockQuantities[i] > 0)
//                            {
//                                var product = request.context.VendorProduct.FirstOrDefault(z => z.VendorProductId == selectedStockIDs[i]);
//                                request.context.ShipmentItem.Add(new ShipmentItem()
//                                {
//                                    ShipmentId = ship.ShipmentId,
//                                    VendorProductId = selectedStockIDs[i],
//                                    Quantity = selectedStockQuantities[i],
//                                    Price = selectedStockPrices[i],
//                                    Size = product.Size,
//                                    Name = product.Name,
//                                    CreatedAt = DateTime.Now,
//                                    CreatedBy = request.UserID,
//                                    StatusId = (int)EnumStatus.At_Warehouse,
//                                });
//                            }
//                        }
//                    }

//                    // Save partial items if exist
//                    if (!ship.IsStock && ship.IsPartialDelivery)
//                    {
//                        if (!string.IsNullOrEmpty(request.ShipDTO.PartialItems))
//                        {
//                            var items = JsonSerializer.Deserialize<List<ShipmentItem>>(request.ShipDTO.PartialItems);
//                            foreach (var item in items)
//                            {
//                                item.ShipmentId = ship.ShipmentId;
//                                item.CreatedAt = DateTime.Now;
//                                item.CreatedBy = request.UserID;
//                                item.StatusId = (int)EnumStatus.Ready_For_Pickup;
//                            }

//                            request.context.ShipmentItem.AddRange(items);
//                        }
//                    }

//                    //Add follow up
//                    var follow = new FollowUpDTO
//                    {
//                        FollowUpTypeId = (int)EnumFollowupType.Order_Added,
//                        Title = "Shipment Added",
//                        CreatedBy = request.UserID,
//                        ShipmentId = ship.ShipmentId,
//                        StatusId = ship.StatusId
//                    };
//                    FollowUpService.Add(follow, request.context);

//                    //Add notification
//                    request.context.Notification.Add(new Notification
//                    {
//                        Body = "New shipment " + ship.RefId + " added",
//                        CreationDate = DateTime.Now,
//                        Icon = ship.RefId,
//                        SenderId = request.UserID,
//                        Title = "New shipment",
//                        RecipientRoleId = (int)EnumRole.BranchManger,
//                        IsDeleted = false,
//                        IsSeen = false,
//                        RecipientId = 0,
//                    });

//                    request.context.SaveChanges();

//                    response.Message = "New shipment " + ship.RefId + " successfully added";
//                    response.Success = true;
//                    response.StatusCode = HttpStatusCode.OK;
//                }
//                catch (Exception ex)
//                {
//                    response.Message = ex.Message;
//                    response.Success = false;
//                    LogHelper.LogException(ex.Message, ex.StackTrace);
//                }
//                return response;
//            });
//            return response;
//        }

//        public static ShipmentResponse AddShipmentRefund(ShipmentRequest request)
//        {
//            var response = new ShipmentResponse();
//            RunBase(request, response, (ShipmentRequest req) =>
//            {
//                try
//                {
//                    var ship = new Shipment();

//                    ship.ShipmentTypeId = (int)EnumShipmentType.CashDelivery;
//                    ship.ShipmentServiceId = (int)EnumShipmentService.Refund;
//                    ship.StatusId = (int)EnumStatus.Ready_For_Refund;
//                    ship.BranchId = 2; // request.ShipDTO.BranchId;
//                    ship.CustomerName = request.ShipDTO.CustomerDetailsDTO.CustomerName;
//                    ship.CustomerPhone = request.ShipDTO.CustomerDetailsDTO.CustomerPhone;
//                    ship.CustomerPhone2 = request.ShipDTO.CustomerDetailsDTO.CustomerPhone2;
//                    ship.CustomerAddress = request.ShipDTO.CustomerDetailsDTO.CustomerAddress;
//                    ship.VendorId = request.ShipDTO.VendorDetailsDTO.VendorId;
//                    ship.ZoneId = request.ShipDTO.ZoneId;
//                    ship.AreaId = request.ShipDTO.AreaId;
//                    ship.Floor = request.ShipDTO.CustomerDetailsDTO.Floor;
//                    ship.Apartment = request.ShipDTO.CustomerDetailsDTO.Apartment;
//                    ship.Landmark = request.ShipDTO.CustomerDetailsDTO.Landmark;
//                    ship.Location = request.ShipDTO.CustomerDetailsDTO.Location;
//                    ship.Description = request.ShipDTO.Description;

//                    ship.CreatedAt = DateTime.Now;
//                    ship.LastModifiedAt = DateTime.Now;
//                    ship.CreatedBy = request.UserID;
//                    ship.LastModifiedBy = request.UserID;
//                    ship.IsDeleted = false;

//                    // Check Shipment Fees
//                    ship.RefundFees = BaseHelper.Constants.RefundFees;
//                    ship.RefundCash = request.ShipDTO.RefundCash;
//                    ship.ShippingFees = request.context.ZoneTax.FirstOrDefault(z => z.ZoneId == ship.ZoneId).Tax;
//                    ship.Total = request.ShipDTO.RefundCash.Value + ship.ShippingFees + ship.RefundFees.Value;

//                    ship.ShipToRefundId = request.ShipDTO.ShipmentId;

//                    CommonUser user = request.context.CommonUser.FirstOrDefault(u => u.Id == request.ShipDTO.VendorDetailsDTO.VendorId);
//                    ship.VendorName = user.Name;
//                    ship.VendorPhone = user.PhoneNumber;

//                    if (ship.Total <= 0 || ship.RefundCash <= 0)
//                    {
//                        response.Message = "Refund cash or total is equal or less than to zero";
//                        response.Success = false;
//                        response.StatusCode = HttpStatusCode.NotImplemented;
//                        return response;
//                    }

//                    request.context.Shipment.Add(ship);
//                    request.context.SaveChanges();

//                    ship.RefId = BaseHelper.GenerateRefId(EnumRefIdType.Shipment_Refund, ship.ShipmentId);

//                    //Add follow up
//                    var follow = new FollowUpDTO
//                    {
//                        FollowUpTypeId = (int)EnumFollowupType.Order_Added,
//                        Title = "Shipment Added",
//                        CreatedBy = request.UserID,
//                        ShipmentId = ship.ShipmentId,
//                        StatusId = ship.StatusId
//                    };
//                    FollowUpService.Add(follow, request.context);

//                    //Add notification
//                    request.context.Notification.Add(new Notification
//                    {
//                        Body = "New shipment " + ship.RefId + " added",
//                        CreationDate = DateTime.Now,
//                        Icon = ship.RefId,
//                        SenderId = request.UserID,
//                        Title = "New shipment",
//                        RecipientRoleId = (int)EnumRole.BranchManger,
//                        IsDeleted = false,
//                        IsSeen = false,
//                        RecipientId = 0,
//                    });

//                    request.context.SaveChanges();

//                    response.Message = "New shipment " + ship.RefId + " successfully added";
//                    response.Success = true;
//                    response.StatusCode = HttpStatusCode.OK;
//                }
//                catch (Exception ex)
//                {
//                    response.Message = ex.Message;
//                    response.Success = false;
//                    LogHelper.LogException(ex.Message, ex.StackTrace);
//                }
//                return response;
//            });
//            return response;
//        }

//        public static ShipmentResponse EditShipment(ShipmentRequest request)
//        {
//            var response = new ShipmentResponse();
//            RunBase(request, response, (ShipmentRequest req) =>
//            {
//                try
//                {
//                    var ship = request.context.Shipment.FirstOrDefault(s => s.ShipmentId == request.ShipDTO.ShipmentId);

//                    ship.ShipmentTypeId = request.ShipDTO.SettingDTO.IsStock ? (int)EnumShipmentType.StockDelivery : (int)EnumShipmentType.PickupDelivery;
//                    ship.StatusId = request.ShipDTO.SettingDTO.IsStock ? (int)EnumStatus.At_Warehouse : (int)EnumStatus.Ready_For_Pickup;
//                    ship.ShipmentServiceId = request.ShipDTO.ShipmentServiceId;
//                    ship.CustomerName = request.ShipDTO.CustomerDetailsDTO.CustomerName;
//                    ship.CustomerPhone = request.ShipDTO.CustomerDetailsDTO.CustomerPhone;
//                    ship.CustomerPhone2 = request.ShipDTO.CustomerDetailsDTO.CustomerPhone2;
//                    ship.CustomerAddress = request.ShipDTO.CustomerDetailsDTO.CustomerAddress;
//                    ship.VendorId = request.ShipDTO.VendorDetailsDTO.VendorId;
//                    ship.ZoneId = request.ShipDTO.ZoneId;
//                    ship.AreaId = request.ShipDTO.AreaId;
//                    ship.Floor = request.ShipDTO.CustomerDetailsDTO.Floor;
//                    ship.Apartment = request.ShipDTO.CustomerDetailsDTO.Apartment;
//                    ship.Landmark = request.ShipDTO.CustomerDetailsDTO.Landmark;
//                    ship.Location = request.ShipDTO.CustomerDetailsDTO.Location;
//                    ship.Description = request.ShipDTO.Description;
//                    ship.IsOpenPackage = request.ShipDTO.SettingDTO.IsOpenPackage;
//                    ship.IsFragilePackage = request.ShipDTO.SettingDTO.IsFragilePackage;
//                    ship.IsPartialDelivery = request.ShipDTO.SettingDTO.IsPartialDelivery;
//                    ship.IsStock = request.ShipDTO.SettingDTO.IsStock;
//                    ship.NoOfItems = request.ShipDTO.NoOfItems;

//                    ship.LastModifiedAt = DateTime.Now;
//                    ship.LastModifiedBy = request.UserID;

//                    ship.Total = request.ShipDTO.FeesDetailsDTO.Total;
//                    ship.VendorCash = request.ShipDTO.FeesDetailsDTO.Total;

//                    ship.Weight = request.ShipDTO.SettingDTO.Weight;
//                    ship.Size = request.ShipDTO.SettingDTO.Size;

//                    // Check packing >> NULL then NOT NULL
//                    if (!ship.WarehousePackingId.HasValue && request.ShipDTO.SettingDTO.WarehousePackingId.HasValue)
//                    {
//                        Packing pack = request.context.Packing.FirstOrDefault(u => u.Id == request.ShipDTO.SettingDTO.PackingId);
//                        ship.PackingId = pack.Id;
//                        ship.WarehousePackingId = pack.Id;
//                        ship.PackingFees = pack.Price;
//                        ship.VendorCash -= pack.Price;
//                        pack.Count--;
//                    } // Check packing >> NOT NULL then NOT NULL
//                    else if (ship.WarehousePackingId.HasValue && request.ShipDTO.SettingDTO.WarehousePackingId.HasValue)
//                    {
//                        // Update Old Data
//                        Packing pack = request.context.Packing.FirstOrDefault(u => u.Id == ship.WarehousePackingId);
//                        pack.Count++;
//                        ship.VendorCash += pack.Price;

//                        request.context.SaveChanges();

//                        // Insert New Data
//                        pack = request.context.Packing.FirstOrDefault(u => u.Id == request.ShipDTO.SettingDTO.WarehousePackingId);
//                        pack.Count--;

//                        ship.WarehousePackingId = pack.Id;
//                        ship.PackingFees = pack.Price;
//                        ship.VendorCash -= pack.Price;
//                    }

//                    // Check partial delivery fees
//                    if (request.ShipDTO.SettingDTO.IsPartialDelivery)
//                    {
//                        ship.PartialDeliveryFees = BaseHelper.Constants.PartialDeliveryFees;
//                        ship.VendorCash -= BaseHelper.Constants.PartialDeliveryFees;
//                    }

//                    // Check weight fees
//                    ship.WeightFees = BaseHelper.CalcWeightFees(request.ShipDTO.SettingDTO.Weight);
//                    ship.VendorCash -= BaseHelper.CalcWeightFees(request.ShipDTO.SettingDTO.Weight);

//                    // Check shipping fees
//                    ship.ShippingFees = request.context.ZoneTax.FirstOrDefault(z => z.ZoneId == ship.ZoneId).Tax;
//                    ship.VendorCash -= ship.ShippingFees;

//                    // Delete old shipment sub items then add new
//                    var oldItems = request.context.ShipmentItem.Where(i => i.ShipmentId == ship.ShipmentId);
//                    foreach (var item in oldItems)
//                        item.IsDeleted = true;

//                    // Check stock items
//                    if (ship.IsStock)
//                    {
//                        var selectedStockIDs = request.ShipDTO.ProductIDs.Split(',').Select(int.Parse).ToList();
//                        var selectedStockQuantities = request.ShipDTO.ProductsQuantity.Split(',').Select(int.Parse).ToList();
//                        var selectedStockPrices = request.ShipDTO.ProductsPrice.Split(',').Select(int.Parse).ToList();

//                        for (int i = 0; i < selectedStockIDs.Count(); i++)
//                        {
//                            if (selectedStockQuantities[i] > 0)
//                            {
//                                var product = request.context.VendorProduct.FirstOrDefault(z => z.VendorProductId == selectedStockIDs[i]);
//                                request.context.ShipmentItem.Add(new ShipmentItem()
//                                {
//                                    ShipmentId = ship.ShipmentId,
//                                    VendorProductId = selectedStockIDs[i],
//                                    Quantity = selectedStockQuantities[i],
//                                    Price = selectedStockPrices[i],
//                                    Size = product.Size,
//                                    Name = product.Name,
//                                    CreatedAt = DateTime.Now,
//                                    CreatedBy = request.UserID,
//                                    StatusId = (int)EnumStatus.At_Warehouse
//                                });
//                            }
//                        }
//                    }

//                    // Check partial items if exist
//                    if (!ship.IsStock && ship.IsPartialDelivery)
//                    {
//                        if (!string.IsNullOrEmpty(request.ShipDTO.PartialItems))
//                        {
//                            var items = JsonSerializer.Deserialize<List<ShipmentItem>>(request.ShipDTO.PartialItems);
//                            foreach (var item in items)
//                            {
//                                item.ShipmentId = ship.ShipmentId;
//                                item.CreatedAt = DateTime.Now;
//                                item.CreatedBy = request.UserID;
//                                item.StatusId = (int)EnumStatus.Ready_For_Pickup;
//                            }

//                            request.context.ShipmentItem.AddRange(items);
//                        }
//                    }

//                    request.context.SaveChanges();

//                    //Add follow up
//                    var follow = new FollowUpDTO
//                    {
//                        FollowUpTypeId = (int)EnumFollowupType.Order_Updated,
//                        Title = "Shipment Updated",
//                        CreatedBy = request.UserID,
//                        ShipmentId = ship.ShipmentId,
//                        StatusId = ship.StatusId
//                    };
//                    FollowUpService.Add(follow, request.context);

//                    //Add notification
//                    request.context.Notification.Add(new Notification
//                    {
//                        Body = "Shipment " + ship.RefId + " updated",
//                        CreationDate = DateTime.Now,
//                        Icon = ship.RefId,
//                        SenderId = request.UserID,
//                        Title = "Shipment updated",
//                        RecipientRoleId = (int)EnumRole.BranchManger,
//                        IsDeleted = false,
//                        IsSeen = false,
//                        RecipientId = 0,
//                    });

//                    request.context.SaveChanges();

//                    response.Message = "Shipment " + ship.RefId + " successfully updated";
//                    response.Success = true;
//                    response.StatusCode = HttpStatusCode.OK;
//                }
//                catch (Exception ex)
//                {
//                    response.Message = ex.Message;
//                    response.Success = false;
//                    LogHelper.LogException(ex.Message, ex.StackTrace);
//                }
//                return response;
//            });
//            return response;
//        }

//        public static ShipmentResponse EditAddress(ShipmentRequest request)
//        {
//            var response = new ShipmentResponse();
//            RunBase(request, response, (ShipmentRequest req) =>
//            {
//                try
//                {
//                    var ship = request.context.Shipment.FirstOrDefault(s => s.ShipmentId == request.ShipDTO.ShipmentId);

//                    ship.CustomerAddress = request.ShipDTO.CustomerDetailsDTO.CustomerAddress;
//                    ship.Location = request.ShipDTO.CustomerDetailsDTO.Location;
//                    ship.Landmark = request.ShipDTO.CustomerDetailsDTO.Landmark;
//                    ship.Floor = request.ShipDTO.CustomerDetailsDTO.Floor;
//                    ship.Apartment = request.ShipDTO.CustomerDetailsDTO.Apartment;

//                    ship.LastModifiedAt = DateTime.Now;
//                    ship.LastModifiedBy = request.UserID;

//                    request.context.SaveChanges();

//                    //Add follow up
//                    var follow = new FollowUpDTO
//                    {
//                        FollowUpTypeId = (int)EnumFollowupType.Order_Updated,
//                        Title = "Customer Data Update",
//                        CreatedBy = request.UserID,
//                        ShipmentId = ship.ShipmentId,
//                        StatusId = ship.StatusId
//                    };
//                    FollowUpService.Add(follow, request.context);

//                    //Add notification
//                    request.context.Notification.Add(new Notification
//                    {
//                        Body = "Shipment " + ship.RefId + " updated",
//                        CreationDate = DateTime.Now,
//                        Icon = ship.RefId,
//                        SenderId = request.UserID,
//                        Title = "Shipment updated",
//                        RecipientRoleId = (int)EnumRole.BranchManger,
//                        IsDeleted = false,
//                        IsSeen = false,
//                        RecipientId = 0,
//                    });

//                    request.context.SaveChanges();

//                    response.Message = "Shipment " + ship.RefId + " successfully updated";
//                    response.Success = true;
//                    response.StatusCode = HttpStatusCode.OK;
//                }
//                catch (Exception ex)
//                {
//                    response.Message = ex.Message;
//                    response.Success = false;
//                    LogHelper.LogException(ex.Message, ex.StackTrace);
//                }
//                return response;
//            });
//            return response;
//        }

//        public static ShipmentResponse EditCallHistory(ShipmentRequest request)
//        {
//            var response = new ShipmentResponse();
//            RunBase(request, response, (ShipmentRequest req) =>
//            {
//                try
//                {
//                    var ship = request.context.Shipment.FirstOrDefault(s => s.ShipmentId == request.ShipDTO.ShipmentId);

//                    ship.CallAnswerCount = request.ShipDTO.CustomerFollowUpDTO.CallAnswerCount;
//                    ship.CallNotAnswerCount = request.ShipDTO.CustomerFollowUpDTO.CallNotAnswerCount;

//                    ship.LastModifiedAt = DateTime.Now;
//                    ship.LastModifiedBy = request.UserID;

//                    request.context.SaveChanges();

//                    //Add follow up
//                    var follow = new FollowUpDTO
//                    {
//                        FollowUpTypeId = (int)EnumFollowupType.Order_Updated,
//                        Title = "Customer Call Update",
//                        CreatedBy = request.UserID,
//                        ShipmentId = ship.ShipmentId,
//                        StatusId = ship.StatusId
//                    };
//                    FollowUpService.Add(follow, request.context);

//                    request.context.SaveChanges();

//                    response.Message = "Shipment " + ship.RefId + " successfully updated";
//                    response.Success = true;
//                    response.StatusCode = HttpStatusCode.OK;
//                }
//                catch (Exception ex)
//                {
//                    response.Message = ex.Message;
//                    response.Success = false;
//                    LogHelper.LogException(ex.Message, ex.StackTrace);
//                }
//                return response;
//            });
//            return response;
//        }

//        public static ShipmentResponse EditPacking(ShipmentRequest request)
//        {
//            var response = new ShipmentResponse();
//            RunBase(request, response, (ShipmentRequest req) =>
//            {
//                try
//                {
//                    var ship = request.context.Shipment.FirstOrDefault(s => s.ShipmentId == request.ShipDTO.ShipmentId);

//                    // Without OLD Packing
//                    if (!ship.WarehousePackingId.HasValue && request.ShipDTO.SettingDTO.WarehousePackingId.HasValue)
//                    {
//                        Packing pack = request.context.Packing.FirstOrDefault(u => u.Id == request.ShipDTO.SettingDTO.PackingId);
//                        ship.PackingId = pack.Id;
//                        ship.WarehousePackingId = pack.Id;
//                        ship.PackingFees = pack.Price;
//                        ship.VendorCash -= pack.Price;
//                        pack.Count--;
//                    }
//                    // Has OLD Packing
//                    else if (ship.WarehousePackingId.HasValue && request.ShipDTO.SettingDTO.WarehousePackingId.HasValue)
//                    {
//                        // Update OLD Packing
//                        Packing pack = request.context.Packing.FirstOrDefault(u => u.Id == ship.WarehousePackingId);
//                        pack.Count++;
//                        ship.VendorCash += pack.Price;

//                        request.context.SaveChanges();

//                        // Update NEW Packing
//                        pack = request.context.Packing.FirstOrDefault(u => u.Id == request.ShipDTO.SettingDTO.WarehousePackingId);
//                        pack.Count--;

//                        ship.WarehousePackingId = pack.Id;
//                        ship.PackingFees = pack.Price;
//                        ship.VendorCash -= pack.Price;
//                    }

//                    ship.WarehouseSize = request.ShipDTO.SettingDTO.WarehouseSize;
//                    ship.WarehouseWeight = request.ShipDTO.SettingDTO.WarehouseWeight;

//                    // Update OLD Weight Fees
//                    ship.VendorCash += ship.WeightFees;
//                    request.context.SaveChanges();

//                    // Update NEW Weight Fees
//                    ship.WeightFees = BaseHelper.CalcWeightFees(request.ShipDTO.SettingDTO.WarehouseWeight);
//                    ship.VendorCash -= ship.WeightFees;

//                    ship.LastModifiedAt = DateTime.Now;
//                    ship.LastModifiedBy = request.UserID;

//                    request.context.SaveChanges();

//                    //Add follow up
//                    var follow = new FollowUpDTO
//                    {
//                        FollowUpTypeId = (int)EnumFollowupType.Order_Updated,
//                        Title = "Packing Updated",
//                        CreatedBy = request.UserID,
//                        ShipmentId = ship.ShipmentId,
//                        StatusId = ship.StatusId
//                    };
//                    FollowUpService.Add(follow, request.context);

//                    request.context.SaveChanges();

//                    response.Message = "Shipment packing" + ship.RefId + " successfully updated";
//                    response.Success = true;
//                    response.StatusCode = HttpStatusCode.OK;
//                }
//                catch (Exception ex)
//                {
//                    response.Message = ex.Message;
//                    response.Success = false;
//                    LogHelper.LogException(ex.Message, ex.StackTrace);
//                }
//                return response;
//            });
//            return response;
//        }

//        public static ShipmentResponse CalculateShipping(ShipmentRequest request)
//        {
//            var response = new ShipmentResponse();
//            RunBase(request, response, (ShipmentRequest req) =>
//            {
//                try
//                {
//                    var result = new ShippingCalculatorDTO();

//                    result.ShipmentServiceId = request.ShippingCalculatorDTO.ShipmentServiceId;
//                    result.SourceAreaId = request.ShippingCalculatorDTO.SourceAreaId;
//                    result.DestinationAreaId = request.ShippingCalculatorDTO.DestinationAreaId;
//                    result.Weight = request.ShippingCalculatorDTO.Weight;
//                    result.Size = request.ShippingCalculatorDTO.Size;

//                    // Check weight fees
//                    result.WeightFees = BaseHelper.CalcWeightFees(request.ShippingCalculatorDTO.Weight);

//                    // Check source shipping fees
//                    //result.SourceZoneId = request.context.City.FirstOrDefault(z => z.Id == request.ShippingCalculatorDTO.SourceAreaId).ZoneId.Value;
//                    //result.SourceShippingFees = request.context.ZoneTax.FirstOrDefault(z => z.ZoneId == result.SourceZoneId).Tax;

//                    // Check destination shipping fees
//                    result.DestinationZoneId = request.context.City.FirstOrDefault(z => z.Id == request.ShippingCalculatorDTO.DestinationAreaId).ZoneId.Value;
//                    result.DestinationShippingFees = request.context.ZoneTax.FirstOrDefault(z => z.ZoneId == result.DestinationZoneId).Tax;

//                    result.TotalFees = result.WeightFees + result.SourceShippingFees + result.DestinationShippingFees;

//                    response.ShippingCalculatorDTO = result;

//                    response.Message = "Shipment Fees " + result.TotalFees;
//                    response.Success = true;
//                    response.StatusCode = HttpStatusCode.OK;
//                }
//                catch (Exception ex)
//                {
//                    response.Message = ex.Message;
//                    response.Success = false;
//                    LogHelper.LogException(ex.Message, ex.StackTrace);
//                }
//                return response;
//            });
//            return response;
//        }

//        #endregion

//        #region Shipment Change Status

//        public static ShipmentResponse ChangeStatus(ShipmentRequest request)
//        {
//            var response = new ShipmentResponse();
//            RunBase(request, response, (ShipmentRequest req) =>
//            {
//                try
//                {
//                    IQueryable<Shipment> shipments;
//                    if (request.ShipDTO.ShipmentIDs != null)
//                        shipments = request.context.Shipment.Where(r => request.ShipDTO.ShipmentIDs.Contains(r.ShipmentId));
//                    else
//                        shipments = request.context.Shipment.Where(r => r.ShipmentId == request.ShipDTO.ShipmentId);

//                    foreach (var ship in shipments)
//                    {
//                        var shipItems = request.context.ShipmentItem.Where(i => i.ShipmentId == ship.ShipmentId);
//                        foreach (var item in shipItems)
//                        {
//                            if (item.StatusId != (int)EnumStatus.Delivered || item.CourierQuantityReturned > 0)
//                                item.StatusId = request.ShipDTO.StatusId;

//                            if (request.ShipDTO.StatusId == (int)EnumStatus.Ready_For_Delivery && ship.IsStock)
//                            {
//                                // Update available stock
//                                var product = request.context.VendorProduct.FirstOrDefault(p => p.VendorProductId == item.VendorProductId);

//                                if (product.AvailableStock < item.Quantity)
//                                {
//                                    response.Message = "Not enough stock";
//                                    response.Success = false;
//                                    response.StatusCode = HttpStatusCode.BadRequest;
//                                    return response;
//                                }
//                                else
//                                    product.AvailableStock -= item.Quantity;
//                            }
//                            else if (request.ShipDTO.StatusId == (int)EnumStatus.At_Warehouse && ship.IsStock)
//                            {
//                                // Update available stock
//                                var product = request.context.VendorProduct.FirstOrDefault(p => p.VendorProductId == item.VendorProductId);
//                                product.AvailableStock += item.Quantity;
//                            }
//                        }

//                        ship.StatusId = request.ShipDTO.StatusId;
//                        ship.LastModifiedAt = DateTime.Now;
//                        ship.LastModifiedBy = request.UserID;

//                        if (request.ShipDTO.StatusId == (int)EnumStatus.Ready_For_Return)
//                        {
//                            ship.PickupRequestId = null;
//                            ship.RefId = BaseHelper.GenerateRefId(EnumRefIdType.Shipment_Return, ship.ShipmentId);
//                        }

//                        //Add follow up
//                        var follow = new FollowUpDTO
//                        {
//                            FollowUpTypeId = (int)EnumFollowupType.Order_Updated,
//                            Title = request.context.Status.FirstOrDefault(s => s.Id == request.ShipDTO.StatusId).Name,
//                            Comment = request.context.CommonUser.FirstOrDefault(u => u.Id == request.UserID).Name,
//                            CreatedBy = request.UserID,
//                            ShipmentId = ship.ShipmentId,
//                            StatusId = request.ShipDTO.StatusId
//                        };
//                        FollowUpService.Add(follow, request.context);
//                    }

//                    request.context.SaveChanges();

//                    response.Message = "Shipment successfully updated";
//                    response.Success = true;
//                    response.StatusCode = HttpStatusCode.OK;
//                }
//                catch (Exception ex)
//                {
//                    response.Message = ex.Message;
//                    response.Success = false;
//                    LogHelper.LogException(ex.Message, ex.StackTrace);
//                }
//                return response;
//            });
//            return response;
//        }

//        public static ShipmentResponse Cancel(ShipmentRequest request)
//        {
//            var response = new ShipmentResponse();
//            RunBase(request, response, (ShipmentRequest req) =>
//            {
//                try
//                {
//                    var ship = request.context.Shipment.FirstOrDefault(r => r.ShipmentId == request.ShipDTO.ShipmentId);
//                    var shipItems = request.context.ShipmentItem.Where(i => i.ShipmentId == ship.ShipmentId);
//                    var problem = request.ShipDTO.ProblemDTOs.FirstOrDefault();

//                    ship.CancelComment = request.ShipDTO.CancelComment;
//                    ship.IsTripCompleted = request.ShipDTO.IsTripCompleted;
//                    ship.LastModifiedAt = DateTime.Now;
//                    ship.LastModifiedBy = request.UserID;

//                    if (ship.ShippingFeesPaid == 0)
//                        ship.IsCashReceived = true;

//                    //*** Add shipment problem
//                    request.context.ShipmentProblem.Add(new ShipmentProblem
//                    {
//                        CreatedAt = DateTime.Now,
//                        CreatedBy = request.UserID,
//                        ProblemTypeId = problem.ProblemTypeId,
//                        Note = problem.Note,
//                        ShipmentId = ship.ShipmentId
//                    });

//                    //*** Add cancel follow up
//                    var Problemfollow = new FollowUpDTO
//                    {
//                        FollowUpTypeId = (int)EnumFollowupType.Problem_Added,
//                        Title = request.context.ProblemType.FirstOrDefault(s => s.ProblemTypeId == problem.ProblemTypeId).NameEn,
//                        Comment = problem.Note,
//                        CreatedBy = request.UserID,
//                        ShipmentId = ship.ShipmentId,
//                        StatusId = (int)EnumStatus.Cancelled
//                    };
//                    FollowUpService.Add(Problemfollow, request.context);

//                    //*** Add follow up
//                    var follow = new FollowUpDTO
//                    {
//                        FollowUpTypeId = (int)EnumFollowupType.Order_Updated,
//                        Title = request.context.Status.FirstOrDefault(s => s.Id == (int)EnumStatus.Cancelled).Name,
//                        Comment = request.context.CommonUser.FirstOrDefault(u => u.Id == request.UserID).Name,
//                        CreatedBy = request.UserID,
//                        ShipmentId = ship.ShipmentId,
//                        StatusId = (int)EnumStatus.Cancelled
//                    };
//                    FollowUpService.Add(follow, request.context);

//                    //*** Check for cancel fees
//                    //*** Note : No cancel fees will applied to below status
//                    var noCancelFeesStatusIDs = new List<int>
//                    {
//                        (int)EnumStatus.Ready_For_Pickup, (int)EnumStatus.Assigned_For_Pickup, (int)EnumStatus.Picked_Up,
//                        (int)EnumStatus.Ready_For_Return, (int)EnumStatus.Assigned_For_Return, (int)EnumStatus.Out_For_Return
//                    };
//                    if (!noCancelFeesStatusIDs.Contains(ship.StatusId))
//                    {
//                        #region Add Cancel Fees

//                        var VendorAccountId = request.context.Account.FirstOrDefault(a => a.UserId == ship.VendorId).AccountId;
//                        #region Vendor Transaction
//                        var VendorTrans = new AccountTransactionDTO();
//                        VendorTrans.CreatedBy = request.UserID;
//                        VendorTrans.ReceiverId = VendorAccountId;
//                        VendorTrans.VendorId = VendorAccountId;
//                        VendorTrans.ShipmentId = ship.ShipmentId;
//                        VendorTrans.StatusId = (int)EnumStatus.Cancelled;
//                        VendorTrans.TypeId = (int)EnumTransactionType.Withdraw;
//                        #endregion
//                        #region RED Transaction
//                        var REDTrans = new AccountTransactionDTO();
//                        REDTrans.CreatedBy = request.UserID;
//                        REDTrans.ReceiverId = (int)EnumAccountId.RED_Main_Account;
//                        REDTrans.VendorId = VendorAccountId;
//                        REDTrans.ShipmentId = ship.ShipmentId;
//                        REDTrans.StatusId = (int)EnumStatus.Cancelled;
//                        REDTrans.TypeId = (int)EnumTransactionType.Deposit;
//                        #endregion

//                        // Trip is Completed
//                        if (ship.IsTripCompleted)
//                        {
//                            ship.CancelFees = ship.ShippingFees +
//                                              ship.PackingFees +
//                                              ship.WeightFees +
//                                              ship.SizeFees +
//                                              ship.PartialDeliveryFees
//                                            - ship.ShippingFeesPaid;

//                            #region Vendor Transaction
//                            VendorTrans.ShippingFees = ship.ShippingFees;
//                            VendorTrans.PackingFees = ship.PackingFees;
//                            VendorTrans.WeightFees = ship.WeightFees;
//                            VendorTrans.SizeFees = ship.SizeFees;
//                            VendorTrans.PartialDeliveryFees = ship.PartialDeliveryFees;
//                            VendorTrans.ShippingFeesPaid = ship.ShippingFeesPaid;

//                            VendorTrans.CancelFees = ship.CancelFees;
//                            VendorTrans.Total = ship.CancelFees;
//                            AccountTransactionService.AddTransaction(request.context, VendorTrans);
//                            #endregion
//                            #region RED Transaction
//                            REDTrans.ShippingFees = ship.ShippingFees;
//                            REDTrans.PackingFees = ship.PackingFees;
//                            REDTrans.WeightFees = ship.WeightFees;
//                            REDTrans.SizeFees = ship.SizeFees;
//                            REDTrans.PartialDeliveryFees = ship.PartialDeliveryFees;
//                            REDTrans.ShippingFeesPaid = ship.ShippingFeesPaid;
//                            REDTrans.CancelFees = ship.CancelFees;

//                            REDTrans.Total = ship.ShippingFees + ship.PackingFees + ship.WeightFees + ship.SizeFees + ship.PartialDeliveryFees;
//                            AccountTransactionService.AddTransaction(request.context, REDTrans);
//                            #endregion
//                        }
//                        else // Trip is NOT Completed
//                        {
//                            ship.CancelFees = BaseHelper.Constants.CancelFees;

//                            #region Vendor Transaction
//                            VendorTrans.CancelFees = ship.CancelFees;
//                            VendorTrans.Total = ship.CancelFees;
//                            AccountTransactionService.AddTransaction(request.context, VendorTrans);
//                            #endregion
//                            #region RED Transaction
//                            REDTrans.CancelFees = ship.CancelFees;
//                            REDTrans.Total = ship.CancelFees;
//                            AccountTransactionService.AddTransaction(request.context, REDTrans);
//                            #endregion
//                        }
//                        #endregion
//                    }

//                    ship.StatusId = (int)EnumStatus.Cancelled;
//                    //*** Change sub items status
//                    foreach (var item in shipItems)
//                        item.StatusId = (int)EnumStatus.Cancelled;

//                    request.context.SaveChanges();

//                    response.Message = "Shipment successfully cancelled";
//                    response.Success = true;
//                    response.StatusCode = HttpStatusCode.OK;
//                }
//                catch (Exception ex)
//                {
//                    response.Message = ex.Message;
//                    response.Success = false;
//                    LogHelper.LogException(ex.Message, ex.StackTrace);
//                }
//                return response;
//            });
//            return response;
//        }

//        public static ShipmentResponse Deliver(ShipmentRequest request)
//        {
//            var response = new ShipmentResponse();
//            RunBase(request, response, (ShipmentRequest req) =>
//            {
//                try
//                {
//                    var shipments = request.context.Shipment.Where(r => request.ShipDTO.ShipmentIDs.Contains(r.ShipmentId));

//                    foreach (var ship in shipments)
//                    {
//                        //*** Deliver Shipment
//                        ship.StatusId = (int)EnumStatus.Delivered;
//                        ship.LastModifiedAt = DateTime.Now;
//                        ship.LastModifiedBy = request.UserID;

//                        #region Update Shipment Items
//                        if (request.ShipDTO.ShipItemDTOs != null)
//                        {
//                            foreach (var item in request.ShipDTO.ShipItemDTOs)
//                            {
//                                var shipItem = request.context.ShipmentItem.FirstOrDefault(i => i.ShipmentItemId == item.ShipmentItemId);

//                                if (item.CourierQuantityDelivered == 0 || item.CourierQuantityDelivered > shipItem.Quantity)
//                                {
//                                    response.Message = "Delivered quantity is equal to zero or greater than original";
//                                    response.Success = false;
//                                    response.StatusCode = HttpStatusCode.NotImplemented;
//                                    return response;
//                                }
//                                else
//                                {
//                                    shipItem.StatusId = (int)EnumStatus.Delivered;
//                                    shipItem.CourierQuantityDelivered = item.CourierQuantityDelivered;
//                                }
//                            }
//                        }
//                        else
//                        {
//                            var lstShipItems = request.context.ShipmentItem.Where(i => i.ShipmentId == ship.ShipmentId);
//                            foreach (var item in lstShipItems)
//                            {
//                                item.StatusId = (int)EnumStatus.Delivered;
//                                item.CourierQuantityDelivered = item.Quantity;
//                            }
//                        }
//                        #endregion

//                        #region Add Cash Transactions / Update Accounts Balances

//                        // *** Check if Shipment is fully delivered / Customer paid amount
//                        double CustomerPaidAmount = 0;
//                        bool takeOriginalCOD = true;
//                        var VendorAccountId = request.context.Account.FirstOrDefault(a => a.UserId == ship.VendorId).AccountId;

//                        var shipItems = request.context.ShipmentItem.Where(i => i.ShipmentId == ship.ShipmentId);
//                        foreach (var item in shipItems)
//                        {
//                            if (item.StatusId != (int)EnumStatus.Delivered || item.CourierQuantityDelivered < item.Quantity)
//                                takeOriginalCOD = false;

//                            CustomerPaidAmount += (item.Price * item.CourierQuantityDelivered);
//                        }

//                        #region Vendor Transactions
//                        var VendorTrans = new AccountTransactionDTO();
//                        VendorTrans.CreatedBy = request.UserID;
//                        VendorTrans.ReceiverId = VendorAccountId;
//                        VendorTrans.VendorId = VendorAccountId;
//                        VendorTrans.ShipmentId = ship.ShipmentId;
//                        VendorTrans.StatusId = (int)EnumStatus.Delivered;

//                        VendorTrans.VendorCash = takeOriginalCOD ? ship.VendorCash :
//                                                (CustomerPaidAmount -
//                                                 ship.ShippingFees -
//                                                 ship.PackingFees -
//                                                 ship.WeightFees -
//                                                 ship.SizeFees -
//                                                 ship.PartialDeliveryFees);
//                        VendorTrans.Total = VendorTrans.VendorCash;
//                        VendorTrans.TypeId = (int)EnumTransactionType.Deposit;
//                        AccountTransactionService.AddTransaction(request.context, VendorTrans);
//                        #endregion

//                        #region RED Transactions
//                        var REDTrans = new AccountTransactionDTO();
//                        REDTrans.CreatedBy = request.UserID;
//                        REDTrans.ReceiverId = (int)EnumAccountId.RED_Main_Account;
//                        REDTrans.VendorId = VendorAccountId;
//                        REDTrans.ShipmentId = ship.ShipmentId;
//                        REDTrans.StatusId = (int)EnumStatus.Delivered;

//                        REDTrans.ShippingFees = ship.ShippingFees;
//                        REDTrans.PackingFees = ship.PackingFees;
//                        REDTrans.WeightFees = ship.WeightFees;
//                        REDTrans.SizeFees = ship.SizeFees;
//                        REDTrans.PartialDeliveryFees = ship.PartialDeliveryFees;

//                        REDTrans.Total = ship.ShippingFees +
//                                         ship.PackingFees +
//                                         ship.WeightFees +
//                                         ship.SizeFees +
//                                         ship.PartialDeliveryFees;

//                        REDTrans.TypeId = (int)EnumTransactionType.Deposit;
//                        AccountTransactionService.AddTransaction(request.context, REDTrans);
//                        #endregion

//                        #endregion

//                        //Add follow up
//                        var follow = new FollowUpDTO
//                        {
//                            FollowUpTypeId = (int)EnumFollowupType.Order_Updated,
//                            Title = request.context.Status.FirstOrDefault(s => s.Id == (int)EnumStatus.Delivered).Name,
//                            Comment = request.context.CommonUser.FirstOrDefault(u => u.Id == request.UserID).Name,
//                            CreatedBy = request.UserID,
//                            ShipmentId = ship.ShipmentId,
//                            StatusId = (int)EnumStatus.Delivered
//                        };
//                        FollowUpService.Add(follow, request.context);
//                    }

//                    request.context.SaveChanges();

//                    response.Message = "Shipment successfully delivered";
//                    response.Success = true;
//                    response.StatusCode = HttpStatusCode.OK;
//                }
//                catch (Exception ex)
//                {
//                    response.Message = ex.Message;
//                    response.Success = false;
//                    LogHelper.LogException(ex.Message, ex.StackTrace);
//                }
//                return response;
//            });
//            return response;
//        }

//        public static ShipmentResponse Return(ShipmentRequest request)
//        {
//            var response = new ShipmentResponse();
//            RunBase(request, response, (ShipmentRequest req) =>
//            {
//                try
//                {
//                    var shipments = request.context.Shipment.Where(r => request.ShipDTO.ShipmentIDs.Contains(r.ShipmentId));

//                    foreach (var ship in shipments)
//                    {
//                        if (request.ShipDTO.ShipItemDTOs != null)
//                        {
//                            foreach (var item in request.ShipDTO.ShipItemDTOs)
//                            {
//                                var shipItem = request.context.ShipmentItem.FirstOrDefault(i => i.ShipmentItemId == item.ShipmentItemId);
//                                if (shipItem.StatusId != (int)EnumStatus.Delivered || item.CourierQuantityReturned > 0)
//                                    shipItem.StatusId = (int)EnumStatus.Returned;
//                            }
//                        }
//                        else
//                        {
//                            var shipItems = request.context.ShipmentItem.Where(i => i.ShipmentId == ship.ShipmentId);
//                            foreach (var item in shipItems)
//                                if (item.StatusId != (int)EnumStatus.Delivered || item.CourierQuantityReturned > 0)
//                                    item.StatusId = (int)EnumStatus.Returned;
//                        }

//                        ship.StatusId = (int)EnumStatus.Returned;
//                        ship.LastModifiedAt = DateTime.Now;
//                        ship.LastModifiedBy = request.UserID;

//                        //Add follow up
//                        var follow = new FollowUpDTO
//                        {
//                            FollowUpTypeId = (int)EnumFollowupType.Order_Updated,
//                            Title = request.context.Status.FirstOrDefault(s => s.Id == (int)EnumStatus.Returned).Name,
//                            Comment = request.context.CommonUser.FirstOrDefault(u => u.Id == request.UserID).Name,
//                            CreatedBy = request.UserID,
//                            ShipmentId = ship.ShipmentId,
//                            StatusId = (int)EnumStatus.Returned
//                        };
//                        FollowUpService.Add(follow, request.context);
//                    }

//                    request.context.SaveChanges();

//                    response.Message = "Shipment successfully returned";
//                    response.Success = true;
//                    response.StatusCode = HttpStatusCode.OK;
//                }
//                catch (Exception ex)
//                {
//                    response.Message = ex.Message;
//                    response.Success = false;
//                    LogHelper.LogException(ex.Message, ex.StackTrace);
//                }
//                return response;
//            });
//            return response;
//        }

//        public static ShipmentResponse Refund(ShipmentRequest request)
//        {
//            var response = new ShipmentResponse();
//            RunBase(request, response, (ShipmentRequest req) =>
//            {
//                try
//                {
//                    var shipments = request.context.Shipment.Where(r => request.ShipDTO.ShipmentIDs.Contains(r.ShipmentId));

//                    foreach (var ship in shipments)
//                    {
//                        ship.StatusId = (int)EnumStatus.Refunded;
//                        ship.LastModifiedAt = DateTime.Now;
//                        ship.LastModifiedBy = request.UserID;

//                        //Add follow up
//                        var follow = new FollowUpDTO
//                        {
//                            FollowUpTypeId = (int)EnumFollowupType.Order_Updated,
//                            Title = request.context.Status.FirstOrDefault(s => s.Id == ship.StatusId).Name,
//                            Comment = request.context.CommonUser.FirstOrDefault(u => u.Id == request.UserID).Name,
//                            CreatedBy = request.UserID,
//                            ShipmentId = ship.ShipmentId,
//                            StatusId = ship.StatusId
//                        };
//                        FollowUpService.Add(follow, request.context);

//                        //Add Transactions
//                        var VendorAccountId = request.context.Account.FirstOrDefault(a => a.UserId == ship.VendorId).AccountId;

//                        #region Vendor Transactions
//                        var VendorTrans = new AccountTransactionDTO();
//                        VendorTrans.CreatedBy = request.UserID;
//                        VendorTrans.SenderId = (int)EnumAccountId.Main_Treasury;
//                        VendorTrans.ReceiverId = VendorAccountId;
//                        VendorTrans.VendorId = VendorAccountId;
//                        VendorTrans.ShipmentId = ship.ShipmentId;
//                        VendorTrans.StatusId = (int)EnumStatus.Refunded;

//                        VendorTrans.TypeId = (int)EnumTransactionType.Withdraw;
//                        VendorTrans.RefundCash = ship.RefundCash;
//                        VendorTrans.RefundFees = ship.RefundFees;
//                        VendorTrans.ShippingFees = ship.ShippingFees;
//                        VendorTrans.Total = ship.RefundCash.Value + ship.RefundFees.Value + ship.ShippingFees;
//                        AccountTransactionService.AddTransaction(request.context, VendorTrans);
//                        #endregion

//                        #region RED Transactions
//                        var REDTrans = new AccountTransactionDTO();
//                        REDTrans.CreatedBy = request.UserID;
//                        REDTrans.SenderId = (int)EnumAccountId.Main_Treasury;
//                        REDTrans.ReceiverId = (int)EnumAccountId.RED_Main_Account;
//                        REDTrans.VendorId = VendorAccountId;
//                        REDTrans.ShipmentId = ship.ShipmentId;
//                        REDTrans.StatusId = (int)EnumStatus.Refunded;

//                        REDTrans.TypeId = (int)EnumTransactionType.Deposit;
//                        REDTrans.RefundFees = ship.RefundFees;
//                        REDTrans.ShippingFees = ship.ShippingFees;
//                        REDTrans.Total = ship.RefundFees.Value + ship.ShippingFees;
//                        AccountTransactionService.AddTransaction(request.context, REDTrans);
//                        #endregion

//                        #region Main Treasury Transactions
//                        var TreasuryTrans = new AccountTransactionDTO();
//                        TreasuryTrans.CreatedBy = request.UserID;
//                        TreasuryTrans.SenderId = (int)EnumAccountId.Main_Treasury;
//                        TreasuryTrans.ReceiverId = (int)EnumAccountId.Main_Treasury;
//                        TreasuryTrans.VendorId = VendorAccountId;
//                        TreasuryTrans.ShipmentId = ship.ShipmentId;
//                        TreasuryTrans.StatusId = (int)EnumStatus.Refunded;

//                        TreasuryTrans.TypeId = (int)EnumTransactionType.Withdraw;
//                        TreasuryTrans.RefundCash = ship.RefundCash;
//                        TreasuryTrans.Total = ship.RefundCash.Value;

//                        AccountTransactionService.AddTransaction(request.context, TreasuryTrans);
//                        #endregion
//                    }

//                    request.context.SaveChanges();

//                    response.Message = "Shipment successfully refunded";
//                    response.Success = true;
//                    response.StatusCode = HttpStatusCode.OK;
//                }
//                catch (Exception ex)
//                {
//                    response.Message = ex.Message;
//                    response.Success = false;
//                    LogHelper.LogException(ex.Message, ex.StackTrace);
//                }
//                return response;
//            });
//            return response;
//        }

//        public static ShipmentResponse Postpone(ShipmentRequest request)
//        {
//            var response = new ShipmentResponse();
//            RunBase(request, response, (ShipmentRequest req) =>
//            {
//                try
//                {
//                    var ship = request.context.Shipment.FirstOrDefault(s => s.ShipmentId == request.ShipDTO.ShipmentId);

//                    ship.LastModifiedAt = DateTime.Now;
//                    ship.LastModifiedBy = request.UserID;
//                    ship.StatusId = request.ShipDTO.CustomerFollowUpDTO.NextCallOn.Value.Day == DateTime.Now.Day ?
//                                    (int)EnumStatus.Ready_For_Delivery : (int)EnumStatus.Postponed;
//                    //ship.StatusId = request.ShipDTO.CustomerFollowUpDTO.NextCallOn.Value.Day == DateTime.Now.Day ?
//                    //                (int)EnumStatus.Ready_For_Delivery : (int)EnumStatus.At_Warehouse;

//                    var shipItems = request.context.ShipmentItem.Where(i => i.ShipmentId == ship.ShipmentId);
//                    foreach (var item in shipItems)
//                        item.StatusId = ship.StatusId;

//                    var shipFollow = new ShipmentCustomerFollowUp
//                    {
//                        ShipmentId = ship.ShipmentId,
//                        NextCallOn = request.ShipDTO.CustomerFollowUpDTO.NextCallOn,
//                        NextCallTimeFrom = request.ShipDTO.CustomerFollowUpDTO.NextCallTimeFrom,
//                        NextCallTimeTo = request.ShipDTO.CustomerFollowUpDTO.NextCallTimeTo,
//                        IsConfirmed = request.ShipDTO.CustomerFollowUpDTO.IsConfirmed,
//                        Note = request.ShipDTO.CustomerFollowUpDTO.Note,

//                        CreatedAt = DateTime.Now,
//                        CreatedBy = request.UserID
//                    };
//                    request.context.ShipmentCustomerFollowUp.Add(shipFollow);

//                    request.context.SaveChanges();

//                    //Add follow up
//                    var follow = new FollowUpDTO
//                    {
//                        FollowUpTypeId = (int)EnumFollowupType.Order_Updated,
//                        Title = "Shipment Postponed",
//                        CreatedBy = request.UserID,
//                        ShipmentId = ship.ShipmentId,
//                        StatusId = ship.StatusId
//                    };
//                    FollowUpService.Add(follow, request.context);

//                    request.context.SaveChanges();

//                    response.Message = "Shipment " + ship.RefId + " successfully postponed";
//                    response.Success = true;
//                    response.StatusCode = HttpStatusCode.OK;
//                }
//                catch (Exception ex)
//                {
//                    response.Message = ex.Message;
//                    response.Success = false;
//                    LogHelper.LogException(ex.Message, ex.StackTrace);
//                }
//                return response;
//            });
//            return response;
//        }

//        public static ShipmentResponse Assign(ShipmentRequest request)
//        {
//            var response = new ShipmentResponse();
//            RunBase(request, response, (ShipmentRequest req) =>
//            {
//                try
//                {
//                    var shipments = request.context.Shipment.Where(r => request.ShipDTO.ShipmentIDs.Contains(r.ShipmentId));
//                    foreach (var ship in shipments)
//                    {
//                        ship.DeliveryManId = request.ShipDTO.DeliveryManId;

//                        if (ship.StatusId == (int)EnumStatus.Ready_For_Delivery)
//                            ship.StatusId = (int)EnumStatus.Assigned_For_Delivery;
//                        else if (ship.StatusId == (int)EnumStatus.Ready_For_Return)
//                            ship.StatusId = (int)EnumStatus.Assigned_For_Return;
//                        else if (ship.StatusId == (int)EnumStatus.Ready_For_Refund)
//                            ship.StatusId = (int)EnumStatus.Assigned_For_Refund;

//                        ship.LastModifiedAt = DateTime.Now;
//                        ship.LastModifiedBy = request.UserID;

//                        var shipItems = request.context.ShipmentItem.Where(i => i.ShipmentId == ship.ShipmentId);
//                        foreach (var item in shipItems)
//                        {
//                            if (item.StatusId != (int)EnumStatus.Delivered)
//                            {
//                                if (item.StatusId == (int)EnumStatus.Ready_For_Delivery)
//                                    item.StatusId = (int)EnumStatus.Assigned_For_Delivery;
//                                else if (item.StatusId == (int)EnumStatus.Ready_For_Return)
//                                    item.StatusId = (int)EnumStatus.Assigned_For_Return;
//                                else if (item.StatusId == (int)EnumStatus.Ready_For_Refund)
//                                    item.StatusId = (int)EnumStatus.Assigned_For_Refund;
//                            }
//                        }

//                        //Add follow up
//                        var follow = new FollowUpDTO
//                        {
//                            FollowUpTypeId = (int)EnumFollowupType.Order_Updated,
//                            Title = request.context.Status.FirstOrDefault(s => s.Id == ship.StatusId).Name,
//                            Comment = request.context.CommonUser.FirstOrDefault(u => u.Id == request.ShipDTO.DeliveryManId).Name,
//                            CreatedBy = request.UserID,
//                            ShipmentId = ship.ShipmentId,
//                            StatusId = ship.StatusId
//                        };

//                        FollowUpService.Add(follow, request.context);
//                    }

//                    request.context.SaveChanges();

//                    response.Message = "Shipment(s) successfully assigned";
//                    response.Success = true;
//                    response.StatusCode = HttpStatusCode.OK;
//                }
//                catch (Exception ex)
//                {
//                    response.Message = ex.Message;
//                    response.Success = false;
//                    LogHelper.LogException(ex.Message, ex.StackTrace);
//                }
//                return response;
//            });
//            return response;
//        }

//        public static ShipmentResponse Unassigned(ShipmentRequest request)
//        {
//            var response = new ShipmentResponse();
//            RunBase(request, response, (ShipmentRequest req) =>
//            {
//                try
//                {
//                    var shipment = request.context.Shipment.FirstOrDefault(s => request.ShipDTO.ShipmentId == s.ShipmentId);

//                    shipment.DeliveryManId = null;
//                    shipment.LastModifiedAt = DateTime.Now;
//                    shipment.LastModifiedBy = request.UserID;

//                    if (shipment.StatusId == (int)EnumStatus.Assigned_For_Delivery)
//                        shipment.StatusId = (int)EnumStatus.Ready_For_Delivery;
//                    else if (shipment.StatusId == (int)EnumStatus.Assigned_For_Return)
//                        shipment.StatusId = (int)EnumStatus.Ready_For_Return;
//                    else if (shipment.StatusId == (int)EnumStatus.Assigned_For_Refund)
//                        shipment.StatusId = (int)EnumStatus.Ready_For_Refund;

//                    var shipItems = request.context.ShipmentItem.Where(i => i.ShipmentId == shipment.ShipmentId);
//                    foreach (var item in shipItems)
//                    {
//                        if (item.StatusId != (int)EnumStatus.Delivered)
//                        {
//                            if (item.StatusId == (int)EnumStatus.Assigned_For_Delivery)
//                                item.StatusId = (int)EnumStatus.Ready_For_Delivery;
//                            else if (item.StatusId == (int)EnumStatus.Assigned_For_Return)
//                                item.StatusId = (int)EnumStatus.Ready_For_Return;
//                            else if (item.StatusId == (int)EnumStatus.Assigned_For_Refund)
//                                item.StatusId = (int)EnumStatus.Ready_For_Refund;
//                        }
//                    }

//                    request.context.SaveChanges();

//                    //Add follow up
//                    var follow = new FollowUpDTO
//                    {
//                        FollowUpTypeId = (int)EnumFollowupType.Order_Updated,
//                        Title = request.context.Status.FirstOrDefault(s => s.Id == shipment.StatusId).Name,
//                        //Comment = request.context.CommonUser.FirstOrDefault(u => u.Id == request.ShipDTO.DeliveryManId).Name,
//                        Comment = "Shipment UnAssigned",
//                        CreatedBy = request.UserID,
//                        CreatedAt = DateTime.Now,
//                        ShipmentId = shipment.ShipmentId,
//                        StatusId = shipment.StatusId
//                    };
//                    FollowUpService.Add(follow, request.context);

//                    request.context.SaveChanges();

//                    response.Message = "Shipment(s) successfully Unassigned";
//                    response.Success = true;
//                    response.StatusCode = HttpStatusCode.OK;
//                }
//                catch (Exception ex)
//                {
//                    response.Message = ex.Message;
//                    response.Success = false;
//                    LogHelper.LogException(ex.Message, ex.StackTrace);
//                }
//                return response;
//            });
//            return response;
//        }

//        public static ShipmentResponse ReceiveReturns(ShipmentRequest request)
//        {
//            var response = new ShipmentResponse();
//            RunBase(request, response, (ShipmentRequest req) =>
//            {
//                try
//                {
//                    var ships = request.context.Shipment.Where(r => request.ShipDTO.ShipmentIDs.Contains(r.ShipmentId));
//                    foreach (var ship in ships)
//                    {
//                        // *** Update shipment ***
//                        if (ship.StatusId == (int)EnumStatus.Out_For_Delivery) // *** Out_For_Delivery
//                        {
//                            ship.StatusId = (int)EnumStatus.At_Warehouse;
//                            ship.ReturnCount += 1;
//                            ship.LastModifiedAt = DateTime.Now;
//                            ship.LastModifiedBy = request.UserID;
//                            ship.IsCourierReturned = true;

//                            var shipItems = request.context.ShipmentItem.Where(i => i.ShipmentId == ship.ShipmentId);
//                            foreach (var item in shipItems)
//                            {
//                                item.StatusId = (int)EnumStatus.At_Warehouse;
//                                item.CourierQuantityReturned = item.Quantity - item.CourierQuantityDelivered;

//                                if (item.VendorProductId.HasValue) // Is Stock
//                                {
//                                    var product = request.context.VendorProduct.FirstOrDefault(p => p.VendorProductId == item.VendorProductId);
//                                    product.AvailableStock += item.CourierQuantityReturned;
//                                }
//                            }
//                        }
//                        else if (ship.StatusId == (int)EnumStatus.Delivered) // *** Delivered
//                        {
//                            ship.ReturnCount += 1;
//                            ship.LastModifiedAt = DateTime.Now;
//                            ship.LastModifiedBy = request.UserID;
//                            ship.IsCourierReturned = true;

//                            var shipItems = request.context.ShipmentItem.Where(i => i.ShipmentId == ship.ShipmentId);
//                            foreach (var item in shipItems)
//                            {
//                                item.CourierQuantityReturned = item.Quantity - item.CourierQuantityDelivered;

//                                if (item.VendorProductId.HasValue) // Is Stock
//                                {
//                                    var product = request.context.VendorProduct.FirstOrDefault(p => p.VendorProductId == item.VendorProductId);
//                                    product.AvailableStock += item.CourierQuantityReturned;
//                                }
//                            }
//                        }
//                        else if (ship.StatusId == (int)EnumStatus.Cancelled) // *** Cancelled
//                        {
//                            ship.ReturnCount += 1;
//                            ship.LastModifiedAt = DateTime.Now;
//                            ship.LastModifiedBy = request.UserID;
//                            ship.IsCourierReturned = true;

//                            var shipItems = request.context.ShipmentItem.Where(i => i.ShipmentId == ship.ShipmentId);
//                            foreach (var item in shipItems)
//                            {
//                                item.CourierQuantityReturned = item.Quantity - item.CourierQuantityDelivered;

//                                if (item.VendorProductId.HasValue) // Is Stock
//                                {
//                                    var product = request.context.VendorProduct.FirstOrDefault(p => p.VendorProductId == item.VendorProductId);
//                                    product.AvailableStock += item.CourierQuantityReturned;
//                                }
//                            }
//                        }
//                        else if (ship.StatusId == (int)EnumStatus.Refunded) // *** Refunded
//                        {
//                            var oldShip = request.context.Shipment.FirstOrDefault(f => f.ShipmentId == ship.ShipToRefundId);

//                            oldShip.StatusId = (int)EnumStatus.Ready_For_Return;
//                            oldShip.ReturnCount += 1;
//                            oldShip.LastModifiedAt = DateTime.Now;
//                            oldShip.LastModifiedBy = request.UserID;
//                            oldShip.IsCourierReturned = true;

//                            ship.ReturnCount += 1;
//                            ship.LastModifiedAt = DateTime.Now;
//                            ship.LastModifiedBy = request.UserID;
//                            ship.IsCourierReturned = true;

//                            var oldShipItems = request.context.ShipmentItem.Where(i => i.ShipmentId == oldShip.ShipmentId);
//                            foreach (var item in oldShipItems)
//                            {
//                                item.StatusId = (int)EnumStatus.Ready_For_Return;
//                            }
//                        }

//                        //Add follow up
//                        var follow = new FollowUpDTO
//                        {
//                            FollowUpTypeId = (int)EnumFollowupType.Order_Updated,
//                            Title = "Courier Return",
//                            Comment = request.context.CommonUser.FirstOrDefault(u => u.Id == ship.DeliveryManId).Name,
//                            CreatedBy = request.UserID,
//                            ShipmentId = ship.ShipmentId,
//                            StatusId = ship.StatusId
//                        };
//                        FollowUpService.Add(follow, request.context);
//                    }

//                    request.context.SaveChanges();

//                    response.Message = "Shipment(s) successfully received";
//                    response.Success = true;
//                    response.StatusCode = HttpStatusCode.OK;
//                }
//                catch (Exception ex)
//                {
//                    response.Message = ex.Message;
//                    response.Success = false;
//                    LogHelper.LogException(ex.Message, ex.StackTrace);
//                }
//                return response;
//            });
//            return response;
//        }

//        public static ShipmentResponse ReceiveCash(ShipmentRequest request)
//        {
//            var response = new ShipmentResponse();
//            RunBase(request, response, (ShipmentRequest req) =>
//            {
//                try
//                {
//                    var ships = request.context.Shipment.Where(r => r.DeliveryManId == request.ShipDTO.DeliveryManId && r.IsCashReceived == false
//                                                        && (r.StatusId == (int)EnumStatus.Delivered || r.StatusId == (int)EnumStatus.Cancelled || r.StatusId == (int)EnumStatus.Paid_To_Vendor));
//                    foreach (var ship in ships)
//                    {
//                        ship.IsCashReceived = true;
//                        //ship.DeliveryManId = null;
//                        ship.LastModifiedAt = DateTime.Now;
//                        ship.LastModifiedBy = request.UserID;

//                        // *** Check if Shipment is fully delivered
//                        double CustomerPaidAmount = 0;
//                        bool takeOriginalCOD = true;
//                        if (ship.StatusId == (int)EnumStatus.Delivered || ship.StatusId == (int)EnumStatus.Paid_To_Vendor)
//                        {
//                            var shipItems = request.context.ShipmentItem.Where(i => i.ShipmentId == ship.ShipmentId);
//                            foreach (var item in shipItems)
//                            {
//                                if ((item.StatusId != (int)EnumStatus.Delivered && item.StatusId != (int)EnumStatus.Paid_To_Vendor) || item.CourierQuantityDelivered < item.Quantity)
//                                    takeOriginalCOD = false;

//                                CustomerPaidAmount += (item.Price * item.CourierQuantityDelivered);
//                            }
//                        }


//                        #region Add Cash Transactions / Update Accounts Balances

//                        var CourierAccountId = request.context.Account.FirstOrDefault(a => a.UserId == request.ShipDTO.DeliveryManId).AccountId;
//                        var VendorAccountId = request.context.Account.FirstOrDefault(a => a.UserId == ship.VendorId).AccountId;

//                        #region Main Treasury Transactions
//                        var TreasuryTrans = new AccountTransactionDTO();
//                        TreasuryTrans.CreatedBy = request.UserID;
//                        TreasuryTrans.TypeId = (int)EnumTransactionType.Deposit;
//                        TreasuryTrans.SenderId = CourierAccountId;
//                        TreasuryTrans.ReceiverId = (int)EnumAccountId.Main_Treasury;
//                        TreasuryTrans.VendorId = VendorAccountId;
//                        TreasuryTrans.ShipmentId = ship.ShipmentId;
//                        TreasuryTrans.StatusId = ship.StatusId;

//                        if (ship.StatusId == (int)EnumStatus.Delivered || ship.StatusId == (int)EnumStatus.Paid_To_Vendor)
//                        {
//                            TreasuryTrans.ShippingFees = ship.ShippingFees;
//                            TreasuryTrans.PackingFees = ship.PackingFees;
//                            TreasuryTrans.WeightFees = ship.WeightFees;
//                            TreasuryTrans.SizeFees = ship.SizeFees;
//                            TreasuryTrans.PartialDeliveryFees = ship.PartialDeliveryFees;
//                            TreasuryTrans.VendorCash = takeOriginalCOD ? ship.VendorCash :
//                                                      (CustomerPaidAmount -
//                                                       ship.ShippingFees -
//                                                       ship.PackingFees -
//                                                       ship.WeightFees -
//                                                       ship.SizeFees -
//                                                       ship.PartialDeliveryFees);
//                            TreasuryTrans.Total = TreasuryTrans.VendorCash +
//                                                  ship.ShippingFees +
//                                                  ship.PackingFees +
//                                                  ship.WeightFees +
//                                                  ship.SizeFees +
//                                                  ship.PartialDeliveryFees;

//                            AccountTransactionService.AddTransaction(request.context, TreasuryTrans);
//                        }
//                        else if (ship.StatusId == (int)EnumStatus.Cancelled)
//                        {
//                            if (ship.ShippingFeesPaid > 0)
//                            {
//                                TreasuryTrans.ShippingFeesPaid = ship.ShippingFeesPaid;
//                                TreasuryTrans.Total = ship.ShippingFeesPaid;

//                                AccountTransactionService.AddTransaction(request.context, TreasuryTrans);
//                            }
//                        }
//                        #endregion

//                        #endregion

//                        //Add follow up
//                        var follow = new FollowUpDTO
//                        {
//                            FollowUpTypeId = (int)EnumFollowupType.Order_Updated,
//                            Title = "Cash Received",
//                            Comment = request.context.CommonUser.FirstOrDefault(u => u.Id == request.UserID).Name,
//                            CreatedBy = request.UserID,
//                            ShipmentId = ship.ShipmentId,
//                            StatusId = ship.StatusId
//                        };
//                        FollowUpService.Add(follow, request.context);
//                    }

//                    request.context.SaveChanges();

//                    response.Message = "Cash successfully received";
//                    response.Success = true;
//                    response.StatusCode = HttpStatusCode.OK;
//                }
//                catch (Exception ex)
//                {
//                    response.Message = ex.Message;
//                    response.Success = false;
//                    LogHelper.LogException(ex.Message, ex.StackTrace);
//                }
//                return response;
//            });
//            return response;
//        }

//        #endregion

//        #region Shipment Problem Actions

//        public static ShipmentResponse AddProblem(ShipmentRequest request)
//        {
//            var response = new ShipmentResponse();
//            RunBase(request, response, (ShipmentRequest req) =>
//            {
//                try
//                {
//                    var ship = request.context.Shipment.FirstOrDefault(s => s.ShipmentId == request.ShipDTO.ShipmentId);

//                    ship.LastModifiedAt = DateTime.Now;
//                    ship.LastModifiedBy = request.UserID;
//                    ship.ShippingFeesPaid = request.ShipDTO.FeesDetailsDTO != null ? request.ShipDTO.FeesDetailsDTO.ShippingFeesPaid : 0;

//                    var problemDTO = request.ShipDTO.ProblemDTOs.First();

//                    // Delete last existing problem
//                    var problem = request.context.ShipmentProblem.LastOrDefault(s => s.IsCourierProblem == problemDTO.IsCourierProblem
//                                                    && s.ShipmentId == ship.ShipmentId && !s.IsDeleted);
//                    if (problem != null)
//                        problem.IsDeleted = true;

//                    var shipProblem = new ShipmentProblem
//                    {
//                        ProblemTypeId = problemDTO.ProblemTypeId,
//                        Note = problemDTO.Note,
//                        IsCourierProblem = problemDTO.IsCourierProblem,
//                        ShipmentId = ship.ShipmentId,
//                        CreatedAt = DateTime.Now,
//                        CreatedBy = request.UserID
//                    };
//                    request.context.ShipmentProblem.Add(shipProblem);

//                    //Add follow up
//                    var follow = new FollowUpDTO
//                    {
//                        FollowUpTypeId = (int)EnumFollowupType.Problem_Added,
//                        Title = request.context.ProblemType.FirstOrDefault(s => s.ProblemTypeId == shipProblem.ProblemTypeId).NameEn,
//                        Comment = shipProblem.Note,
//                        CreatedBy = request.UserID,
//                        ShipmentId = ship.ShipmentId,
//                        StatusId = ship.StatusId
//                    };
//                    FollowUpService.Add(follow, request.context);

//                    request.context.SaveChanges();

//                    response.Message = "Shipment " + ship.RefId + " successfully updated";
//                    response.Success = true;
//                    response.StatusCode = HttpStatusCode.OK;
//                }
//                catch (Exception ex)
//                {
//                    response.Message = ex.Message;
//                    response.Success = false;
//                    LogHelper.LogException(ex.Message, ex.StackTrace);
//                }
//                return response;
//            });
//            return response;
//        }

//        public static ShipmentResponse ReportProblemToVendor(ShipmentRequest request)
//        {
//            var response = new ShipmentResponse();
//            RunBase(request, response, (ShipmentRequest req) =>
//            {
//                try
//                {
//                    var ship = request.context.Shipment.FirstOrDefault(s => s.ShipmentId == request.ShipDTO.ShipmentId);

//                    ship.LastModifiedAt = DateTime.Now;
//                    ship.LastModifiedBy = request.UserID;

//                    var problemDTO = request.ShipDTO.ProblemDTOs.First();

//                    var problem = request.context.ShipmentProblem.LastOrDefault(s => s.IsCourierProblem == problemDTO.IsCourierProblem
//                                                    && s.ShipmentId == ship.ShipmentId && !s.IsDeleted);

//                    problem.ProblemTypeId = problemDTO.ProblemTypeId;
//                    problem.Note = problemDTO.Note;
//                    problem.IsReportToVendor = true;

//                    //Add follow up
//                    var follow = new FollowUpDTO
//                    {
//                        FollowUpTypeId = (int)EnumFollowupType.Problem_Added,
//                        Title = request.context.ProblemType.FirstOrDefault(s => s.ProblemTypeId == problemDTO.ProblemTypeId).NameEn,
//                        Comment = problem.Note,
//                        CreatedBy = request.UserID,
//                        ShipmentId = ship.ShipmentId,
//                        StatusId = ship.StatusId
//                    };
//                    FollowUpService.Add(follow, request.context);

//                    request.context.SaveChanges();

//                    response.Message = "Shipment " + ship.RefId + " successfully updated";
//                    response.Success = true;
//                    response.StatusCode = HttpStatusCode.OK;
//                }
//                catch (Exception ex)
//                {
//                    response.Message = ex.Message;
//                    response.Success = false;
//                    LogHelper.LogException(ex.Message, ex.StackTrace);
//                }
//                return response;
//            });
//            return response;
//        }

//        public static ShipmentResponse VendorReply(ShipmentRequest request)
//        {
//            var response = new ShipmentResponse();
//            RunBase(request, response, (ShipmentRequest req) =>
//            {
//                try
//                {
//                    var ship = request.context.Shipment.FirstOrDefault(s => s.ShipmentId == request.ShipDTO.ShipmentId);

//                    ship.LastModifiedAt = DateTime.Now;
//                    ship.LastModifiedBy = request.UserID;

//                    var problemDTO = request.ShipDTO.ProblemDTOs.Last();

//                    var problem = request.context.ShipmentProblem.LastOrDefault(s => !s.IsCourierProblem && s.ShipmentId == ship.ShipmentId && !s.IsDeleted && !s.IsReportToVendor);

//                    if (problem != null)
//                    {
//                        problem.Solution = problemDTO.Solution;

//                        //Add follow up
//                        var follow = new FollowUpDTO
//                        {
//                            FollowUpTypeId = (int)EnumFollowupType.Vendor_Replied,
//                            Title = "Vendor Reply",
//                            Comment = problemDTO.Solution,
//                            CreatedBy = request.UserID,
//                            ShipmentId = ship.ShipmentId,
//                            StatusId = ship.StatusId
//                        };
//                        FollowUpService.Add(follow, request.context);

//                        request.context.SaveChanges();

//                        response.Message = "Shipment " + ship.RefId + " successfully updated";
//                        response.Success = true;
//                        response.StatusCode = HttpStatusCode.OK;
//                    }
//                    else
//                    {
//                        response.Message = "Error while sending message";
//                        response.Success = false;
//                    }
//                }
//                catch (Exception ex)
//                {
//                    response.Message = ex.Message;
//                    response.Success = false;
//                    LogHelper.LogException(ex.Message, ex.StackTrace);
//                }
//                return response;
//            });
//            return response;
//        }

//        public static ShipmentResponse SolveProblem(ShipmentRequest request)
//        {
//            var response = new ShipmentResponse();
//            RunBase(request, response, (ShipmentRequest req) =>
//            {
//                try
//                {
//                    var ship = request.context.Shipment.FirstOrDefault(s => s.ShipmentId == request.ShipDTO.ShipmentId);

//                    ship.LastModifiedAt = DateTime.Now;
//                    ship.LastModifiedBy = request.UserID;

//                    var problemDTO = request.ShipDTO.ProblemDTOs.First();

//                    var shipProblem = request.context.ShipmentProblem
//                                             .LastOrDefault(s => s.IsCourierProblem == problemDTO.IsCourierProblem
//                                             && s.ShipmentId == ship.ShipmentId && !s.IsSolved && !s.IsDeleted);

//                    shipProblem.IsSolved = problemDTO.IsSolved;
//                    shipProblem.Solution = problemDTO.Solution;

//                    //Add follow up
//                    var follow = new FollowUpDTO
//                    {
//                        FollowUpTypeId = problemDTO.IsCourierProblem && !problemDTO.IsSolved ? (int)EnumFollowupType.Replied_To_Courier : (int)EnumFollowupType.Problem_Solved,
//                        Title = problemDTO.IsCourierProblem && !problemDTO.IsSolved ? "Replied To Courier" : "Problem Solved",
//                        Comment = problemDTO.Solution,
//                        CreatedBy = request.UserID,
//                        ShipmentId = ship.ShipmentId,
//                        StatusId = ship.StatusId
//                    };
//                    FollowUpService.Add(follow, request.context);

//                    request.context.SaveChanges();

//                    response.Message = "Shipment " + ship.RefId + " successfully updated";
//                    response.Success = true;
//                    response.StatusCode = HttpStatusCode.OK;
//                }
//                catch (Exception ex)
//                {
//                    response.Message = ex.Message;
//                    response.Success = false;
//                    LogHelper.LogException(ex.Message, ex.StackTrace);
//                }
//                return response;
//            });
//            return response;
//        }

//        public static ShipmentResponse DeleteProblem(ShipmentRequest request)
//        {
//            var response = new ShipmentResponse();
//            RunBase(request, response, (ShipmentRequest req) =>
//            {
//                try
//                {
//                    var ship = request.context.Shipment.FirstOrDefault(s => s.ShipmentId == request.ShipDTO.ShipmentId);

//                    ship.LastModifiedAt = DateTime.Now;
//                    ship.LastModifiedBy = request.UserID;

//                    var problemDTO = request.ShipDTO.ProblemDTOs.First();

//                    var shipProblem = request.context.ShipmentProblem.LastOrDefault(s => s.IsCourierProblem == problemDTO.IsCourierProblem
//                                                                       && s.ShipmentId == ship.ShipmentId && !s.IsDeleted);
//                    shipProblem.IsDeleted = true;

//                    request.context.SaveChanges();

//                    //Add follow up
//                    var follow = new FollowUpDTO
//                    {
//                        FollowUpTypeId = (int)EnumFollowupType.Problem_Deleted,
//                        Title = "Problem Deleted",
//                        CreatedBy = request.UserID,
//                        ShipmentId = ship.ShipmentId,
//                        StatusId = ship.StatusId
//                    };
//                    FollowUpService.Add(follow, request.context);

//                    request.context.SaveChanges();

//                    response.Message = "Shipment " + ship.RefId + " successfully updated";
//                    response.Success = true;
//                    response.StatusCode = HttpStatusCode.OK;
//                }
//                catch (Exception ex)
//                {
//                    response.Message = ex.Message;
//                    response.Success = false;
//                    LogHelper.LogException(ex.Message, ex.StackTrace);
//                }
//                return response;
//            });
//            return response;
//        }

//        #endregion

//        #region Shipment Select Queries

//        public static ShipmentResponse GetShipment(ShipmentRequest request)
//        {
//            var response = new ShipmentResponse();
//            RunBase(request, response, (ShipmentRequest req) =>
//            {
//                try
//                {
//                    IQueryable<Shipment> ship;
//                    if (string.IsNullOrEmpty(request.ShipDTO.RefId))
//                        ship = request.context.Shipment.Where(s => s.ShipmentId == request.ShipDTO.ShipmentId && !s.IsDeleted);
//                    else
//                        ship = request.context.Shipment.Where(s => s.RefId.Contains(request.ShipDTO.RefId));

//                    var query = ship
//                    .Select(s => new ShipDTO
//                    {
//                        ShipmentId = s.ShipmentId,
//                        ShipmentTypeId = s.ShipmentTypeId,
//                        ShipmentServiceId = s.ShipmentServiceId,
//                        RefId = s.RefId,
//                        BranchId = s.BranchId,
//                        BranchName = s.Branch.BranchName,
//                        AreaId = s.AreaId,
//                        AreaName = s.Area.CityNameAr,
//                        ZoneId = s.ZoneId,
//                        ZoneName = s.Zone.NameEn,
//                        StatusId = s.StatusId,
//                        DeliveryManId = s.DeliveryManId,
//                        DeliveryManName = s.DeliveryManId.HasValue ? s.DeliveryMan.Name : null,
//                        ShipmentServiceName = s.ShipmentService.ServiceName,
//                        ReturnCount = s.ReturnCount,
//                        CancelComment = s.CancelComment,
//                        CreatedAt = s.CreatedAt,
//                        PickupRequestId = s.PickupRequestId,
//                        NoOfItems = s.NoOfItems,
//                        IsCashReceived = s.IsCashReceived,
//                        CashTransferId = s.CashTransferId,
//                        CashTransferRefId = s.CashTransferId.HasValue ? s.CashTransfer.RefId : null,
//                        IsCourierReturned = s.IsCourierReturned,
//                        Description = s.Description,
//                        ShipToRefundId = s.ShipToRefundId,
//                        RefundCash = s.RefundCash,
//                        RefundFees = s.RefundFees,
//                        ShipToRefundRefId = s.ShipToRefundId.HasValue ? request.context.Shipment.FirstOrDefault(rf => rf.ShipmentId == s.ShipToRefundId.Value).RefId : null,
//                        HasRefunded = request.context.Shipment.Any(rr => rr.ShipToRefundId == s.ShipmentId),
//                        CanReturnToVendor = CanReturnToVendor(s, s.FollowUp.ToList()),

//                        StatusDTO = new StatusDTO
//                        {
//                            Id = s.StatusId,
//                            Name = s.Status.Name,
//                            DeliveryManName = s.DeliveryManId.HasValue ? s.DeliveryMan.Name : null,
//                        },

//                        CustomerDetailsDTO = request.HasCustomerDetailsDTO ? new CustomerDetailsDTO
//                        {
//                            CustomerId = s.CustomerId,
//                            CustomerName = s.CustomerName,
//                            CustomerPhone = s.CustomerPhone,
//                            CustomerPhone2 = s.CustomerPhone2,
//                            CustomerAddress = s.CustomerAddress,
//                            Landmark = s.Landmark,
//                            Location = s.Location,
//                            Floor = s.Floor,
//                            Apartment = s.Apartment
//                        } : null,

//                        VendorDetailsDTO = request.HasVendorDetailsDTO ? new VendorDetailsDTO
//                        {
//                            VendorId = s.VendorId,
//                            VendorName = s.VendorName,
//                            VendorPhone = s.VendorPhone,
//                            VendorAddress = s.Vendor.Address,
//                            PaidToVendorAt = s.PaidToVendorAt,
//                            IsPaidToVendor = s.IsPaidToVendor
//                        } : null,

//                        SettingDTO = request.HasSettingDTO ? new ShipSettingDTO
//                        {
//                            IsFragilePackage = s.IsFragilePackage,
//                            IsOpenPackage = s.IsOpenPackage,
//                            IsPartialDelivery = s.IsPartialDelivery,
//                            IsStock = s.IsStock,
//                            Size = s.Size,
//                            WarehouseSize = s.WarehouseSize,
//                            Weight = s.Weight,
//                            WarehouseWeight = s.WarehouseWeight,
//                            PackingId = s.PackingId,
//                            PackingTypeId = s.PackingId.HasValue ? s.Packing.PackingTypeId : 0,
//                            WarehousePackingId = s.WarehousePackingId,
//                            WarehousePackingTypeId = s.WarehousePackingId.HasValue ? s.WarehousePacking.PackingTypeId : 0,
//                            PackingName =
//                            s.WarehousePackingId.HasValue ? s.WarehousePacking.NameEn : s.PackingId.HasValue ? s.Packing.NameEn : null,
//                            PackingTypeName =
//                            s.WarehousePackingId.HasValue ? s.WarehousePacking.PackingType.NameEn : s.PackingId.HasValue ? s.Packing.PackingType.NameEn : null,
//                        } : null,

//                        FeesDetailsDTO = request.HasFeesDetailsDTO ? new FeesDetailsDTO
//                        {
//                            IsAfford = s.IsAfford,
//                            ShippingFees = s.ShippingFees,
//                            CancelFees = s.CancelFees,
//                            PackingFees = s.PackingFees,
//                            PartialDeliveryFees = s.PartialDeliveryFees,
//                            WeightFees = s.WeightFees,
//                            SizeFees = s.SizeFees,
//                            Total = s.Total,
//                            VendorCash = s.VendorCash,
//                            ShippingFeesPaid = s.ShippingFeesPaid,
//                        } : null,

//                        CustomerFollowUpDTO = request.HasCustomerFollowUpDTO ? new CustomerFollowUpDTO
//                        {
//                            CallAnswerCount = s.CallAnswerCount,
//                            CallNotAnswerCount = s.CallNotAnswerCount
//                        } : null,

//                        ProblemDTOs = request.HasProblemDTOs ? s.ShipmentProblem
//                                                 .Where(sh => !sh.IsDeleted)
//                                                 .Select(csp => new ShipProblemDTO
//                                                 {
//                                                     IsCourierProblem = csp.IsCourierProblem,
//                                                     Note = csp.Note,
//                                                     ProblemTypeName = csp.ProblemType.NameEn,
//                                                     Solution = csp.Solution,
//                                                     IsReportToVendor = csp.IsReportToVendor,
//                                                     IsSolved = csp.IsSolved,
//                                                     IsDeleted = csp.IsDeleted,
//                                                 }) : null,

//                        FollowUpDTOs = request.HasFollowUpDTOs ? s.FollowUp
//                                              .Select(f => new FollowUpDTO
//                                              {
//                                                  CreatedByName = f.CreatedByNavigation.Name,
//                                                  Title = f.Title,
//                                                  Comment = f.Comment,
//                                                  CreatedAt = f.CreatedAt,
//                                                  FollowUpTypeId = f.FollowUpTypeId,
//                                                  StatusId = f.StatusId,
//                                                  ShipmentId = f.ShipmentId,
//                                              }) : null,

//                        ShipItemDTOs = request.HasShipItemDTOs ? s.ShipmentItem
//                                              .Select(i => new ShipItemDTO
//                                              {
//                                                  ShipmentId = i.ShipmentId,
//                                                  ShipmentItemId = i.ShipmentItemId,
//                                                  VendorProductId = i.VendorProductId,
//                                                  Quantity = i.Quantity,
//                                                  Price = i.Price,
//                                                  Name = i.VendorProductId.HasValue ? i.VendorProduct.Name : i.Name,
//                                                  Size = i.VendorProductId.HasValue ? i.VendorProduct.Size : i.Size,
//                                                  ImageUrl = i.ImageUrl,
//                                                  StatusId = i.StatusId,
//                                                  CourierQuantityDelivered = i.CourierQuantityDelivered,
//                                                  CourierQuantityReturned = i.CourierQuantityReturned,
//                                                  StatusDTO = new StatusDTO
//                                                  {
//                                                      Id = i.StatusId,
//                                                      Name = i.Status.Name,
//                                                      DeliveryManName = s.DeliveryManId.HasValue ? s.DeliveryMan.Name : null,
//                                                  },
//                                              }) : null,
//                    });

//                    response.ShipDTO = query.FirstOrDefault();

//                    response.Message = HttpStatusCode.OK.ToString();
//                    response.Success = true;
//                    response.StatusCode = HttpStatusCode.OK;
//                }
//                catch (Exception ex)
//                {
//                    response.Message = ex.Message;
//                    response.Success = false;
//                    LogHelper.LogException(ex.Message, ex.StackTrace);
//                }
//                return response;
//            });
//            return response;
//        }

//        public static ShipmentResponse GetAllShipments(ShipmentRequest request)
//        {
//            var response = new ShipmentResponse();
//            RunBase(request, response, (ShipmentRequest req) =>
//            {
//                try
//                {
//                    var query = request.context.Shipment.Where(s => !s.IsDeleted)
//                    .Select(s => new ShipDTO
//                    {
//                        ShipmentId = s.ShipmentId,
//                        ShipmentTypeId = s.ShipmentTypeId,
//                        ShipmentServiceId = s.ShipmentServiceId,
//                        RefId = s.RefId,
//                        BranchId = s.BranchId,
//                        BranchName = s.Branch.BranchName,
//                        AreaId = s.AreaId,
//                        AreaName = s.Area.CityNameAr,
//                        ZoneId = s.ZoneId,
//                        ZoneName = s.Zone.NameEn,
//                        StatusId = s.StatusId,
//                        DeliveryManId = s.DeliveryManId,
//                        DeliveryManName = s.DeliveryManId.HasValue ? s.DeliveryMan.Name : null,
//                        ShipmentServiceName = s.ShipmentService.ServiceName,
//                        ReturnCount = s.ReturnCount,
//                        CancelComment = s.CancelComment,
//                        CreatedAt = s.CreatedAt,
//                        PickupRequestId = s.PickupRequestId,
//                        NoOfItems = s.NoOfItems,
//                        IsCashReceived = s.IsCashReceived,
//                        CashTransferId = s.CashTransferId,
//                        CashTransferRefId = s.CashTransferId.HasValue ? s.CashTransfer.RefId : null,
//                        IsCourierReturned = s.IsCourierReturned,
//                        Description = s.Description,
//                        ShipToRefundId = s.ShipToRefundId,
//                        RefundCash = s.RefundCash,
//                        RefundFees = s.RefundFees,
//                        ShipToRefundRefId = s.ShipToRefundId.HasValue ? request.context.Shipment.FirstOrDefault(rf => rf.ShipmentId == s.ShipToRefundId.Value).RefId : null,
//                        HasRefunded = request.context.Shipment.Any(rr => rr.ShipToRefundId == s.ShipmentId),
//                        CanReturnToVendor = CanReturnToVendor(s, s.FollowUp.ToList()),

//                        StatusDTO = new StatusDTO
//                        {
//                            Id = s.StatusId,
//                            Name = s.Status.Name,
//                            DeliveryManName = s.DeliveryManId.HasValue ? s.DeliveryMan.Name : null,
//                        },

//                        CustomerDetailsDTO = request.HasCustomerDetailsDTO ? new CustomerDetailsDTO
//                        {
//                            CustomerId = s.CustomerId,
//                            CustomerName = s.CustomerName,
//                            CustomerPhone = s.CustomerPhone,
//                            CustomerPhone2 = s.CustomerPhone2,
//                            CustomerAddress = s.CustomerAddress,
//                            Landmark = s.Landmark,
//                            Location = s.Location,
//                            Floor = s.Floor,
//                            Apartment = s.Apartment
//                        } : null,

//                        VendorDetailsDTO = request.HasVendorDetailsDTO ? new VendorDetailsDTO
//                        {
//                            VendorId = s.VendorId,
//                            VendorName = s.VendorName,
//                            VendorPhone = s.VendorPhone,
//                            VendorAddress = s.Vendor.Address,
//                            PaidToVendorAt = s.PaidToVendorAt,
//                            IsPaidToVendor = s.IsPaidToVendor
//                        } : null,

//                        SettingDTO = request.HasSettingDTO ? new ShipSettingDTO
//                        {
//                            IsFragilePackage = s.IsFragilePackage,
//                            IsOpenPackage = s.IsOpenPackage,
//                            IsPartialDelivery = s.IsPartialDelivery,
//                            IsStock = s.IsStock,
//                            Size = s.Size,
//                            WarehouseSize = s.WarehouseSize,
//                            Weight = s.Weight,
//                            WarehouseWeight = s.WarehouseWeight,
//                            PackingId = s.PackingId,
//                            PackingTypeId = s.PackingId.HasValue ? s.Packing.PackingTypeId : 0,
//                            WarehousePackingId = s.WarehousePackingId,
//                            WarehousePackingTypeId = s.WarehousePackingId.HasValue ? s.WarehousePacking.PackingTypeId : 0,
//                            PackingName =
//                            s.WarehousePackingId.HasValue ? s.WarehousePacking.NameEn : s.PackingId.HasValue ? s.Packing.NameEn : null,
//                            PackingTypeName =
//                            s.WarehousePackingId.HasValue ? s.WarehousePacking.PackingType.NameEn : s.PackingId.HasValue ? s.Packing.PackingType.NameEn : null,
//                        } : null,

//                        FeesDetailsDTO = request.HasFeesDetailsDTO ? new FeesDetailsDTO
//                        {
//                            IsAfford = s.IsAfford,
//                            ShippingFees = s.ShippingFees,
//                            CancelFees = s.CancelFees,
//                            PackingFees = s.PackingFees,
//                            PartialDeliveryFees = s.PartialDeliveryFees,
//                            WeightFees = s.WeightFees,
//                            SizeFees = s.SizeFees,
//                            Total = s.Total,
//                            VendorCash = s.VendorCash,
//                            ShippingFeesPaid = s.ShippingFeesPaid,
//                        } : null,

//                        CustomerFollowUpDTO = request.HasCustomerFollowUpDTO ? new CustomerFollowUpDTO
//                        {
//                            CallAnswerCount = s.CallAnswerCount,
//                            CallNotAnswerCount = s.CallNotAnswerCount
//                        } : null,

//                        ProblemDTOs = request.HasProblemDTOs ? s.ShipmentProblem
//                                                 .Where(sh => !sh.IsDeleted)
//                                                 .Select(csp => new ShipProblemDTO
//                                                 {
//                                                     IsCourierProblem = csp.IsCourierProblem,
//                                                     Note = csp.Note,
//                                                     ProblemTypeName = csp.ProblemType.NameEn,
//                                                     Solution = csp.Solution,
//                                                     IsReportToVendor = csp.IsReportToVendor,
//                                                     IsSolved = csp.IsSolved,
//                                                     IsDeleted = csp.IsDeleted,
//                                                 }) : null,

//                        FollowUpDTOs = request.HasFollowUpDTOs ? s.FollowUp
//                                              .Select(f => new FollowUpDTO
//                                              {
//                                                  CreatedByName = f.CreatedByNavigation.Name,
//                                                  Title = f.Title,
//                                                  Comment = f.Comment,
//                                                  CreatedAt = f.CreatedAt,
//                                                  StatusId = f.StatusId,
//                                                  FollowUpTypeId = f.FollowUpTypeId,
//                                                  ShipmentId = f.ShipmentId,
//                                              }) : null,

//                        ShipItemDTOs = request.HasShipItemDTOs ? s.ShipmentItem
//                                              .Select(i => new ShipItemDTO
//                                              {
//                                                  ShipmentId = i.ShipmentId,
//                                                  ShipmentItemId = i.ShipmentItemId,
//                                                  VendorProductId = i.VendorProductId,
//                                                  Quantity = i.Quantity,
//                                                  Price = i.Price,
//                                                  Name = i.VendorProductId.HasValue ? i.VendorProduct.Name : i.Name,
//                                                  Size = i.VendorProductId.HasValue ? i.VendorProduct.Size : i.Size,
//                                                  ImageUrl = i.ImageUrl,
//                                                  StatusId = i.StatusId,
//                                                  CourierQuantityDelivered = i.CourierQuantityDelivered,
//                                                  CourierQuantityReturned = i.CourierQuantityReturned,
//                                                  StatusDTO = new StatusDTO
//                                                  {
//                                                      Id = i.StatusId,
//                                                      Name = i.Status.Name,
//                                                      DeliveryManName = s.DeliveryManId.HasValue ? s.DeliveryMan.Name : null,
//                                                  },
//                                              }) : null,
//                    });

//                    query = query.OrderByDescending(s => s.ShipmentId);

//                    if (request.applyFilter)
//                        query = ApplyFilter(query, request.ShipDTO, request.RoleID, request.UserID);

//                    if (request.PageSize > 0)
//                        query = ApplyPaging(query, request.PageSize, request.PageIndex);

//                    //if (string.IsNullOrEmpty(request.OrderByColumn))
//                    //{
//                    //    request.OrderByColumn = "ShipmentId";
//                    //}
//                    //query = OrderByDynamic(query, request.OrderByColumn, request.IsDesc);

//                    response.ShipDTOs = query.ToList();
//                    //response.ShipDTOs = query.OrderByDescending(s => s.ShipmentId).ToList();

//                    response.Message = HttpStatusCode.OK.ToString();
//                    response.Success = true;
//                    response.StatusCode = HttpStatusCode.OK;
//                }
//                catch (Exception ex)
//                {
//                    response.Message = ex.Message;
//                    response.Success = false;
//                    LogHelper.LogException(ex.Message, ex.StackTrace);
//                }
//                return response;
//            });
//            return response;
//        }

//        public static ShipmentResponse GetVendorShipments(ShipmentRequest request)
//        {
//            var response = new ShipmentResponse();
//            RunBase(request, response, (ShipmentRequest req) =>
//            {
//                try
//                {
//                    var query = request.context.Shipment.Where(s => !s.IsDeleted
//                     && (request.ShipDTO.VendorDetailsDTO.VendorId > 0 ?
//                    (s.VendorId == request.ShipDTO.VendorDetailsDTO.VendorId && GetStatusGroup(request.ShipDTO.StatusId).Contains(s.StatusId)) : true))
//                    .GroupBy(t => t.VendorId)
//                    .Select(s => new VendorReportDTO
//                    {
//                        VendorId = s.Key,
//                        VendorName = s.FirstOrDefault().VendorName,

//                        TotalShipsCount = s.Count(),
//                        CurrentShipsCount = s.Where(g => GetStatusGroup((int)EnumStatus.Current).Contains(g.StatusId)).Count(),
//                        DoneShipsCount = s.Where(g => GetStatusGroup((int)EnumStatus.Done).Contains(g.StatusId)).Count(),
//                        CancelledShipsCount = s.Where(g => GetStatusGroup((int)EnumStatus.Cancelled).Contains(g.StatusId)).Count(),

//                        // Success %  = Done / Total * 100
//                        SuccessPerc = (int)(((double)s.Where(g => GetStatusGroup((int)EnumStatus.Done).Contains(g.StatusId)).Count() / (double)s.Count()) * 100),

//                        TotalDelivered = request.context.AccountTransaction
//                        .Where(t => t.ReceiverId != (int)EnumAccountId.Main_Treasury
//                                 && t.VendorId == request.context.Account.FirstOrDefault(f => f.UserId == s.Key).AccountId
//                                  && t.StatusId == (int)EnumStatus.Delivered && t.TypeId == (int)EnumTransactionType.Deposit)
//                        .Sum(t => t.Total),

//                        TotalRefunded = request.context.AccountTransaction
//                        .Where(t => t.ReceiverId == t.VendorId
//                                  && t.VendorId == request.context.Account.FirstOrDefault(f => f.UserId == s.Key).AccountId
//                                  && t.StatusId == (int)EnumStatus.Refunded && t.TypeId == (int)EnumTransactionType.Withdraw)
//                        .Sum(t => t.Total),

//                        TotalOthers = request.context.AccountTransaction
//                        .Where(t => t.ReceiverId == (int)EnumAccountId.RED_Main_Account
//                                 && t.VendorId == request.context.Account.FirstOrDefault(f => f.UserId == s.Key).AccountId
//                                 && (t.StatusId != (int)EnumStatus.Delivered && t.StatusId != (int)EnumStatus.Refunded)
//                                 && t.TypeId == (int)EnumTransactionType.Deposit)
//                        .Sum(t => t.Total),


//                        ShippingFees = request.context.AccountTransaction
//                        .Where(t => t.ReceiverId == (int)EnumAccountId.RED_Main_Account && t.TypeId == (int)EnumTransactionType.Deposit
//                                    && t.VendorId == request.context.Account.FirstOrDefault(f => f.UserId == s.Key).AccountId)
//                        .Sum(t => t.ShippingFees),

//                        PackingFees = request.context.AccountTransaction
//                        .Where(t => t.ReceiverId == (int)EnumAccountId.RED_Main_Account && t.TypeId == (int)EnumTransactionType.Deposit
//                                    && t.VendorId == request.context.Account.FirstOrDefault(f => f.UserId == s.Key).AccountId)
//                        .Sum(t => t.PackingFees),

//                        WeightFees = request.context.AccountTransaction
//                        .Where(t => t.ReceiverId == (int)EnumAccountId.RED_Main_Account && t.TypeId == (int)EnumTransactionType.Deposit
//                        && t.VendorId == request.context.Account.FirstOrDefault(f => f.UserId == s.Key).AccountId)
//                        .Sum(t => t.WeightFees),

//                        SizeFees = request.context.AccountTransaction
//                        .Where(t => t.ReceiverId == (int)EnumAccountId.RED_Main_Account && t.TypeId == (int)EnumTransactionType.Deposit
//                        && t.VendorId == request.context.Account.FirstOrDefault(f => f.UserId == s.Key).AccountId)
//                        .Sum(t => t.SizeFees),

//                        PartialDeliveryFees = request.context.AccountTransaction
//                        .Where(t => t.ReceiverId == (int)EnumAccountId.RED_Main_Account && t.TypeId == (int)EnumTransactionType.Deposit
//                        && t.VendorId == request.context.Account.FirstOrDefault(f => f.UserId == s.Key).AccountId)
//                        .Sum(t => t.PartialDeliveryFees),

//                        PickupFees = request.context.AccountTransaction
//                        .Where(t => t.ReceiverId == (int)EnumAccountId.RED_Main_Account && t.TypeId == (int)EnumTransactionType.Deposit
//                        && t.VendorId == request.context.Account.FirstOrDefault(f => f.UserId == s.Key).AccountId)
//                        .Sum(t => t.PickupFees),

//                        CancelFees = request.context.AccountTransaction
//                        .Where(t => t.ReceiverId == t.VendorId && t.TypeId == (int)EnumTransactionType.Withdraw
//                        && t.ShippingFees == 0
//                        && t.VendorId == request.context.Account.FirstOrDefault(f => f.UserId == s.Key).AccountId)
//                        .Sum(t => t.CancelFees),

//                        Balance = request.context.Account.FirstOrDefault(t => t.UserId == s.Key).Balance,

//                        lstShips = s.OrderByDescending(t => t.ShipmentId).Select(t => new ShipDTO
//                        {
//                            ShipmentId = t.ShipmentId,
//                            ShipmentTypeId = t.ShipmentTypeId,
//                            ShipmentServiceId = t.ShipmentServiceId,
//                            ShipmentServiceName = t.ShipmentService.ServiceName,
//                            RefId = t.RefId,
//                            AreaId = t.AreaId,
//                            AreaName = t.Area.CityNameAr,
//                            ZoneId = t.ZoneId,
//                            ZoneName = t.Zone.NameEn,
//                            StatusId = t.StatusId,
//                            CreatedAt = t.CreatedAt,
//                            PickupRequestId = t.PickupRequestId,
//                            CashTransferId = t.CashTransferId,
//                            CashTransferRefId = t.CashTransferId.HasValue ? t.CashTransfer.RefId : null,
//                            RefundCash = t.RefundCash,
//                            RefundFees = t.RefundFees,

//                            StatusDTO = new StatusDTO
//                            {
//                                Id = t.StatusId,
//                                Name = t.Status.Name,
//                                DeliveryManName = t.DeliveryManId.HasValue ? t.DeliveryMan.Name : null,
//                            },

//                            CustomerDetailsDTO = new CustomerDetailsDTO
//                            {
//                                CustomerId = t.CustomerId,
//                                CustomerName = t.CustomerName,
//                                CustomerPhone = t.CustomerPhone,
//                                CustomerPhone2 = t.CustomerPhone2,
//                                CustomerAddress = t.CustomerAddress,
//                                Landmark = t.Landmark,
//                                Location = t.Location,
//                                Floor = t.Floor,
//                                Apartment = t.Apartment
//                            },

//                            VendorDetailsDTO = new VendorDetailsDTO
//                            {
//                                VendorId = t.VendorId,
//                                VendorName = t.VendorName,
//                                VendorPhone = t.VendorPhone,
//                                VendorAddress = t.Vendor.Address,
//                                PaidToVendorAt = t.PaidToVendorAt,
//                                IsPaidToVendor = t.IsPaidToVendor
//                            },

//                            FeesDetailsDTO = new FeesDetailsDTO
//                            {
//                                ShippingFees = t.ShippingFees,
//                                ShippingFeesPaid = t.ShippingFeesPaid,
//                                PackingFees = t.PackingFees,
//                                WeightFees = t.WeightFees,
//                                SizeFees = t.SizeFees,
//                                PartialDeliveryFees = t.PartialDeliveryFees,
//                                Total = t.Total,
//                                VendorCash = t.VendorCash,
//                                CancelFees = t.CancelFees
//                            },

//                            TransactionDTOs = t.AccountTransaction.Select(tr =>
//                            new AccountTransactionDTO
//                            {
//                                AccountTransactionId = tr.AccountTransactionId,
//                                ShipRefId = tr.Shipment.RefId,
//                                TypeId = tr.TypeId,
//                                SenderId = tr.SenderId,
//                                ReceiverId = tr.ReceiverId,
//                                VendorId = tr.VendorId,
//                                ShipmentId = tr.ShipmentId,
//                                PackingFees = tr.PackingFees,
//                                WeightFees = tr.WeightFees,
//                                SizeFees = tr.SizeFees,
//                                PartialDeliveryFees = tr.PartialDeliveryFees,
//                                CancelFees = tr.CancelFees,
//                                PickupFees = tr.PickupFees,
//                                ShippingFees = tr.ShippingFees,
//                                ShippingFeesPaid = tr.ShippingFeesPaid,
//                                VendorCash = tr.VendorCash,
//                                Total = tr.Total,
//                                RefundCash = tr.RefundCash,
//                                RefundFees = tr.RefundFees,

//                                StatusId = tr.StatusId,
//                                StatusName = tr.Status.Name,
//                                StatusDTO = new StatusDTO
//                                {
//                                    Id = tr.StatusId.Value,
//                                    Name = tr.Status.Name,
//                                },
//                                CreatedAt = tr.CreatedAt,
//                            })
//                        }),

//                    });

//                    response.VendorReportDTOs = query.ToList();

//                    response.Message = HttpStatusCode.OK.ToString();
//                    response.Success = true;
//                    response.StatusCode = HttpStatusCode.OK;
//                }
//                catch (Exception ex)
//                {
//                    response.Message = ex.Message;
//                    response.Success = false;
//                    LogHelper.LogException(ex.Message, ex.StackTrace);
//                }
//                return response;
//            });
//            return response;
//        }

//        public static ShipmentResponse GetShipmentItems(ShipmentRequest request)
//        {
//            var response = new ShipmentResponse();
//            RunBase(request, response, (ShipmentRequest req) =>
//            {
//                try
//                {
//                    var query = request.context.ShipmentItem.Where(s => s.ShipmentId == request.ShipItemDTO.ShipmentId && !s.IsDeleted)
//                    .Select(p => new ShipItemDTO
//                    {
//                        ShipmentId = p.ShipmentId,
//                        ShipmentItemId = p.ShipmentItemId,
//                        Name = p.VendorProductId.HasValue ? p.VendorProduct.Name : p.Name,
//                        Size = p.VendorProductId.HasValue ? p.VendorProduct.Size : p.Size,
//                        Quantity = p.Quantity,
//                        Price = p.Price,
//                        ImageUrl = p.ImageUrl,
//                        CourierQuantityDelivered = p.CourierQuantityDelivered,
//                        CourierQuantityReturned = p.CourierQuantityReturned,
//                        StatusId = p.StatusId,

//                        StatusDTO = new StatusDTO
//                        {
//                            Id = p.StatusId,
//                            Name = p.Status.Name,
//                            DeliveryManName = p.Shipment.DeliveryManId.HasValue ? p.Shipment.DeliveryMan.Name : null,
//                        },
//                    });

//                    response.ShipItemDTOs = query.ToList();

//                    response.Message = HttpStatusCode.OK.ToString();
//                    response.Success = true;
//                    response.StatusCode = HttpStatusCode.OK;
//                }
//                catch (Exception ex)
//                {
//                    response.Message = ex.Message;
//                    response.Success = false;
//                    LogHelper.LogException(ex.Message, ex.StackTrace);
//                }
//                return response;
//            });
//            return response;
//        }
//        #endregion

//        #region Filters

//        private static IQueryable<ShipDTO> ApplyFilter(IQueryable<ShipDTO> query, ShipDTO filter, int RoleId, int UserId)
//        {
//            if (!string.IsNullOrEmpty(filter.Search))
//            {
//                filter.Search = filter.Search.ToLower();
//                query = query.Where(c => c.RefId.ToLower().Contains(filter.Search)
//                         || c.VendorDetailsDTO.VendorName.ToLower().Contains(filter.Search)
//                         || c.CustomerDetailsDTO.CustomerName.ToLower().Contains(filter.Search)
//                         || c.CustomerDetailsDTO.CustomerPhone.Contains(filter.Search));
//            }

//            if (filter.ShipmentId > 0)
//                query = query.Where(c => c.ShipmentId == filter.ShipmentId);

//            if (filter.HasProblem)
//                query = query.Where(c => c.ProblemDTOs.Any());

//            if (!string.IsNullOrEmpty(filter.RefId))
//                query = query.Where(c => c.RefId == filter.RefId);

//            if (filter.DeliveryManId.HasValue)
//                if (filter.DeliveryManId.Value > 0)
//                    query = query.Where(p => p.DeliveryManId == filter.DeliveryManId.Value);

//            if (filter.PickupRequestId.HasValue)
//                if (filter.PickupRequestId.Value > 0)
//                    query = query.Where(p => p.PickupRequestId == filter.PickupRequestId.Value);

//            if (filter.HasPickup.HasValue)
//                if (!filter.HasPickup.Value)
//                    query = query.Where(p => p.PickupRequestId == null);

//            if (filter.VendorDetailsDTO != null)
//                if (filter.VendorDetailsDTO.VendorId > 0)
//                    query = query.Where(c => c.VendorDetailsDTO.VendorId == filter.VendorDetailsDTO.VendorId);

//            if (RoleId == (int)EnumRole.Vendor)
//                query = query.Where(p => filter.VendorDetailsDTO.VendorId == UserId);

//            if (filter.StatusIDs != null)
//                if (filter.StatusIDs.Count > 0)
//                    query = query.Where(s => filter.StatusIDs.Contains(s.StatusId));

//            if (filter.ShipmentIDs != null && filter.ShipmentIDs.Count > 0)
//                query = query.Where(s => filter.ShipmentIDs.Contains(s.ShipmentId));

//            if (filter.StatusId > 0)
//                query = query.Where(p => p.StatusId == filter.StatusId);

//            if (filter.ZoneId > 0)
//                query = query.Where(c => c.ZoneId == filter.ZoneId);

//            if (filter.AreaId > 0)
//                query = query.Where(c => c.AreaId == filter.AreaId);

//            if (filter.ReturnCount > 0)
//                query = query.Where(c => c.ReturnCount > 0);

//            if (filter.IsCourierReturned)
//                query = query.Where(c => c.IsCourierReturned);

//            if (filter.IsForReceiveReturns)
//            {
//                query = query.Where(s => (s.StatusId == (int)EnumStatus.Out_For_Delivery
//                                      || s.StatusId == (int)EnumStatus.Cancelled
//                                      || s.StatusId == (int)EnumStatus.Postponed
//                                      || s.StatusId == (int)EnumStatus.Refunded
//                                      || (s.StatusId == (int)EnumStatus.Delivered && s.ShipItemDTOs.Where(i => i.StatusId != (int)EnumStatus.Delivered).Count() > 0)
//                                      || (s.StatusId == (int)EnumStatus.Delivered) && s.ShipItemDTOs.Where(i => (i.StatusId != (int)EnumStatus.Delivered) || (i.StatusId == (int)EnumStatus.Delivered && i.CourierQuantityDelivered < i.Quantity)).Count() > 0)
//                                      && s.IsCourierReturned == false);
//            }

//            if (filter.IsForReceiveCash)
//                query = query.Where(c => c.IsCashReceived == false);

//            if (filter.IsForTransferCash)
//                query = query.Where(c => c.VendorDetailsDTO.IsPaidToVendor == false);

//            if (filter.CashTransferId.HasValue)
//                if (filter.CashTransferId.Value > 0)
//                    query = query.Where(c => c.CashTransferId == filter.CashTransferId);

//            if (filter.DateFrom.HasValue)
//                query = query.Where(p => p.CreatedAt.Date >= filter.DateFrom);

//            if (filter.DateTo.HasValue)
//                query = query.Where(p => p.CreatedAt.Date <= filter.DateTo);

//            return query;
//        }

//        #endregion

//        #region Helper Methods

//        public static List<int> GetStatusGroup(int groupType)
//        {
//            var result = new List<int>();

//            switch (groupType)
//            {
//                case (int)EnumStatus.All:
//                    result = new List<int>
//                    {
//                        (int)EnumStatus.Ready_For_Delivery,
//                        (int)EnumStatus.Assigned_For_Delivery,
//                        (int)EnumStatus.Out_For_Delivery,

//                        (int)EnumStatus.Ready_For_Pickup,
//                        (int)EnumStatus.Assigned_For_Pickup,
//                        (int)EnumStatus.Picked_Up,

//                        (int)EnumStatus.At_Warehouse,
//                        (int)EnumStatus.Postponed,

//                        (int)EnumStatus.Ready_For_Return,
//                        (int)EnumStatus.Assigned_For_Return,
//                        (int)EnumStatus.Out_For_Return,

//                        (int)EnumStatus.Ready_For_Refund,
//                        (int)EnumStatus.Assigned_For_Refund,
//                        (int)EnumStatus.Out_For_Refund,

//                        (int)EnumStatus.Delivered,
//                        (int)EnumStatus.Paid_To_Vendor,
//                        (int)EnumStatus.Refunded,
//                        (int)EnumStatus.Returned,

//                        (int)EnumStatus.Cancelled,
//                        (int)EnumStatus.Cancelled_Pickup,
//                        (int)EnumStatus.Cancelled_Refund,

//                        (int)EnumStatus.Not_Delivered,
//                        (int)EnumStatus.Not_Received,

//                        (int)EnumStatus.Archive
//                    };
//                    break;
//                case (int)EnumStatus.Current:
//                    result = new List<int>
//                    {
//                        (int)EnumStatus.Ready_For_Delivery,
//                        (int)EnumStatus.Assigned_For_Delivery,
//                        (int)EnumStatus.Out_For_Delivery,

//                        (int)EnumStatus.Ready_For_Pickup,
//                        (int)EnumStatus.Assigned_For_Pickup,
//                        (int)EnumStatus.Picked_Up,

//                        (int)EnumStatus.At_Warehouse,
//                        (int)EnumStatus.Postponed,

//                        (int)EnumStatus.Ready_For_Return,
//                        (int)EnumStatus.Assigned_For_Return,
//                        (int)EnumStatus.Out_For_Return,

//                        (int)EnumStatus.Ready_For_Refund,
//                        (int)EnumStatus.Assigned_For_Refund,
//                        (int)EnumStatus.Out_For_Refund
//                    };
//                    break;
//                case (int)EnumStatus.Done:
//                    result = new List<int>
//                    {
//                        (int)EnumStatus.Delivered,
//                        (int)EnumStatus.Paid_To_Vendor,
//                        (int)EnumStatus.Refunded,
//                        (int)EnumStatus.Returned
//                    };
//                    break;
//                case (int)EnumStatus.Cancelled:
//                    result = new List<int>
//                    {
//                        (int)EnumStatus.Cancelled,
//                        (int)EnumStatus.Cancelled_Pickup,
//                        (int)EnumStatus.Cancelled_Refund,

//                        (int)EnumStatus.Not_Delivered,
//                        (int)EnumStatus.Not_Received,

//                        (int)EnumStatus.Archive
//                    };
//                    break;
//            }

//            return result;
//        }

//        public static bool CanReturnToVendor(Shipment ship, List<FollowUp> lstShipFollowup)
//        {
//            var result = true;
//            var canNotReturnStatus = new List<int>
//            {
//                (int)EnumStatus.Ready_For_Return,
//                (int)EnumStatus.Assigned_For_Return,
//                (int)EnumStatus.Out_For_Return,
//                (int)EnumStatus.Returned,

//                (int)EnumStatus.Ready_For_Refund,
//                (int)EnumStatus.Assigned_For_Refund,
//                (int)EnumStatus.Out_For_Refund,
//                (int)EnumStatus.Refunded
//            };

//            #region check can return
//            if (ship.IsStock)
//                result = false;

//            if (canNotReturnStatus.Contains(ship.StatusId))
//                result = false;

//            if (lstShipFollowup.Any(f => f.StatusId == (int)EnumStatus.Returned))
//                result = false;

//            #endregion

//            return result;
//        }

//        #endregion
//    }
//}
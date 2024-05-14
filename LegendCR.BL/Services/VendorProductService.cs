//using LegendCR.CommonDefinitions.DTO;
//using LegendCR.CommonDefinitions.DTO.VendorProductDTOs;
//using LegendCR.CommonDefinitions.Requests;
//using LegendCR.CommonDefinitions.Responses;
//using LegendCR.DAL.DB;
//using LegendCR.Helpers;
//using System;
//using System.Linq;
//using System.Net;
//using System.Security.Cryptography;

//namespace LegendCR.BL.Services
//{
//    public class VendorProductService : BaseService
//    {

//        public static VendorProductResponse GetAllProducts(VendorProductRequest request)
//        {
//            var Response = new VendorProductResponse();
//            RunBase(request, Response, (VendorProductRequest ProductRequest) =>
//            {
//                try
//                {
//                    var query = request.context.VendorProduct.Select(c => new VendorProductDTO
//                    {
//                        VendorProductId = c.VendorProductId,
//                        OriginalPrice = c.OriginalPrice,
//                        Description = c.Description,
//                        Name = c.Name,
//                        ImageUrl = c.ImageUrl,
//                        VendorId = c.VendorId,
//                        VendorName = c.Vendor.Name,
//                        StockPrice = c.StockPrice,
//                        AvailableStock = c.AvailableStock,
//                        Size = c.Size,
//                    });



//                    if (request.applyFilter)
//                        query = ApplyFilter(query, request.VendorProductDTO);

//                    if (request.PageSize > 0)
//                        query = ApplyPaging(query, request.PageSize, request.PageIndex);

//                    Response.VendorProductDTOs = query.ToList();


//                    Response.VendorProductDTO = new VendorProductDTO
//                    {

//                        RemainingItems = query.Sum(s => s.AvailableStock),
//                        TotalItems = query.Count(),
//                    //    ShippedItems = request.context.ShipmentItem
//                    //.Where(s => s.VendorProductId == Response.VendorProductDTOs
//                    //.FirstOrDefault().VendorProductId && s.StatusId == (int)EnumStatus.Delivered)
//                    //.Sum(s => s.CourierQuantityDelivered)
//                    };

//                    Response.Message = HttpStatusCode.OK.ToString();
//                    Response.Success = true;
//                    Response.StatusCode = HttpStatusCode.OK;
//                }
//                catch (Exception ex)
//                {
//                    Response.Message = ex.Message;
//                    Response.Success = false;
//                    LogHelper.LogException(ex.Message, ex.StackTrace);
//                }
//                return Response;
//            });
//            return Response;
//        }

//        public static VendorProductResponse GetLastProduct(VendorProductRequest request)
//        {
//            var Response = new VendorProductResponse();
//            RunBase(request, Response, (VendorProductRequest ProductRequest) =>
//            {
//                try
//                {
//                    var query = request.context.VendorProduct.Select(c => new VendorProductDTO
//                    {
//                        VendorProductId = c.VendorProductId,
//                        OriginalPrice = c.OriginalPrice,
//                        Description = c.Description,
//                        Name = c.Name,
//                        ImageUrl = c.ImageUrl,
//                        VendorId = c.VendorId,
//                        VendorName = c.Vendor.Name,
//                        StockPrice = c.StockPrice,
//                        AvailableStock = c.AvailableStock,
//                        Size = c.Size,
//                    }).LastOrDefault();

//                    Response.VendorProductDTO = query;
//                    Response.Message = HttpStatusCode.OK.ToString();
//                    Response.Success = true;
//                    Response.StatusCode = HttpStatusCode.OK;
//                }
//                catch (Exception ex)
//                {
//                    Response.Message = ex.Message;
//                    Response.Success = false;
//                    LogHelper.LogException(ex.Message, ex.StackTrace);
//                }
//                return Response;
//            });
//            return Response;
//        }

//        public static VendorProductResponse GetProductById(VendorProductRequest request)
//        {
//            var Response = new VendorProductResponse();
//            RunBase(request, Response, (VendorProductRequest ProductRequest) =>
//            {
//                try
//                {
//                    var query = request.context.VendorProduct.Where(i => i.VendorProductId == request.VendorProductDTO.VendorProductId)
//                    .Select(c => new VendorProductDTO
//                    {
//                        VendorProductId = c.VendorProductId,
//                        OriginalPrice = c.OriginalPrice,
//                        Description = c.Description,
//                        Name = c.Name,
//                        ImageUrl = c.ImageUrl,
//                        VendorId = c.VendorId,
//                        VendorName = c.Vendor.Name,
//                        StockPrice = c.StockPrice,
//                        AvailableStock = c.AvailableStock,
//                        Size = c.Size,
//                    }).FirstOrDefault();

//                    Response.VendorProductDTO = query;
//                    Response.Message = HttpStatusCode.OK.ToString();
//                    Response.Success = true;
//                    Response.StatusCode = HttpStatusCode.OK;
//                }
//                catch (Exception ex)
//                {
//                    Response.Message = ex.Message;
//                    Response.Success = false;
//                    LogHelper.LogException(ex.Message, ex.StackTrace);
//                }
//                return Response;
//            });
//            return Response;
//        }

//        public static VendorProductResponse AddProduct(VendorProductRequest request)
//        {
//            var Response = new VendorProductResponse();
//            RunBase(request, Response, (VendorProductRequest ProductRequest) =>
//            {
//                try
//                {
//                    var VendorProduct = new VendorProduct
//                    {
//                        VendorProductId = request.VendorProductDTO.VendorProductId,
//                        OriginalPrice = request.VendorProductDTO.OriginalPrice,
//                        Description = request.VendorProductDTO.Description,
//                        ImageUrl = request.VendorProductDTO.ImageUrl,
//                        Name = request.VendorProductDTO.Name,
//                        VendorId = request.VendorProductDTO.VendorId,
//                        StockPrice = request.VendorProductDTO.StockPrice,
//                        AvailableStock = request.VendorProductDTO.AvailableStock,
//                        Size = request.VendorProductDTO.Size,
//                    };
//                    request.context.VendorProduct.Add(VendorProduct);
//                    request.context.SaveChanges();
//                    Response.VendorProductDTO = request.VendorProductDTO;
//                    Response.Message = HttpStatusCode.OK.ToString();
//                    Response.Success = true;
//                    Response.StatusCode = HttpStatusCode.OK;
//                }
//                catch (Exception ex)
//                {
//                    Response.Message = ex.Message;
//                    Response.Success = false;
//                    LogHelper.LogException(ex.Message, ex.StackTrace);
//                }
//                return Response;
//            });
//            return Response;
//        }

//        public static VendorProductResponse EditProduct(VendorProductRequest request)
//        {
//            var Response = new VendorProductResponse();
//            RunBase(request, Response, (VendorProductRequest ProductRequest) =>
//            {
//                try
//                {
//                    var VendorProduct = request.context.VendorProduct.Find(ProductRequest.VendorProductDTO.VendorProductId);
//                    if (VendorProduct != null)
//                    {
//                        VendorProduct.OriginalPrice = request.VendorProductDTO.OriginalPrice;
//                        VendorProduct.Description = request.VendorProductDTO.Description;
//                        VendorProduct.ImageUrl = request.VendorProductDTO.ImageUrl;
//                        VendorProduct.Name = request.VendorProductDTO.Name;
//                        VendorProduct.VendorId = request.VendorProductDTO.VendorId;
//                        VendorProduct.Size = request.VendorProductDTO.Size;


//                        request.context.SaveChanges();
//                        Response.VendorProductDTO = request.VendorProductDTO;
//                        Response.Message = HttpStatusCode.OK.ToString();
//                        Response.Success = true;
//                        Response.StatusCode = HttpStatusCode.OK;
//                    }
//                    else
//                    {
//                        Response.Message = "Invalid";
//                        Response.Success = false;
//                    }
//                }
//                catch (Exception ex)
//                {
//                    Response.Message = ex.Message;
//                    Response.Success = false;
//                    LogHelper.LogException(ex.Message, ex.StackTrace);
//                }
//                return Response;
//            });
//            return Response;
//        }

//        public static VendorProductResponse DeleteProduct(VendorProductRequest Request)
//        {
//            var Response = new VendorProductResponse();
//            RunBase(Request, Response, (VendorProductRequest ProductRequest) =>
//            {
//                try
//                {
//                    var VendorProduct = Request.context.VendorProduct.FirstOrDefault(vp => vp.VendorProductId == Request.VendorProductDTO.VendorProductId);
//                    VendorProduct.IsDeleted = true;
//                    Request.context.SaveChanges();
//                    Response.Message = HttpStatusCode.OK.ToString();
//                    Response.Success = true;
//                    Response.StatusCode = HttpStatusCode.OK;
//                }
//                catch (Exception ex)
//                {
//                    Response.Message = ex.Message;
//                    Response.Success = false;
//                    LogHelper.LogException(ex.Message, ex.StackTrace);
//                }
//                return Response;
//            });
//            return Response;
//        }

//        private static IQueryable<VendorProductDTO> ApplyFilter(IQueryable<VendorProductDTO> query, VendorProductDTO filter)
//        {
//            if (!string.IsNullOrEmpty(filter.Search))
//            {
//                query = query.Where
//                    (c => (!string.IsNullOrEmpty(c.Name) && c.Name.Contains(filter.Search))
//                        || (!string.IsNullOrEmpty(c.VendorName) && c.VendorName.Contains(filter.Search)));
//            }


//            if (filter.VendorProductId > 0)
//                query = query.Where(c => c.VendorProductId == filter.VendorProductId);

//            if (filter.VendorId > 0)
//                query = query.Where(c => c.VendorId == filter.VendorId);

//            return query;
//        }
//    }
//}

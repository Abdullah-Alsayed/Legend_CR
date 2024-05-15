using DicomApp.CommonDefinitions.DTO;
using DicomApp.CommonDefinitions.DTO.ShipmentDTOs;
using DicomApp.CommonDefinitions.Requests;
using DicomApp.CommonDefinitions.Responses;
using DicomApp.DAL.DB;
using DicomApp.Helpers;
using System;
using System.Linq;
using System.Net;

namespace DicomApp.BL.Services
{
    public class PackingService : BaseService
    {

        public static PackingResponse GetPackings(PackingRequest request)
        {
            var res = new PackingResponse();
            RunBase(request, res, (PackingRequest req) =>
            {
                try
                {
                    var query = request.context.Packing.Select(p => new PackingDTO
                    {
                        id = p.Id,
                        Count = p.Count,
                        CreatedAt = p.CreatedAt,
                        CreatedBy = p.CreatedBy,
                        ImgUrl = p.ImgUrl,
                        NameAr = p.NameAr,
                        NameEn = p.NameEn,
                        PackingTypeId = p.PackingTypeId,
                        PackingTypeName = p.PackingType.NameEn,
                        Price = p.Price,
                        Size = p.Size,
                        //Description = p.Description,
                        ShipDTOs = p.ShipmentWarehousePacking.Select(s => new ShipDTO
                        {
                            ShipmentId = s.ShipmentId
                        })
                    });

                    if (request.PackingDTO != null)
                        query = ApplyFilter(query, request.PackingDTO);

                    res.TotalCount = query.Count();

                    query = OrderByDynamic(query, request.OrderByColumn ?? "id", request.IsDesc);

                    if (request.PageSize > 0)
                        query = ApplyPaging(query, request.PageSize, request.PageIndex);

                    res.PackingDTOs = query.ToList();
                    res.Message = HttpStatusCode.OK.ToString();
                    res.Success = true;
                    res.StatusCode = HttpStatusCode.OK;
                }
                catch (Exception ex)
                {
                    res.Message = ex.Message;
                    res.Success = false;
                    LogHelper.LogException(ex.Message, ex.StackTrace);
                }
                return res;
            });
            return res;
        }

        public static PackingResponse GetLastPacking(PackingRequest request)
        {
            var res = new PackingResponse();
            RunBase(request, res, (PackingRequest req) =>
                {
                    try
                    {
                        var query = request.context.Packing.Select(p => new PackingDTO
                        {
                            id = p.Id,
                            Count = p.Count,
                            CreatedAt = p.CreatedAt,
                            CreatedBy = p.CreatedBy,
                            ImgUrl = p.ImgUrl,
                            NameAr = p.NameAr,
                            NameEn = p.NameEn,
                            PackingTypeId = p.PackingTypeId,
                            PackingTypeName = p.PackingType.NameEn,
                            Price = p.Price,
                            Size = p.Size,
                            //Description = p.Description,
                        }).LastOrDefault();

                        res.TotalCount = 1;

                        res.PackingDTO = query;
                        res.Message = HttpStatusCode.OK.ToString();
                        res.Success = true;
                        res.StatusCode = HttpStatusCode.OK;
                    }
                    catch (Exception ex)
                    {
                        res.Message = ex.Message;
                        res.Success = false;
                        LogHelper.LogException(ex.Message, ex.StackTrace);
                    }
                    return res;
                });
            return res;
        }
        public static PackingResponse GetPacking(PackingRequest request)
        {
            var res = new PackingResponse();
            RunBase(request, res, (PackingRequest req) =>
            {
                try
                {
                    var query = request.context.Packing.Where(p=>p.Id == request.PackingDTO.id).Select(p => new PackingDTO
                    {
                        id = p.Id,
                        Count = p.Count,
                        CreatedAt = p.CreatedAt,
                        CreatedBy = p.CreatedBy,
                        ImgUrl = p.ImgUrl,
                        NameAr = p.NameAr,
                        NameEn = p.NameEn,
                        PackingTypeId = p.PackingTypeId,
                        PackingTypeName = p.PackingType.NameEn,
                        Price = p.Price,
                        Size = p.Size,
                        //Description = p.Description,
                    });

                    res.TotalCount = 1;
                    res.PackingDTO = query.FirstOrDefault();
                    res.Message = HttpStatusCode.OK.ToString();
                    res.Success = true;
                    res.StatusCode = HttpStatusCode.OK;
                }
                catch (Exception ex)
                {
                    res.Message = ex.Message;
                    res.Success = false;
                    LogHelper.LogException(ex.Message, ex.StackTrace);
                }
                return res;
            });
            return res;
        }
        public static PackingResponse EditPacking(PackingRequest request)
        {

            var res = new PackingResponse();
            RunBase(request, res, (PackingRequest req) =>
            {
                try
                {
                    var model = request.PackingDTO;
                    int PackingID = model.id;
                    var Packing = request.context.Packing.Find(PackingID);
                    if (Packing != null)
                    {
                        //update whole Packing
                        Packing = AddOrEditPacking(request.UserID, request.PackingDTO, Packing);
                        request.context.SaveChanges();

                        res.Message = HttpStatusCode.OK.ToString();
                        res.Success = true;
                        res.StatusCode = HttpStatusCode.OK;
                    }
                    else
                    {
                        res.Message = "Invalid";
                        res.Success = false;
                    }
                }
                catch (Exception ex)
                {
                    res.Message = ex.Message;
                    res.Success = false;
                    LogHelper.LogException(ex.Message, ex.StackTrace);
                }
                return res;
            });
            return res;
        }

        public static PackingResponse AddPacking(PackingRequest request)
        {

            var res = new PackingResponse();
            RunBase(request, res, (PackingRequest req) =>
            {
                try
                {
                    var Packing = AddOrEditPacking(request.UserID, request.PackingDTO);
                    request.context.Packing.Add(Packing);
                    request.context.SaveChanges();

                    res.Message = "Added Successfully";
                    res.Success = true;
                    res.StatusCode = HttpStatusCode.OK;


                }
                catch (Exception ex)
                {
                    res.Message = ex.Message;
                    res.Success = false;
                    LogHelper.LogException(ex.Message, ex.StackTrace);
                }
                return res;
            });
            return res;
        }

        private static Packing AddOrEditPacking(int createdBy, PackingDTO record, Packing oldPacking = null)
        {
            if (oldPacking == null)//new Packing
            {
                oldPacking = new Packing();
                oldPacking.CreatedAt = DateTime.Now;
                oldPacking.CreatedBy = createdBy;
            }
            oldPacking.ImgUrl = record.ImgUrl;
            oldPacking.CreatedBy = record.CreatedBy;
            oldPacking.Id = record.id;
            oldPacking.BranchId = record.BranchId;
            oldPacking.LastModifiedBy = createdBy;
            oldPacking.LastModifiedAt = DateTime.Now;
            oldPacking.Count = record.Count;
            oldPacking.PackingTypeId = record.PackingTypeId;
            oldPacking.NameAr = record.NameAr;
            oldPacking.NameEn = record.NameEn;
            oldPacking.Price = record.Price;
            oldPacking.Size = record.Size;
            return oldPacking;
        }

        private static IQueryable<PackingDTO> ApplyFilter(IQueryable<PackingDTO> query, PackingDTO packingDTO)
        {
            if (!string.IsNullOrEmpty(packingDTO.search))
                query = query.Where
                    (c => (!string.IsNullOrEmpty(c.NameEn) && c.NameEn.Contains(packingDTO.search))
                       || (!string.IsNullOrEmpty(c.NameAr) && c.NameAr.Contains(packingDTO.search)));

            if (packingDTO.PackingTypeId > 0)
                query = query.Where(p => p.PackingTypeId == packingDTO.PackingTypeId);

            if (packingDTO.Count > 0)
                query = query.Where(p => p.Count <= packingDTO.Count);

            if (packingDTO.DateFrom.HasValue)
                query = query.Where(p => p.CreatedAt.Date >= packingDTO.DateFrom);

            if (packingDTO.DateTo.HasValue)
                query = query.Where(p => p.CreatedAt.Date <= packingDTO.DateTo);

            return query;

        }
    }
}

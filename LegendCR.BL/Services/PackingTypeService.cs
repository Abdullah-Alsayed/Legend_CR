//using LegendCR.CommonDefinitions.DTO;
//using LegendCR.CommonDefinitions.Requests;
//using LegendCR.CommonDefinitions.Responses;
//using LegendCR.DAL.DB;
//using LegendCR.Helpers;
//using System.Net;

//namespace LegendCR.BL.Services
//{
//    public class PackingTypeService : BaseService
//    {

//        public static PackingTypeResponse GetPackingType(PackingTypeRequest request)
//        {
//            var res = new PackingTypeResponse();
//            RunBase(request, res, (PackingTypeRequest req) =>
//            {
//                try
//                {
//                    var query = request.context.PackingType.Select(p => new PackingTypeDTO
//                    {
//                        Id = p.Id,
//                        CreatedBy = p.CreatedBy,
//                        LastModifiedAt = p.LastModifiedAt,
//                        LastModifiedBy = p.LastModifiedBy,
//                        NameEn = p.NameEn,
//                        NameAr = p.NameAr,
//                        CreatedAt = p.CreatedAt,
//                        BranchId = p.BranchId,
//                    });

//                    if (request.PackingTypeDTO != null)
//                        query = ApplyFilter(query, request.PackingTypeDTO);

//                    res.TotalCount = query.Count();

//                    query = OrderByDynamic(query, request.OrderByColumn ?? "Id", request.IsDesc);

//                    if (request.PageSize > 0)
//                        query = ApplyPaging(query, request.PageSize, request.PageIndex);

//                    res.PackingTypeDTOs = query.ToList();
//                    res.Message = HttpStatusCode.OK.ToString();
//                    res.Success = true;
//                    res.StatusCode = HttpStatusCode.OK;
//                }
//                catch (Exception ex)
//                {
//                    res.Message = ex.Message;
//                    res.Success = false;
//                    LogHelper.LogException(ex.Message, ex.StackTrace);
//                }
//                return res;
//            });
//            return res;
//        }
//        public static PackingTypeResponse DeletePackingType(PackingTypeRequest request)
//        {

//            var res = new PackingTypeResponse();
//            RunBase(request, res, (PackingTypeRequest req) =>
//            {
//                try
//                {
//                    var model = request.PackingTypeDTO;
//                    var PackingType = request.context.PackingType.FirstOrDefault(c => c.Id == model.Id);
//                    if (PackingType != null)
//                    {
//                        //update Agency IsDeleted
//                        request.context.SaveChanges();

//                        res.Message = MessageKey.DeletedSuccessfully.ToString();
//                        res.Success = true;
//                        res.StatusCode = HttpStatusCode.OK;
//                    }
//                    else
//                    {
//                        res.Message = MessageKey.InvalidData.ToString();
//                        res.Success = false;
//                    }

//                }
//                catch (Exception ex)
//                {
//                    res.Message = ex.Message;
//                    res.Success = false;
//                    LogHelper.LogException(ex.Message, ex.StackTrace);
//                }
//                return res;
//            });
//            return res;
//        }


//        public static PackingTypeResponse AddPackingType(PackingTypeRequest request)
//        {

//            var res = new PackingTypeResponse();
//            RunBase(request, res, (PackingTypeRequest req) =>
//            {
//                try
//                {
//                    var PackingType = AddOrEditPackingType(request.PackingTypeDTO);
//                    request.context.PackingType.Add(PackingType);
//                    request.context.SaveChanges();

//                    res.Message = "AddedSuccessfully";
//                    res.Success = true;
//                    res.StatusCode = HttpStatusCode.OK;


//                }
//                catch (Exception ex)
//                {
//                    res.Message = ex.Message;
//                    res.Success = false;
//                    LogHelper.LogException(ex.Message, ex.StackTrace);
//                }
//                return res;
//            });
//            return res;
//        }

//        public static PackingTypeResponse EditPackingType(PackingTypeRequest request)
//        {

//            var res = new PackingTypeResponse();
//            RunBase(request, res, (PackingTypeRequest req) =>
//            {
//                try
//                {
//                    var model = request.PackingTypeDTO;
//                    var PackingType = request.context.PackingType.Find(model.Id);
//                    if (PackingType != null)
//                    {
//                        //update whole Agency
//                        PackingType = AddOrEditPackingType(request.PackingTypeDTO, PackingType);
//                        request.context.SaveChanges();

//                        res.Message = "Updated Successfully";
//                        res.Success = true;
//                        res.StatusCode = HttpStatusCode.OK;
//                    }
//                    else
//                    {
//                        res.Message = "Invalid";
//                        res.Success = false;
//                    }
//                }
//                catch (Exception ex)
//                {
//                    res.Message = ex.Message;
//                    res.Success = false;
//                    LogHelper.LogException(ex.Message, ex.StackTrace);
//                }
//                return res;
//            });
//            return res;
//        }


//        public static PackingType AddOrEditPackingType(PackingTypeDTO PackingTypeDTO, PackingType PackingType = null)
//        {
//            if (PackingType == null)
//            {
//                PackingType = new PackingType();
//            }


//            PackingType.Id = PackingTypeDTO.Id;
//            PackingType.CreatedAt = PackingTypeDTO.CreatedAt;
//            PackingType.CreatedBy = PackingTypeDTO.CreatedBy;
//            PackingType.NameAr = PackingTypeDTO.NameAr;
//            PackingType.NameEn = PackingTypeDTO.NameEn;
//            PackingType.LastModifiedBy = PackingTypeDTO.LastModifiedBy;
//            PackingType.LastModifiedAt = PackingTypeDTO.LastModifiedAt;
//            PackingType.BranchId = PackingTypeDTO.BranchId;
//            return PackingType;
//        }

//        private static IQueryable<PackingTypeDTO> ApplyFilter(IQueryable<PackingTypeDTO> query, PackingTypeDTO record)
//        {
//            if (!string.IsNullOrEmpty(record.Search))
//            {
//                query = query.Where
//                    (c => (!string.IsNullOrEmpty(c.NameEn) && c.NameEn.Contains(record.Search))
//                        || (!string.IsNullOrEmpty(c.NameAr) && c.NameAr.Contains(record.Search)));
//            }

//            if (record.Id > 0)
//            {
//                query = query.Where(p => p.Id == record.Id);
//            }
//            return query;
//        }
//    }
//}
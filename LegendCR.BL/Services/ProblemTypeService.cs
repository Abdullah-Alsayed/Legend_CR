//using LegendCR.CommonDefinitions.DTO;
//using LegendCR.CommonDefinitions.Requests;
//using LegendCR.CommonDefinitions.Responses;
//using LegendCR.Helpers;
//using System;
//using System.Linq;
//using System.Net;
//using LegendCR.DAL.DB;

//namespace LegendCR.BL.Services
//{
//    public class ProblemTypeService : BaseService
//    {

//        public static ProblemTypeResponse ListProblemType(ProblemTypeRequest request)
//        {
//            var response = new ProblemTypeResponse();
//            RunBase(request, response, (ProblemTypeRequest req) =>
//            {
//                try
//                {
//                    var query = request.context.ProblemType.Where(p => (p.IsDeleted.HasValue ? !p.IsDeleted.Value : true)).Select(p => new ProblemTypeDTO
//                    {
//                        ProblemTypeId = p.ProblemTypeId,
//                        Type = p.Type,
//                        NameEn = p.NameEn,
//                        NameAr = p.NameAr,
//                    });

//                    if (request.ProblemTypeDTO != null)
//                        query = ApplyFilter(query, request.ProblemTypeDTO);

//                    query = OrderByDynamic(query, request.OrderByColumn ?? "ProblemTypeId", request.IsDesc);

//                    if (request.PageSize > 0)
//                        query = ApplyPaging(query, request.PageSize, request.PageIndex);

//                    response.ProblemTypeDTOs = query.ToList();

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

//        public static ProblemTypeResponse GetLastProblemType(ProblemTypeRequest request)
//        {
//            var response = new ProblemTypeResponse();
//            RunBase(request, response, (ProblemTypeRequest req) =>
//            {
//                try
//                {
//                    var query = request.context.ProblemType.Where(p => (p.IsDeleted.HasValue ? !p.IsDeleted.Value : true)).Select(p => new ProblemTypeDTO
//                    {
//                        ProblemTypeId = p.ProblemTypeId,
//                        Type = p.Type,
//                        NameEn = p.NameEn,
//                        NameAr = p.NameAr,
//                    });

//                    response.ProblemTypeDTO = query.LastOrDefault();

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

//        public static ProblemTypeResponse DeleteProblemType(ProblemTypeRequest request)
//        {
//            var response = new ProblemTypeResponse();
//            RunBase(request, response, (ProblemTypeRequest req) =>
//            {
//                try
//                {
//                    var model = request.ProblemTypeDTO;
//                    var ProblemType = request.context.ProblemType.FirstOrDefault(c => c.ProblemTypeId == model.ProblemTypeId);
//                    if (ProblemType != null)
//                    {
//                        //update Agency IsDeleted
//                        ProblemType.IsDeleted = true;
//                        request.context.SaveChanges();

//                        response.Message = MessageKey.DeletedSuccessfully.ToString();
//                        response.Success = true;
//                        response.StatusCode = HttpStatusCode.OK;
//                    }
//                    else
//                    {
//                        response.Message = MessageKey.InvalidData.ToString();
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

//        public static ProblemTypeResponse AddProblemType(ProblemTypeRequest request)
//        {
//            var response = new ProblemTypeResponse();
//            RunBase(request, response, (ProblemTypeRequest req) =>
//            {
//                try
//                {
//                    var ProblemType = AddOrEditProblemType(request.ProblemTypeDTO);
//                    request.context.ProblemType.Add(ProblemType);
//                    request.context.SaveChanges();

//                    response.Message = "AddedSuccessfully";
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

//        public static ProblemTypeResponse EditProblemType(ProblemTypeRequest request)
//        {
//            var response = new ProblemTypeResponse();
//            RunBase(request, response, (ProblemTypeRequest req) =>
//            {
//                try
//                {
//                    var model = request.ProblemTypeDTO;
//                    var ProblemType = request.context.ProblemType.Find(model.ProblemTypeId);
//                    if (ProblemType != null)
//                    {
//                        //update whole Agency
//                        ProblemType = AddOrEditProblemType(request.ProblemTypeDTO, ProblemType);
//                        request.context.SaveChanges();

//                        response.Message = "Updated Successfully";
//                        response.Success = true;
//                        response.StatusCode = HttpStatusCode.OK;
//                    }
//                    else
//                    {
//                        response.Message = "Invalid";
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

//        private static ProblemType AddOrEditProblemType(ProblemTypeDTO ProblemTypeRecord, ProblemType ProblemType = null)
//        {
//            if (ProblemType == null)
//            {
//                ProblemType = new ProblemType();
//            }
//            ProblemType.ProblemTypeId = ProblemTypeRecord.ProblemTypeId;
//            ProblemType.NameEn = ProblemTypeRecord.NameEn;
//            ProblemType.NameAr = ProblemTypeRecord.NameAr;
//            ProblemType.Type = ProblemTypeRecord.Type;
//            return ProblemType;
//        }

//        private static IQueryable<ProblemTypeDTO> ApplyFilter(IQueryable<ProblemTypeDTO> query, ProblemTypeDTO record)
//        {
//            if (!string.IsNullOrEmpty(record.Search))
//            {
//                query = query.Where
//                    (c => !string.IsNullOrEmpty(c.NameEn) && (c.NameEn.Contains(record.Search) || c.NameAr.Contains(record.Search)));
//            }
//            return query;
//        }
//    }
//}

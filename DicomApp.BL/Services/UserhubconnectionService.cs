using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using DicomApp.CommonDefinitions.DTO;
using DicomApp.CommonDefinitions.Requests;
using DicomApp.CommonDefinitions.Responses;
using DicomApp.DAL.DB;
using DicomApp.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DicomApp.BL.Services
{
    public class UserhubconnectionService : BaseService
    {
        public static UserhubconnectionResponse ListUserhubconnection(
            UserhubconnectionRequest request
        )
        {
            var res = new UserhubconnectionResponse();
            RunBase(
                request,
                res,
                (UserhubconnectionRequest req) =>
                {
                    try
                    {
                        var query = request.context.Userhubconnection.Select(
                            p => new UserhubconnectionDTO
                            {
                                ID = p.Id,
                                ConnectionID = p.ConnectionId,
                                IsOnline = p.IsOnline,
                                CreatedBy = p.CreatedBy,
                                CreationDate = p.CreationDate,
                                ModificationDate = p.ModificationDate,
                                ModifiedBy = p.ModifiedBy,
                                IsDeleted = p.IsDeleted,
                            }
                        );

                        if (request.UserhubconnectionRecord != null)
                            query = ApplyFilter(query, request.UserhubconnectionRecord);

                        res.TotalCount = query.Count();

                        query = OrderByDynamic(
                            query,
                            request.OrderByColumn ?? "id",
                            request.IsDesc
                        );

                        if (request.PageSize > 0)
                            query = ApplyPaging(query, request.PageSize, request.PageIndex);

                        res.UserhubconnectionRecords = query.ToList();
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
                }
            );
            return res;
        }

        public static UserhubconnectionResponse DeleteUserhubconnection(
            UserhubconnectionRequest request
        )
        {
            var res = new UserhubconnectionResponse();
            RunBase(
                request,
                res,
                (UserhubconnectionRequest req) =>
                {
                    try
                    {
                        var model = request.UserhubconnectionRecord;
                        var Userhubconnection = request.context.Userhubconnection.FirstOrDefault(
                            c => c.Id == model.ID
                        );
                        if (Userhubconnection != null)
                        {
                            //update Agency IsDeleted
                            request.context.SaveChanges();

                            res.Message = SystemEnums.DeletedSuccessfully.ToString();
                            res.Success = true;
                            res.StatusCode = HttpStatusCode.OK;
                        }
                        else
                        {
                            res.Message = SystemEnums.InvalidData.ToString();
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
                }
            );
            return res;
        }

        public static UserhubconnectionResponse AddUserhubconnection(
            UserhubconnectionRequest request
        )
        {
            var res = new UserhubconnectionResponse();
            RunBase(
                request,
                res,
                (UserhubconnectionRequest req) =>
                {
                    try
                    {
                        var Userhubconnection = AddOrEditUserhubconnection(
                            request.UserhubconnectionRecord
                        );
                        request.context.Userhubconnection.Add(Userhubconnection);
                        request.context.SaveChanges();

                        res.Message = "AddedSuccessfully";
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
                }
            );
            return res;
        }

        public static UserhubconnectionResponse EditUserhubconnection(
            UserhubconnectionRequest request
        )
        {
            var res = new UserhubconnectionResponse();
            RunBase(
                request,
                res,
                (UserhubconnectionRequest req) =>
                {
                    try
                    {
                        var model = request.UserhubconnectionRecord;
                        var Userhubconnection = request.context.Userhubconnection.Find(model.ID);
                        if (Userhubconnection != null)
                        {
                            //update whole Agency
                            Userhubconnection = AddOrEditUserhubconnection(
                                request.UserhubconnectionRecord,
                                Userhubconnection
                            );
                            request.context.SaveChanges();

                            res.Message = "UpdatedSuccessfully";
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
                }
            );
            return res;
        }

        private static Userhubconnection AddOrEditUserhubconnection(
            UserhubconnectionDTO userhubconnectionRecord,
            Userhubconnection userhubconnection = null
        )
        {
            if (userhubconnection == null)
            {
                userhubconnection = new Userhubconnection();
            }

            userhubconnection.ConnectionId = userhubconnectionRecord.ConnectionID;
            userhubconnection.IsOnline = userhubconnectionRecord.IsOnline;
            userhubconnection.CreatedBy = userhubconnectionRecord.CreatedBy;
            userhubconnection.CreationDate = userhubconnectionRecord.CreationDate;
            userhubconnection.ModificationDate = userhubconnectionRecord.ModificationDate.Value;
            userhubconnection.ModifiedBy = userhubconnectionRecord.ModifiedBy;
            userhubconnection.IsDeleted = userhubconnectionRecord.IsDeleted;
            return userhubconnection;
        }

        private static IQueryable<UserhubconnectionDTO> ApplyFilter(
            IQueryable<UserhubconnectionDTO> query,
            UserhubconnectionDTO record
        )
        {
            return query;
        }
    }
}

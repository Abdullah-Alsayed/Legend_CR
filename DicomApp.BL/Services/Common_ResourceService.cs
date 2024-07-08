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
    public class Common_ResourceService : BaseService
    {
        public static Common_ResourceResponse ListCommon_Resource(Common_ResourceRequest request)
        {
            var res = new Common_ResourceResponse();
            RunBase(
                request,
                res,
                (Common_ResourceRequest req) =>
                {
                    try
                    {
                        var query = request.context.CommonResource.Select(
                            p => new Common_ResourceDTO
                            {
                                ID = p.Id,
                                ResourceKey = p.ResourceKey,
                                ResourceValue = p.ResourceValue,
                                ResourceLang = p.ResourceLang,
                                CreatedBy = p.CreatedBy,
                                ModifiedBy = p.ModifiedBy,
                                LastUpdateDate = p.LastUpdateDate,
                                CreationDate = p.CreationDate,
                                IsDeleted = p.IsDeleted,
                                ApplicationId = p.ApplicationId,
                                Url = p.Url,
                                MediaUrl = p.MediaUrl,
                            }
                        );

                        if (request.Common_ResourceRecord != null)
                            query = ApplyFilter(query, request.Common_ResourceRecord);

                        res.TotalCount = query.Count();

                        query = OrderByDynamic(
                            query,
                            request.OrderByColumn ?? "id",
                            request.IsDesc
                        );

                        if (request.PageSize > 0)
                            query = ApplyPaging(query, request.PageSize, request.PageIndex);

                        res.Common_ResourceRecords = query.ToList();
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

        public static Common_ResourceResponse DeleteCommon_Resource(Common_ResourceRequest request)
        {
            var res = new Common_ResourceResponse();
            RunBase(
                request,
                res,
                (Common_ResourceRequest req) =>
                {
                    try
                    {
                        var model = request.Common_ResourceRecord;
                        var Common_Resource = request.context.CommonResource.FirstOrDefault(c =>
                            c.Id == model.ID
                        );
                        if (Common_Resource != null)
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

        public static Common_ResourceResponse AddCommon_Resource(Common_ResourceRequest request)
        {
            var res = new Common_ResourceResponse();
            RunBase(
                request,
                res,
                (Common_ResourceRequest req) =>
                {
                    try
                    {
                        var Common_Resource = AddOrEditCommon_Resource(
                            request.Common_ResourceRecord
                        );
                        request.context.CommonResource.Add(Common_Resource);
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

        public static Common_ResourceResponse EditCommon_Resource(Common_ResourceRequest request)
        {
            var res = new Common_ResourceResponse();
            RunBase(
                request,
                res,
                (Common_ResourceRequest req) =>
                {
                    try
                    {
                        var model = request.Common_ResourceRecord;
                        var Common_Resource = request.context.CommonResource.Find(model.ID);
                        if (Common_Resource != null)
                        {
                            //update whole Agency
                            Common_Resource = AddOrEditCommon_Resource(
                                request.Common_ResourceRecord,
                                Common_Resource
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

        private static CommonResource AddOrEditCommon_Resource(
            Common_ResourceDTO common_ResourceRecord,
            CommonResource common_Resource = null
        )
        {
            if (common_Resource == null)
            {
                common_Resource = new CommonResource();
            }

            common_Resource.ResourceKey = common_ResourceRecord.ResourceKey;
            common_Resource.ResourceValue = common_ResourceRecord.ResourceValue;
            common_Resource.ResourceLang = common_ResourceRecord.ResourceLang;
            common_Resource.CreatedBy = common_ResourceRecord.CreatedBy;
            common_Resource.ModifiedBy = common_ResourceRecord.ModifiedBy;
            common_Resource.LastUpdateDate = common_ResourceRecord.LastUpdateDate.Value;
            common_Resource.CreationDate = common_ResourceRecord.CreationDate;
            common_Resource.IsDeleted = common_ResourceRecord.IsDeleted;
            common_Resource.ApplicationId = common_ResourceRecord.ApplicationId;
            common_Resource.Url = common_ResourceRecord.Url;
            common_Resource.MediaUrl = common_ResourceRecord.MediaUrl;
            return common_Resource;
        }

        private static IQueryable<Common_ResourceDTO> ApplyFilter(
            IQueryable<Common_ResourceDTO> query,
            Common_ResourceDTO record
        )
        {
            if (!string.IsNullOrWhiteSpace(record.ResourceKey))
            {
                query = query.Where(p =>
                    p.ResourceKey != null && p.ResourceKey.Contains(record.ResourceKey)
                );
            }
            if (!string.IsNullOrWhiteSpace(record.ResourceValue))
            {
                query = query.Where(p =>
                    p.ResourceValue != null && p.ResourceValue.Contains(record.ResourceValue)
                );
            }
            if (!string.IsNullOrWhiteSpace(record.ResourceLang))
            {
                query = query.Where(p =>
                    p.ResourceLang != null && p.ResourceLang.Contains(record.ResourceLang)
                );
            }

            if (record.LastUpdateDate.HasValue)
            {
                query = query.Where(p => p.LastUpdateDate == record.LastUpdateDate);
            }

            if (!string.IsNullOrWhiteSpace(record.Url))
            {
                query = query.Where(p => p.Url != null && p.Url.Contains(record.Url));
            }
            if (!string.IsNullOrWhiteSpace(record.MediaUrl))
            {
                query = query.Where(p =>
                    p.MediaUrl != null && p.MediaUrl.Contains(record.MediaUrl)
                );
            }
            return query;
        }
    }
}

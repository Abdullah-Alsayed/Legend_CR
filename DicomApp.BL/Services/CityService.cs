using DicomApp.BL.Services;
using DicomApp.CommonDefinitions.DTO;
using DicomApp.CommonDefinitions.Records;
using DicomApp.CommonDefinitions.Responses;
using DicomApp.DAL.DB;
using DicomApp.Helpers;
using DicomDB.CommonDefinitions.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace DicomDB.BL.Services
{
    public class CityService : BaseService
    {
        #region CityServices
        public static CityResponse GetCity(CityRequest request)
        {

            var res = new CityResponse();
            RunBase(request, res, (CityRequest req) =>
            {
                try
                {
                    var query = request.context.City.Select(p => new CityDTO
                    {
                        Id = p.Id,
                        CityName = p.CityName,
                        CityNameAr = p.CityNameAr,
                        CreatedAt = p.CreatedAt,
                        CreatedBy = p.CreatedBy,
                        ZoneNameAr = p.Zone.NameAr,
                        ZoneNameEn = p.Zone.NameEn,
                        //ZoneTax = p.Zone.ZoneTax.FirstOrDefault(z => z.ZoneId == p.Id),
                        //ShippingFees = p.Zone.ZoneTax.FirstOrDefault(z => z.ZoneId == p.Id).Tax,
                        LastModifiedAt = p.LastModifiedAt,
                        LastModifiedBy = p.LastModifiedBy,
                        Lat = p.Lat,
                        Lng = p.Lng,
                        StateId = p.StateId,
                        ZoneId = p.ZoneId,
                        Address = p.Address,
                        IsDeleted = p.IsDeleted,
                    });

                    if (request.CityDTO != null)
                        query = ApplyFilter(query, request.CityDTO);

                    //query = OrderByDynamic(query, request.OrderByColumn ?? "Id", request.IsDesc);

                    //if (request.PageSize > 0)
                    //    query = ApplyPaging(query, request.PageSize, request.PageIndex);

                    res.CityDTOs = query.ToList();

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

        public static CityResponse GetLastCity(CityRequest request)
        {
            var res = new CityResponse();
            RunBase(request, res, (CityRequest req) =>
            {
                try
                {
                    var query = request.context.City.Select(p => new CityDTO
                    {
                        Id = p.Id,
                        CityName = p.CityName,
                        CityNameAr = p.CityNameAr,
                        CreatedAt = p.CreatedAt,
                        CreatedBy = p.CreatedBy,
                        ZoneNameAr = p.Zone.NameAr,
                        ZoneNameEn = p.Zone.NameEn,
                        //ZoneTax = p.Zone.ZoneTax.FirstOrDefault(z => z.ZoneId == p.Id),
                        //ShippingFees = p.Zone.ZoneTax.FirstOrDefault(z => z.ZoneId == p.Id).Tax,
                        LastModifiedAt = p.LastModifiedAt,
                        LastModifiedBy = p.LastModifiedBy,
                        Lat = p.Lat,
                        Lng = p.Lng,
                        StateId = p.StateId,
                        ZoneId = p.ZoneId,
                        Address = p.Address,
                        IsDeleted = p.IsDeleted,
                    }).LastOrDefault();

                    res.CityDTO = query;

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

        public static CityResponse DeleteCity(CityRequest request)
        {
            var res = new CityResponse();
            RunBase(request, res, (CityRequest req) =>
            {
                try
                {
                    var City = request.context.City.FirstOrDefault(c => c.Id == request.CityDTO.Id);
                    if (City != null)
                    {
                        //update IsDeleted
                        City.IsDeleted = true;
                        City.LastModifiedAt = DateTime.Now;
                        request.context.SaveChanges();

                        res.Message = MessageKey.DeletedSuccessfully.ToString();
                        res.Success = true;
                        res.StatusCode = HttpStatusCode.OK;
                    }
                    else
                    {
                        res.Message = MessageKey.InvalidData.ToString();
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

        public static CityResponse AddCity(CityRequest request)
        {
            var res = new CityResponse();
            RunBase(request, res, (CityRequest req) =>
            {
                try
                {
                    var City = AddOrEditCity(req.UserID, request.CityDTO);
                    request.context.City.Add(City);
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

        public static CityResponse EditCity(CityRequest request)
        {
            var res = new CityResponse();
            RunBase(request, res, (CityRequest req) =>
            {
                try
                {
                    var model = request.CityDTO;
                    var City = request.context.City.Find(model.Id);
                    if (City != null)
                    {
                        //update whole Agency
                        City = AddOrEditCity(req.UserID, request.CityDTO, City);
                        request.context.SaveChanges();

                        res.Message = "Updated Successfully";
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

        public static IQueryable<CityDTO> ApplyFilter(IQueryable<CityDTO> query, CityDTO cityRecord)
        {
            query = query.Where(c => c.IsDeleted != true);

            if (!string.IsNullOrEmpty(cityRecord.Search))
            {
                query = query.Where
                       (c => (!string.IsNullOrEmpty(c.CityName) && c.CityName.Contains(cityRecord.Search))
                    || (!string.IsNullOrEmpty(c.CityNameAr) && c.CityNameAr.Contains(cityRecord.Search)));
            }
            if (cityRecord.ZoneId > 0)
                query = query.Where(z => z.ZoneId == cityRecord.ZoneId);

            return query;
        }

        public static City AddOrEditCity(int createdBy, CityDTO cityRecord, City city = null)
        {
            if (city == null)
            {
                city = new City();
                city.CreatedAt = DateTime.Now;
                city.CreatedBy = createdBy.ToString();
            }
            city.Id = cityRecord.Id;
            city.CityName = cityRecord.CityName;
            city.CityNameAr = cityRecord.CityNameAr;
            city.LastModifiedAt = cityRecord.LastModifiedAt;
            city.LastModifiedBy = cityRecord.LastModifiedBy ?? "";
            city.Lat = cityRecord.Lat;
            city.Lng = cityRecord.Lng;
            city.StateId = cityRecord.StateId;
            city.ZoneId = cityRecord.ZoneId;
            city.Address = cityRecord.Address;
            return city;
        }

        #endregion
    }
}
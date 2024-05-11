using LegendCR.CommonDefinitions.DTO;
using LegendCR.CommonDefinitions.Requests;
using LegendCR.CommonDefinitions.Responses;
using LegendCR.DAL.DB;
using LegendCR.Helpers;
using System.Net;

namespace LegendCR.BL.Services
{
    public class ZoneService : BaseService
    {
        public static ZoneResponse GetZones(ZoneRequest request)
        {

            var res = new ZoneResponse();
            RunBase(request, res, (ZoneRequest req) =>
            {
                try
                {
                    var query = request.context.Zone.Where(z => !z.IsDeleted).Select(p => new ZoneDTO
                    {

                        Id = p.Id,
                        BranchId = p.BranchId,
                        NameEn = p.NameEn,
                        CreatedAt = p.CreatedAt,
                        CreatedBy = p.CreatedBy,
                        Description = p.Description,
                        IsDeleted = p.IsDeleted,
                        LastModifiedAt = p.LastModifiedAt,
                        LastModifiedBy = p.LastModifiedBy,
                        NameAr = p.NameAr,
                    });

                    if (request.ZoneDTO != null)
                        query = ApplyFilter(query, request.ZoneDTO);

                    //query = OrderByDynamic(query, request.OrderByColumn ?? "Id", request.IsDesc);

                    //if (request.PageSize > 0)
                    //    query = ApplyPaging(query, request.PageSize, request.PageIndex);

                    res.ZoneDTOs = query.ToList();

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

        public static ZoneResponse DeleteZone(ZoneRequest request)
        {
            var res = new ZoneResponse();
            RunBase(request, res, (ZoneRequest req) =>
            {
                try
                {
                    var Zone = request.context.Zone.FirstOrDefault(c => c.Id == request.ZoneDTO.Id);
                    if (Zone != null)
                    {
                        //update Agency IsDeleted
                        Zone.IsDeleted = true;
                        Zone.LastModifiedAt = DateTime.Now;
                        Zone.LastModifiedBy = request.UserID;
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

        public static ZoneResponse EditZone(ZoneRequest request)
        {
            var res = new ZoneResponse();
            RunBase(request, res, (ZoneRequest req) =>
            {
                try
                {
                    var model = request.ZoneDTO;
                    var Zone = request.context.Zone.Find(model.Id);
                    if (Zone != null)
                    {
                        //update whole Agency
                        Zone = AddOrEditZone(request.UserID, request.ZoneDTO, Zone);

                        request.context.ZoneTax.FirstOrDefault(x => x.ZoneId == request.ZoneDTO.Id).Tax = request.ZoneDTO.Tax;

                        var AreaIncluded = request.context.City.Where(c => c.ZoneId == Zone.Id).ToList();

                        //Reset All Areas Of Zone 
                        foreach (var area in AreaIncluded)
                            area.ZoneId = null;

                        //Assign zone For Areas
                        City Area = new City();
                        foreach (var area in request.ZoneDTO.AreaList)
                        {
                            Area = request.context.City.FirstOrDefault(a => a.Id == area);
                            if (Area != null)
                            {
                                Area.ZoneId = Zone.Id;
                                request.context.SaveChanges();
                            }
                        }

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

        public static ZoneResponse AddZone(ZoneRequest request)
        {
            var res = new ZoneResponse();
            RunBase(request, res, (ZoneRequest req) =>
            {
                try
                {
                    var Zone = AddOrEditZone(request.UserID, request.ZoneDTO);

                    request.context.Zone.Add(Zone);

                    request.context.SaveChanges();

                    //Assign For Area
                    City Area = new City();
                    foreach (var area in request.ZoneDTO.AreaList)
                    {
                        Area = request.context.City.FirstOrDefault(a => a.Id == area);
                        if (Area != null)
                        {
                            Area.ZoneId = Zone.Id;
                            request.context.SaveChanges();
                        }
                    }

                    //Add Zone Tax
                    var ZoneTax = new ZoneTaxDTO
                    {
                        CreatedAt = DateTime.Now,
                        IsDeleted = false,
                        LastModifiedAt = DateTime.Now,
                        LastModifiedBy = request.UserID,
                        CreatedBy = request.UserID,
                        ZoneId = Zone.Id,
                        Tax = request.ZoneDTO.Tax
                    };
                    ZoneTaxService.Add(ZoneTax, request.context);

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

        public static Zone AddOrEditZone(int CreatedBy, ZoneDTO zoneDTO, Zone Zone = null)
        {
            if (Zone == null)
            {
                Zone = new Zone();
                Zone.CreatedAt = DateTime.Now;
                Zone.CreatedBy = CreatedBy;
            }
            Zone.BranchId = zoneDTO.BranchId;
            Zone.NameEn = zoneDTO.NameEn;
            Zone.Description = zoneDTO.Description;
            Zone.IsDeleted = zoneDTO.IsDeleted;
            Zone.LastModifiedAt = DateTime.Now;
            Zone.LastModifiedBy = CreatedBy;
            Zone.NameAr = zoneDTO.NameAr;
            return Zone;
        }

        private static IQueryable<ZoneDTO> ApplyFilter(IQueryable<ZoneDTO> query, ZoneDTO record)
        {
            if (!string.IsNullOrEmpty(record.Search))
            {
                query = query.Where
                    (c => (!string.IsNullOrEmpty(c.NameAr) && c.NameAr.Contains(record.Search))
                        || (!string.IsNullOrEmpty(c.NameEn) && c.NameEn.Contains(record.Search)));
            }

            if (record.Id > 0)
            {
                query = query.Where(p => p.Id == record.Id);
            }

            return query;
        }
    }
}
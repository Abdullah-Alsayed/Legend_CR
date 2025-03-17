using System;
using System.Linq;
using System.Net;
using DicomApp.CommonDefinitions.DTO;
using DicomApp.CommonDefinitions.DTO.AdvertisementDTOs;
using DicomApp.CommonDefinitions.Requests;
using DicomApp.CommonDefinitions.Responses;
using DicomApp.Helpers;

namespace DicomApp.BL.Services
{
    public class CountryService : BaseService
    {
        public static CountryResponse GetCountries(CountryRequest request)
        {
            var res = new CountryResponse();
            RunBase(
                request,
                res,
                (CountryRequest req) =>
                {
                    try
                    {
                        var query = request.context.Countries.Select(p => new CountryDTO
                        {
                            CountryCode = p.CountryCode,
                            CountryId = p.CountryId,
                            FlagUrl = p.FlagUrl,
                            NameAr = p.NameAr,
                            NameEn = p.NameEn
                        });

                        if (request.CountryDTO != null)
                            query = ApplyFilter(query, request.CountryDTO);

                        res.TotalCount = query.Count();

                        query = OrderByDynamic(
                            query,
                            request.OrderByColumn ?? "CountryId",
                            request.IsDesc
                        );

                        if (request.PageSize > 0)
                            query = ApplyPaging(query, request.PageSize, request.PageIndex);

                        res.CountryDTOs = query.ToList();
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

        public static IQueryable<CountryDTO> ApplyFilter(
            IQueryable<CountryDTO> query,
            CountryDTO CountryRecord
        )
        {
            if (!string.IsNullOrEmpty(CountryRecord.Search))
            {
                query = query.Where(c =>
                    (!string.IsNullOrEmpty(c.NameEn) && c.NameEn.Contains(CountryRecord.Search))
                    || (!string.IsNullOrEmpty(c.NameAr) && c.NameAr.Contains(CountryRecord.Search))
                );
            }

            return query;
        }
    }
}

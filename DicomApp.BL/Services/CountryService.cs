using System;
using System.Linq;
using System.Net;
using DicomApp.CommonDefinitions.DTO;
using DicomApp.CommonDefinitions.Requests;
using DicomApp.CommonDefinitions.Responses;
using DicomApp.DAL.DB;
using DicomApp.Helpers;

namespace DicomApp.BL.Services
{
    public class CountryService : BaseService
    {
        #region CountryServices
        public static CountryResponse GetCountry(CountryRequest request)
        {
            var res = new CountryResponse();
            RunBase(
                request,
                res,
                (CountryRequest req) =>
                {
                    try
                    {
                        var query = request.context.Countries.Select(c => new CountryDTO
                        {
                            CountryId = c.CountryId,
                            CountryCode = c.CountryCode,
                            FlagUrl = c.FlagUrl,
                            NameAr = c.NameAr,
                            NameEn = c.NameEn,
                        });

                        if (request.CountryDTO != null)
                            query = ApplyFilter(query, request.CountryDTO);

                        query = OrderByDynamic(
                            query,
                            request.OrderByColumn ?? nameof(Country.CountryCode),
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

        #endregion
    }
}

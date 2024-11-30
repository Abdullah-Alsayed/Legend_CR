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

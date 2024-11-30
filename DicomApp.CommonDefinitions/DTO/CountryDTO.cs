using System;
using System.Collections.Generic;
using System.Text;

namespace DicomApp.CommonDefinitions.DTO
{
    public class CountryDTO
    {
        public int CountryId { get; set; }
        public string NameEn { get; set; }
        public string NameAr { get; set; }
        public string FlagUrl { get; set; }
        public string CountryCode { get; set; }

        //Filter
        public string Search { get; set; }
    }
}

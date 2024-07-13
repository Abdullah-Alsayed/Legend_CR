using System;
using System.Collections.Generic;
using System.Text;

namespace DicomApp.Helpers.Services.GetCounter
{
    public class CountryResponse
    {
        public string ip { get; set; } = string.Empty;
        public string country_code2 { get; set; } = string.Empty;
        public string country_name { get; set; } = string.Empty;
    }
}

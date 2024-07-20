using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DicomApp.Helpers.Services.GetCounter
{
    public interface IApiCountryService
    {
        Task<CountryResponse> GetCountryInfoAsync(string ipAddress);
    }
}

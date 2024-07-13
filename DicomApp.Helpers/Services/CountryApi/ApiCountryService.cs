using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace DicomApp.Helpers.Services.GetCounter
{
    public class ApiCountryService : IApiCountryService
    {
        private readonly string _apiKey;

        public ApiCountryService(IConfiguration configuration) =>
            _apiKey = configuration["CountryApiKey"];

        public async Task<CountryResponse> GetCountryInfoAsync(string ipAddress)
        {
            string url =
                $"https://api.ipgeolocation.io/ipgeo?apiKey={_apiKey}&ip={ipAddress}&fields=country_code2,country_name";

            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var apiResponse = JsonSerializer.Deserialize<CountryResponse>(responseBody);
                    return apiResponse;
                }
                else
                {
                    Console.WriteLine("ipAddress Not Valid");
                    return new CountryResponse { };
                }
            }
        }
    }
}

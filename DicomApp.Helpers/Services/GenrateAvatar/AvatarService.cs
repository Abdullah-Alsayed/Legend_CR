using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;

namespace DicomApp.Helpers.Services.GenrateAvatar
{
    public class AvatarService
    {
        private readonly HttpClient _httpClient;

        public AvatarService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IFormFile> GetAvatarAsFormFileAsync(string name, int size = 64)
        {
            try
            {
                var random = new Random();
                var backgrounds = new List<string>
                {
                    "#FBA834",
                    "#333A73",
                    "#387ADF",
                    "#50C4ED",
                    "41B06E",
                    "41B06E"
                };
                var background = backgrounds[random.Next(0, backgrounds.Count - 1)];
                var url =
                    $"https://ui-avatars.com/api/?name={Uri.EscapeDataString(name)}&size={size}&rounded=true&background={background}&bold=true";

                using (var response = await _httpClient.GetAsync(url))
                {
                    response.EnsureSuccessStatusCode();

                    var byteArray = await response.Content.ReadAsByteArrayAsync();
                    var stream = new MemoryStream(byteArray);

                    var formFile = new FormFile(stream, 0, stream.Length, "avatar", "avatar.png")
                    {
                        Headers = new HeaderDictionary(),
                        ContentType = "image/png"
                    };
                    return formFile;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
    }
}

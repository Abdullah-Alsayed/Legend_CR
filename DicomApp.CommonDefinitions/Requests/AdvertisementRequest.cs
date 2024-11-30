using DicomApp.CommonDefinitions.DTO.AdvertisementDTOs;
using Microsoft.Extensions.Localization;

namespace DicomApp.CommonDefinitions.Requests
{
    public class AdvertisementRequest : BaseRequest
    {
        public string RoutPath { get; set; }
        public int Top { get; set; }

        public AdsDTO AdsDTO { get; set; } = new AdsDTO();
        public IStringLocalizer Localizer { get; set; }
    }
}

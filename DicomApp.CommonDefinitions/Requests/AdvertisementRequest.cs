using System.Collections.Generic;
using DicomApp.CommonDefinitions.DTO.AdvertisementDTOs;
using DicomApp.CommonDefinitions.DTO.AdvertisementDTOs;
using Microsoft.AspNetCore.Http;

namespace DicomApp.CommonDefinitions.Requests
{
    public class AdvertisementRequest : BaseRequest
    {
        public string RoutPath { get; set; }
        public AdsDTO AdsDTO { get; set; } = new AdsDTO();
    }
}

using System.Collections.Generic;
using DicomApp.CommonDefinitions.DTO;
using DicomApp.CommonDefinitions.DTO.AdvertisementDTOs;
using DicomApp.CommonDefinitions.DTO.AdvertisementDTOs;

namespace DicomApp.CommonDefinitions.Responses
{
    public class AdvertisementResponse : BaseResponse
    {
        public AdsDTO AdsDTO { get; set; }
        public List<AdsDTO> AdsDTOs { get; set; }
        public List<VendorReportDTO> VendorReportDTOs { get; set; }
        public List<ShipItemDTO> ShipItemDTOs { get; set; }
        public ShippingCalculatorDTO ShippingCalculatorDTO { get; set; }
        public List<SelectOptionDTO> SelectOption { get; set; }
    }
}

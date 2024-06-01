
using DicomApp.CommonDefinitions.DTO.ShipmentDTOs;
using System.Collections.Generic;

namespace DicomApp.CommonDefinitions.Responses
{
    public class ShipmentResponse : BaseResponse
    {
        public ShipDTO ShipDTO { get; set; }
        public List<ShipDTO> ShipDTOs { get; set; }
        public List<VendorReportDTO> VendorReportDTOs { get; set; }
        public List<ShipItemDTO> ShipItemDTOs { get; set; }
        public ShippingCalculatorDTO ShippingCalculatorDTO { get; set; }
    }
}
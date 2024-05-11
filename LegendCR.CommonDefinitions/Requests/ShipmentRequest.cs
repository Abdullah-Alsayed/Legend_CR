using LegendCR.CommonDefinitions.DTO.ShipmentDTOs;

namespace LegendCR.CommonDefinitions.Requests
{
    public class ShipmentRequest : BaseRequest
    {
        
        public ShipItemDTO ShipItemDTO { get; set; }
        public ShipDTO ShipDTO { get; set; }

        public ShipDTO_Filter ShipDTO_Filter { get; set; }
        public ShippingCalculatorDTO ShippingCalculatorDTO { get; set; }

        public bool HasSettingDTO { get; set; }
        public bool HasCustomerDetailsDTO { get; set; }
        public bool HasVendorDetailsDTO { get; set; }
        public bool HasFeesDetailsDTO { get; set; }

        // Sub Tables
        public bool HasCustomerFollowUpDTO { get; set; }
        public bool HasProblemDTOs { get; set; }
        public bool HasFollowUpDTOs { get; set; }
        public bool HasShipItemDTOs { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace DicomApp.CommonDefinitions.DTO.ShipmentDTOs
{
    public class FeesDetailsDTO
    {
        public double ShippingFeesPaid { get; set; }
        public bool? IsAfford { get; set; }
        public double GameFees { get; set; }
        public double ShippingFees { get; set; }
        public double VendorCash { get; set; }
        public double Total { get; set; }

        public double CancelFees { get; set; }
        public double WeightFees { get; set; }
        public double SizeFees { get; set; }
        public double PartialDeliveryFees { get; set; }
        public double TotalReceived { get; set; }
        public double TotalRedFees { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace DicomApp.CommonDefinitions.DTO.AdvertisementDTOs
{
    public class ShippingCalculatorDTO
    {
        public int SourceAreaId { get; set; }
        public int DestinationAreaId { get; set; }
        public int SourceZoneId { get; set; }
        public int DestinationZoneId { get; set; }
        public int ShipmentServiceId { get; set; }

        public int? GameId { get; set; }
        public int? CategoryId { get; set; }

        public int Weight { get; set; }
        public string Size { get; set; }
        public bool IsPartialDelivery { get; set; }
        public bool IsStock { get; set; }
        public bool IsOpenPackage { get; set; }
        public bool IsFragilePackage { get; set; }

        public double SourceShippingFees { get; set; }
        public double DestinationShippingFees { get; set; }
        public double GameFees { get; set; }
        public double WeightFees { get; set; }
        public double SizeFees { get; set; }
        public double PartialDeliveryFees { get; set; }
        public double CancelFees { get; set; }
        public double TotalFees { get; set; }
    }
}

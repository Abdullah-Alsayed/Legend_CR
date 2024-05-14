using System;
using System.Collections.Generic;
using System.Text;

namespace LegendCR.CommonDefinitions.DTO.ShipmentDTOs
{
    public class VendorDetailsDTO
    {
        public string VendorId { get; set; }
        public string VendorName { get; set; }
        public string VendorPhone { get; set; }
        public string VendorAddress { get; set; }
        public DateTime? PaidToVendorAt { get; set; }
        public bool IsPaidToVendor { get; set; }
    }
}

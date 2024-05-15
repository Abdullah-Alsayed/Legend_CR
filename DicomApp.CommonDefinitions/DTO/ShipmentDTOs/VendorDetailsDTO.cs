using System;
using System.Collections.Generic;
using System.Text;

namespace DicomApp.CommonDefinitions.DTO.ShipmentDTOs
{
    public class VendorDetailsDTO
    {
        public int VendorId { get; set; }
        public string VendorName { get; set; }
        public string VendorPhone { get; set; }
        public string VendorAddress { get; set; }
        public DateTime? PaidToVendorAt { get; set; }
        public bool IsPaidToVendor { get; set; }

    }
}
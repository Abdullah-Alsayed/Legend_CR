using System;
using System.Collections.Generic;
using System.Text;

namespace DicomApp.CommonDefinitions.DTO.ShipmentDTOs
{
    public class CustomerDetailsDTO
    {
        public int? CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerPhone2 { get; set; }
        public string CustomerAddress { get; set; }

        public int Floor { get; set; }
        public int Apartment { get; set; }
        public string Landmark { get; set; }
        public string Location { get; set; }
    }
}

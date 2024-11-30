using System;
using System.Collections.Generic;
using System.Text;
using DicomApp.DAL.DB;

namespace DicomApp.CommonDefinitions.Records
{
    public class CityDTO
    {
        public int Id { get; set; }
        public string CityName { get; set; }
        public string CityNameAr { get; set; }
        public int StateId { get; set; }
        public string CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime LastModifiedAt { get; set; }
        public string Lat { get; set; }
        public string Lng { get; set; }
        public string Address { get; set; }
        public int? ZoneId { get; set; }
        public string ZoneNameEn { get; set; }
        public string ZoneNameAr { get; set; }
        public double ShippingFees { get; set; }
        public string Search { get; set; }
        public bool? IsDeleted { get; set; }
    }
}

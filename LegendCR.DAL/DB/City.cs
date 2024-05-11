
using System;
using System.Collections.Generic;

namespace LegendCR.DAL.DB
{
    public partial class City
    {
        public City()
        {

            //Shipment = new HashSet<Shipment>();
        }

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
        public int? BranchId { get; set; }
        public bool? IsDeleted { get; set; }


       // public virtual ICollection<Shipment> Shipment { get; set; }
    }
}

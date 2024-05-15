
using System;
using System.Collections.Generic;

namespace DicomApp.DAL.DB
{
    public partial class City
    {
        public City()
        {
            CashTransfer = new HashSet<CashTransfer>();
            PickupRequest = new HashSet<PickupRequest>();
            Shipment = new HashSet<Shipment>();
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

        public virtual Branch Branch { get; set; }
        public virtual Zone Zone { get; set; }
        public virtual ICollection<CashTransfer> CashTransfer { get; set; }
        public virtual ICollection<PickupRequest> PickupRequest { get; set; }
        public virtual ICollection<Shipment> Shipment { get; set; }
    }
}

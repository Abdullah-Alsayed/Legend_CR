
using System;
using System.Collections.Generic;

namespace DicomApp.DAL.DB
{
    public partial class Zone
    {
        public Zone()
        {
            CashTransfer = new HashSet<CashTransfer>();
            City = new HashSet<City>();
            PickupRequest = new HashSet<PickupRequest>();
            Shipment = new HashSet<Shipment>();
            ZoneTax = new HashSet<ZoneTax>();
        }

        public int Id { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public int LastModifiedBy { get; set; }
        public DateTime LastModifiedAt { get; set; }
        public bool IsDeleted { get; set; }
        public string Description { get; set; }
        public int? BranchId { get; set; }

        public virtual Branch Branch { get; set; }
        public virtual CommonUser CreatedByNavigation { get; set; }
        public virtual CommonUser LastModifiedByNavigation { get; set; }
        public virtual ICollection<CashTransfer> CashTransfer { get; set; }
        public virtual ICollection<City> City { get; set; }
        public virtual ICollection<PickupRequest> PickupRequest { get; set; }
        public virtual ICollection<Shipment> Shipment { get; set; }
        public virtual ICollection<ZoneTax> ZoneTax { get; set; }
    }
}

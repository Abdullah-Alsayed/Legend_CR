using System;
using System.Collections.Generic;

namespace DicomApp.DAL.DB
{
    public partial class Branch
    {
        public Branch()
        {
            CityNavigation = new HashSet<City>();
            CommonUser = new HashSet<CommonUser>();
            PickupRequest = new HashSet<PickupRequest>();
            Shipment = new HashSet<Shipment>();
            Zone = new HashSet<Zone>();
        }

        public int BranchId { get; set; }
        public string Address { get; set; }
        public string BranchName { get; set; }
        public string City { get; set; }
        public string ContactPerson { get; set; }
        public int CurrencyId { get; set; }
        public string Description { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public bool? IsDeleted { get; set; }

        public virtual ICollection<City> CityNavigation { get; set; }
        public virtual ICollection<CommonUser> CommonUser { get; set; }
        public virtual ICollection<PickupRequest> PickupRequest { get; set; }
        public virtual ICollection<Shipment> Shipment { get; set; }
        public virtual ICollection<Zone> Zone { get; set; }
    }
}

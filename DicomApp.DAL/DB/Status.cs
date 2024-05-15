
using System;
using System.Collections.Generic;

namespace DicomApp.DAL.DB
{
    public partial class Status
    {
        public Status()
        {
            AccountTransaction = new HashSet<AccountTransaction>();
            PickupRequest = new HashSet<PickupRequest>();
            PickupRequestItem = new HashSet<PickupRequestItem>();
            Shipment = new HashSet<Shipment>();
            ShipmentItem = new HashSet<ShipmentItem>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<AccountTransaction> AccountTransaction { get; set; }
        public virtual ICollection<PickupRequest> PickupRequest { get; set; }
        public virtual ICollection<PickupRequestItem> PickupRequestItem { get; set; }
        public virtual ICollection<Shipment> Shipment { get; set; }
        public virtual ICollection<ShipmentItem> ShipmentItem { get; set; }
    }
}

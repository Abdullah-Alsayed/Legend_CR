
using System;
using System.Collections.Generic;

namespace DicomApp.DAL.DB
{
    public partial class PickupRequestItem
    {
        public int PickupRequestItemId { get; set; }
        public int PickupRequestId { get; set; }
        public int? VendorProductId { get; set; }
        public int? ShipmentId { get; set; }
        public int? Quantity { get; set; }
        public double? Price { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CreatedBy { get; set; }
        public bool IsDeleted { get; set; }
        public int StatusId { get; set; }

        public virtual PickupRequest PickupRequest { get; set; }
        public virtual Shipment Shipment { get; set; }
        public virtual Status Status { get; set; }
        public virtual VendorProduct VendorProduct { get; set; }
    }
}


using System;
using System.Collections.Generic;

namespace DicomApp.DAL.DB
{
    public partial class ShipmentItem
    {
        public int ShipmentItemId { get; set; }
        public int ShipmentId { get; set; }
        public int? VendorProductId { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public string Size { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public int CourierQuantityDelivered { get; set; }
        public int CourierQuantityReturned { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CreatedBy { get; set; }
        public bool IsDeleted { get; set; }
        public int StatusId { get; set; }

        public virtual Shipment Shipment { get; set; }
        public virtual Status Status { get; set; }
        public virtual VendorProduct VendorProduct { get; set; }
    }
}

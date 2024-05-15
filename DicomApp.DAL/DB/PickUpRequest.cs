
using System;
using System.Collections.Generic;

namespace DicomApp.DAL.DB
{
    public partial class PickupRequest
    {
        public PickupRequest()
        {
            AccountTransaction = new HashSet<AccountTransaction>();
            PickupRequestItem = new HashSet<PickupRequestItem>();
            Shipment = new HashSet<Shipment>();
        }

        public int PickupRequestId { get; set; }
        public int PickupRequestTypeId { get; set; }
        public double PickupFees { get; set; }
        public int StatusId { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CreatedBy { get; set; }
        public bool IsDeleted { get; set; }
        public int VendorId { get; set; }
        public string VendorName { get; set; }
        public string VendorPhone { get; set; }
        public string VendorAddress { get; set; }
        public string VendorLocation { get; set; }
        public string VendorLandmark { get; set; }
        public int VendorFloor { get; set; }
        public int? VendorApartment { get; set; }
        public int ZoneId { get; set; }
        public int AreaId { get; set; }
        public DateTime PickupDate { get; set; }
        public DateTime TimeFrom { get; set; }
        public DateTime TimeTo { get; set; }
        public string RefId { get; set; }
        public int? DeliveryManId { get; set; }
        public string Notes { get; set; }
        public int? BranchId { get; set; }

        public virtual City Area { get; set; }
        public virtual Branch Branch { get; set; }
        public virtual CommonUser DeliveryMan { get; set; }
        public virtual PickupRequestType PickupRequestType { get; set; }
        public virtual Status Status { get; set; }
        public virtual CommonUser Vendor { get; set; }
        public virtual Zone Zone { get; set; }
        public virtual ICollection<AccountTransaction> AccountTransaction { get; set; }
        public virtual ICollection<PickupRequestItem> PickupRequestItem { get; set; }
        public virtual ICollection<Shipment> Shipment { get; set; }
    }
}

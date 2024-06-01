using System;
using System.Collections.Generic;

namespace DicomApp.DAL.DB
{
    public partial class Shipment
    {
        public Shipment()
        {
            AccountTransaction = new HashSet<AccountTransaction>();
            FollowUp = new HashSet<FollowUp>();
            PickupRequestItem = new HashSet<PickupRequestItem>();
            ShipmentCustomerFollowUp = new HashSet<ShipmentCustomerFollowUp>();
            ShipmentItem = new HashSet<ShipmentItem>();
            ShipmentProblem = new HashSet<ShipmentProblem>();
        }

        public int ShipmentId { get; set; }
        public string RefId { get; set; }
        public int ShipmentTypeId { get; set; }
        public int ShipmentServiceId { get; set; }
        public int StatusId { get; set; }
        public int BranchId { get; set; }
        public int VendorId { get; set; }
        public string VendorName { get; set; }
        public string VendorPhone { get; set; }
        public int? DeliveryManId { get; set; }
        public int? CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerPhone2 { get; set; }
        public string CustomerAddress { get; set; }
        public int AreaId { get; set; }
        public int ZoneId { get; set; }
        public int Floor { get; set; }
        public int Apartment { get; set; }
        public string Landmark { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public int NoOfItems { get; set; }
        public int? PickupRequestId { get; set; }
        public bool IsOpenPackage { get; set; }
        public bool IsFragilePackage { get; set; }
        public bool IsStock { get; set; }
        public bool? IsAfford { get; set; }
        public int? GameId { get; set; }
        public int? WarehouseGameId { get; set; }
        public double GameFees { get; set; }
        public int Weight { get; set; }
        public int WarehouseWeight { get; set; }
        public double WeightFees { get; set; }
        public string Size { get; set; }
        public string WarehouseSize { get; set; }
        public double SizeFees { get; set; }
        public bool IsPartialDelivery { get; set; }
        public double PartialDeliveryFees { get; set; }
        public double ShippingFees { get; set; }
        public double ShippingFeesPaid { get; set; }
        public double VendorCash { get; set; }
        public double Total { get; set; }
        public double CancelFees { get; set; }
        public string CancelComment { get; set; }
        public int ReturnCount { get; set; }
        public int CallAnswerCount { get; set; }
        public int CallNotAnswerCount { get; set; }
        public bool IsTripCompleted { get; set; }
        public bool IsCashReceived { get; set; }
        public bool IsPaidToVendor { get; set; }
        public DateTime? PaidToVendorAt { get; set; }
        public int? CashTransferId { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CreatedBy { get; set; }
        public DateTime LastModifiedAt { get; set; }
        public int LastModifiedBy { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsCourierReturned { get; set; }
        public int? ShipToRefundId { get; set; }
        public double? RefundCash { get; set; }
        public double? RefundFees { get; set; }

        public virtual City Area { get; set; }
        public virtual Branch Branch { get; set; }
        public virtual CashTransfer CashTransfer { get; set; }
        public virtual CommonUser Customer { get; set; }
        public virtual CommonUser DeliveryMan { get; set; }
        public virtual CommonUser LastModifiedByNavigation { get; set; }
        public virtual Game Game { get; set; }
        public virtual PickupRequest PickupRequest { get; set; }
        public virtual ShipmentService ShipmentService { get; set; }
        public virtual ShipmentType ShipmentType { get; set; }
        public virtual Status Status { get; set; }
        public virtual CommonUser Vendor { get; set; }
        public virtual Game WarehouseGame { get; set; }
        public virtual Zone Zone { get; set; }
        public virtual ICollection<AccountTransaction> AccountTransaction { get; set; }
        public virtual ICollection<FollowUp> FollowUp { get; set; }
        public virtual ICollection<PickupRequestItem> PickupRequestItem { get; set; }
        public virtual ICollection<ShipmentCustomerFollowUp> ShipmentCustomerFollowUp { get; set; }
        public virtual ICollection<ShipmentItem> ShipmentItem { get; set; }
        public virtual ICollection<ShipmentProblem> ShipmentProblem { get; set; }
    }
}

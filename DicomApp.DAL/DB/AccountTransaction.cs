using System;
using System.Collections.Generic;

namespace DicomApp.DAL.DB
{
    public partial class AccountTransaction
    {
        public int AccountTransactionId { get; set; }
        public string RefId { get; set; }
        public byte TypeId { get; set; }
        public int? SenderId { get; set; }
        public int ReceiverId { get; set; }
        public int? VendorId { get; set; }
        public int? StatusId { get; set; }
        public int? ShipmentId { get; set; }
        public int? PickupRequestId { get; set; }
        public double GameFees { get; set; }
        public double WeightFees { get; set; }
        public double SizeFees { get; set; }
        public double PartialDeliveryFees { get; set; }
        public double CancelFees { get; set; }
        public double PickupFees { get; set; }
        public double ShippingFees { get; set; }
        public double? ShippingFeesPaid { get; set; }
        public double VendorCash { get; set; }
        public double Total { get; set; }
        public int? CashTransferId { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CreatedBy { get; set; }
        public int LastModifiedBy { get; set; }
        public DateTime LastModifiedAt { get; set; }
        public bool IsDeleted { get; set; }
        public double? RefundCash { get; set; }
        public double? RefundFees { get; set; }

        public virtual CashTransfer CashTransfer { get; set; }
        public virtual CommonUser CreatedByNavigation { get; set; }
        public virtual CommonUser LastModifiedByNavigation { get; set; }
        public virtual PickupRequest PickupRequest { get; set; }
        public virtual Account Receiver { get; set; }
        public virtual Account Sender { get; set; }
        public virtual Shipment Shipment { get; set; }
        public virtual Status Status { get; set; }
        public virtual Account Vendor { get; set; }
    }
}

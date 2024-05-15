using System;
using System.Collections.Generic;

namespace DicomApp.CommonDefinitions.DTO.CashDTOs
{
    public class AccountTransactionDTO
    {
        public int AccountTransactionId { get; set; }
        public string RefId { get; set; }
        public string ShipRefId { get; set; }
        public string PickupRefId { get; set; }

        public string CashTransferRefId { get; set; }
        public byte TypeId { get; set; }
        public int? SenderId { get; set; }
        public int ReceiverId { get; set; }
        public int? VendorId { get; set; }
        public string VendorName { get; set; }
        public int? StatusId { get; set; }
        public string StatusName { get; set; }
        public int? ShipmentId { get; set; }
        public int? PickupRequestId { get; set; }
        public double PackingFees { get; set; }
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
        public double? RefundCash { get; set; }
        public double? RefundFees { get; set; }

        public DateTime CreatedAt { get; set; }
        public int CreatedBy { get; set; }
        public int LastModifiedBy { get; set; }
        public DateTime LastModifiedAt { get; set; }
        public bool IsDeleted { get; set; }

        public StatusDTO StatusDTO { get; set; }
    }
}
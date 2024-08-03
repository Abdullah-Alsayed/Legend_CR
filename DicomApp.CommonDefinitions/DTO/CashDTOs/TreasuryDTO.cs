using System;
using System.Collections.Generic;
using System.Text;

namespace DicomApp.CommonDefinitions.DTO.CashDTOs
{
    public class TreasuryDTO
    {
        public int AccountId { get; set; }
        public int? VendorId { get; set; }
        public string VendorName { get; set; }
        public double GameFees { get; set; }
        public double WeightFees { get; set; }
        public double SizeFees { get; set; }
        public double PartialDeliveryFees { get; set; }
        public double CancelFees { get; set; }

        public double PickupFees { get; set; }
        public double ShippingFees { get; set; }
        public double ShippingFeesPaid { get; set; }
        public double VendorCash { get; set; }
        public double Total { get; set; }
        public double Balance { get; set; }
        public double Withdraw { get; set; }
        public double Deposit { get; set; }
        public double Deductions { get; set; }
        public double RefundCash { get; set; }
        public double RefundFees { get; set; }
        public double TotalDelivered { get; set; }
        public double TotalOthers { get; set; }
        public double TotalRefunded { get; set; }
        public IEnumerable<TransactionDTO> TransactionDTOs { get; set; }
    }
}

using System;
using System.Text;
using System.Collections.Generic;

namespace DicomApp.CommonDefinitions.DTO.CashDTOs
{
    public class InvoiceDTO
    {
        public double ShippingFees { get; set; }
        public double PackingFees { get; set; }
        public double WeightFees { get; set; }
        public double SizeFees { get; set; }
        public double PartialDeliveryFees { get; set; }
        public double PickupFees { get; set; }
        public double REDFees { get; set; }
        public double VendorCash { get; set; }
        public double VendorBalance { get; set; }

        public double CancelFees { get; set; }
        public double Paid { get; set; }
        public double Withdraw { get; set; }
        public double Deposit { get; set; }
        public double TreasuryBalance { get; set; }
        public double Total { get; set; }
                
        public IEnumerable<CashTransferDTO> CashTransferDTOs { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Text;

namespace LegendCR.CommonDefinitions.DTO.ShipmentDTOs
{
    public class VendorReportDTO
    {
        public int VendorId { get; set; }
        public string VendorName { get; set; }

        public int TotalShipsCount { get; set; }
        public int CurrentShipsCount { get; set; }
        public int DoneShipsCount { get; set; }
        public int CancelledShipsCount { get; set; }
        public int SuccessPerc { get; set; }


        public double REDFees { get; set; }
        public double Total { get; set; }
        public double Balance { get; set; }
        public double TotalDelivered { get; set; }
        public double TotalOthers { get; set; }
        public double TotalRefunded { get; set; }

        public double ShippingFees { get; set; }
        public double PackingFees { get; set; }
        public double WeightFees { get; set; }
        public double SizeFees { get; set; }
        public double PartialDeliveryFees { get; set; }
        public double CancelFees { get; set; }
        public double PickupFees { get; set; }
        public double VendorCash { get; set; }

        public IEnumerable<ShipDTO> lstShips { get; set; }
    }
}

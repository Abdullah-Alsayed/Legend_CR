
using System;
using System.Collections.Generic;

namespace DicomApp.DAL.DB
{
    public partial class ShipmentCustomerFollowUp
    {
        public int ShipmentCustomerFollowUpId { get; set; }
        public int ShipmentId { get; set; }
        public string Note { get; set; }
        public bool? IsConfirmed { get; set; }
        public bool? IsCallAnswered { get; set; }
        public DateTime? NextCallOn { get; set; }
        public DateTime? NextCallTimeFrom { get; set; }
        public DateTime? NextCallTimeTo { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CreatedBy { get; set; }

        public virtual CommonUser CreatedByNavigation { get; set; }
        public virtual Shipment Shipment { get; set; }
    }
}


using System;
using System.Collections.Generic;

namespace DicomApp.DAL.DB
{
    public partial class ShipmentProblem
    {
        public int ShipmentProblemId { get; set; }
        public int ShipmentId { get; set; }
        public int ProblemTypeId { get; set; }
        public bool IsSolved { get; set; }
        public string Solution { get; set; }
        public string Note { get; set; }
        public bool IsCourierProblem { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CreatedBy { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsReportToVendor { get; set; }

        public virtual CommonUser CreatedByNavigation { get; set; }
        public virtual ProblemType ProblemType { get; set; }
        public virtual Shipment Shipment { get; set; }
    }
}

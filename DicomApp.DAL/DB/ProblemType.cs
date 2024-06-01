
using System;
using System.Collections.Generic;

namespace DicomApp.DAL.DB
{
    public partial class ProblemType
    {
        public ProblemType()
        {
            ShipmentProblem = new HashSet<ShipmentProblem>();
        }

        public int ProblemTypeId { get; set; }
        public string NameEn { get; set; }
        public string NameAr { get; set; }
        public int? Type { get; set; }
        public bool? IsDeleted { get; set; }

        public virtual ICollection<ShipmentProblem> ShipmentProblem { get; set; }
    }
}

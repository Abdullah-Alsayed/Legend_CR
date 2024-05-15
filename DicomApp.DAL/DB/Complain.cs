
using System;
using System.Collections.Generic;

namespace DicomApp.DAL.DB
{
    public partial class Complain
    {
        public int Id { get; set; }
        public int? EmployeeId { get; set; }
        public int? VendorId { get; set; }
        public int ActionBy { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public bool? IsSolved { get; set; }
        public string Comments { get; set; }
        public bool? IsDeleted { get; set; }
        public string Department { get; set; }

        public virtual CommonUser ActionByNavigation { get; set; }
        public virtual CommonUser Employee { get; set; }
        public virtual CommonUser Vendor { get; set; }
    }
}

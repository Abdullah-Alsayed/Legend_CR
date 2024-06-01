using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DicomDB.CommonDefinitions.DTO
{
    public class ComplainsDTO
    {
        public int ComplainsId { get; set; }
        public int? EmployeeId { get; set; }
        public string EmployeeName { get; set; }

        public int? VendorId { get; set; }
        public string VendorName { get; set; }

        [ForeignKey("CreateBy")]
        public int ActionBy { get; set; }

        public string CreateBy { get; set; }

        public DateTime Date { get; set; }
        public string Description { get; set; }
        public string Department { get; set; }
        public bool Solved { get; set; }
        public string Comments { get; set; }
        public string Search { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? DateTo { get; set; }
        public DateTime? DateFrom { get; set; }
        public bool? IsSolved { get; set; }
    }

}

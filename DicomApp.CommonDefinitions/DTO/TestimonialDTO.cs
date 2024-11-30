using System;
using System.Collections.Generic;
using System.Text;
using DicomApp.DAL.DB;

namespace DicomApp.CommonDefinitions.DTO
{
    public class TestimonialDTO
    {
        public int TestimonialId { get; set; }
        public string Comment { get; set; }
        public int Rate { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CreatedBy { get; set; }
        public int? LastModifiedBy { get; set; }
        public DateTime LastModifiedAt { get; set; }
        public bool IsDeleted { get; set; }
        public virtual UserDTO UserDTO { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public string Search { get; set; }
    }
}

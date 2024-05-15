
using System;
using System.Collections.Generic;

namespace DicomApp.DAL.DB
{
    public partial class ZoneTax
    {
        public int Id { get; set; }
        public int ZoneId { get; set; }
        public double Tax { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public int LastModifiedBy { get; set; }
        public DateTime LastModifiedAt { get; set; }
        public bool IsDeleted { get; set; }

        public virtual Zone Zone { get; set; }
    }
}

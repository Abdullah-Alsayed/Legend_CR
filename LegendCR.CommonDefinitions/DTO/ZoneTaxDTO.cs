using System;
using System.Collections.Generic;
using System.Text;

namespace LegendCR.CommonDefinitions.DTO
{
    public class ZoneTaxDTO
    {
        public int Id { get; set; }
        public int ZoneId { get; set; }
        public double Tax { get; set; }
        public int CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; }
        public int LastModifiedBy { get; set; }
        public DateTime LastModifiedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
}

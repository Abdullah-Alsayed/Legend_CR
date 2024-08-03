using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DicomApp.DAL.DB
{
    public class Testimonial
    {
        public int TestimonialId { get; set; }
        public string Comment { get; set; }
        public int Rate { get; set; }
        public DateTime CreatedAt { get; set; }

        [ForeignKey(nameof(CreatedByNavigation))]
        public int CreatedBy { get; set; }
        public int? LastModifiedBy { get; set; }
        public DateTime LastModifiedAt { get; set; }
        public bool IsDeleted { get; set; }

        public virtual CommonUser CreatedByNavigation { get; set; }
        public virtual CommonUser LastModifiedByNavigation { get; set; }
        public DateTime? DeletedOn { get; set; }
        public int? DeleteBy { get; set; }
    }
}

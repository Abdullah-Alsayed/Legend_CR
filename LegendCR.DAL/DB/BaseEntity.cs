﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LegendCR.DAL.DB
{
    public class BaseEntity
    {
        public Guid ID { get; set; } = Guid.NewGuid();

        [ForeignKey(nameof(User)), StringLength(450), Required]
        public string CreateBy { get; set; }

        public string ModifyBy { get; set; }

        public string DeletedBy { get; set; }

        [Required]
        public DateTime CreateAt { get; set; } = DateTime.UtcNow;
        public DateTime? ModifyAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;

        public virtual User User { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DicomApp.DAL.DB
{
    public partial class FollowUp
    {
        public int Id { get; set; }
        public int? AdvertisementId { get; set; }
        public int? StatusId { get; set; }
        public string Comment { get; set; }

        [ForeignKey((nameof(CreatedByNavigation)))]
        public int CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public int LastModifiedBy { get; set; }
        public DateTime LastModifiedAt { get; set; }
        public bool IsDeleted { get; set; }
        public string Lat { get; set; }
        public string Lng { get; set; }
        public string Title { get; set; }

        public virtual CommonUser CreatedByNavigation { get; set; }
        public virtual Advertisement Advertisement { get; set; }
    }
}

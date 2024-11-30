using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DicomApp.DAL.DB
{
    public partial class FollowUp
    {
        public int Id { get; set; }
        public int? AdvertisementId { get; set; }
        public string Message { get; set; }
        public EntityTypeEnum EntityType { get; set; }
        public ActionTypeEnum ActionType { get; set; }

        [ForeignKey((nameof(CreatedByNavigation)))]
        public int CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual CommonUser CreatedByNavigation { get; set; }
        public virtual Advertisement Advertisement { get; set; }
    }
}

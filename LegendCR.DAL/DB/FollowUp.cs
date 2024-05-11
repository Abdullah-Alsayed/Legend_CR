
using System;
using System.Collections.Generic;

namespace LegendCR.DAL.DB
{
    public partial class FollowUp
    {
        public int Id { get; set; }
        public int? ShipmentId { get; set; }
        public int? StatusId { get; set; }
        public string Comment { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public int LastModifiedBy { get; set; }
        public DateTime LastModifiedAt { get; set; }
        public bool IsDeleted { get; set; }
        public string Lat { get; set; }
        public string Lng { get; set; }
        public int? FollowUpTypeId { get; set; }
        public string Title { get; set; }

        public virtual CommonUser CreatedByNavigation { get; set; }
        public virtual FollowUpType FollowUpType { get; set; }
        //public virtual Shipment Shipment { get; set; }
    }
}

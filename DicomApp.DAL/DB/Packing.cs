
using System;
using System.Collections.Generic;

namespace DicomApp.DAL.DB
{
    public partial class Packing
    {
        public Packing()
        {
            ShipmentPacking = new HashSet<Shipment>();
            ShipmentWarehousePacking = new HashSet<Shipment>();
        }

        public int Id { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string Size { get; set; }
        public string ImgUrl { get; set; }
        public int Count { get; set; }
        public double Price { get; set; }
        public int PackingTypeId { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? LastModifiedBy { get; set; }
        public DateTime LastModifiedAt { get; set; }
        public int? BranchId { get; set; }
        public string Description { get; set; }

        public virtual Branch Branch { get; set; }
        public virtual PackingType PackingType { get; set; }
        public virtual ICollection<Shipment> ShipmentPacking { get; set; }
        public virtual ICollection<Shipment> ShipmentWarehousePacking { get; set; }
    }
}

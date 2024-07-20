using System;
using System.Collections.Generic;

namespace DicomApp.DAL.DB
{
    public partial class Game
    {
        public Game()
        {
            Advertisements = new HashSet<Advertisement>();
            GamerServices = new HashSet<GamerService>();
            ShipmentWarehouseGame = new HashSet<Advertisement>();
        }

        public int Id { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string ImgUrl { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? LastModifiedBy { get; set; }
        public DateTime? LastModifiedAt { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedOn { get; set; }
        public int? DeleteBy { get; set; }

        public virtual Category Category { get; set; }
        public virtual ICollection<GamerService> GamerServices { get; set; }
        public virtual ICollection<Advertisement> Advertisements { get; set; }
        public virtual ICollection<Advertisement> ShipmentWarehouseGame { get; set; }
    }
}

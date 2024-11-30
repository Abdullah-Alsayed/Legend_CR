using System;
using System.Collections;
using System.Collections.Generic;

namespace DicomApp.DAL.DB
{
    public class GameCharge
    {
        public int Id { get; set; }
        public int GameId { get; set; }
        public int Price { get; set; }
        public int Count { get; set; }
        public int Discount { get; set; }
        public string Img { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? LastModifiedBy { get; set; }
        public DateTime? LastModifiedAt { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedOn { get; set; }
        public int? DeleteBy { get; set; }

        public virtual Game Game { get; set; }
        public virtual ICollection<GamerService> GamerServices { get; set; }
    }
}

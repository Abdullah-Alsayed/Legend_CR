
using System;
using System.Collections.Generic;

namespace DicomApp.DAL.DB
{
    public partial class PackingType
    {
        public PackingType()
        {
            Packing = new HashSet<Packing>();
        }

        public int Id { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? LastModifiedBy { get; set; }
        public DateTime LastModifiedAt { get; set; }
        public int? BranchId { get; set; }

        public virtual Branch Branch { get; set; }
        public virtual ICollection<Packing> Packing { get; set; }
    }
}

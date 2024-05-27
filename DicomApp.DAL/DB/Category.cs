using System;
using System.Collections.Generic;

namespace DicomApp.DAL.DB
{
    public partial class Category
    {
        public Category()
        {
            Game = new HashSet<Game>();
        }

        public int Id { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? LastModifiedBy { get; set; }
        public DateTime LastModifiedAt { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime DeletedOn { get; set; }
        public int DeleteBy { get; set; }

        public virtual ICollection<Game> Game { get; set; }
    }
}


using System;
using System.Collections.Generic;

namespace DicomApp.DAL.DB
{
    public partial class AppServiceClass
    {
        public AppServiceClass()
        {
            AppService = new HashSet<AppService>();
            AppServiceGroup = new HashSet<AppServiceGroup>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsDeleted { get; set; }

        public virtual ICollection<AppService> AppService { get; set; }
        public virtual ICollection<AppServiceGroup> AppServiceGroup { get; set; }
    }
}

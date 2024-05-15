
using System;
using System.Collections.Generic;

namespace DicomApp.DAL.DB
{
    public partial class AppServiceGroup
    {
        public AppServiceGroup()
        {
            AppService = new HashSet<AppService>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? AppServiceClassId { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsDeleted { get; set; }

        public virtual AppServiceClass AppServiceClass { get; set; }
        public virtual ICollection<AppService> AppService { get; set; }
    }
}

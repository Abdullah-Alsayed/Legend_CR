
using System;
using System.Collections.Generic;

namespace LegendCR.DAL.DB
{
    public partial class AppService
    {
        public AppService()
        {
            RoleAppService = new HashSet<RoleAppService>();
        }

        public int Id { get; set; }
        public bool AllowAnonymous { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime CreationDate { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime ModificationDate { get; set; }
        public int? ModifiedBy { get; set; }
        public string Name { get; set; }
        public bool? ShowToUser { get; set; }
        public string Title { get; set; }
        public string ClassName { get; set; }
        public bool AllowLog { get; set; }
        public string LogMessage { get; set; }
        public int? AppServiceClassId { get; set; }
        public int? AppServiceGroupId { get; set; }

        public virtual AppServiceClass AppServiceClass { get; set; }
        public virtual AppServiceGroup AppServiceGroup { get; set; }
        public virtual ICollection<RoleAppService> RoleAppService { get; set; }
    }
}

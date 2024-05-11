
using System;
using System.Collections.Generic;

namespace LegendCR.DAL.DB
{
    public partial class RoleAppService
    {
        public int Id { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime CreationDate { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime ModificationDate { get; set; }
        public int? ModifiedBy { get; set; }
        public int AppServiceId { get; set; }
        public bool Enabled { get; set; }
        public int RoleId { get; set; }

        public virtual AppService AppService { get; set; }
        public virtual Role Role { get; set; }
    }
}

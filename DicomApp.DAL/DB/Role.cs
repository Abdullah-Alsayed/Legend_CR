
using System;
using System.Collections.Generic;

namespace DicomApp.DAL.DB
{
    public partial class Role
    {
        public Role()
        {
            Account = new HashSet<Account>();
            CommonUser = new HashSet<CommonUser>();
            Notification = new HashSet<Notification>();
            RoleAppService = new HashSet<RoleAppService>();
        }

        public int Id { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime CreationDate { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime ModificationDate { get; set; }
        public int? ModifiedBy { get; set; }
        public string Name { get; set; }
        public bool? Editable { get; set; }
        public bool IsInternal { get; set; }

        public virtual ICollection<Account> Account { get; set; }
        public virtual ICollection<CommonUser> CommonUser { get; set; }
        public virtual ICollection<Notification> Notification { get; set; }
        public virtual ICollection<RoleAppService> RoleAppService { get; set; }
    }
}

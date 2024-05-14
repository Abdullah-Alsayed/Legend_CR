using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace LegendCR.DAL.DB
{
    public class Role : IdentityRole
    {
        public Role()
        {
            //  RoleAppService = new HashSet<RoleAppService>();
        }

        public bool IsMaster { get; set; }

        public string CreateBy { get; set; }
        public string ModifyBy { get; set; }
        public string DeletedBy { get; set; }

        [Required]
        public DateTime CreateAt { get; set; } = DateTime.Now;
        public DateTime? ModifyAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;

        public virtual ICollection<RoleAppService> RoleAppService { get; set; }
    }
}

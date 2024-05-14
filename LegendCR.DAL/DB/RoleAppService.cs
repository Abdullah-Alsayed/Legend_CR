using System.ComponentModel.DataAnnotations;

namespace LegendCR.DAL.DB
{
    public class RoleAppService : BaseEntity
    {
        [Required]
        public int AppServiceId { get; set; }

        [Required]
        public string RoleId { get; set; }
        public bool Enabled { get; set; }

        public virtual AppService AppService { get; set; }
        public virtual Role Role { get; set; }
    }
}

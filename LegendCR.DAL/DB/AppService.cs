using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LegendCR.DAL.DB
{
    public class AppService
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public AppService() => RoleAppService = new HashSet<RoleAppService>();

        [StringLength(100), Required]
        public string Name { get; set; }

        [StringLength(100)]
        public string Title { get; set; }

        [StringLength(100), Required]
        public string ClassName { get; set; }

        public bool ShowToUser { get; set; }
        public bool AllowAnonymous { get; set; }
        public virtual ICollection<RoleAppService> RoleAppService { get; set; }
    }
}

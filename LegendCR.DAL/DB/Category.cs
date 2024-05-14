using System.ComponentModel.DataAnnotations;

namespace LegendCR.DAL.DB
{
    public class Category : BaseEntity
    {
        [StringLength(100), Required]
        public string Name { get; set; }

        [Required]
        public string Icon { get; set; }

        public virtual ICollection<Game> Games { get; set; }
    }
}

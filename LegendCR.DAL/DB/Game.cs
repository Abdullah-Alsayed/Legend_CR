using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LegendCR.DAL.DB
{
    public class Game : BaseEntity
    {
        [StringLength(100), Required]
        public string Name { get; set; } = string.Empty;

        [StringLength(100), Required]
        public string Logo { get; set; } = string.Empty;

        [StringLength(225), Required]
        public string Description { get; set; } = string.Empty;

        public int CategoryID { get; set; }

        public virtual Category Category { get; set; }

        public virtual ICollection<Account> Accounts { get; set; }
    }
}

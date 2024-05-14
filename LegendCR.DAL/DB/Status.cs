using System.ComponentModel.DataAnnotations;

namespace LegendCR.DAL.DB
{
    public class Status : BaseEntity
    {
        [StringLength(100), Required]
        public string Name { get; set; } = string.Empty;

        public virtual ICollection<Account> Accounts { get; set; }
    }
}

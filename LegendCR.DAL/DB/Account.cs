using System.ComponentModel.DataAnnotations;

namespace LegendCR.DAL.DB
{
    public class Account : BaseEntity
    {
        [StringLength(100), Required]
        public string UserName { get; set; } = string.Empty;

        [StringLength(100), Required]
        public string Password { get; set; } = string.Empty;

        [StringLength(255), Required]
        public string Description { get; set; } = string.Empty;

        [Range(0, 10000000)]
        public decimal SellingPrice { get; set; } = decimal.Zero;

        [Range(1, 10000000), Required]
        public decimal PurchasePrice { get; set; } = decimal.Zero;

        [Range(1, 10000000)]
        public decimal Profit { get; set; } = decimal.Zero;
        public int Level { get; set; }
        public bool IsAccepted { get; set; }

        public int StatusID { get; set; }
        public int GameID { get; set; }

        public virtual Status Status { get; set; }
        public virtual Game Game { get; set; }
        public virtual ICollection<Invoice> Invoice { get; set; }
        public virtual ICollection<Favorite> Favorites { get; set; }
    }
}

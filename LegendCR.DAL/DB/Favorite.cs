using System.ComponentModel.DataAnnotations;

namespace LegendCR.DAL.DB
{
    public class Favorite : BaseEntity
    {
        [Required]
        public int AccountID { get; set; }

        public virtual Account Account { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace LegendCR.DAL.DB
{
    public class FeedBack : BaseEntity
    {
        [StringLength(225), Required]
        public string Comment { get; set; }

        [Range(1, 5), Required]
        public int Rate { get; set; }
    }
}

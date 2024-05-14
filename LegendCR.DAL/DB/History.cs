using System.ComponentModel.DataAnnotations;
using LegendCR.DAL.Enums;

namespace LegendCR.DAL.DB
{
    public class History : BaseEntity
    {
        [StringLength(100), Required]
        public string Comment { get; set; }

        [StringLength(100), Required]
        public string Title { get; set; }

        [StringLength(100), Required]
        public string Icon { get; set; }
        public OperationTypeEnum Action { get; set; }
        public EntitiesEnum Entity { get; set; }
    }
}

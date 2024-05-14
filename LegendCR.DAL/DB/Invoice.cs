using System.ComponentModel.DataAnnotations;

namespace LegendCR.DAL.DB
{
    public class Invoice : BaseEntity
    {
        public int AccountID { get; set; }
        public bool IsReturn { get; set; } = false;

        public virtual Account Account { get; set; }
    }
}

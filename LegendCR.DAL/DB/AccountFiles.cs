using System.ComponentModel.DataAnnotations;

namespace LegendCR.DAL.DB
{
    public class AccountFiles : BaseEntity
    {
        public Guid AccountId { get; set; }
        public string File { get; set; } = string.Empty;
    }
}

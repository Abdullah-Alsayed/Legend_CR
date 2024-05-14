namespace LegendCR.DAL.DB
{
    public class Settings : BaseEntity
    {
        public string Address { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string FaceBook { get; set; } = string.Empty;
        public string Instagram { get; set; } = string.Empty;
        public string MainColor { get; set; } = string.Empty;
        public string Logo { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Whatsapp { get; set; } = string.Empty;
        public string Youtube { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
    }
}

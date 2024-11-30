using DicomApp.CommonDefinitions.DTO;

namespace DicomApp.CommonDefinitions.Requests
{
    public class DashboardRequest : BaseRequest
    {
        public int TopGames { get; set; }
        public int TopDriver { get; set; }
        public int TopBuyer { get; set; }
        public DashboardDTO DashboardDTO { get; set; }
        public int PackagingStock { get; set; }
    }
}

using DicomApp.CommonDefinitions.DTO;
using Microsoft.AspNetCore.Http;

namespace DicomApp.CommonDefinitions.Requests
{
    public class GameChargeRequest : BaseRequest
    {
        public GameChargeDTO GameChargeDTO { get; set; }
        public string RoutPath { get; set; }
    }
}

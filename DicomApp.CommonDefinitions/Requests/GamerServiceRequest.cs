using DicomApp.CommonDefinitions.DTO;
using DicomApp.CommonDefinitions.Responses;

namespace DicomApp.CommonDefinitions.Requests
{
    public class GamerServiceRequest : BaseRequest
    {
        public string RoutPath { get; set; }
        public ServiceDTO ServiceDTO { get; set; } = new ServiceDTO();
    }
}

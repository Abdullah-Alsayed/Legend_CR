using DicomApp.CommonDefinitions.DTO;

namespace DicomApp.CommonDefinitions.Requests
{
    public class RoleAppServiceRequest : BaseRequest
    {
        public RoleAppServiceDTO RoleAppServiceDTO { get; set; }
        public AppServiceDTO AppServiceDTO { get; set; }
    }
}

using DicomApp.CommonDefinitions.DTO.PickupDTOs;

namespace DicomApp.CommonDefinitions.Requests
{
    public class PickUpRequestRequest : BaseRequest
    {
        public PickupDTO PickupDTO { get; set; }
        public PickupItemDTO PickupItemDTO { get; set; }
        public bool HasPickupItemDTO { get; set; }
    }
}

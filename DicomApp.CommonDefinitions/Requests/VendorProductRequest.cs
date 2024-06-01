
using DicomApp.CommonDefinitions.DTO.VendorProductDTOs;
using DicomApp.DAL.DB;
using DicomApp.CommonDefinitions.Requests;

namespace DicomApp.CommonDefinitions.Requests
{
    public class VendorProductRequest : BaseRequest
    {
        public VendorProductDTO VendorProductDTO { get; set; }
    }
}

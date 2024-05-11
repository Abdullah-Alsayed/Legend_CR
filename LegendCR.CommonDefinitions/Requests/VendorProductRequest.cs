
using LegendCR.CommonDefinitions.DTO.VendorProductDTOs;
using LegendCR.DAL.DB;
using LegendCR.CommonDefinitions.Requests;

namespace LegendCR.CommonDefinitions.Requests
{
    public class VendorProductRequest : BaseRequest
    {
        public VendorProductDTO VendorProductDTO { get; set; }
    }
}

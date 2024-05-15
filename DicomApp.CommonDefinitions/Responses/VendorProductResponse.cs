
using DicomApp.CommonDefinitions.DTO.VendorProductDTOs;
using DicomApp.DAL.DB;
using DicomApp.CommonDefinitions.Responses;
using System.Collections.Generic;

namespace DicomApp.CommonDefinitions.Responses
{
    public class VendorProductResponse : BaseResponse
    {
        public List<VendorProductDTO> VendorProductDTOs { get; set; }
        public VendorProductDTO VendorProductDTO { get; set; }
    }
}

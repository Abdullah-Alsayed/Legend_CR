
using LegendCR.CommonDefinitions.DTO.VendorProductDTOs;
using LegendCR.DAL.DB;
using LegendCR.CommonDefinitions.Responses;
using System.Collections.Generic;

namespace LegendCR.CommonDefinitions.Responses
{
    public class VendorProductResponse : BaseResponse
    {
        public List<VendorProductDTO> VendorProductDTOs { get; set; }
        public VendorProductDTO VendorProductDTO { get; set; }
    }
}

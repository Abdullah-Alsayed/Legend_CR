
using LegendCR.CommonDefinitions.DTO;
using LegendCR.CommonDefinitions.DTO;
using System.Collections.Generic;

namespace LegendCR.CommonDefinitions.Responses
{
    public class PackingResponse : BaseResponse
    {

        public List<PackingDTO> PackingDTOs { get; set; }
        public PackingDTO PackingDTO { get; set; }
    }
}
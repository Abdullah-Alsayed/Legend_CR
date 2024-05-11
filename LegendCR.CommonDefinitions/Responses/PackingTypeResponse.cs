
using LegendCR.CommonDefinitions.DTO;
using System.Collections.Generic;

namespace LegendCR.CommonDefinitions.Responses
{
    public class PackingTypeResponse : BaseResponse
    {

        public List<PackingTypeDTO> PackingTypeDTOs { get; set; }
    }
}
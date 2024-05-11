
using LegendCR.CommonDefinitions.DTO;
using System.Collections.Generic;

namespace LegendCR.CommonDefinitions.Responses
{
    public class ZoneResponse : BaseResponse
    {

        public List<ZoneDTO> ZoneDTOs { get; set; }
    }
}
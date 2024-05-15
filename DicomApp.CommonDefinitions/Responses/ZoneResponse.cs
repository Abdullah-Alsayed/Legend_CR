
using DicomApp.CommonDefinitions.DTO;
using System.Collections.Generic;

namespace DicomApp.CommonDefinitions.Responses
{
    public class ZoneResponse : BaseResponse
    {

        public List<ZoneDTO> ZoneDTOs { get; set; }
    }
}
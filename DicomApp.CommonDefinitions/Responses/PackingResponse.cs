
using DicomApp.CommonDefinitions.DTO;
using DicomApp.CommonDefinitions.DTO;
using System.Collections.Generic;

namespace DicomApp.CommonDefinitions.Responses
{
    public class PackingResponse : BaseResponse
    {

        public List<PackingDTO> PackingDTOs { get; set; }
        public PackingDTO PackingDTO { get; set; }
    }
}
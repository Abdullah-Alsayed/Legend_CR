
using DicomApp.CommonDefinitions.DTO;
using System.Collections.Generic;

namespace DicomApp.CommonDefinitions.Responses
{
    public class PackingTypeResponse : BaseResponse
    {

        public List<PackingTypeDTO> PackingTypeDTOs { get; set; }
    }
}
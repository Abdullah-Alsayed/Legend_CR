
using DicomApp.CommonDefinitions.DTO;
using System.Collections.Generic;

namespace DicomApp.CommonDefinitions.Responses
{
    public class Common_ResourceResponse : BaseResponse
    {

        public List<Common_ResourceDTO> Common_ResourceRecords { get; set; }
    }
}
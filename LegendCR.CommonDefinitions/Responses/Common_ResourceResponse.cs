
using LegendCR.CommonDefinitions.DTO;
using System.Collections.Generic;

namespace LegendCR.CommonDefinitions.Responses
{
    public class Common_ResourceResponse : BaseResponse
    {

        public List<Common_ResourceDTO> Common_ResourceRecords { get; set; }
    }
}

using LegendCR.CommonDefinitions.DTO;
using System.Collections.Generic;

namespace LegendCR.CommonDefinitions.Responses
{
    public class Common_UserDeviceResponse : BaseResponse
    {

        public List<Common_UserDeviceDTO> Common_UserDeviceRecords { get; set; }
    }
}
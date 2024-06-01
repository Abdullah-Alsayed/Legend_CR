
using DicomApp.CommonDefinitions.DTO;
using System.Collections.Generic;

namespace DicomApp.CommonDefinitions.Responses
{
    public class Common_UserDeviceResponse : BaseResponse
    {

        public List<Common_UserDeviceDTO> Common_UserDeviceRecords { get; set; }
    }
}
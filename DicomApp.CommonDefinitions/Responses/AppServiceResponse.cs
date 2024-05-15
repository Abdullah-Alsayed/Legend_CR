using DicomApp.CommonDefinitions.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace DicomApp.CommonDefinitions.Responses
{
    public class AppServiceResponse : BaseResponse
    {
        public List<AppServiceDTO> AppServiceRecords { get; set; }
    }
}


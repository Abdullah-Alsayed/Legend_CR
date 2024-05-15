
using DicomApp.CommonDefinitions.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace DicomApp.CommonDefinitions.Responses
{
    public class StatusResponse : BaseResponse
    {
        public List<StatusDTO> StatusDTOs { get; set; }
    }
}

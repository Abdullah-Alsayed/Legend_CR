
using LegendCR.CommonDefinitions.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace LegendCR.CommonDefinitions.Responses
{
    public class StatusResponse : BaseResponse
    {
        public List<StatusDTO> StatusDTOs { get; set; }
    }
}

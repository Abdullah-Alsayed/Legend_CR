
using LegendCR.CommonDefinitions.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace LegendCR.CommonDefinitions.Requests
{
    public class ZoneRequest : BaseRequest
    {
        public ZoneDTO ZoneDTO { get; set; }
        public bool HasDetails { get; set; }
    
}
}
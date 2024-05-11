using LegendCR.CommonDefinitions.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace LegendCR.CommonDefinitions.Requests
{
    public class AppServiceRequest : BaseRequest
    {
        public AppServiceDTO AppServiceRecord { get; set; }
    }
}


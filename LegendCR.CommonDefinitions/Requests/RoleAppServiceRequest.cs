using LegendCR.CommonDefinitions.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace LegendCR.CommonDefinitions.Requests
{
    public class RoleAppServiceRequest : BaseRequest
    {
        public RoleAppServiceDTO RoleAppServiceDTO { get; set; }
        public AppServiceDTO AppServiceDTO { get; set; }
    }
}

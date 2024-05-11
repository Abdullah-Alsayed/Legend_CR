
using LegendCR.CommonDefinitions.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace LegendCR.CommonDefinitions.Responses
{
    public class RoleAppServiceResponse : BaseResponse
    {
        public List<RoleAppServiceDTO> RoleAppServiceDTOs { get; set; }
        public List<AppServiceDTO> AppServiceDTOs { get; set; }
    }
}


using DicomApp.CommonDefinitions.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace DicomApp.CommonDefinitions.Responses
{
    public class RoleAppServiceResponse : BaseResponse
    {
        public List<RoleAppServiceDTO> RoleAppServiceDTOs { get; set; }
        public List<AppServiceDTO> AppServiceDTOs { get; set; }
    }
}

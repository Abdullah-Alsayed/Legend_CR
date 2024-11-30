using System;
using System.Collections.Generic;
using System.Text;
using DicomApp.CommonDefinitions.DTO;

namespace DicomApp.CommonDefinitions.Responses
{
    public class RoleResponse : BaseResponse
    {
        public List<RoleDTO> RoleDTOs { get; set; }
        public RoleDTO RoleDTO { get; set; }
    }
}

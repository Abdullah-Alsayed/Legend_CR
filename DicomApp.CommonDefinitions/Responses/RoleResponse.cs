
using DicomApp.CommonDefinitions.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace DicomApp.CommonDefinitions.Responses
{
    public class RoleResponse : BaseResponse
    {
        public List<RoleDTO> RoleDTOs { get; set; }
    }
}

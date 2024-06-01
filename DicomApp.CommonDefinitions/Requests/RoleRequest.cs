using DicomApp.CommonDefinitions.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace DicomApp.CommonDefinitions.Requests
{
    public class RoleRequest : BaseRequest
    {
        public RoleDTO RoleDTO { get; set; }
    }
}


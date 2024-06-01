using DicomApp.CommonDefinitions.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace DicomApp.CommonDefinitions.Requests
{
    public class UserRequest : BaseRequest
    {
        public UserDTO UserDTO { get; set; }
        public bool StaffOnly { get; set; }
        public bool HasCourierShipments { get; set; }
    }
}


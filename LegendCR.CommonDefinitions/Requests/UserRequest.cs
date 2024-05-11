using LegendCR.CommonDefinitions.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace LegendCR.CommonDefinitions.Requests
{
    public class UserRequest : BaseRequest
    {
        public UserDTO UserDTO { get; set; }
        public bool StaffOnly { get; set; }
        public bool HasCourierShipments { get; set; }
    }
}


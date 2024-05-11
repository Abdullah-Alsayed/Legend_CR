
using LegendCR.CommonDefinitions.DTO;
using System.Collections.Generic;

namespace LegendCR.CommonDefinitions.Responses
{
    public class UserResponse : BaseResponse
    {
        public List<UserDTO> UserDTOs { get; set; }
        public UserDTO UserDTO { get; set; }
    }
}

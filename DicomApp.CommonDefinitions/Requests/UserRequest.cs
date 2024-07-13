using System;
using System.Collections.Generic;
using System.Text;
using DicomApp.CommonDefinitions.DTO;
using DicomApp.Helpers.Services.GenrateAvatar;
using DicomApp.Helpers.Services.GetCounter;

namespace DicomApp.CommonDefinitions.Requests
{
    public class UserRequest : BaseRequest
    {
        public UserDTO UserDTO { get; set; }
        public bool StaffOnly { get; set; }
        public bool HasCourierShipments { get; set; }
        public string WebRootPath { get; set; }
        public AvatarService avatarService { get; set; }
        public IApiCountryService CountryService { get; set; } = null;
        public string UserIP { get; set; }
    }
}

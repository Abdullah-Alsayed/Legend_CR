
using System;
using System.Collections.Generic;
using System.Text;
using DicomApp.CommonDefinitions.Responses;
using DicomApp.CommonDefinitions.DTO;

namespace DicomApp.CommonDefinitions.Responses
{
    public class UserNotificationResponse : BaseResponse
    {
        public List<UserNotificationDTO> UserNotificationRecords { get; set; }
    }
}

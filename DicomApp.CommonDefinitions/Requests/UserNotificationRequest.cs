
using System;
using System.Collections.Generic;
using System.Text;
using DicomApp.CommonDefinitions.DTO;
using DicomApp.CommonDefinitions.Requests;

namespace DicomApp.CommonDefinitions.Requests
{
    public class UserNotificationRequest : BaseRequest
    {
        public UserNotificationDTO UserNotificationRecord { get; set; }
        public bool GetMineOnly { get; set; }
        public bool MarkAsSeen { get; set; }
        public bool NotSeen { get; set; }
    }
}

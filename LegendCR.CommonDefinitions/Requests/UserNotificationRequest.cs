
using System;
using System.Collections.Generic;
using System.Text;
using LegendCR.CommonDefinitions.DTO;
using LegendCR.CommonDefinitions.Requests;

namespace LegendCR.CommonDefinitions.Requests
{
    public class UserNotificationRequest : BaseRequest
    {
        public UserNotificationDTO UserNotificationRecord { get; set; }
        public bool GetMineOnly { get; set; }
        public bool MarkAsSeen { get; set; }
        public bool NotSeen { get; set; }
    }
}

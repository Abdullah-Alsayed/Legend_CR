
using System;
using System.Collections.Generic;
using System.Text;
using LegendCR.CommonDefinitions.Responses;
using LegendCR.CommonDefinitions.DTO;

namespace LegendCR.CommonDefinitions.Responses
{
    public class UserNotificationResponse : BaseResponse
    {
        public List<UserNotificationDTO> UserNotificationRecords { get; set; }
    }
}

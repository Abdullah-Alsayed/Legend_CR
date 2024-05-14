using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace LegendCR.CommonDefinitions.DTO
{
    public class UserNotificationDTO
    {
        public int NotificationId { get; set; }
        public string? SenderId { get; set; }
        public int? RecipientId { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string Icon { get; set; }
        public bool? IsSeen { get; set; }
        public DateTime SeenDate { get; set; }
        public DateTime CreationDate { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime DeletionDate { get; set; }
        public string TargetPath { get; set; }
        public string RecipientRoleId { get; set; }
        public int? SeenBy { get; set; }
        public int? DeletedBy { get; set; }
        public string SenderName { get; set; }
        public List<int> Ids { get; set; }
    }
}

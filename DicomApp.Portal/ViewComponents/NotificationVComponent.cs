using System.Linq;
using DicomApp.BL.Services;
using DicomApp.CommonDefinitions.DTO;
using DicomApp.CommonDefinitions.Requests;
using DicomApp.DAL.DB;
using DicomApp.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace TmiReporting.ViewComponents
{
    public class NotificationVComponent : ViewComponent
    {
        private readonly LegendDBContext _context;

        public NotificationVComponent(LegendDBContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            var notificationRequest = new UserNotificationRequest
            {
                context = _context,
                PageSize = BaseRequest.DefaultPageSize,
                PageIndex = 0,
                RoleID = AuthHelper.GetClaimValue(UserClaimsPrincipal, "RoleID"),
                UserID = AuthHelper.GetClaimValue(UserClaimsPrincipal, "UserID"),
                //GetMineOnly = true,
                NotSeen = true,
            };
            var notificationResponse = UserNotificationService.ListUserNotification(
                notificationRequest
            );
            return View(notificationResponse.UserNotificationRecords);
        }
    }
}

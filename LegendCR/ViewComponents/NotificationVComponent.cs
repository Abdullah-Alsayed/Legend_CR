using Microsoft.AspNetCore.Mvc;
using LegendCR.CommonDefinitions.DTO;
using LegendCR.CommonDefinitions.Requests;
using System.Linq;
using LegendCR.BL.Services;
using LegendCR.Helpers;
using LegendCR.DAL.DB;

namespace TmiReporting.ViewComponents
{
    public class NotificationVComponent : ViewComponent
    {
        private readonly ApplicationDBContext _context;

        public NotificationVComponent(ApplicationDBContext context)
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
                NotSeen=true,
            };
            var notificationResponse = UserNotificationService.ListUserNotification(notificationRequest);
            return View(notificationResponse.UserNotificationRecords);
        }
    }
}
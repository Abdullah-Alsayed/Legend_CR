using Microsoft.AspNetCore.Mvc;
using DicomApp.CommonDefinitions.DTO;
using DicomApp.CommonDefinitions.Requests;
using System.Linq;
using DicomApp.BL.Services;
using DicomApp.Helpers;
using DicomApp.DAL.DB;

namespace TmiReporting.ViewComponents
{
    public class NotificationVComponent : ViewComponent
    {
        private readonly ShippingDBContext _context;

        public NotificationVComponent(ShippingDBContext context)
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
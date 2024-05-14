using System.Net;
using LegendCR.CommonDefinitions.DTO;
using LegendCR.CommonDefinitions.Requests;
using LegendCR.CommonDefinitions.Responses;
using LegendCR.DAL.DB;
using LegendCR.Helpers;

namespace LegendCR.BL.Services
{
    public class UserNotificationService : BaseService
    {
        public static UserNotificationResponse ListUserNotification(UserNotificationRequest request)
        {
            var res = new UserNotificationResponse();
            RunBase(
                request,
                res,
                (UserNotificationRequest req) =>
                {
                    try
                    {
                        var query = request
                            .context.Notification.Where(c => !c.IsDeleted)
                            .Select(c => new UserNotificationDTO
                            {
                                SenderId = c.CreateBy,
                                Title = c.Title,
                                CreationDate = c.CreateAt
                            });

                        query = ApplyFilter(request, query);
                        res.TotalCount = query.Count();

                        query = OrderByDynamic(query, "NotificationId", true);
                        query = ApplyPaging(query, 50, request.PageIndex);
                        //res.TotalCount = query.Count();
                        var userNotificationList = query.ToList();

                        res.UserNotificationRecords = userNotificationList;

                        //if(request.MarkAsSeen)
                        //{
                        //    var ids = userNotificationList.Select(p => p.NotificationId).ToList();
                        //    var notrecords = request._context.Notification.Where(p => ids.Contains(p.NotificationId));
                        //    foreach (var item in notrecords)
                        //    {
                        //        item.IsSeen = true;
                        //    }
                        //    request._context.SaveChanges();
                        //}
                        res.Message = HttpStatusCode.OK.ToString();
                        res.Success = true;
                        res.StatusCode = HttpStatusCode.OK;
                    }
                    catch (Exception ex)
                    {
                        res.Message = ex.Message;
                        res.Success = false;
                        LogHelper.LogException(ex.Message, ex.StackTrace);
                    }
                    return res;
                }
            );
            return res;
        }

        public static UserNotificationResponse DeleteUserNotification(
            UserNotificationRequest request
        )
        {
            var res = new UserNotificationResponse();
            RunBase(
                request,
                res,
                (UserNotificationRequest req) =>
                {
                    try
                    {
                        var model = request.UserNotificationRecord;
                        var userNotification = request.context.Notification.FirstOrDefault(c =>
                            !c.IsDeleted
                        );
                        if (userNotification != null)
                        {
                            //update userNotification IsDeleted
                            userNotification.IsDeleted = true;
                            request.context.SaveChanges();

                            res.Message = HttpStatusCode.OK.ToString();
                            res.Success = true;
                            res.StatusCode = HttpStatusCode.OK;
                        }
                        else
                        {
                            res.Message = "Invalid userNotification";
                            res.Success = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        res.Message = ex.Message;
                        res.Success = false;
                        LogHelper.LogException(ex.Message, ex.StackTrace);
                    }
                    return res;
                }
            );
            return res;
        }

        public static UserNotificationResponse AddUserNotification(UserNotificationRequest request)
        {
            var res = new UserNotificationResponse();
            RunBase(
                request,
                res,
                (UserNotificationRequest req) =>
                {
                    try
                    {
                        var userNotification = AddOrEditUserNotification(
                            request.BaseUrl,
                            request.UserID,
                            request.RoleID,
                            request.UserNotificationRecord
                        );
                        request.context.Notification.Add(userNotification);

                        request.context.SaveChanges();

                        res.Message = HttpStatusCode.OK.ToString();
                        res.Success = true;
                        res.StatusCode = HttpStatusCode.OK;
                    }
                    catch (Exception ex)
                    {
                        res.Message = ex.Message;
                        res.Success = false;
                        LogHelper.LogException(ex.Message, ex.StackTrace);
                    }
                    return res;
                }
            );
            return res;
        }

        private const string UserNotificationPath = "{0}/ContentFiles/UserNotificationImages/{1}";

        private static Notification AddOrEditUserNotification(
            string baseUrl,
            string createdBy,
            string roleid,
            UserNotificationDTO record,
            Notification oldUserNotification = null
        )
        {
            if (oldUserNotification == null) //new userNotification
            {
                //oldUserNotification = new Notification();
                //oldUserNotification.CreationDate = DateTime.Now;
                //oldUserNotification.RecipientId = record.RecipientId;
                //oldUserNotification.SenderId = createdBy;
                //oldUserNotification.Icon = record.Icon;
                //oldUserNotification.Title = record.Title;
                //oldUserNotification.Body = record.Body;
                //oldUserNotification.IsSeen = false;
                //oldUserNotification.RecipientRoleId = roleid;
                //oldUserNotification.Icon = record.Icon;
            }
            else { }

            return oldUserNotification;
        }

        private static IQueryable<UserNotificationDTO> ApplyFilter(
            UserNotificationRequest request,
            IQueryable<UserNotificationDTO> query
        )
        {
            if (request.UserNotificationRecord != null)
            {
                if (request.UserNotificationRecord.NotificationId > 0)
                    query = query.Where(c =>
                        c.NotificationId == request.UserNotificationRecord.NotificationId
                    );
            }

            if (request.GetMineOnly)
            {
                query = query.Where(c => c.RecipientRoleId == request.RoleID);
            }
            if (request.NotSeen)
            {
                query = query.Where(c => c.IsSeen != true);
            }
            return query;
        }
    }
}

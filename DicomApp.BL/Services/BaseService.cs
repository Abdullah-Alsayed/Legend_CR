using System;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;
using DicomApp.CommonDefinitions.DTO;
using DicomApp.CommonDefinitions.Requests;
using DicomApp.CommonDefinitions.Responses;
using DicomApp.DAL.DB;
using DicomApp.Helpers;
using Microsoft.EntityFrameworkCore;

namespace DicomApp.BL.Services
{
    public class BaseService
    {
        protected const string IDColumn = "Id";
        protected const string OrderByCommand = "OrderBy";
        protected const string OrderByDescCommand = "OrderByDescending";

        protected static IQueryable<Q> OrderByDynamic<Q>(
            IQueryable<Q> query,
            string orderByColumn,
            bool isDesc
        )
        {
            var QType = typeof(Q);
            orderByColumn = char.ToUpper(orderByColumn[0]) + orderByColumn.Substring(1);
            // Dynamically creates a call like this: query.OrderBy(p => p.SortColumn)
            var parameter = Expression.Parameter(QType, "p");
            Expression resultExpression = null;
            var property = QType.GetProperty(orderByColumn ?? "ID");
            // this is the part p.SortColumn
            var propertyAccess = Expression.MakeMemberAccess(parameter, property);
            // this is the part p => p.SortColumn
            var orderByExpression = Expression.Lambda(propertyAccess, parameter);

            resultExpression = Expression.Call(
                typeof(Queryable),
                isDesc ? "OrderByDescending" : "OrderBy",
                new Type[] { QType, property.PropertyType },
                query.Expression,
                Expression.Quote(orderByExpression)
            );

            return query.Provider.CreateQuery<Q>(resultExpression);
        }

        protected static IQueryable<Q> ApplyPaging<Q>(
            IQueryable<Q> query,
            int pageSize,
            int pageIndex
        )
        {
            var skipedPages = pageSize * pageIndex;
            query = query.Skip(skipedPages).Take(pageSize);
            return query;
        }

        public static RS RunBase<RQ, RS>(RQ request, RS response, Func<RQ, RS> myMethod)
            where RQ : BaseRequest
            where RS : BaseResponse
        {
            try
            {
                var methodName = myMethod.Method.Name;
                var serviceName = methodName
                    .Substring(0, methodName.LastIndexOf(">"))
                    .Replace('<', ' ')
                    .Trim();

                bool canAccess = true;
                //canAccess = CheckRoleAccessability(
                //    request.RoleID,
                //    serviceName,
                //    request.context,
                //    created: request.UserID,
                //    message: request.Message
                //);
                if (!canAccess)
                {
                    response.Message = "Not allowed";
                    response.Success = false;
                    response.StatusCode = HttpStatusCode.MethodNotAllowed;
                    return response;
                }
                response = myMethod(request);
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Success = false;
                LogHelper.LogException(ex.Message, ex.StackTrace);
            }

            return response;
        }

        public static async Task<RS> RunBaseAsync<RQ, RS>(
            RQ request,
            RS response,
            Func<RQ, Task<RS>> myMethod
        )
            where RQ : BaseRequest
            where RS : BaseResponse
        {
            try
            {
                var methodName = myMethod.Method.Name;
                var serviceName = methodName
                    .Substring(0, methodName.LastIndexOf(">"))
                    .Replace('<', ' ')
                    .Trim();

                bool canAccess = true;
                //canAccess = CheckRoleAccessability(
                //    request.RoleID,
                //    serviceName,
                //    request.context,
                //    created: request.UserID,
                //    message: request.Message
                //);
                if (!canAccess)
                {
                    response.Message = "Not allowed";
                    response.Success = false;
                    response.StatusCode = HttpStatusCode.MethodNotAllowed;
                    return response;
                }
                response = await myMethod(request);
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Success = false;
                LogHelper.LogException(ex.Message, ex.StackTrace);
            }

            return response;
        }

        public static bool CheckRoleAccessability(
            long roleID,
            string serviceName,
            ShippingDBContext context = null,
            string connectionString = null,
            long created = 0,
            string message = ""
        )
        {
            if (context == null)
                context = new ShippingDBContext(GetDBContextConnectionOptions(connectionString));

            var role = context.Role.FirstOrDefault(x => x.Id == roleID);
            if (role != null && role.Name == SystemConstants.Role.SuperAdmin)
                return true;

            var canAccess = true;

            var service = context
                .RoleAppService.Include(x => x.AppService)
                .FirstOrDefault(c =>
                    !c.IsDeleted && c.RoleId == roleID && c.AppService.Name == serviceName
                );
            if (service != null)
                return true;

            if (service != null && !service.AppService.AllowAnonymous)
            {
                if (roleID == 0)
                    return false;
                else if (service.AppService.ShowToUser.Value)
                    canAccess = context.RoleAppService.Any(c =>
                        c.RoleId == roleID && c.AppServiceId == service.Id && c.Enabled
                    );
            }
            else
                return false;
            return canAccess;
        }

        public static DbContextOptions<ShippingDBContext> GetDBContextConnectionOptions(
            string connectionString
        )
        {
            return SqlServerDbContextOptionsExtensions
                .UseSqlServer(new DbContextOptionsBuilder<ShippingDBContext>(), connectionString)
                .Options;
        }
    }
}

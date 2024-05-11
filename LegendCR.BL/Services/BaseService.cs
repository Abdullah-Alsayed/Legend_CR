using LegendCR.CommonDefinitions.DTO;
using LegendCR.CommonDefinitions.Requests;
using LegendCR.CommonDefinitions.Responses;
using LegendCR.DAL.DB;
using LegendCR.Helpers;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Net;

namespace LegendCR.BL.Services
{
    public class BaseService
    {
        protected const string IDColumn = "Id";
        protected const string OrderByCommand = "OrderBy";
        protected const string OrderByDescCommand = "OrderByDescending";


        protected static IQueryable<Q> OrderByDynamic<Q>(IQueryable<Q> query, string orderByColumn, bool isDesc)
        {
            var QType = typeof(Q);
            // Dynamically creates a call like this: query.OrderBy(p => p.SortColumn)
            var parameter = Expression.Parameter(QType, "p");
            Expression resultExpression = null;
            var property = QType.GetProperty(orderByColumn ?? IDColumn);
            // this is the part p.SortColumn
            var propertyAccess = Expression.MakeMemberAccess(parameter, property);
            // this is the part p => p.SortColumn
            var orderByExpression = Expression.Lambda(propertyAccess, parameter);

            // finally, call the "OrderBy" / "OrderByDescending" method with the order by lamba expression
            resultExpression = Expression.Call(typeof(Queryable), isDesc ? OrderByDescCommand : OrderByCommand, new Type[] { QType, property.PropertyType }, query.Expression, Expression.Quote(orderByExpression));

            return query.Provider.CreateQuery<Q>(resultExpression);
        }

        protected static IQueryable<Q> ApplyPaging<Q>(IQueryable<Q> query, int pageSize, int pageIndex)
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
                var serviceName = methodName.Substring(0, methodName.LastIndexOf(">")).Replace('<', ' ').Trim();

                //check if user role can access this service
                bool canAccess = true;
                //canAccess = CheckRoleAccessability(request.RoleID, serviceName, request._context, created: request.CreatedBy, message: request.Message);
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

        public static bool CheckRoleAccessability(long roleID, string serviceName, ApplicationDBContext context = null, string connectionString = null, long created = 0, string message = "")
        {
            if (context == null)
                context = new ApplicationDBContext(GetDBContextConnectionOptions(connectionString));

            if (roleID == (long)EnumRole.SuperAdmin || roleID == (long)EnumRole.Admin)  //Super Admin
                return true;

            var canAccess = true;

            var service = context.AppService.FirstOrDefault(c => !c.IsDeleted && c.Name == serviceName);
            if (service == null)
                return true;
            if (!service.AllowAnonymous)
            {
                if (roleID == 0)
                    return false;
                else if (service.ShowToUser.Value)
                    canAccess = context.RoleAppService.Any(c => c.RoleId == roleID && c.AppServiceId == service.Id && c.Enabled);
            }
            return canAccess;
        }

        public static DbContextOptions<ApplicationDBContext> GetDBContextConnectionOptions(string connectionString)
        {
            return SqlServerDbContextOptionsExtensions.UseSqlServer(new DbContextOptionsBuilder<ApplicationDBContext>(), connectionString).Options;
        }
    }
}
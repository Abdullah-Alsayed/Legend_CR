using System.Net;
using LegendCR.CommonDefinitions.DTO;
using LegendCR.CommonDefinitions.Requests;
using LegendCR.CommonDefinitions.Responses;
using LegendCR.DAL.DB;
using LegendCR.Helpers;

namespace LegendCR.BL.Services
{
    public class PermissionService : BaseService
    {
        public static RoleAppServiceResponse ListAppService(RoleAppServiceRequest request)
        {
            var res = new RoleAppServiceResponse();
            RunBase(
                request,
                res,
                (RoleAppServiceRequest req) =>
                {
                    try
                    {
                        var query = request
                            .context.AppService.Where(c => !c.AllowAnonymous && c.ShowToUser)
                            .Select(c => new AppServiceDTO
                            {
                                Id = c.ID,
                                Name = c.Name,
                                Title = c.Title,
                                AllowAnonymous = c.AllowAnonymous,
                                ShowToUser = c.ShowToUser,
                                RoleID = request.RoleAppServiceDTO.RoleId,
                                //RoleName =
                                //    request
                                //        .context.Roles.FirstOrDefault(r =>
                                //            r.Id == request.RoleAppServiceDTO.RoleId
                                //        )
                                //        ?.Name ?? "",

                                IsChecked = c.RoleAppService.Any(r =>
                                    r.AppServiceId == c.ID
                                    && r.RoleId == request.RoleAppServiceDTO.RoleId
                                    && !r.IsDeleted
                                )
                            });

                        res.AppServiceDTOs = query.ToList();

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

        public static RoleAppServiceResponse ListRoleAppService(RoleAppServiceRequest request)
        {
            var res = new RoleAppServiceResponse();
            RunBase(
                request,
                res,
                (RoleAppServiceRequest req) =>
                {
                    try
                    {
                        var query = request
                            .context.RoleAppService.Where(c =>
                                !c.AppService.AllowAnonymous && c.AppService.ShowToUser
                            )
                            .Select(c => new RoleAppServiceDTO
                            {
                                Id = c.ID,
                                RoleId = c.RoleId,
                                //  RoleName = c.Role?.Name ?? "",
                                ClassName = c.AppService.ClassName,

                                AppServiceId = c.AppServiceId,
                                ServiceName = c.AppService.Name,
                                ServiceTitle = c.AppService.Title,
                                ServiceAllowAnonymous = c.AppService.AllowAnonymous,
                                ServiceShowToUser = c.AppService.ShowToUser,

                                Enabled = c.Enabled,
                            });

                        if (request.RoleAppServiceDTO != null)
                            query = ApplyFilter(query, request.RoleAppServiceDTO);

                        query = OrderByDynamic(query, request.OrderByColumn, request.IsDesc);

                        if (request.PageSize > 0)
                            query = ApplyPaging(query, request.PageSize, request.PageIndex);

                        res.RoleAppServiceDTOs = query.ToList();

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

        public static RoleAppServiceResponse UpdateRoleAppService(RoleAppServiceRequest request)
        {
            var res = new RoleAppServiceResponse();
            RunBase(
                request,
                res,
                (RoleAppServiceRequest req) =>
                {
                    try
                    {
                        var model = request.AppServiceDTO;

                        //*** Delete Current Role Permissions
                        var toDeleteData = request.context.RoleAppService.Where(r =>
                            r.RoleId == model.RoleID
                        );
                        request.context.RoleAppService.RemoveRange(toDeleteData);
                        request.context.SaveChanges();

                        //*** Add New Role Permissions
                        foreach (var item in model.SelectedAppServiceIDs)
                        {
                            var newRoleAppService = new RoleAppService();
                            newRoleAppService.AppServiceId = item;
                            newRoleAppService.RoleId = model.RoleID;
                            newRoleAppService.Enabled = true;
                            newRoleAppService.CreateAt = DateTime.Now;
                            newRoleAppService.CreateBy = request.UserID;

                            request.context.RoleAppService.Add(newRoleAppService);
                        }

                        request.context.SaveChanges();

                        res.Message = "Saved Successfully";
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

        private static RoleAppService AddOrEditRoleAppService(
            long createdBy,
            RoleAppServiceDTO record,
            RoleAppService oldRoleAppService = null
        )
        {
            if (oldRoleAppService == null) //new roleAppService
            {
                oldRoleAppService = new RoleAppService();
            }
            else { }

            return oldRoleAppService;
        }

        private static IQueryable<RoleAppServiceDTO> ApplyFilter(
            IQueryable<RoleAppServiceDTO> query,
            RoleAppServiceDTO roleAppServiceRecord
        )
        {
            if (roleAppServiceRecord.Id != Guid.Empty)
                query = query.Where(c => c.Id == roleAppServiceRecord.Id);

            if (!string.IsNullOrWhiteSpace(roleAppServiceRecord.RoleId))
                query = query.Where(c => c.RoleId == roleAppServiceRecord.RoleId);

            return query;
        }
    }
}

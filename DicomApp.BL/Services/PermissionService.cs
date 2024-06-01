using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using DicomApp.CommonDefinitions.DTO;
using DicomApp.CommonDefinitions.Requests;
using DicomApp.CommonDefinitions.Responses;
using DicomApp.DAL.DB;
using DicomApp.Helpers;

namespace DicomApp.BL.Services
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
                            .context.AppService.Where(c =>
                                !c.IsDeleted && !c.AllowAnonymous && c.ShowToUser.Value
                            )
                            .Select(c => new AppServiceDTO
                            {
                                Id = c.Id,
                                Name = c.Name,
                                Title = c.Title,
                                AllowAnonymous = c.AllowAnonymous,
                                ShowToUser = c.ShowToUser.Value,
                                RoleID = request.RoleAppServiceDTO.RoleId,
                                RoleName = request
                                    .context.Role.FirstOrDefault(r =>
                                        r.Id == request.RoleAppServiceDTO.RoleId
                                    )
                                    .Name,
                                ClassName = c.ClassName,
                                IsChecked = c.RoleAppService.Any(r =>
                                    r.AppServiceId == c.Id
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
                                !c.AppService.IsDeleted
                                && !c.AppService.AllowAnonymous
                                && c.AppService.ShowToUser.Value
                            )
                            .Select(c => new RoleAppServiceDTO
                            {
                                Id = c.Id,
                                RoleId = c.RoleId,
                                RoleName = c.Role.Name,
                                ClassName = c.AppService.ClassName,

                                AppServiceId = c.AppServiceId,
                                ServiceName = c.AppService.Name,
                                ServiceTitle = c.AppService.Title,
                                ServiceAllowAnonymous = c.AppService.AllowAnonymous,
                                ServiceShowToUser = c.AppService.ShowToUser.Value,
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

        public static RoleAppServiceResponse SeedPermission(SeedPermissionRequest request)
        {
            var res = new RoleAppServiceResponse();
            RunBase(
                request,
                res,
                (SeedPermissionRequest req) =>
                {
                    try
                    {
                        var roles = req.context.Role.Where(x => !x.IsDeleted).ToList();
                        var AppServiceList = new List<AppService>();
                        foreach (var Controller in req.Permissions)
                        {
                            foreach (var Permission in Controller.Value)
                            {
                                if (!AppServiceList.Any(x => x.Name == Permission))
                                {
                                    AppServiceList.Add(
                                        new AppService
                                        {
                                            ClassName = Controller.Key.Replace("Controller", ""),
                                            CreatedBy = req.UserID,
                                            Name = Permission,
                                            Title = Permission,
                                            LogMessage = string.Empty,
                                            CreationDate = DateTime.Now,
                                            AllowLog = true,
                                            ShowToUser = true,
                                            IsDeleted = false,
                                            AllowAnonymous = false,
                                        }
                                    );
                                }
                            }
                        }
                        req.context.AppService.AddRange(AppServiceList);
                        request.context.SaveChanges();

                        var RoleAppServiceList = new List<RoleAppService>();
                        foreach (var role in roles)
                        {
                            foreach (var Service in AppServiceList)
                            {
                                RoleAppServiceList.Add(
                                    new RoleAppService
                                    {
                                        RoleId = role.Id,
                                        AppServiceId = Service.Id,
                                        IsDeleted = false,
                                        CreatedBy = req.UserID,
                                        Enabled = true,
                                        CreationDate = DateTime.Now,
                                    }
                                );
                            }
                        }
                        req.context.RoleAppService.AddRange(RoleAppServiceList);
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
                            newRoleAppService.CreationDate = DateTime.Now;
                            newRoleAppService.CreatedBy = request.UserID;

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
            if (roleAppServiceRecord.Id > 0)
                query = query.Where(c => c.Id == roleAppServiceRecord.Id);

            if (roleAppServiceRecord.RoleId > 0)
                query = query.Where(c => c.RoleId == roleAppServiceRecord.RoleId);

            return query;
        }
    }
}

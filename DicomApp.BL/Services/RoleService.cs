using DicomApp.CommonDefinitions.DTO;
using DicomApp.DAL.DB;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using DicomApp.CommonDefinitions.Requests;
using DicomApp.Helpers;
using DicomApp.CommonDefinitions.Responses;
using System.Net;

namespace DicomApp.BL.Services
{
    public class RoleService : BaseService
    {
        public static RoleResponse ListRole(RoleRequest request)
        {
            var res = new RoleResponse();
            RunBase(request, res, (RoleRequest req) =>
             {

                 try
                 {
                     var query = request.context.Role.Where(c => !c.IsDeleted).Select(c => new RoleDTO
                     {
                         Id = c.Id,
                         Name = c.Name,
                         Editable = c.Editable.Value
                     });

                     if (request.RoleDTO != null)
                         query = ApplyFilter(query, request.RoleDTO);

                     query = OrderByDynamic(query, request.OrderByColumn, request.IsDesc);

                     if (request.PageSize > 0)
                         query = ApplyPaging(query, request.PageSize, request.PageIndex);

                     res.RoleDTOs = query.ToList();
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
             });
            return res;
        }

        public static RoleResponse DeleteRole(RoleRequest request)
        {
            var res = new RoleResponse();
            RunBase(request, res, (RoleRequest req) =>
             {
                 try
                 {
                     var role = request.context.Role.FirstOrDefault( r=> r.Id == request.RoleDTO.Id);
                     if (role != null)
                     {
                         //update role IsDeleted
                         role.IsDeleted = true;
                         role.ModificationDate = DateTime.Now;
                         request.context.SaveChanges();

                         res.Message = HttpStatusCode.OK.ToString();
                         res.Success = true;
                         res.StatusCode = HttpStatusCode.OK;

                     }
                     else
                     {
                         res.Message = "Invalid role";
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
             });
            return res;
        }

        public static RoleResponse EditRole(RoleRequest request)
        {

            var res = new RoleResponse();
            RunBase(request, res, (RoleRequest req) =>
            {
                try
                {
                    var model = request.RoleDTO;
                    var role = request.context.Role.Find(model.Id);
                    if (role != null)
                    {
                        //update whole role
                        role = AddOrEditRole(request.UserID, request.RoleDTO, role);
                        request.context.SaveChanges();

                        res.Message = HttpStatusCode.OK.ToString();
                        res.Success = true;
                        res.StatusCode = HttpStatusCode.OK;

                    }
                    else
                    {
                        res.Message = "Invalid role";
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
            });
            return res;
        }

        public static RoleResponse AddRole(RoleRequest request)
        {

            var res = new RoleResponse();
            RunBase(request, res, (RoleRequest req) =>
            {
                try
                {
                    var RoleExist = request.context.Role.Any(m => m.Name.ToLower() == request.RoleDTO.Name.ToLower() && !m.IsDeleted);
                    if (!RoleExist)
                    {
                        var role = AddOrEditRole(request.UserID, request.RoleDTO);
                        request.context.Role.Add(role);
                        request.context.SaveChanges();


                        //loop appservices
                        //insert in roleappservice
                        //roleid  appserviceid enalbled=false
                        var appservices = request.context.AppService.ToList();
                        foreach (var item in appservices)
                        {
                            var approle = new RoleAppService()
                            {
                                AppServiceId = item.Id,
                                CreatedBy = request.UserID,
                                CreationDate = DateTime.Now,
                                Enabled = false,
                                IsDeleted = false,
                                RoleId = role.Id
                            };
                        }
                        request.context.SaveChanges();
                        res.Message = HttpStatusCode.OK.ToString();
                        res.Success = true;
                        res.StatusCode = HttpStatusCode.OK;
                    }
                    else
                    {
                        res.Message = "Role already exist";
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
            });
            return res;
        }

        private static Role AddOrEditRole(int createdBy, RoleDTO record, Role oldRole = null)
        {
            if (oldRole == null)//new role
            {
                oldRole = new Role();
                oldRole.CreationDate = DateTime.Now;
                oldRole.CreatedBy = createdBy;
            }
            else
            {
                oldRole.ModificationDate = DateTime.Now;
                oldRole.ModifiedBy = createdBy;
            }
            oldRole.Editable = true;
            oldRole.Name = record.Name;
            return oldRole;
        }

        private static IQueryable<RoleDTO> ApplyFilter(IQueryable<RoleDTO> query, RoleDTO RoleDTO)
        {
            if (RoleDTO.Id > 0)
                query = query.Where(c => c.Id == RoleDTO.Id);


            if (!string.IsNullOrWhiteSpace(RoleDTO.Name))
                query = query.Where(c => c.Name.Contains(RoleDTO.Name));

            return query;
        }

    }
}
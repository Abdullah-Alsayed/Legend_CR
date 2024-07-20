using System;
using System.Linq;
using System.Net;
using DicomApp.BL.Services;
using DicomApp.CommonDefinitions.DTO;
using DicomApp.DAL.DB;
using DicomApp.Helpers;
using DicomDB.CommonDefinitions.DTO;
using DicomDB.CommonDefinitions.Requests;
using DicomDB.CommonDefinitions.Responses;

namespace DicomDB.BL.Services
{
    public class ComplainsService : BaseService
    {
        #region ComplainsServices
        public static ComplainsResponse GetComplains(ComplainsRequest request)
        {
            var res = new ComplainsResponse();
            RunBase(
                request,
                res,
                (ComplainsRequest req) =>
                {
                    try
                    {
                        var query = request.context.Complain.Select(p => new ComplainsDTO
                        {
                            ComplainsId = p.Id,
                            ActionBy = p.ActionBy,
                            CreateBy = p.ActionByNavigation.Name,
                            Comments = p.Comments,
                            Date = p.Date,
                            Department = p.Department,
                            Description = p.Description,
                            EmployeeId = p.EmployeeId,
                            EmployeeName = p.Employee.Name,
                            VendorId = p.VendorId,
                            VendorName = p.Vendor.Name,
                            IsSolved = p.IsSolved,
                            IsDeleted = p.IsDeleted,
                        });

                        if (request.ComplainsDTO != null)
                            query = ApplyFilter(query, request.ComplainsDTO);

                        query = OrderByDynamic(
                            query,
                            request.OrderByColumn ?? "ComplainsId",
                            request.IsDesc
                        );

                        if (request.PageSize > 0)
                            query = ApplyPaging(query, request.PageSize, request.PageIndex);

                        res.ComplainsDTOs = query.ToList();
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

        public static ComplainsResponse DeleteComplains(ComplainsRequest request)
        {
            var res = new ComplainsResponse();
            RunBase(
                request,
                res,
                (ComplainsRequest req) =>
                {
                    try
                    {
                        var model = request.ComplainsDTO;
                        var Complains = request.context.Complain.FirstOrDefault(c =>
                            c.Id == model.ComplainsId
                        );
                        if (Complains != null)
                        {
                            //update Agency IsDeleted
                            Complains.IsDeleted = true;
                            request.context.SaveChanges();

                            res.Message = SystemEnums.DeletedSuccessfully.ToString();
                            res.Success = true;
                            res.StatusCode = HttpStatusCode.OK;
                        }
                        else
                        {
                            res.Message = SystemEnums.InvalidData.ToString();
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

        public static ComplainsResponse AddComplains(ComplainsRequest request)
        {
            var res = new ComplainsResponse();
            RunBase(
                request,
                res,
                (ComplainsRequest req) =>
                {
                    try
                    {
                        var Complains = AddOrEditComplains(request.ComplainsDTO);
                        request.context.Complain.Add(Complains);
                        request.context.SaveChanges();

                        res.Message = "Added Successfully";
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

        public static ComplainsResponse SolveComplains(ComplainsRequest request)
        {
            var res = new ComplainsResponse();
            RunBase(
                request,
                res,
                (ComplainsRequest req) =>
                {
                    try
                    {
                        var model = request.ComplainsDTO;
                        var Complains = request.context.Complain.FirstOrDefault(c =>
                            c.Id == model.ComplainsId
                        );
                        if (Complains != null)
                        {
                            //update Solve Complains
                            Complains.IsSolved = true;
                            Complains.Comments = request.ComplainsDTO.Comments;
                            request.context.SaveChanges();

                            res.Message = "Solve Complains";
                            res.Success = true;
                            res.StatusCode = HttpStatusCode.OK;
                        }
                        else
                        {
                            res.Message = SystemEnums.InvalidData.ToString();
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

        public static Complain AddOrEditComplains(
            ComplainsDTO ComplainsRecord,
            Complain Complains = null
        )
        {
            if (Complains == null)
            {
                Complains = new Complain();
            }
            Complains.EmployeeId = ComplainsRecord.EmployeeId;
            Complains.VendorId = ComplainsRecord.VendorId;
            Complains.ActionBy = ComplainsRecord.ActionBy;
            Complains.Date = ComplainsRecord.Date;
            Complains.Description = ComplainsRecord.Description;
            Complains.Department = ComplainsRecord.Department;
            Complains.IsSolved = ComplainsRecord.Solved;
            Complains.Comments = ComplainsRecord.Comments;
            Complains.IsDeleted = ComplainsRecord.IsDeleted;
            return Complains;
        }

        public static IQueryable<ComplainsDTO> ApplyFilter(
            IQueryable<ComplainsDTO> query,
            ComplainsDTO record
        )
        {
            if (!string.IsNullOrEmpty(record.Search))
            {
                query = query.Where(c =>
                    (!string.IsNullOrEmpty(c.Department) && c.Department.Contains(record.Search))
                    || (!string.IsNullOrEmpty(c.VendorName) && c.VendorName.Contains(record.Search))
                    || (!string.IsNullOrEmpty(c.CreateBy) && c.CreateBy.Contains(record.Search))
                    || (
                        !string.IsNullOrEmpty(c.EmployeeName)
                        && c.EmployeeName.Contains(record.Search)
                    )
                );
            }
            if (!string.IsNullOrEmpty(record.Department))
                query = query.Where(p => p.Department.Contains(record.Department));

            if (record.ComplainsId > 0)
                query = query.Where(p => p.ComplainsId == record.ComplainsId);

            if (record.EmployeeId > 0)
                query = query.Where(p => p.EmployeeId == record.EmployeeId);

            if (record.VendorId > 0)
                query = query.Where(p => p.VendorId == record.VendorId);

            if (record.ActionBy > 0)
                query = query.Where(p => p.ActionBy == record.ActionBy);

            if (record.IsSolved.HasValue)
                query = query.Where(p => p.Solved == record.Solved);

            if (record.IsDeleted == false)
                query = query.Where(p => p.IsDeleted == false);

            if (record.DateFrom.HasValue)
                query = query.Where(p => p.Date >= record.DateFrom);

            if (record.DateTo.HasValue)
                query = query.Where(p => p.Date <= record.DateTo);

            if (!string.IsNullOrWhiteSpace(record.Department))
                query = query.Where(p =>
                    p.Department != null && p.Department.Contains(record.Department)
                );

            return query;
        }
        #endregion
    }
}

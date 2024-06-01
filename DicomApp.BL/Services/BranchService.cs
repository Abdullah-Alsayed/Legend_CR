using DicomApp.CommonDefinitions.DTO;
using DicomApp.CommonDefinitions.Requests;
using DicomApp.CommonDefinitions.Responses;
using DicomApp.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using DicomApp.DAL.DB;

namespace DicomApp.BL.Services
{
    public class BranchService : BaseService
    {


        public static BranchResponse GetBranchs(BranchRequest request)
        {
            var res = new BranchResponse();
            RunBase(request, res, (BranchRequest req) =>
            {
                try
                {
                    var query = request.context.Branch.Where(b => (b.IsDeleted.HasValue ? !b.IsDeleted.Value : true)).Select(p => new BranchDTO
                    {

                        BranchId = p.BranchId,
                        Address = p.Address,
                        BranchName = p.BranchName,
                        //City = p.City,
                        ContactPerson = p.ContactPerson,
                        CurrencyId = p.CurrencyId,
                        Description = p.Description,
                        Email = p.Email,
                        Phone = p.Phone,
                        State = p.State,
                        ZipCode = p.ZipCode,
                        IsDeleted = p.IsDeleted ?? false,

                    });

                    if (request.BranchDTO != null)
                        query = ApplyFilter(query, request.BranchDTO);

                    res.TotalCount = query.Count();

                    query = OrderByDynamic(query, request.OrderByColumn ?? "BranchId", request.IsDesc);

                    if (request.PageSize > 0)
                        query = ApplyPaging(query, request.PageSize, request.PageIndex);

                    res.BranchDTOs = query.ToList();
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
        public static BranchResponse DeleteBranch(BranchRequest request)
        {

            var res = new BranchResponse();
            RunBase(request, res, (BranchRequest req) =>
            {
                try
                {
                    var model = request.BranchDTO;
                    var Branch = request.context.Branch.FirstOrDefault(c => c.BranchId == model.BranchId);
                    if (Branch != null)
                    {
                        //update Agency IsDeleted
                        Branch.IsDeleted = true;
                        request.context.SaveChanges();

                        res.Message = MessageKey.DeletedSuccessfully.ToString();
                        res.Success = true;
                        res.StatusCode = HttpStatusCode.OK;
                    }
                    else
                    {
                        res.Message = MessageKey.InvalidData.ToString();
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


        public static BranchResponse AddBranch(BranchRequest request)
        {

            var res = new BranchResponse();
            RunBase(request, res, (BranchRequest req) =>
            {
                try
                {
                    var Branch = AddOrEditBranch(request.BranchDTO);
                    request.context.Branch.Add(Branch);
                    request.context.SaveChanges();

                    res.Message = "New Branch added successfully";
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

        public static BranchResponse EditBranch(BranchRequest request)
        {

            var res = new BranchResponse();
            RunBase(request, res, (BranchRequest req) =>
            {
                try
                {
                    var model = request.BranchDTO;
                    var Branch = request.context.Branch.Find(model.BranchId);
                    if (Branch != null)
                    {
                        //update whole Agency
                        Branch = AddOrEditBranch(request.BranchDTO, Branch);
                        request.context.SaveChanges();

                        res.Message = "Updated Successfully";
                        res.Success = true;
                        res.StatusCode = HttpStatusCode.OK;
                    }
                    else
                    {
                        res.Message = "Invalid";
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


        public static Branch AddOrEditBranch(BranchDTO branchRecord, Branch branch = null)
        {
            if (branch == null)
            {
                branch = new Branch();
            }


            branch.BranchId = branchRecord.BranchId;
            branch.Address = branchRecord.Address;
            branch.BranchName = branchRecord.BranchName;
            //branch.City = branchRecord.City;
            branch.ContactPerson = branchRecord.ContactPerson;
            branch.CurrencyId = branchRecord.CurrencyId;
            branch.Description = branchRecord.Description;
            branch.Email = branchRecord.Email;
            branch.Phone = branchRecord.Phone;
            branch.State = branchRecord.State;
            branch.ZipCode = branchRecord.ZipCode;
            return branch;
        }

        private static IQueryable<BranchDTO> ApplyFilter(IQueryable<BranchDTO> query, BranchDTO record)
        {
            if (!string.IsNullOrEmpty(record.Search))
            {
                query = query.Where
                    (c => (!string.IsNullOrEmpty(c.BranchName) && c.BranchName.Contains(record.Search))
                        || (!string.IsNullOrEmpty(c.Phone) && c.Phone.Contains(record.Search)));
            }

            if (record.BranchId > 0)
            {
                query = query.Where(p => p.BranchId == record.BranchId);
            }
            if (!string.IsNullOrWhiteSpace(record.Address))
            {
                query = query.Where(p => p.Address != null && p.Address.Contains(record.Address));
            }
            if (!string.IsNullOrWhiteSpace(record.BranchName))
            {
                query = query.Where(p => p.BranchName != null && p.BranchName.Contains(record.BranchName));
            }
            if (!string.IsNullOrWhiteSpace(record.City))
            {
                query = query.Where(p => p.City != null && p.City.Contains(record.City));
            }
            if (!string.IsNullOrWhiteSpace(record.ContactPerson))
            {
                query = query.Where(p => p.ContactPerson != null && p.ContactPerson.Contains(record.ContactPerson));
            }

            if (!string.IsNullOrWhiteSpace(record.Description))
            {
                query = query.Where(p => p.Description != null && p.Description.Contains(record.Description));
            }
            if (!string.IsNullOrWhiteSpace(record.Email))
            {
                query = query.Where(p => p.Email != null && p.Email.Contains(record.Email));
            }
            if (!string.IsNullOrWhiteSpace(record.Phone))
            {
                query = query.Where(p => p.Phone != null && p.Phone.Contains(record.Phone));
            }
            if (!string.IsNullOrWhiteSpace(record.State))
            {
                query = query.Where(p => p.State != null && p.State.Contains(record.State));
            }
            if (!string.IsNullOrWhiteSpace(record.ZipCode))
            {
                query = query.Where(p => p.ZipCode != null && p.ZipCode.Contains(record.ZipCode));
            }
            return query;
        }
    }
}
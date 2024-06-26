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

    public class ContactUsService : BaseService
    {
        public static ContactUsResponse GetContactUss(ContactUsRequest request)
        {
            var res = new ContactUsResponse();
            RunBase(request, res, (ContactUsRequest req) =>
            {
                try
                {
                    var query = request.context.ContactUs.Select(p => new ContactUsDTO
                    {
                        Email = p.Email,
                        FirstName = p.FirstName,
                        LastName = p.LastName,
                        Id = p.Id,
                        Message = p.Message,
                        PhoneNumber = p.PhoneNumber,
                        Subject = p.Subject,
                        User = p.User,
                        UserID = p.UserId,
                        UserName = p.User.Name
                    });

                    if (request.ContactUsDTO != null)
                        query = ApplyFilter(query, request.ContactUsDTO);

                    res.TotalCount = query.Count();

                    query = OrderByDynamic(query, request.OrderByColumn ?? "Id", request.IsDesc);

                    if (request.PageSize > 0)
                        query = ApplyPaging(query, request.PageSize, request.PageIndex);

                    res.ContactUsDTOs = query.ToList();
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
     

        public static ContactUsResponse AddContactUs(ContactUsRequest request)
        {
            var res = new ContactUsResponse();
            RunBase(request, res, (ContactUsRequest req) =>
            {
                try
                {
                    ContactUs ContactUs = new ContactUs()
                    {
                        FirstName = request.ContactUsDTO.FirstName,
                        LastName = request.ContactUsDTO.LastName,
                        PhoneNumber = request.ContactUsDTO.PhoneNumber,
                        Email = request.ContactUsDTO.Email,
                        Message = request.ContactUsDTO.Message,
                        Subject = request.ContactUsDTO.Subject,
                        UserId = request.UserID,
                    };

                    request.context.ContactUs.Add(ContactUs);
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
            });
            return res;
        }

        public static ContactUs AddOrEditContactUs(ContactUsDTO ContactUsRecord, ContactUs ContactUs = null)
        {
            return ContactUs;
        }

        private static IQueryable<ContactUsDTO> ApplyFilter(IQueryable<ContactUsDTO> query, ContactUsDTO record)
        {
            if (!string.IsNullOrEmpty(record.Search))
            {
                query = query.Where
                    (c => (!string.IsNullOrEmpty(c.FirstName) && c.FirstName.Contains(record.Search))
                        || (!string.IsNullOrEmpty(c.LastName) && c.LastName.Contains(record.Search))
                        || (!string.IsNullOrEmpty(c.Subject) && c.Subject.Contains(record.Search)));
            }
            return query;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using DicomApp.CommonDefinitions.DTO;
using DicomApp.CommonDefinitions.Requests;
using DicomApp.CommonDefinitions.Responses;
using DicomApp.DAL.DB;
using DicomApp.Helpers;

namespace DicomApp.BL.Services
{
    public class TestimonialService : BaseService
    {
        public static TestimonialResponse GetTestimonials(TestimonialRequest request)
        {
            var res = new TestimonialResponse();
            RunBase(
                request,
                res,
                (TestimonialRequest req) =>
                {
                    try
                    {
                        var query = request
                            .context.Testimonial.Where(z => !z.IsDeleted)
                            .Select(p => new TestimonialDTO
                            {
                                Comment = p.Comment,
                                CreatedAt = p.CreatedAt,
                                CreatedBy = p.CreatedBy,
                                IsDeleted = p.IsDeleted,
                                LastModifiedAt = p.LastModifiedAt,
                                LastModifiedBy = p.LastModifiedBy,
                                Rate = p.Rate,
                                TestimonialId = p.TestimonialId,
                                UserDTO = new UserDTO { Name = p.CreatedByNavigation.Name }
                            });

                        if (request.TestimonialDTO != null)
                            query = ApplyFilter(query, request.TestimonialDTO);

                        query = OrderByDynamic(
                            query,
                            request.OrderByColumn ?? nameof(Testimonial.CreatedAt),
                            request.IsDesc
                        );

                        if (request.PageSize > 0)
                            query = ApplyPaging(query, request.PageSize, request.PageIndex);

                        res.TestimonialDTOs = query.ToList();
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

        public static TestimonialResponse CanAddTestimonials(TestimonialRequest request)
        {
            var res = new TestimonialResponse();
            RunBase(
                request,
                res,
                (TestimonialRequest req) =>
                {
                    try
                    {
                        if (!request.context.Testimonial.Any(x => x.CreatedBy == req.UserID))
                        {
                            res.Message = "you have already evaluated";
                            res.Success = false;
                            res.StatusCode = HttpStatusCode.OK;
                        }
                        else
                        {
                            res.Message = "Added Successfully";
                            res.Success = true;
                            res.StatusCode = HttpStatusCode.OK;
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

        public static TestimonialResponse AddTestimonial(TestimonialRequest request)
        {
            var res = new TestimonialResponse();
            RunBase(
                request,
                res,
                (TestimonialRequest req) =>
                {
                    try
                    {
                        if (!request.context.Testimonial.Any(x => x.CreatedBy == req.UserID))
                        {
                            res.Message = "you have already evaluated";
                            res.Success = false;
                            res.StatusCode = HttpStatusCode.OK;
                        }
                        else
                        {
                            var Testimonial = new Testimonial()
                            {
                                Comment = req.TestimonialDTO.Comment,
                                CreatedAt = DateTime.Now,
                                CreatedBy = req.UserID,
                                IsDeleted = req.TestimonialDTO.IsDeleted,
                                LastModifiedAt = req.TestimonialDTO.LastModifiedAt,
                                LastModifiedBy = req.TestimonialDTO.LastModifiedBy,
                                Rate = req.TestimonialDTO.Rate,
                                TestimonialId = req.TestimonialDTO.TestimonialId
                            };
                            request.context.Testimonial.Add(Testimonial);
                            request.context.SaveChanges();
                            res.Message = "Added Successfully";
                            res.Success = true;
                            res.StatusCode = HttpStatusCode.OK;
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

        public static TestimonialResponse DeleteTestimonial(TestimonialRequest request)
        {
            var res = new TestimonialResponse();
            RunBase(
                request,
                res,
                (TestimonialRequest req) =>
                {
                    try
                    {
                        var Testimonial = request.context.Testimonial.FirstOrDefault(c =>
                            c.TestimonialId == request.TestimonialDTO.TestimonialId
                        );
                        if (Testimonial != null)
                        {
                            Testimonial.IsDeleted = true;
                            Testimonial.DeletedOn = DateTime.UtcNow;
                            Testimonial.DeleteBy = request.UserID;
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

        private static IQueryable<TestimonialDTO> ApplyFilter(
            IQueryable<TestimonialDTO> query,
            TestimonialDTO record
        )
        {
            if (record.TestimonialId > 0)
                query = query.Where(p => p.TestimonialId == record.TestimonialId);

            if (!string.IsNullOrEmpty(record.Search))
            {
                record.Search = record.Search.ToLower();
                query = query.Where(c =>
                    c.Comment.ToLower().Contains(record.Search)
                    || c.UserDTO.Name.ToLower().Contains(record.Search)
                );
            }

            if (record.DateFrom.HasValue)
                query = query.Where(p => p.CreatedAt.Date >= record.DateFrom);

            if (record.DateTo.HasValue)
                query = query.Where(p => p.CreatedAt.Date <= record.DateTo);

            return query;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using DicomApp.CommonDefinitions.DTO;
using DicomApp.CommonDefinitions.DTO.AdvertisementDTOs;
using DicomApp.CommonDefinitions.Requests;
using DicomApp.CommonDefinitions.Responses;
using DicomApp.DAL.DB;
using DicomApp.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DicomApp.BL.Services
{
    public class CategoryService : BaseService
    {
        public static CategoryResponse GetCategorys(CategoryRequest request)
        {
            var res = new CategoryResponse();
            RunBase(
                request,
                res,
                (CategoryRequest req) =>
                {
                    try
                    {
                        var query = request
                            .context.Category.Where(x => !x.IsDeleted)
                            .Select(p => new CategoryDTO
                            {
                                Id = p.Id,
                                CreatedBy = p.CreatedBy,
                                LastModifiedAt = p.LastModifiedAt,
                                LastModifiedBy = p.LastModifiedBy,
                                NameEn = p.NameEn,
                                NameAr = p.NameAr,
                                CreatedAt = p.CreatedAt,
                            });

                        if (request.CategoryDTO != null)
                            query = ApplyFilter(query, request.CategoryDTO);

                        res.TotalCount = query.Count();

                        query = OrderByDynamic(
                            query,
                            request.OrderByColumn ?? "Id",
                            request.IsDesc
                        );

                        if (request.PageSize > 0)
                            query = ApplyPaging(query, request.PageSize, request.PageIndex);

                        res.CategoryDTOs = query.ToList();
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

        public static CategoryResponse GetCategory(CategoryRequest request)
        {
            var res = new CategoryResponse();
            RunBase(
                request,
                res,
                (CategoryRequest req) =>
                {
                    try
                    {
                        var query = request
                            .context.Category.Where(x => x.Id == req.ID)
                            .Select(p => new CategoryDTO
                            {
                                Id = p.Id,
                                CreatedBy = p.CreatedBy,
                                LastModifiedAt = p.LastModifiedAt,
                                LastModifiedBy = p.LastModifiedBy,
                                NameEn = p.NameEn,
                                NameAr = p.NameAr,
                                CreatedAt = p.CreatedAt,
                            });

                        if (request.CategoryDTO != null)
                            query = ApplyFilter(query, request.CategoryDTO);

                        res.TotalCount = query.Count();

                        query = OrderByDynamic(
                            query,
                            request.OrderByColumn ?? "Id",
                            request.IsDesc
                        );

                        if (request.PageSize > 0)
                            query = ApplyPaging(query, request.PageSize, request.PageIndex);

                        res.CategoryDTO = query.FirstOrDefault();
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

        public static CategoryResponse DeleteCategory(CategoryRequest request)
        {
            var res = new CategoryResponse();
            RunBase(
                request,
                res,
                (CategoryRequest req) =>
                {
                    try
                    {
                        var model = request.CategoryDTO;
                        var Category = request.context.Category.FirstOrDefault(c =>
                            c.Id == model.Id
                        );
                        if (Category != null)
                        {
                            Category.IsDeleted = true;
                            Category.DeletedOn = DateTime.UtcNow;
                            Category.DeleteBy = request.UserID;
                            request.context.SaveChanges();

                            res.CategoryDTOs = new List<CategoryDTO>
                            {
                                new CategoryDTO
                                {
                                    Id = Category.Id,
                                    NameAr = Category.NameAr,
                                    NameEn = Category.NameEn,
                                    LastModifiedAt = DateTime.Now,
                                    CreatedAt = Category.CreatedAt,
                                    CreatedBy = Category.CreatedBy,
                                }
                            };
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
                }
            );
            return res;
        }

        public static CategoryResponse AddCategory(CategoryRequest request)
        {
            var res = new CategoryResponse();
            RunBase(
                request,
                res,
                (CategoryRequest req) =>
                {
                    try
                    {
                        var Category = AddOrEditCategory(request.CategoryDTO);
                        request.context.Category.Add(Category);
                        request.context.SaveChanges();

                        res.CategoryDTOs = new List<CategoryDTO>
                        {
                            new CategoryDTO
                            {
                                Id = Category.Id,
                                NameAr = Category.NameAr,
                                NameEn = Category.NameEn,
                                CreatedAt = Category.CreatedAt,
                                CreatedBy = Category.CreatedBy,
                                LastModifiedAt = Category.LastModifiedAt,
                                LastModifiedBy = Category.LastModifiedBy
                            }
                        };
                        res.Message = "AddedSuccessfully";
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

        public static CategoryResponse EditCategory(CategoryRequest request)
        {
            var res = new CategoryResponse();
            RunBase(
                request,
                res,
                (CategoryRequest req) =>
                {
                    try
                    {
                        var model = request.CategoryDTO;
                        var Category = request.context.Category.Find(model.Id);
                        if (Category != null)
                        {
                            //update whole Agency
                            Category = AddOrEditCategory(request.CategoryDTO, Category);
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
                }
            );
            return res;
        }

        public static Category AddOrEditCategory(CategoryDTO CategoryDTO, Category Category = null)
        {
            if (Category == null)
            {
                Category = new Category();
                Category.Id = CategoryDTO.Id;
                Category.CreatedAt = CategoryDTO.CreatedAt;
                Category.CreatedBy = CategoryDTO.CreatedBy;
            }
            Category.NameAr = CategoryDTO.NameAr;
            Category.NameEn = CategoryDTO.NameEn;
            Category.LastModifiedBy = CategoryDTO.LastModifiedBy;
            Category.LastModifiedAt = CategoryDTO.LastModifiedAt;

            return Category;
        }

        private static IQueryable<CategoryDTO> ApplyFilter(
            IQueryable<CategoryDTO> query,
            CategoryDTO record
        )
        {
            if (!string.IsNullOrEmpty(record.Search))
            {
                query = query.Where(c =>
                    (!string.IsNullOrEmpty(c.NameEn) && c.NameEn.Contains(record.Search))
                    || (!string.IsNullOrEmpty(c.NameAr) && c.NameAr.Contains(record.Search))
                );
            }

            if (record.Id > 0)
            {
                query = query.Where(p => p.Id == record.Id);
            }
            return query;
        }
    }
}

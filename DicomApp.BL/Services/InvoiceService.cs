using System;
using System.Linq;
using System.Net;
using DicomApp.CommonDefinitions.DTO;
using DicomApp.CommonDefinitions.DTO.AdvertisementDTOs;
using DicomApp.CommonDefinitions.Requests;
using DicomApp.CommonDefinitions.Responses;
using DicomApp.DAL.DB;
using DicomApp.Helpers;

namespace DicomApp.BL.Services
{
    public class InvoiceService : BaseService
    {
        public static InvoiceResponse GetAll(InvoiceRequest request)
        {
            var response = new InvoiceResponse();
            RunBase(
                request,
                response,
                (InvoiceRequest req) =>
                {
                    try
                    {
                        IQueryable<Invoice> ship;

                        if (!string.IsNullOrEmpty(request.InvoiceDTO.RefId))
                            ship = request.context.Invoices.Where(s =>
                                s.InvoiceId == request.InvoiceDTO.InvoiceId && !s.IsDeleted
                            );
                        else
                            ship = request.context.Invoices.Where(s => !s.IsDeleted);

                        if (request.PageSize > 0)
                            ship = ApplyPaging(ship, request.PageSize, request.PageIndex);

                        if (request.applyFilter)
                            ship = ApplyFilter(ship, request.InvoiceDTO, req.RoleID, req.UserID);

                        if (!string.IsNullOrEmpty(request.OrderByColumn))
                            ship = OrderByDynamic(ship, request.OrderByColumn, request.IsDesc);

                        var query = ship.Select(s => new InvoiceDTO
                        {
                            InvoiceId = s.InvoiceId,
                            AdvertisementId = s.AdvertisementId,
                            GamerServiceId = s.GamerServiceId,
                            InvoiceType = s.InvoiceType,
                            IsRefund = s.IsRefund,
                            RefId = s.RefId,
                            Price = s.Price,
                            IsDeleted = s.IsDeleted,
                            CreatedAt = s.CreatedAt,
                            CreatedBy = s.CreatedBy,
                            LastModifiedAt = s.LastModifiedAt,
                            LastModifiedBy = s.LastModifiedBy,
                            GamerService = s.GamerServiceId.HasValue
                                ? new ServiceDTO
                                {
                                    GameServiceType = s.GamerService.GameServiceType
                                }
                                : null,
                        });
                        response.InvoiceDTOs = query.ToList();
                        response.Message = HttpStatusCode.OK.ToString();
                        response.Success = true;
                        response.StatusCode = HttpStatusCode.OK;
                    }
                    catch (Exception ex)
                    {
                        response.Message = ex.Message;
                        response.Success = false;
                        LogHelper.LogException(ex.Message, ex.StackTrace);
                    }
                    return response;
                }
            );
            return response;
        }

        #region Filters

        private static IQueryable<Invoice> ApplyFilter(
            IQueryable<Invoice> query,
            InvoiceDTO filter,
            int RoleId,
            int UserId
        )
        {
            if (!string.IsNullOrEmpty(filter.Search))
            {
                filter.Search = filter.Search.ToLower();
                query = query.Where(c =>
                    c.RefId.ToLower().Contains(filter.Search)
                    || c.Advertisement.RefId.ToLower().Contains(filter.Search)
                    || c.GamerService.RefId.Contains(filter.Search)
                    || c.Advertisement.UserName.Contains(filter.Search)
                    || c.GamerService.UserName.Contains(filter.Search)
                );
            }

            if (filter.InvoiceId > 0)
                query = query.Where(c => c.InvoiceId == filter.InvoiceId);

            if (!string.IsNullOrEmpty(filter.RefId))
                query = query.Where(c => c.RefId == filter.RefId);

            if (filter.InvoiceId != 0)
                query = query.Where(s => filter.InvoiceId == s.InvoiceId);

            if (filter.InvoiceType != 0)
                query = query.Where(p => p.InvoiceType == filter.InvoiceType);

            if (filter.Price != 0)
                query = query.Where(c => c.Price > filter.Price);

            if (filter.LessPrice != 0)
                query = query.Where(c => c.Price < filter.LessPrice);

            if (filter.GreeterPrice != 0)
                query = query.Where(c => c.Price > filter.GreeterPrice);

            if (filter.DateFrom.HasValue)
                query = query.Where(p => p.CreatedAt.Date >= filter.DateFrom);

            if (filter.DateTo.HasValue)
                query = query.Where(p => p.CreatedAt.Date <= filter.DateTo);

            return query;
        }

        #endregion
    }
}

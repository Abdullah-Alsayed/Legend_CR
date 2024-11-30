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
        public static InvoiceResponse AddInvoice(InvoiceRequest request)
        {
            var response = new InvoiceResponse();
            RunBase(
                request,
                response,
                (InvoiceRequest req) =>
                {
                    try
                    {
                        var status = request.context.Status.FirstOrDefault(x =>
                            x.StatusType == (int)StatusTypeEnum.Sold
                        );
                        var Inv = new Invoice()
                        {
                            InvoiceType = request.InvoiceDTO.InvoiceType,
                            IsRefund = false,
                            CreatedAt = DateTime.Now,
                            CreatedBy = request.UserID,
                            IsDeleted = false,
                        };
                        if (request.InvoiceDTO.InvoiceType == (int)InvoiceTypeEnum.Advertisement)
                        {
                            var advertisement = request.context.Advertisement.FirstOrDefault(x =>
                                x.AdvertisementId == request.InvoiceDTO.ItemId
                            );
                            if (advertisement != null)
                            {
                                Inv.AdvertisementId = advertisement.AdvertisementId;
                                Inv.Price = advertisement.Price;
                                advertisement.StatusId = status.Id;
                                var follow = new HistoryDTO
                                {
                                    Message = request
                                        .Localizer["createInvoice", Inv.RefId]
                                        .ToString(),
                                    CreatedBy = request.UserID,
                                    AdvertisementId = advertisement.AdvertisementId,
                                    EntityType = EntityTypeEnum.Invoice,
                                    ActionType = ActionTypeEnum.Add,
                                    CreatedAt = DateTime.Now
                                };
                                FollowUpService.Add(follow, request.context);
                            }
                            else
                                return new InvoiceResponse()
                                {
                                    Message = request.Localizer[SystemConstants.Message.NotFound],
                                    Success = false
                                };
                        }
                        else
                        {
                            var gamerService = request.context.GamerService.FirstOrDefault(x =>
                                x.GamerServiceId == request.InvoiceDTO.ItemId
                            );
                            if (gamerService != null)
                            {
                                Inv.GamerServiceId = gamerService.GamerServiceId;
                                Inv.Price = gamerService.Price;
                                gamerService.StatusId = status.Id;
                            }
                            else
                                return new InvoiceResponse()
                                {
                                    Message = request.Localizer[SystemConstants.Message.NotFound],
                                    Success = false
                                };
                        }

                        request.context.Invoices.Add(Inv);
                        request.context.SaveChanges();
                        Inv.RefId = BaseHelper.GenerateRefId(EnumRefIdType.Invoice, Inv.InvoiceId);

                        //Add notification
                        request.context.Notification.Add(
                            new Notification
                            {
                                Body = "New Invoice " + Inv.RefId + " added",
                                CreationDate = DateTime.Now,
                                Icon = Inv.RefId,
                                SenderId = request.UserID,
                                Title = "New Invoice",
                                RecipientRoleId = request
                                    .context.Role.FirstOrDefault(x =>
                                        x.Name == SystemConstants.Role.Admin
                                    )
                                    ?.Id,
                                IsDeleted = false,
                                IsSeen = false,
                                RecipientId = 0,
                            }
                        );

                        request.context.SaveChanges();
                        response.Message = request.Localizer[SystemConstants.Message.Success];
                        response.Success = true;
                        response.StatusCode = HttpStatusCode.OK;
                    }
                    catch (Exception ex)
                    {
                        response.Message = ex.Message;
                        response.Success = false;
                        response.InvoiceDTO = request.InvoiceDTO;
                        LogHelper.LogException(ex.Message, ex.StackTrace);
                    }
                    return response;
                }
            );
            return response;
        }

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
                            GamerService =
                                s.GamerService != null
                                    ? new ServiceDTO
                                    {
                                        GameServiceType = s.GamerService.GameServiceType
                                    }
                                    : null,
                            Advertisement =
                                s.Advertisement != null
                                    ? new AdsDTO { RefId = s.Advertisement.RefId }
                                    : null
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

        public static InvoiceResponse RefundInvoice(InvoiceRequest request)
        {
            var response = new InvoiceResponse();
            RunBase(
                request,
                response,
                (InvoiceRequest req) =>
                {
                    try
                    {
                        var invoice = request.context.Invoices.FirstOrDefault(x =>
                            x.InvoiceId == request.InvoiceDTO.InvoiceId
                        );
                        if (invoice != null)
                        {
                            if (invoice.InvoiceType == (int)InvoiceTypeEnum.Advertisement)
                            {
                                invoice.IsRefund = true;
                                invoice.LastModifiedAt = DateTime.Now;
                                invoice.LastModifiedBy = request.UserID;
                                var advertisement = request.context.Advertisement.FirstOrDefault(
                                    x => x.AdvertisementId == invoice.AdvertisementId
                                );
                                var publishStatus = request.context.Status.FirstOrDefault(x =>
                                    x.StatusType == (int)StatusTypeEnum.Published
                                );
                                if (advertisement != null && publishStatus != null)
                                {
                                    advertisement.StatusId = publishStatus.Id;
                                    //Add follow up
                                    var follow = new HistoryDTO
                                    {
                                        Message = request.Localizer["refundInvoice", invoice.RefId],
                                        CreatedBy = request.UserID,
                                        CreatedAt = DateTime.Now,
                                        ActionType = ActionTypeEnum.Edit,
                                        EntityType = EntityTypeEnum.Invoice
                                    };
                                    FollowUpService.Add(follow, request.context);
                                }
                                else
                                    return new InvoiceResponse()
                                    {
                                        Message = "Not Found",
                                        Success = false
                                    };
                            }
                            else
                            {
                                var gamerService = request.context.GamerService.FirstOrDefault(x =>
                                    x.GamerServiceId == invoice.GamerServiceId
                                );
                                var acceptStatus = request.context.Status.FirstOrDefault(x =>
                                    x.StatusType == (int)StatusTypeEnum.Accept
                                );
                                if (gamerService != null && acceptStatus != null)
                                    gamerService.StatusId = acceptStatus.Id;
                                else
                                    return new InvoiceResponse()
                                    {
                                        Message = request.Localizer[
                                            SystemConstants.Message.NotFound
                                        ],
                                        Success = false
                                    };
                            }
                            request.context.Invoices.Remove(invoice);
                            request.context.SaveChanges();

                            //Add notification
                            request.context.Notification.Add(
                                new Notification
                                {
                                    Body = "Invoice " + invoice.RefId + " Refunded",
                                    CreationDate = DateTime.Now,
                                    Icon = invoice.RefId,
                                    SenderId = request.UserID,
                                    Title = "Refunded Invoice",
                                    RecipientRoleId = request
                                        .context.Role.FirstOrDefault(x =>
                                            x.Name == SystemConstants.Role.Admin
                                        )
                                        ?.Id,
                                    IsDeleted = false,
                                    IsSeen = false,
                                    RecipientId = 0,
                                }
                            );

                            request.context.SaveChanges();
                            response.Message = response.Message = request.Localizer[
                                SystemConstants.Message.Success
                            ];
                            response.Success = true;
                            response.StatusCode = HttpStatusCode.OK;
                        }
                        else
                            return new InvoiceResponse()
                            {
                                Message = request.Localizer[SystemConstants.Message.NotFound],
                                Success = false
                            };
                    }
                    catch (Exception ex)
                    {
                        response.Message = ex.Message;
                        response.Success = false;
                        response.InvoiceDTO = request.InvoiceDTO;
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
            query = query.Where(s => s.IsRefund == filter.IsRefund);
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

using System;
using System.Linq;
using System.Net;
using DicomApp.CommonDefinitions.DTO;
using DicomApp.CommonDefinitions.Requests;
using DicomApp.CommonDefinitions.Responses;
using DicomApp.DAL.DB;
using DicomApp.Helpers;

namespace DicomApp.BL.Services
{
    public class FollowUpService : BaseService
    {
        public static void Add(HistoryDTO followUpDTO, ShippingDBContext context)
        {
            context.FollowUp.Add(
                new FollowUp
                {
                    Id = followUpDTO.Id,
                    AdvertisementId = followUpDTO.AdvertisementId,
                    ActionType = followUpDTO.ActionType,
                    CreatedAt = followUpDTO.CreatedAt,
                    CreatedBy = followUpDTO.CreatedBy,
                    Message = followUpDTO.Message,
                    EntityType = followUpDTO.EntityType,
                }
            );
        }

        public static FollowUpResponse GetAll(FollowUpRequest request)
        {
            var res = new FollowUpResponse();
            RunBase(
                request,
                res,
                (FollowUpRequest req) =>
                {
                    try
                    {
                        var query = request.context.FollowUp.Select(p => new HistoryDTO
                        {
                            Id = p.Id,
                            Message = p.Message,
                            EntityType = p.EntityType,
                            AdvertisementId = p.AdvertisementId,
                            ActionType = p.ActionType,
                            CreatedBy = p.CreatedBy,
                            CreatedAt = p.CreatedAt,
                            User = new UserDTO
                            {
                                Name = p.CreatedByNavigation.Name,
                                ImgUrl = p.CreatedByNavigation.ImgUrl,
                            }
                        });

                        if (request.applyFilter)
                            query = ApplyFilter(query, request.FollowUpDTO);

                        res.TotalCount = query.Count();
                        query = OrderByDynamic(
                            query,
                            request.OrderByColumn ?? "Id",
                            request.IsDesc
                        );

                        if (request.PageSize > 0)
                            query = ApplyPaging(query, request.PageSize, request.PageIndex);

                        res.FollowUpDTOs = query.ToList();
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

        private static IQueryable<HistoryDTO> ApplyFilter(
            IQueryable<HistoryDTO> query,
            HistoryDTO record
        )
        {
            return query;
        }
    }
}

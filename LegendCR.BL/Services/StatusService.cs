using System;
using System.Linq;
using System.Net;
using LegendCR.CommonDefinitions.DTO;
using LegendCR.CommonDefinitions.Requests;
using LegendCR.CommonDefinitions.Responses;
using LegendCR.Helpers;

namespace LegendCR.BL.Services
{
    public class StatusService : BaseService
    {
        public static StatusResponse GetStatus(StatusRequest request)
        {
            var response = new StatusResponse();
            RunBase(
                request,
                response,
                (StatusRequest req) =>
                {
                    try
                    {
                        var query = request.context.Statuses.Select(c => new StatusDTO
                        {
                            Id = c.ID,
                            Name = c.Name
                        });

                        response.StatusDTOs = query.ToList();

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
    }
}

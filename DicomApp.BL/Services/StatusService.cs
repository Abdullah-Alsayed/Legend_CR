﻿using System;
using System.Linq;
using System.Net;
using DicomApp.CommonDefinitions.DTO;
using DicomApp.CommonDefinitions.Requests;
using DicomApp.CommonDefinitions.Responses;
using DicomApp.Helpers;

namespace DicomApp.BL.Services
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
                        var query = request.context.Status.Select(c => new StatusDTO
                        {
                            Id = c.Id,
                            NameEN = c.NameEN
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

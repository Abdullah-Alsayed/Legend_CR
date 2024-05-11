using LegendCR.CommonDefinitions.DTO;
using LegendCR.DAL.DB;
using System;

namespace LegendCR.BL.Services
{
    public static class FollowUpService
    {
        public static void Add(FollowUpDTO followUpDTO, ApplicationDBContext context)
        {
            context.FollowUp.Add(new FollowUp
            {
                ShipmentId = followUpDTO.ShipmentId,
                StatusId = followUpDTO.StatusId,
                FollowUpTypeId = followUpDTO.FollowUpTypeId,
                Title = followUpDTO.Title,
                Comment = followUpDTO.Comment,
                CreatedBy = followUpDTO.CreatedBy,
                LastModifiedBy = followUpDTO.CreatedBy,
                CreatedAt = DateTime.Now,
                LastModifiedAt = DateTime.Now,
                IsDeleted = false

            });
        }
    }
}
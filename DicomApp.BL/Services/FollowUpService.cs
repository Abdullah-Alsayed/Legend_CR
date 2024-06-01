using System;
using DicomApp.CommonDefinitions.DTO;
using DicomApp.DAL.DB;

namespace DicomApp.BL.Services
{
    public static class FollowUpService
    {
        public static void Add(FollowUpDTO followUpDTO, ShippingDBContext context)
        {
            context.FollowUp.Add(
                new FollowUp
                {
                    AdvertisementId = followUpDTO.AdvertisementId,
                    StatusId = followUpDTO.StatusId,
                    FollowUpTypeId = followUpDTO.FollowUpTypeId,
                    Title = followUpDTO.Title,
                    Comment = followUpDTO.Comment,
                    CreatedBy = followUpDTO.CreatedBy,
                    LastModifiedBy = followUpDTO.CreatedBy,
                    CreatedAt = DateTime.Now,
                    LastModifiedAt = DateTime.Now,
                    IsDeleted = false
                }
            );
        }
    }
}

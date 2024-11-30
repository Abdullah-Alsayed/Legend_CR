using System;
using DicomApp.DAL.DB;

namespace DicomApp.CommonDefinitions.DTO
{
    public class HistoryDTO
    {
        public int Id { get; set; }
        public int? AdvertisementId { get; set; }
        public string Message { get; set; }
        public EntityTypeEnum EntityType { get; set; }
        public ActionTypeEnum ActionType { get; set; }

        public int CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public UserDTO User { get; set; }

        public string Search { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DicomApp.CommonDefinitions.DTO
{
    public class FollowUpDTO
    {
        public int? AdvertisementId { get; set; }
        public int? StatusId { get; set; }
        public string Comment { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedByName { get; set; }

        public DateTime CreatedAt { get; set; }
        public string Title { get; set; }
    }
}

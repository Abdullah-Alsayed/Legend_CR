using DicomApp.CommonDefinitions.DTO.ShipmentDTOs;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace DicomApp.CommonDefinitions.DTO
{
   public class PackingDTO
    {
        public int id { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string Size { get; set; }
        public string ImgUrl { get; set; }

        public int Count { get; set; }
        public Double Price { get; set; }
        public int PackingTypeId { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? LastModifiedBy { get; set; }
        public DateTime LastModifiedAt { get; set; }
        public string Description { get; set; }
        public IFormFile File { get; set; }
        public IEnumerable<ShipDTO> ShipDTOs { get; set; }
        public string Messege { get; set; }
        public string search { get; set; }
        public string PackingTypeName { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public int? BranchId { get; set; }
    }
}

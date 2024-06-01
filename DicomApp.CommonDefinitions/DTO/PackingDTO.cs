using System;
using System.Collections.Generic;
using System.Text;
using DicomApp.CommonDefinitions.DTO.ShipmentDTOs;
using Microsoft.AspNetCore.Http;

namespace DicomApp.CommonDefinitions.DTO
{
    public class GameDTO
    {
        public int Id { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string Size { get; set; }
        public string ImgUrl { get; set; }
        public int CategoryId { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? LastModifiedBy { get; set; }
        public DateTime LastModifiedAt { get; set; }
        public string Description { get; set; }
        public IFormFile File { get; set; }
        public IEnumerable<ShipDTO> ShipDTOs { get; set; }
        public string Messege { get; set; }
        public string search { get; set; }
        public string CategoryName { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
    }
}

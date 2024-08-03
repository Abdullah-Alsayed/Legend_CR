using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace DicomApp.CommonDefinitions.DTO
{
    public class GameChargeDTO
    {
        public int Id { get; set; }
        public int GameId { get; set; }
        public int Price { get; set; }
        public int Count { get; set; }
        public int Discount { get; set; }
        public string Img { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? LastModifiedBy { get; set; }
        public DateTime? LastModifiedAt { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedOn { get; set; }
        public int? DeleteBy { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public string Search { get; set; }
        public IFormFile File { get; set; }
        public GameDTO Game { get; set; }
    }
}

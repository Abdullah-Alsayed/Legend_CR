using System;
using DicomApp.DAL.DB;

namespace DicomApp.CommonDefinitions.DTO
{
    public class CategoryDTO
    {
        public string search;

        public int Id { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? LastModifiedBy { get; set; }
        public DateTime LastModifiedAt { get; set; }

        public string Search { get; set; }
    }
}

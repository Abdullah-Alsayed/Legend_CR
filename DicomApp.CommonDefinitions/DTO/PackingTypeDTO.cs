using DicomApp.DAL.DB;
using System;

namespace DicomApp.CommonDefinitions.DTO
{
    public class PackingTypeDTO
    {

        public int Id { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? LastModifiedBy { get; set; }
        public DateTime LastModifiedAt { get; set; }

        public int? BranchId { get; set; }

        public virtual Branch Branch { get; set; }
        public string Search { get; set; }
    }
}
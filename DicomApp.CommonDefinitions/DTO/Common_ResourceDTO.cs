using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using DicomApp.DAL.DB;
using System.Linq;
using DicomApp.Helpers;

namespace DicomApp.CommonDefinitions.DTO
{
    public class Common_ResourceDTO
    {
        public int ID { get; set; }
        public string ResourceKey { get; set; }
        public string ResourceValue { get; set; }
        public string ResourceLang { get; set; }
        public int? CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }

        [IgnoreDataMember]
        public DateTime? LastUpdateDate { get; set; }
        public string LastUpdateDateStr
        {
            get { return DateTimeHelper.ToDate(LastUpdateDate ?? DateTime.Now); }
            set { }
        }

        [IgnoreDataMember]
        public DateTime CreationDate { get; set; }
        public string CreationDateStr
        {
            get { return DateTimeHelper.ToDate(CreationDate); }
            set { }
        }
        public bool IsDeleted { get; set; }
        public int? ApplicationId { get; set; }
        public string Url { get; set; }
        public string MediaUrl { get; set; }
    }
}
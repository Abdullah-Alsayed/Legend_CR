
using System;
using System.Collections.Generic;

namespace LegendCR.DAL.DB
{
    public partial class CommonResource
    {
        public int Id { get; set; }
        public string ResourceKey { get; set; }
        public string ResourceValue { get; set; }
        public string ResourceLang { get; set; }
        public int? CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime LastUpdateDate { get; set; }
        public DateTime CreationDate { get; set; }
        public bool IsDeleted { get; set; }
        public int? ApplicationId { get; set; }
        public string Url { get; set; }
        public string MediaUrl { get; set; }
    }
}

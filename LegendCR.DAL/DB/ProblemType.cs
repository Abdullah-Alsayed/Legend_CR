
using System;
using System.Collections.Generic;

namespace LegendCR.DAL.DB
{
    public partial class ProblemType
    {
        public ProblemType()
        {
        }

        public int ProblemTypeId { get; set; }
        public string NameEn { get; set; }
        public string NameAr { get; set; }
        public int? Type { get; set; }
        public bool? IsDeleted { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace DicomApp.CommonDefinitions.DTO
{
    public class ProblemTypeDTO
    {
        public int ProblemTypeId { get; set; }
        public string NameEn { get; set; }
        public string NameAr { get; set; }
        public int? Type { get; set; }

        public bool IsDeleted { get; set; }

        public string Search { get; set; }
    }
}

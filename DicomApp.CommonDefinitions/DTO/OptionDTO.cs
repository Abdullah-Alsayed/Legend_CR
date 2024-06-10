using System;
using System.Collections.Generic;
using System.Text;

namespace DicomApp.CommonDefinitions.DTO
{
    public class OptionDTO
    {
        public string Action { get; set; }
        public string Controller { get; set; }
        public string SelectListName { get; set; } = "";
        public byte[] SelectListType { get; set; }
        public string PDFReportName { get; set; }
        public int Value { get; set; }

        public int Columns { get; set; }
        public bool? HasForm { get; set; }
        public string ClassName { get; set; }
        public int Count { get; set; }
        public LookupDTO Lookup { get; set; }
        public int StatusId { get; set; }
        public int StatusType { get; set; }
    }
}

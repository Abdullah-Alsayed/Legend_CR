using System;
using System.Collections.Generic;
using System.Text;
using DicomApp.Helpers;

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
        public bool FilterOnly { get; set; } = false;
        public int Columns { get; set; }
        public bool? HasForm { get; set; }
        public string ClassName { get; set; }
        public string Language { get; set; } = SystemConstants.Languages.English;

        public int Count { get; set; }
        public LookupDTO Lookup { get; set; }
        public int StatusId { get; set; }
        public int StatusType { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DicomApp.CommonDefinitions.DTO
{
    public class StatusDTO
    {
        public int Id { get; set; }
        public string NameEN { get; set; }
        public string NameAR { get; set; }

        public int StatusType { get; set; }
    }
}

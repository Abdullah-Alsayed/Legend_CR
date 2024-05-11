using System;
using System.Collections.Generic;
using System.Text;

namespace LegendCR.CommonDefinitions.DTO
{
    public class AppServiceGroupDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }
        public int? AppServiceClassId { get; set; }
        public string AppServiceClassName { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
}

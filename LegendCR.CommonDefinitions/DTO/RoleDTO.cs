using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LegendCR.CommonDefinitions.DTO
{
    public class RoleDTO
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Required")]
        public string Name { get; set; }

        public bool IsMaster { get; set; }
    }
}

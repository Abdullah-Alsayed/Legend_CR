using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DicomApp.CommonDefinitions.DTO
{
    public class RoleDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Required")]
        public string Name { get; set; }

        public bool Editable { get; set; }
    }
}

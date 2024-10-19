using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DicomApp.CommonDefinitions.DTO
{
    public class ResetPasswordDTO
    {
        public string EmailUser { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string Token { get; set; }
    }
}

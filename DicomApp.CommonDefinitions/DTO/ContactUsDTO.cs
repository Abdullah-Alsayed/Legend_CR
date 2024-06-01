using DicomApp.DAL.DB;
using System;
using System.Collections.Generic;
using System.Text;

namespace DicomApp.CommonDefinitions.DTO
{
    public class ContactUsDTO
    {
        public int Id { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }
        public string Subject { get; set; }
        public int UserID { get; set; }
        public string UserName { get; set; }

        public CommonUser User { get; set; }
        public string Search { get; set; }
    }
}

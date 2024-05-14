using System;
using System.Collections.Generic;
using System.Text;
using LegendCR.DAL.DB;

namespace LegendCR.CommonDefinitions.DTO
{
    public class ContactUsDTO
    {
        public Guid Id { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }
        public string Subject { get; set; }
        public string UserID { get; set; }
        public string UserName { get; set; }

        public User User { get; set; }
        public string Search { get; set; }
    }
}

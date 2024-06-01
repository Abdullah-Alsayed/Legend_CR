
using System;
using System.Collections.Generic;

namespace DicomApp.DAL.DB
{
    public partial class ContactUs
    {
        public int Id { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public int UserId { get; set; }

        public virtual CommonUser User { get; set; }
    }
}

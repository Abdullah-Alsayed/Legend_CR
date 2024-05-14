using System.ComponentModel.DataAnnotations;

namespace LegendCR.DAL.DB
{
    public class ContactUs : BaseEntity
    {
        [StringLength(100), Required]
        public string LastName { get; set; }

        [StringLength(100), Required]
        public string FirstName { get; set; }

        [
            RegularExpression(
                "^(\\+\\d{1,2}\\s?)?(1\\s?)?((\\(\\d{3}\\))|\\d{3})[\\s.-]?\\d{3}[\\s.-]?\\d{4}$"
            ),
            Required
        ]
        public string PhoneNumber { get; set; }

        [StringLength(100), Required, EmailAddress]
        public string Email { get; set; }

        [StringLength(100), Required]
        public string Subject { get; set; }

        [StringLength(225), Required]
        public string Message { get; set; }
    }
}

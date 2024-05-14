using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LegendCR.DAL.Enums;
using Microsoft.AspNetCore.Identity;

namespace LegendCR.DAL.DB
{
    public class User : IdentityUser
    {
        public User()
        {
            Account = new HashSet<Account>();
        }

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
        public string PhoneNumber { get; set; } = string.Empty;

        public DateOnly Birthday { get; set; }
        public string Lat { get; set; }
        public string Lng { get; set; }
        public string Address { get; set; }
        public string Bank { get; set; }
        public int AccountNumber { get; set; }
        public int IbanNumber { get; set; }
        public int VodafoneCashNumber { get; set; }
        public string InstPayAccount { get; set; }
        public string Photo { get; set; } = string.Empty;
        public DateTime? LastLoginDate { get; set; }
        public bool IsVerified { get; set; }
        public bool IsLoggedIn { get; set; }
        public UserGanderEnum Gander { get; set; }
        public LanguageEnum Language { get; set; }

        public string CreateBy { get; set; }

        public string ModifyBy { get; set; }

        public string DeletedBy { get; set; }

        [Required]
        public DateTime CreateAt { get; set; } = DateTime.Now;
        public DateTime? ModifyAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;

        public virtual ICollection<Account> Account { get; set; }
        public virtual ICollection<AccountFiles> AccountFiles { get; set; }
        public virtual ICollection<Category> Category { get; set; }
        public virtual ICollection<ContactUs> ContactUs { get; set; }
        public virtual ICollection<Favorite> Favorite { get; set; }
        public virtual ICollection<FeedBack> FeedBack { get; set; }
        public virtual ICollection<Game> Game { get; set; }
        public virtual ICollection<History> History { get; set; }
        public virtual ICollection<Invoice> Invoice { get; set; }
        public virtual ICollection<Notification> Notification { get; set; }
        public virtual ICollection<Status> Status { get; set; }
    }
}

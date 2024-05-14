using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LegendCR.DAL.DB
{
    public class ApplicationDBContext : IdentityDbContext<User, Role, string>
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options)
            : base(options) { }

        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<AccountFiles> AccountFiles { get; set; }
        public virtual DbSet<AppService> AppService { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<ContactUs> ContactUs { get; set; }
        public virtual DbSet<Favorite> Favorites { get; set; }
        public virtual DbSet<FeedBack> FeedBacks { get; set; }
        public virtual DbSet<Game> Games { get; set; }
        public virtual DbSet<History> Histories { get; set; }
        public virtual DbSet<Invoice> Invoices { get; set; }
        public virtual DbSet<Notification> Notification { get; set; }
        public virtual DbSet<RoleAppService> RoleAppService { get; set; }
        public virtual DbSet<Status> Statuses { get; set; }
        public virtual DbSet<Settings> Settings { get; set; }
    }
}

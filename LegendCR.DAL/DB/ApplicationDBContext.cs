using Microsoft.EntityFrameworkCore;

namespace LegendCR.DAL.DB
{
    public partial class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext()
        {
        }

        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AppService> AppService { get; set; }
        public virtual DbSet<AppServiceClass> AppServiceClass { get; set; }
        public virtual DbSet<AppServiceGroup> AppServiceGroup { get; set; }
        public virtual DbSet<City> City { get; set; }
        public virtual DbSet<CommonResource> CommonResource { get; set; }
        public virtual DbSet<CommonUser> CommonUser { get; set; }
        public virtual DbSet<CommonUserDevice> CommonUserDevice { get; set; }
        public virtual DbSet<ContactUs> ContactUs { get; set; }
        public virtual DbSet<FollowUp> FollowUp { get; set; }
        public virtual DbSet<FollowUpType> FollowUpType { get; set; }
        public virtual DbSet<Notification> Notification { get; set; }
        public virtual DbSet<ProblemType> ProblemType { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<RoleAppService> RoleAppService { get; set; }
        //public virtual DbSet<Shipment> Shipment { get; set; }
        //public virtual DbSet<ShipmentService> ShipmentService { get; set; }
        //public virtual DbSet<ShipmentType> ShipmentType { get; set; }
        public virtual DbSet<Status> Status { get; set; }
        public virtual DbSet<UserLocation> UserLocation { get; set; }
        public virtual DbSet<VendorProduct> VendorProduct { get; set; }
        public virtual DbSet<Warehouse> Warehouse { get; set; }
        public virtual DbSet<Zone> Zone { get; set; }
        public virtual DbSet<ZoneTax> ZoneTax { get; set; }
    }
}

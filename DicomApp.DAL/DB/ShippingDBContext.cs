using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DicomApp.DAL.DB
{
    public partial class ShippingDBContext : DbContext
    {
        public ShippingDBContext() { }

        public ShippingDBContext(DbContextOptions<ShippingDBContext> options)
            : base(options) { }

        public virtual DbSet<Account> Account { get; set; }
        public virtual DbSet<AccountTransaction> AccountTransaction { get; set; }
        public virtual DbSet<AppService> AppService { get; set; }
        public virtual DbSet<AppServiceClass> AppServiceClass { get; set; }
        public virtual DbSet<AppServiceGroup> AppServiceGroup { get; set; }
        public virtual DbSet<Branch> Branch { get; set; }
        public virtual DbSet<CashTransfer> CashTransfer { get; set; }
        public virtual DbSet<City> City { get; set; }
        public virtual DbSet<CommonResource> CommonResource { get; set; }
        public virtual DbSet<CommonUser> CommonUser { get; set; }
        public virtual DbSet<CommonUserDevice> CommonUserDevice { get; set; }
        public virtual DbSet<Complain> Complain { get; set; }
        public virtual DbSet<ContactUs> ContactUs { get; set; }
        public virtual DbSet<FollowUp> FollowUp { get; set; }
        public virtual DbSet<FollowUpType> FollowUpType { get; set; }
        public virtual DbSet<Notification> Notification { get; set; }
        public virtual DbSet<Packing> Packing { get; set; }
        public virtual DbSet<PackingType> PackingType { get; set; }
        public virtual DbSet<PickupRequest> PickupRequest { get; set; }
        public virtual DbSet<PickupRequestItem> PickupRequestItem { get; set; }
        public virtual DbSet<PickupRequestType> PickupRequestType { get; set; }
        public virtual DbSet<ProblemType> ProblemType { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<RoleAppService> RoleAppService { get; set; }
        public virtual DbSet<Shipment> Shipment { get; set; }
        public virtual DbSet<ShipmentCustomerFollowUp> ShipmentCustomerFollowUp { get; set; }
        public virtual DbSet<ShipmentItem> ShipmentItem { get; set; }
        public virtual DbSet<ShipmentProblem> ShipmentProblem { get; set; }
        public virtual DbSet<ShipmentService> ShipmentService { get; set; }
        public virtual DbSet<ShipmentType> ShipmentType { get; set; }
        public virtual DbSet<Status> Status { get; set; }
        public virtual DbSet<UserLocation> UserLocation { get; set; }
        public virtual DbSet<Userhubconnection> Userhubconnection { get; set; }
        public virtual DbSet<VendorProduct> VendorProduct { get; set; }
        public virtual DbSet<Warehouse> Warehouse { get; set; }
        public virtual DbSet<Zone> Zone { get; set; }
        public virtual DbSet<ZoneTax> ZoneTax { get; set; }

        //        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //        {
        //            if (!optionsBuilder.IsConfigured)
        //            {
        //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
        //                optionsBuilder.UseSqlServer("Server=.;Database=ShippingDB_Online1;Trusted_Connection=True;");
        //            }
        //        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity<Account>(entity =>
            {
                entity.Property(e => e.Ban).HasColumnName("BAN");

                entity
                    .Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Iban).HasColumnName("IBAN");

                entity
                    .Property(e => e.LastModifiedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Name).IsRequired().HasMaxLength(250);

                entity.Property(e => e.RefId).HasMaxLength(100);

                entity
                    .HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.AccountCreatedByNavigation)
                    .HasForeignKey(d => d.CreatedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Account_CommonUser1");

                entity
                    .HasOne(d => d.LastModifiedByNavigation)
                    .WithMany(p => p.AccountLastModifiedByNavigation)
                    .HasForeignKey(d => d.LastModifiedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Account_CommonUser2");

                entity
                    .HasOne(d => d.Role)
                    .WithMany(p => p.Account)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK_Account_Role");

                entity
                    .HasOne(d => d.User)
                    .WithMany(p => p.AccountUser)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Account_CommonUser");
            });

            modelBuilder.Entity<AccountTransaction>(entity =>
            {
                entity
                    .Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity
                    .Property(e => e.LastModifiedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.RefId).HasMaxLength(100);

                entity
                    .HasOne(d => d.CashTransfer)
                    .WithMany(p => p.AccountTransaction)
                    .HasForeignKey(d => d.CashTransferId)
                    .HasConstraintName("FK_AccountTransaction_CashTransfer");

                entity
                    .HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.AccountTransactionCreatedByNavigation)
                    .HasForeignKey(d => d.CreatedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AccountTransaction_CommonUser");

                entity
                    .HasOne(d => d.LastModifiedByNavigation)
                    .WithMany(p => p.AccountTransactionLastModifiedByNavigation)
                    .HasForeignKey(d => d.LastModifiedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AccountTransaction_CommonUser1");

                entity
                    .HasOne(d => d.PickupRequest)
                    .WithMany(p => p.AccountTransaction)
                    .HasForeignKey(d => d.PickupRequestId)
                    .HasConstraintName("FK_AccountTransaction_PickupRequest");

                entity
                    .HasOne(d => d.Receiver)
                    .WithMany(p => p.AccountTransactionReceiver)
                    .HasForeignKey(d => d.ReceiverId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AccountTransaction_Account1");

                entity
                    .HasOne(d => d.Sender)
                    .WithMany(p => p.AccountTransactionSender)
                    .HasForeignKey(d => d.SenderId)
                    .HasConstraintName("FK_AccountTransaction_Account");

                entity
                    .HasOne(d => d.Shipment)
                    .WithMany(p => p.AccountTransaction)
                    .HasForeignKey(d => d.ShipmentId)
                    .HasConstraintName("FK_AccountTransaction_Shipment");

                entity
                    .HasOne(d => d.Status)
                    .WithMany(p => p.AccountTransaction)
                    .HasForeignKey(d => d.StatusId)
                    .HasConstraintName("FK_AccountTransaction_Status");

                entity
                    .HasOne(d => d.Vendor)
                    .WithMany(p => p.AccountTransactionVendor)
                    .HasForeignKey(d => d.VendorId)
                    .HasConstraintName("FK_AccountTransaction_Account2");
            });

            modelBuilder.Entity<AppService>(entity =>
            {
                entity.HasIndex(e => e.Name).HasName("Service_Name_unique").IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AppServiceClassId).HasColumnName("AppServiceClassID");

                entity.Property(e => e.AppServiceGroupId).HasColumnName("AppServiceGroupID");

                entity.Property(e => e.ClassName).HasMaxLength(150);

                entity
                    .Property(e => e.CreationDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.LogMessage).HasMaxLength(2000);

                entity
                    .Property(e => e.ModificationDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Name).IsRequired().HasMaxLength(150);

                entity.Property(e => e.ShowToUser).IsRequired().HasDefaultValueSql("((1))");

                entity.Property(e => e.Title).IsRequired().HasMaxLength(150);

                entity
                    .HasOne(d => d.AppServiceClass)
                    .WithMany(p => p.AppService)
                    .HasForeignKey(d => d.AppServiceClassId)
                    .HasConstraintName("FK_AppService_AppServiceClass");

                entity
                    .HasOne(d => d.AppServiceGroup)
                    .WithMany(p => p.AppService)
                    .HasForeignKey(d => d.AppServiceGroupId)
                    .HasConstraintName("FK_AppService_AppServiceGroup");
            });

            modelBuilder.Entity<AppServiceClass>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity
                    .Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Name).HasMaxLength(250);
            });

            modelBuilder.Entity<AppServiceGroup>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AppServiceClassId).HasColumnName("AppServiceClassID");

                entity
                    .Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Name).HasMaxLength(250);

                entity
                    .HasOne(d => d.AppServiceClass)
                    .WithMany(p => p.AppServiceGroup)
                    .HasForeignKey(d => d.AppServiceClassId)
                    .HasConstraintName("FK_AppServiceGroup_AppServiceClass");
            });

            modelBuilder.Entity<Branch>(entity =>
            {
                entity.Property(e => e.BranchName).IsRequired();

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");
            });

            modelBuilder.Entity<CashTransfer>(entity =>
            {
                entity
                    .Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Email).HasMaxLength(150);

                entity
                    .Property(e => e.LastModifiedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Name).IsRequired().HasMaxLength(250);

                entity.Property(e => e.Otp).HasColumnName("OTP").HasMaxLength(100);

                entity.Property(e => e.Phone).HasMaxLength(100);

                entity.Property(e => e.RefId).HasMaxLength(100);

                entity
                    .HasOne(d => d.Area)
                    .WithMany(p => p.CashTransfer)
                    .HasForeignKey(d => d.AreaId)
                    .HasConstraintName("FK_CashTransfer_City");

                entity
                    .HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.CashTransferCreatedByNavigation)
                    .HasForeignKey(d => d.CreatedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CashTransfer_CommonUser");

                entity
                    .HasOne(d => d.LastModifiedByNavigation)
                    .WithMany(p => p.CashTransferLastModifiedByNavigation)
                    .HasForeignKey(d => d.LastModifiedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CashTransfer_CommonUser1");

                entity
                    .HasOne(d => d.Receiver)
                    .WithMany(p => p.CashTransfer)
                    .HasForeignKey(d => d.ReceiverId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CashTransfer_Account");

                entity
                    .HasOne(d => d.Zone)
                    .WithMany(p => p.CashTransfer)
                    .HasForeignKey(d => d.ZoneId)
                    .HasConstraintName("FK_CashTransfer_Zone");
            });

            modelBuilder.Entity<City>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Address).HasMaxLength(150);

                entity.Property(e => e.CityName).HasColumnName("City_Name").HasMaxLength(150);

                entity
                    .Property(e => e.CityNameAr)
                    .IsRequired()
                    .HasColumnName("City_Name_AR")
                    .HasMaxLength(150);

                entity
                    .Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy).IsRequired().HasMaxLength(100);

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity
                    .Property(e => e.LastModifiedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.LastModifiedBy).IsRequired().HasMaxLength(100);

                entity.Property(e => e.Lat).HasMaxLength(50);

                entity.Property(e => e.Lng).HasMaxLength(50);

                entity
                    .Property(e => e.StateId)
                    .HasColumnName("State_ID")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.ZoneId).HasColumnName("ZoneID");

                entity
                    .HasOne(d => d.Branch)
                    .WithMany(p => p.CityNavigation)
                    .HasForeignKey(d => d.BranchId)
                    .HasConstraintName("FK_Branch_City");

                entity
                    .HasOne(d => d.Zone)
                    .WithMany(p => p.City)
                    .HasForeignKey(d => d.ZoneId)
                    .HasConstraintName("FK_City_Zone");
            });

            modelBuilder.Entity<CommonResource>(entity =>
            {
                entity.ToTable("Common_Resource");

                entity
                    .HasIndex(e => new { e.ResourceKey, e.ResourceLang })
                    .HasName("IX_Common_Resource")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID");

                entity
                    .Property(e => e.CreationDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity
                    .Property(e => e.LastUpdateDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.MediaUrl).HasMaxLength(150);

                entity.Property(e => e.ResourceKey).IsRequired().HasMaxLength(50);

                entity.Property(e => e.ResourceLang).HasMaxLength(5);

                entity.Property(e => e.ResourceValue).IsRequired();

                entity.Property(e => e.Url).HasMaxLength(150);
            });

            modelBuilder.Entity<CommonUser>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AccountName).HasMaxLength(150);

                entity.Property(e => e.Address).HasMaxLength(150);

                entity.Property(e => e.AddressDetails).HasMaxLength(150);

                entity.Property(e => e.Bank).HasMaxLength(150);

                entity.Property(e => e.Code).HasMaxLength(50);

                entity
                    .Property(e => e.CreationDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Email).IsRequired().HasMaxLength(150);

                entity.Property(e => e.FullName).HasMaxLength(100);

                entity.Property(e => e.IbanNumber).HasColumnName("IBAN_Number");

                entity.Property(e => e.InstaPayAccountName).HasMaxLength(150);

                entity.Property(e => e.IsVerified).HasDefaultValueSql("((0))");

                entity.Property(e => e.LastLocationDate).HasColumnType("datetime");

                entity.Property(e => e.LastLoginDate).HasColumnType("datetime");

                entity.Property(e => e.Lat).HasMaxLength(50);

                entity.Property(e => e.Lng).HasMaxLength(50);

                entity
                    .Property(e => e.ModificationDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Name).IsRequired().HasMaxLength(150);

                entity.Property(e => e.NationalId).IsRequired().HasMaxLength(150);

                entity.Property(e => e.PageName).HasMaxLength(100);

                entity.Property(e => e.Password).IsRequired().HasMaxLength(150);

                entity.Property(e => e.PhoneNumber).IsRequired().HasMaxLength(150);

                entity.Property(e => e.ProductType).HasMaxLength(100);

                entity.Property(e => e.RoleId).HasColumnName("RoleID");

                entity
                    .HasOne(d => d.Branch)
                    .WithMany(p => p.CommonUser)
                    .HasForeignKey(d => d.BranchId)
                    .HasConstraintName("FK_Branch_CommonUser");

                entity
                    .HasOne(d => d.Role)
                    .WithMany(p => p.CommonUser)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CommonUser_Role");
            });

            modelBuilder.Entity<CommonUserDevice>(entity =>
            {
                entity.ToTable("Common_UserDevice");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity
                    .Property(e => e.AuthCreationDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity
                    .Property(e => e.AuthExpirationDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.AuthIp).HasColumnName("AuthIP").HasMaxLength(100);

                entity.Property(e => e.AuthToken).HasMaxLength(250);

                entity.Property(e => e.CommonUserId).HasColumnName("Common_UserID");

                entity
                    .Property(e => e.CreationDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DeviceEmail).HasMaxLength(150);

                entity.Property(e => e.DeviceImei).HasColumnName("DeviceIMEI").HasMaxLength(150);

                entity.Property(e => e.DeviceMobileNumber).HasMaxLength(50);

                entity.Property(e => e.DeviceName).IsRequired().HasMaxLength(150);

                entity
                    .Property(e => e.DeviceOsversion)
                    .HasColumnName("DeviceOSVersion")
                    .HasMaxLength(150);

                entity.Property(e => e.DeviceToken).HasMaxLength(500);

                entity.Property(e => e.DeviceType).IsRequired().HasMaxLength(50);

                entity.Property(e => e.EnableNotification).IsRequired().HasDefaultValueSql("((1))");

                entity.Property(e => e.Lang).HasMaxLength(50);

                entity.Property(e => e.LastActiveDate).HasColumnType("datetime");

                entity
                    .Property(e => e.LastUpdateDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity
                    .HasOne(d => d.CommonUser)
                    .WithMany(p => p.CommonUserDevice)
                    .HasForeignKey(d => d.CommonUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Common_UserDevice_Common_User");
            });

            modelBuilder.Entity<Complain>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.Department).HasMaxLength(150);

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.Property(e => e.IsSolved).HasDefaultValueSql("((0))");

                entity
                    .HasOne(d => d.ActionByNavigation)
                    .WithMany(p => p.ComplainActionByNavigation)
                    .HasForeignKey(d => d.ActionBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Complains_ActionBy");

                entity
                    .HasOne(d => d.Employee)
                    .WithMany(p => p.ComplainEmployee)
                    .HasForeignKey(d => d.EmployeeId)
                    .HasConstraintName("FK_Complains_Employee");

                entity
                    .HasOne(d => d.Vendor)
                    .WithMany(p => p.ComplainVendor)
                    .HasForeignKey(d => d.VendorId)
                    .HasConstraintName("FK_Complains_Vendor");
            });

            modelBuilder.Entity<ContactUs>(entity =>
            {
                entity.Property(e => e.Email).IsRequired().HasMaxLength(150).IsUnicode(false);

                entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100).IsUnicode(false);

                entity.Property(e => e.LastName).IsRequired().HasMaxLength(100).IsUnicode(false);

                entity.Property(e => e.Message).IsRequired().HasMaxLength(255).IsUnicode(false);

                entity.Property(e => e.PhoneNumber).IsRequired().HasMaxLength(50).IsUnicode(false);

                entity.Property(e => e.Subject).IsRequired().HasMaxLength(150).IsUnicode(false);

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity
                    .HasOne(d => d.User)
                    .WithMany(p => p.ContactUs)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ContactUs_CommonUser");
            });

            modelBuilder.Entity<FollowUp>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Comment).HasMaxLength(500);

                entity
                    .Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.FollowUpTypeId).HasColumnName("FollowUpTypeID");

                entity
                    .Property(e => e.LastModifiedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Lat).HasMaxLength(50);

                entity.Property(e => e.Lng).HasMaxLength(50);

                entity.Property(e => e.StatusId).HasColumnName("StatusID");

                entity.Property(e => e.Title).HasMaxLength(100);

                entity
                    .HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.FollowUp)
                    .HasForeignKey(d => d.CreatedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Visit_FollowUp_CommonUser");

                entity
                    .HasOne(d => d.FollowUpType)
                    .WithMany(p => p.FollowUp)
                    .HasForeignKey(d => d.FollowUpTypeId)
                    .HasConstraintName("FK_FollowUp_FollowUpType");

                entity
                    .HasOne(d => d.Shipment)
                    .WithMany(p => p.FollowUp)
                    .HasForeignKey(d => d.ShipmentId)
                    .HasConstraintName("FK_FollowUp_Shipment");
            });

            modelBuilder.Entity<FollowUpType>(entity =>
            {
                entity.Property(e => e.FollowUpTypeId).HasColumnName("FollowUpTypeID");

                entity.Property(e => e.TypeName).IsRequired().HasMaxLength(50);
            });

            modelBuilder.Entity<Notification>(entity =>
            {
                entity.Property(e => e.NotificationId).HasColumnName("NotificationID");

                entity
                    .Property(e => e.CreationDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DeletionDate).HasColumnType("datetime");

                entity.Property(e => e.Icon).HasMaxLength(50);

                entity.Property(e => e.RecipientId).HasColumnName("RecipientID");

                entity.Property(e => e.SeenDate).HasColumnType("datetime");

                entity.Property(e => e.SenderId).HasColumnName("SenderID");

                entity.Property(e => e.Title).HasMaxLength(200);

                entity
                    .HasOne(d => d.RecipientRole)
                    .WithMany(p => p.Notification)
                    .HasForeignKey(d => d.RecipientRoleId)
                    .HasConstraintName("FK_Notification_Notification1");

                entity
                    .HasOne(d => d.Sender)
                    .WithMany(p => p.Notification)
                    .HasForeignKey(d => d.SenderId)
                    .HasConstraintName("FK_Notification_Notification");
            });

            modelBuilder.Entity<Packing>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity
                    .Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Description).HasMaxLength(250);

                entity.Property(e => e.ImgUrl).IsRequired();

                entity
                    .Property(e => e.LastModifiedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity
                    .Property(e => e.NameAr)
                    .IsRequired()
                    .HasColumnName("Name_Ar")
                    .HasMaxLength(150);

                entity
                    .Property(e => e.NameEn)
                    .IsRequired()
                    .HasColumnName("Name_En")
                    .HasMaxLength(150);

                entity.Property(e => e.Size).IsRequired().HasMaxLength(100);

                entity
                    .HasOne(d => d.Branch)
                    .WithMany(p => p.Packing)
                    .HasForeignKey(d => d.BranchId)
                    .HasConstraintName("FK_Branch_Packing");

                entity
                    .HasOne(d => d.PackingType)
                    .WithMany(p => p.Packing)
                    .HasForeignKey(d => d.PackingTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Packing_PackingType");
            });

            modelBuilder.Entity<PackingType>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity
                    .Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity
                    .Property(e => e.LastModifiedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity
                    .Property(e => e.NameAr)
                    .IsRequired()
                    .HasColumnName("Name_Ar")
                    .HasMaxLength(150);

                entity
                    .Property(e => e.NameEn)
                    .IsRequired()
                    .HasColumnName("Name_En")
                    .HasMaxLength(150);

                entity
                    .HasOne(d => d.Branch)
                    .WithMany(p => p.PackingType)
                    .HasForeignKey(d => d.BranchId)
                    .HasConstraintName("FK_Branch_PackingType");
            });

            modelBuilder.Entity<PickupRequest>(entity =>
            {
                entity
                    .Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity
                    .Property(e => e.PickupDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.RefId).HasColumnName("RefID");

                entity
                    .Property(e => e.TimeFrom)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity
                    .Property(e => e.TimeTo)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.VendorName).IsRequired();

                entity.Property(e => e.VendorPhone).IsRequired().HasMaxLength(250);

                entity
                    .HasOne(d => d.Area)
                    .WithMany(p => p.PickupRequest)
                    .HasForeignKey(d => d.AreaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PickupRequest_City");

                entity
                    .HasOne(d => d.Branch)
                    .WithMany(p => p.PickupRequest)
                    .HasForeignKey(d => d.BranchId)
                    .HasConstraintName("FK_Branch_PickupRequest");

                entity
                    .HasOne(d => d.DeliveryMan)
                    .WithMany(p => p.PickupRequestDeliveryMan)
                    .HasForeignKey(d => d.DeliveryManId)
                    .HasConstraintName("FK_PickupRequest_CommonUser2");

                entity
                    .HasOne(d => d.PickupRequestType)
                    .WithMany(p => p.PickupRequest)
                    .HasForeignKey(d => d.PickupRequestTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PickupRequestType_PickupRequest");

                entity
                    .HasOne(d => d.Status)
                    .WithMany(p => p.PickupRequest)
                    .HasForeignKey(d => d.StatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PickupRequest_StatusId");

                entity
                    .HasOne(d => d.Vendor)
                    .WithMany(p => p.PickupRequestVendor)
                    .HasForeignKey(d => d.VendorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PickupRequest_CommonUser");

                entity
                    .HasOne(d => d.Zone)
                    .WithMany(p => p.PickupRequest)
                    .HasForeignKey(d => d.ZoneId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PickupRequest_Zone");
            });

            modelBuilder.Entity<PickupRequestItem>(entity =>
            {
                entity
                    .Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity
                    .HasOne(d => d.PickupRequest)
                    .WithMany(p => p.PickupRequestItem)
                    .HasForeignKey(d => d.PickupRequestId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PickupRequestItem_PickupRequest");

                entity
                    .HasOne(d => d.Shipment)
                    .WithMany(p => p.PickupRequestItem)
                    .HasForeignKey(d => d.ShipmentId)
                    .HasConstraintName("FK_PickupRequestItem_Shipment");

                entity
                    .HasOne(d => d.Status)
                    .WithMany(p => p.PickupRequestItem)
                    .HasForeignKey(d => d.StatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PickupRequestItem_StatusId");

                entity
                    .HasOne(d => d.VendorProduct)
                    .WithMany(p => p.PickupRequestItem)
                    .HasForeignKey(d => d.VendorProductId)
                    .HasConstraintName("FK_PickupRequestItem_VendorProduct");
            });

            modelBuilder.Entity<PickupRequestType>(entity =>
            {
                entity.Property(e => e.Name).IsRequired().HasMaxLength(500);
            });

            modelBuilder.Entity<ProblemType>(entity =>
            {
                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.Property(e => e.NameAr).HasMaxLength(150);

                entity.Property(e => e.NameEn).HasMaxLength(150);
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity
                    .Property(e => e.CreationDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Editable).IsRequired().HasDefaultValueSql("((1))");

                entity
                    .Property(e => e.ModificationDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Name).IsRequired().HasMaxLength(150);
            });

            modelBuilder.Entity<RoleAppService>(entity =>
            {
                entity
                    .HasIndex(e => new { e.AppServiceId, e.RoleId })
                    .HasName("RoleIDServiceIDUniq")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AppServiceId).HasColumnName("AppServiceID");

                entity
                    .Property(e => e.CreationDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity
                    .Property(e => e.ModificationDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.RoleId).HasColumnName("RoleID");

                entity
                    .HasOne(d => d.AppService)
                    .WithMany(p => p.RoleAppService)
                    .HasForeignKey(d => d.AppServiceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RoleAppService_AppService");

                entity
                    .HasOne(d => d.Role)
                    .WithMany(p => p.RoleAppService)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RoleAppService_Role1");
            });

            modelBuilder.Entity<Shipment>(entity =>
            {
                entity.Property(e => e.AreaId).HasColumnName("AreaID");

                entity.Property(e => e.CancelComment).HasMaxLength(500);

                entity
                    .Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CustomerAddress).HasMaxLength(500);

                entity.Property(e => e.CustomerName).HasMaxLength(250);

                entity.Property(e => e.CustomerPhone).HasMaxLength(250);

                entity.Property(e => e.CustomerPhone2).HasMaxLength(250);

                entity.Property(e => e.Landmark).HasMaxLength(255);

                entity
                    .Property(e => e.LastModifiedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.PaidToVendorAt).HasColumnType("datetime");

                entity.Property(e => e.RefId).HasMaxLength(255);

                entity.Property(e => e.Size).HasMaxLength(100);

                entity.Property(e => e.VendorName).IsRequired().HasMaxLength(250);

                entity.Property(e => e.VendorPhone).IsRequired().HasMaxLength(250);

                entity.Property(e => e.WarehouseSize).HasMaxLength(100);

                entity.Property(e => e.ZoneId).HasColumnName("ZoneID");

                entity
                    .HasOne(d => d.Area)
                    .WithMany(p => p.Shipment)
                    .HasForeignKey(d => d.AreaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Shipment_City");

                entity
                    .HasOne(d => d.Branch)
                    .WithMany(p => p.Shipment)
                    .HasForeignKey(d => d.BranchId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Shipment_Branch");

                entity
                    .HasOne(d => d.CashTransfer)
                    .WithMany(p => p.Shipment)
                    .HasForeignKey(d => d.CashTransferId)
                    .HasConstraintName("FK_Shipment_CashTransfer");

                entity
                    .HasOne(d => d.Customer)
                    .WithMany(p => p.ShipmentCustomer)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK_Shipment_CommonUser");

                entity
                    .HasOne(d => d.DeliveryMan)
                    .WithMany(p => p.ShipmentDeliveryMan)
                    .HasForeignKey(d => d.DeliveryManId)
                    .HasConstraintName("FK_Shipment_Shipment");

                entity
                    .HasOne(d => d.LastModifiedByNavigation)
                    .WithMany(p => p.ShipmentLastModifiedByNavigation)
                    .HasForeignKey(d => d.LastModifiedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Shipment_CommonUser3");

                entity
                    .HasOne(d => d.Packing)
                    .WithMany(p => p.ShipmentPacking)
                    .HasForeignKey(d => d.PackingId)
                    .HasConstraintName("FK_Shipment_Packing");

                entity
                    .HasOne(d => d.PickupRequest)
                    .WithMany(p => p.Shipment)
                    .HasForeignKey(d => d.PickupRequestId)
                    .HasConstraintName("FK_Shipment_PickupRequest");

                entity
                    .HasOne(d => d.ShipmentService)
                    .WithMany(p => p.Shipment)
                    .HasForeignKey(d => d.ShipmentServiceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Shipment_ShipmentService");

                entity
                    .HasOne(d => d.ShipmentType)
                    .WithMany(p => p.Shipment)
                    .HasForeignKey(d => d.ShipmentTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Shipment_ShipmentType");

                entity
                    .HasOne(d => d.Status)
                    .WithMany(p => p.Shipment)
                    .HasForeignKey(d => d.StatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Shipment_Status");

                entity
                    .HasOne(d => d.Vendor)
                    .WithMany(p => p.ShipmentVendor)
                    .HasForeignKey(d => d.VendorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Shipment_CommonUser1");

                entity
                    .HasOne(d => d.WarehousePacking)
                    .WithMany(p => p.ShipmentWarehousePacking)
                    .HasForeignKey(d => d.WarehousePackingId)
                    .HasConstraintName("FK_Shipment_WarehousePackingId");

                entity
                    .HasOne(d => d.Zone)
                    .WithMany(p => p.Shipment)
                    .HasForeignKey(d => d.ZoneId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Shipment_Zone");
            });

            modelBuilder.Entity<ShipmentCustomerFollowUp>(entity =>
            {
                entity
                    .Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.NextCallOn).HasColumnType("datetime");

                entity.Property(e => e.NextCallTimeFrom).HasColumnType("datetime");

                entity
                    .Property(e => e.NextCallTimeTo)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity
                    .HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.ShipmentCustomerFollowUp)
                    .HasForeignKey(d => d.CreatedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ShipmentCustomerFollowup_CommonUser");

                entity
                    .HasOne(d => d.Shipment)
                    .WithMany(p => p.ShipmentCustomerFollowUp)
                    .HasForeignKey(d => d.ShipmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ShipmentCustomerFollowup_Shipment");
            });

            modelBuilder.Entity<ShipmentItem>(entity =>
            {
                entity
                    .Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Size).HasMaxLength(100);

                entity
                    .HasOne(d => d.Shipment)
                    .WithMany(p => p.ShipmentItem)
                    .HasForeignKey(d => d.ShipmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ShipmentItem_Shipment");

                entity
                    .HasOne(d => d.Status)
                    .WithMany(p => p.ShipmentItem)
                    .HasForeignKey(d => d.StatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ShipmentItem_StatusId");

                entity
                    .HasOne(d => d.VendorProduct)
                    .WithMany(p => p.ShipmentItem)
                    .HasForeignKey(d => d.VendorProductId)
                    .HasConstraintName("FK_ShipmentItem_VendorProduct");
            });

            modelBuilder.Entity<ShipmentProblem>(entity =>
            {
                entity
                    .Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity
                    .HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.ShipmentProblem)
                    .HasForeignKey(d => d.CreatedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ShipmentProblem_CommonUser");

                entity
                    .HasOne(d => d.ProblemType)
                    .WithMany(p => p.ShipmentProblem)
                    .HasForeignKey(d => d.ProblemTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ShipmentProblem_ProblemType");

                entity
                    .HasOne(d => d.Shipment)
                    .WithMany(p => p.ShipmentProblem)
                    .HasForeignKey(d => d.ShipmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ShipmentProblem_Shipment");
            });

            modelBuilder.Entity<ShipmentService>(entity =>
            {
                entity.Property(e => e.ServiceName).IsRequired().HasMaxLength(150);
            });

            modelBuilder.Entity<ShipmentType>(entity =>
            {
                entity.Property(e => e.Name).IsRequired();
            });

            modelBuilder.Entity<Status>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<UserLocation>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Address).HasMaxLength(250);

                entity
                    .Property(e => e.ApplicationId)
                    .HasColumnName("ApplicationID")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.AquirityDistance).HasDefaultValueSql("((0))");

                entity.Property(e => e.ChannelId).HasColumnName("ChannelID");

                entity.Property(e => e.Description).HasMaxLength(500);

                entity.Property(e => e.DirectionAngle).HasDefaultValueSql("((0))");

                entity.Property(e => e.ImagePath).HasMaxLength(250);

                entity.Property(e => e.IsOnline).IsRequired().HasDefaultValueSql("((1))");

                entity.Property(e => e.Latitude).HasColumnType("decimal(18, 10)");

                entity.Property(e => e.Longitude).HasColumnType("decimal(18, 10)");

                entity.Property(e => e.Phone).HasMaxLength(50);

                entity
                    .Property(e => e.PlaceId)
                    .IsRequired()
                    .HasColumnName("PlaceID")
                    .HasMaxLength(250);

                entity.Property(e => e.SubmitDateTime).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Title).HasMaxLength(250);

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity
                    .HasOne(d => d.User)
                    .WithMany(p => p.UserLocation)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserLocation_Common_User");
            });

            modelBuilder.Entity<Userhubconnection>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity
                    .Property(e => e.ConnectionId)
                    .IsRequired()
                    .HasColumnName("ConnectionID")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity
                    .Property(e => e.CreationDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity
                    .Property(e => e.ModificationDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<VendorProduct>(entity =>
            {
                entity
                    .Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Name).IsRequired();

                entity.Property(e => e.Size).HasMaxLength(150);

                entity
                    .HasOne(d => d.Vendor)
                    .WithMany(p => p.VendorProduct)
                    .HasForeignKey(d => d.VendorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_VendorProduct_CommonUser");
            });

            modelBuilder.Entity<Warehouse>(entity =>
            {
                entity.Property(e => e.WarehouseName).IsRequired();
            });

            modelBuilder.Entity<Zone>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity
                    .Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity
                    .Property(e => e.LastModifiedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.NameAr).HasColumnName("Name_Ar").HasMaxLength(150);

                entity.Property(e => e.NameEn).HasColumnName("Name_En").HasMaxLength(150);

                entity
                    .HasOne(d => d.Branch)
                    .WithMany(p => p.Zone)
                    .HasForeignKey(d => d.BranchId)
                    .HasConstraintName("FK_Branch_Zone");

                entity
                    .HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.ZoneCreatedByNavigation)
                    .HasForeignKey(d => d.CreatedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Zone_CommonUser");

                entity
                    .HasOne(d => d.LastModifiedByNavigation)
                    .WithMany(p => p.ZoneLastModifiedByNavigation)
                    .HasForeignKey(d => d.LastModifiedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Zone_CommonUser1");
            });

            modelBuilder.Entity<ZoneTax>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity
                    .Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity
                    .Property(e => e.LastModifiedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ZoneId).HasColumnName("ZoneID");

                entity
                    .HasOne(d => d.Zone)
                    .WithMany(p => p.ZoneTax)
                    .HasForeignKey(d => d.ZoneId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ZoneTax_Zone");
            });
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DicomApp.DAL.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppService",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AllowAnonymous = table.Column<bool>(nullable: false),
                    CreatedBy = table.Column<int>(nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    IsDeleted = table.Column<bool>(nullable: false),
                    ModificationDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    ModifiedBy = table.Column<int>(nullable: true),
                    Name = table.Column<string>(maxLength: 150, nullable: false),
                    ShowToUser = table.Column<bool>(nullable: false, defaultValueSql: "((1))"),
                    Title = table.Column<string>(maxLength: 150, nullable: false),
                    ClassName = table.Column<string>(maxLength: 150, nullable: true),
                    AllowLog = table.Column<bool>(nullable: false),
                    LogMessage = table.Column<string>(maxLength: 2000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppService", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Branch",
                columns: table => new
                {
                    BranchId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Address = table.Column<string>(nullable: true),
                    BranchName = table.Column<string>(nullable: false),
                    City = table.Column<string>(nullable: true),
                    ContactPerson = table.Column<string>(nullable: true),
                    CurrencyId = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: true),
                    State = table.Column<string>(nullable: true),
                    ZipCode = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true, defaultValueSql: "((0))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Branch", x => x.BranchId);
                });

            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name_Ar = table.Column<string>(maxLength: 150, nullable: false),
                    Name_En = table.Column<string>(maxLength: 150, nullable: false),
                    CreatedBy = table.Column<int>(nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    LastModifiedBy = table.Column<int>(nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    DeleteBy = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Common_Resource",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ResourceKey = table.Column<string>(maxLength: 50, nullable: false),
                    ResourceValue = table.Column<string>(nullable: false),
                    ResourceLang = table.Column<string>(maxLength: 5, nullable: true),
                    CreatedBy = table.Column<int>(nullable: true),
                    ModifiedBy = table.Column<int>(nullable: true),
                    LastUpdateDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    CreationDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    IsDeleted = table.Column<bool>(nullable: false),
                    ApplicationId = table.Column<int>(nullable: true),
                    Url = table.Column<string>(maxLength: 150, nullable: true),
                    MediaUrl = table.Column<string>(maxLength: 150, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Common_Resource", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    CountryId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    NameEn = table.Column<string>(nullable: true),
                    NameAr = table.Column<string>(nullable: true),
                    FlagUrl = table.Column<string>(nullable: true),
                    CountryCode = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.CountryId);
                });

            migrationBuilder.CreateTable(
                name: "FollowUpType",
                columns: table => new
                {
                    FollowUpTypeID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TypeName = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FollowUpType", x => x.FollowUpTypeID);
                });

            migrationBuilder.CreateTable(
                name: "PickupRequestType",
                columns: table => new
                {
                    PickupRequestTypeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PickupRequestType", x => x.PickupRequestTypeId);
                });

            migrationBuilder.CreateTable(
                name: "ProblemType",
                columns: table => new
                {
                    ProblemTypeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    NameEn = table.Column<string>(maxLength: 150, nullable: true),
                    NameAr = table.Column<string>(maxLength: 150, nullable: true),
                    Type = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true, defaultValueSql: "((0))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProblemType", x => x.ProblemTypeId);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<int>(nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    IsDeleted = table.Column<bool>(nullable: false),
                    ModificationDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    ModifiedBy = table.Column<int>(nullable: true),
                    Name = table.Column<string>(maxLength: 150, nullable: false),
                    Editable = table.Column<bool>(nullable: false, defaultValueSql: "((1))"),
                    DeleteBy = table.Column<int>(nullable: true),
                    DeleteAt = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ShipmentType",
                columns: table => new
                {
                    ShipmentTypeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShipmentType", x => x.ShipmentTypeId);
                });

            migrationBuilder.CreateTable(
                name: "Status",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    NameEN = table.Column<string>(maxLength: 50, nullable: true),
                    NameAR = table.Column<string>(maxLength: 50, nullable: true),
                    StatusType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Status", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Userhubconnection",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ConnectionID = table.Column<string>(unicode: false, maxLength: 100, nullable: false),
                    UserId = table.Column<string>(nullable: true),
                    IsOnline = table.Column<bool>(nullable: false),
                    CreatedBy = table.Column<int>(nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    ModificationDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    ModifiedBy = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Userhubconnection", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Warehouse",
                columns: table => new
                {
                    WarehouseId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BranchId = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    WarehouseName = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Warehouse", x => x.WarehouseId);
                });

            migrationBuilder.CreateTable(
                name: "Game",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name_Ar = table.Column<string>(maxLength: 150, nullable: false),
                    Name_En = table.Column<string>(maxLength: 150, nullable: false),
                    ImgUrl = table.Column<string>(nullable: false),
                    Description = table.Column<string>(maxLength: 250, nullable: true),
                    CategoryId = table.Column<int>(nullable: false),
                    CreatedBy = table.Column<int>(nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    LastModifiedBy = table.Column<int>(nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    DeleteBy = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Game", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Game_Category",
                        column: x => x.CategoryId,
                        principalTable: "Category",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CommonUser",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<int>(nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    IsDeleted = table.Column<bool>(nullable: false),
                    ModificationDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    ModifiedBy = table.Column<int>(nullable: true),
                    Name = table.Column<string>(maxLength: 150, nullable: false),
                    Email = table.Column<string>(maxLength: 150, nullable: false),
                    ConfirmEmail = table.Column<bool>(nullable: false),
                    LastLoginDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Password = table.Column<string>(maxLength: 150, nullable: false),
                    RoleID = table.Column<int>(nullable: false),
                    PhoneNumber = table.Column<string>(maxLength: 150, nullable: false),
                    IsLoggedIn = table.Column<bool>(nullable: false),
                    TelegramUserName = table.Column<string>(nullable: true),
                    CountryId = table.Column<int>(nullable: true),
                    Age = table.Column<int>(nullable: false),
                    Gender = table.Column<int>(nullable: false),
                    NationalId = table.Column<string>(maxLength: 150, nullable: false),
                    AreaId = table.Column<int>(nullable: true),
                    Code = table.Column<string>(maxLength: 50, nullable: true),
                    Lat = table.Column<string>(maxLength: 50, nullable: true),
                    Lng = table.Column<string>(maxLength: 50, nullable: true),
                    Address = table.Column<string>(maxLength: 150, nullable: true),
                    LastLocationDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    VerificationCode = table.Column<long>(nullable: true),
                    IsVerified = table.Column<bool>(nullable: true, defaultValueSql: "((0))"),
                    FullName = table.Column<string>(maxLength: 100, nullable: true),
                    AddressDetails = table.Column<string>(maxLength: 150, nullable: true),
                    ProductType = table.Column<string>(maxLength: 100, nullable: true),
                    Averageorders = table.Column<int>(nullable: true),
                    OpenPackage = table.Column<bool>(nullable: true),
                    PartialDelivery = table.Column<bool>(nullable: true),
                    VisaPayment = table.Column<bool>(nullable: true),
                    PageName = table.Column<string>(maxLength: 100, nullable: true),
                    LocationUrl = table.Column<string>(nullable: true),
                    Landmark = table.Column<string>(nullable: true),
                    Floor = table.Column<int>(nullable: true),
                    Apartment = table.Column<int>(nullable: true),
                    BranchId = table.Column<int>(nullable: true),
                    Bank = table.Column<string>(maxLength: 150, nullable: true),
                    AccountName = table.Column<string>(maxLength: 150, nullable: true),
                    AccountNumber = table.Column<int>(nullable: true),
                    IBAN_Number = table.Column<int>(nullable: true),
                    WalletNumber = table.Column<int>(nullable: true),
                    InstaPayAccountName = table.Column<string>(maxLength: 150, nullable: true),
                    ZoneId = table.Column<int>(nullable: true),
                    ImgUrl = table.Column<string>(nullable: true),
                    HashedPassword = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommonUser", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Branch_CommonUser",
                        column: x => x.BranchId,
                        principalTable: "Branch",
                        principalColumn: "BranchId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CommonUser_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "CountryId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CommonUser_Role",
                        column: x => x.RoleID,
                        principalTable: "Role",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RoleAppService",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<int>(nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    IsDeleted = table.Column<bool>(nullable: false),
                    ModificationDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    ModifiedBy = table.Column<int>(nullable: true),
                    AppServiceID = table.Column<int>(nullable: false),
                    Enabled = table.Column<bool>(nullable: false),
                    RoleID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleAppService", x => x.ID);
                    table.ForeignKey(
                        name: "FK_RoleAppService_AppService",
                        column: x => x.AppServiceID,
                        principalTable: "AppService",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RoleAppService_Role1",
                        column: x => x.RoleID,
                        principalTable: "Role",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Account",
                columns: table => new
                {
                    AccountId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 250, nullable: false),
                    RefId = table.Column<string>(maxLength: 100, nullable: true),
                    Balance = table.Column<double>(nullable: false),
                    BAN = table.Column<string>(nullable: true),
                    IBAN = table.Column<string>(nullable: true),
                    BankName = table.Column<string>(nullable: true),
                    VodafoneCash = table.Column<int>(nullable: true),
                    InstaPay = table.Column<string>(nullable: true),
                    UserId = table.Column<int>(nullable: false),
                    RoleId = table.Column<int>(nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    CreatedBy = table.Column<int>(nullable: false),
                    LastModifiedBy = table.Column<int>(nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Account", x => x.AccountId);
                    table.ForeignKey(
                        name: "FK_Account_CommonUser1",
                        column: x => x.CreatedBy,
                        principalTable: "CommonUser",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Account_CommonUser2",
                        column: x => x.LastModifiedBy,
                        principalTable: "CommonUser",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Account_Role",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Account_CommonUser",
                        column: x => x.UserId,
                        principalTable: "CommonUser",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Common_UserDevice",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Common_UserID = table.Column<int>(nullable: false),
                    DeviceName = table.Column<string>(maxLength: 150, nullable: false),
                    DeviceIMEI = table.Column<string>(maxLength: 150, nullable: true),
                    DeviceType = table.Column<string>(maxLength: 50, nullable: false),
                    DeviceOSVersion = table.Column<string>(maxLength: 150, nullable: true),
                    DeviceToken = table.Column<string>(maxLength: 500, nullable: true),
                    DeviceEmail = table.Column<string>(maxLength: 150, nullable: true),
                    EnableNotification = table.Column<bool>(nullable: false, defaultValueSql: "((1))"),
                    AuthToken = table.Column<string>(maxLength: 250, nullable: true),
                    AuthIP = table.Column<string>(maxLength: 100, nullable: true),
                    AuthCreationDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    AuthExpirationDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    IsLoggedIn = table.Column<bool>(nullable: false),
                    DeviceMobileNumber = table.Column<string>(maxLength: 50, nullable: true),
                    LastActiveDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Lang = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedBy = table.Column<int>(nullable: true),
                    ModifiedBy = table.Column<int>(nullable: true),
                    LastUpdateDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    CreationDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    IsDeleted = table.Column<bool>(nullable: false),
                    IsGoogleSupport = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Common_UserDevice", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Common_UserDevice_Common_User",
                        column: x => x.Common_UserID,
                        principalTable: "CommonUser",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Complain",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    EmployeeId = table.Column<int>(nullable: true),
                    VendorId = table.Column<int>(nullable: true),
                    ActionBy = table.Column<int>(nullable: false),
                    Date = table.Column<DateTime>(type: "datetime", nullable: false),
                    Description = table.Column<string>(nullable: true),
                    IsSolved = table.Column<bool>(nullable: true, defaultValueSql: "((0))"),
                    Comments = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true, defaultValueSql: "((0))"),
                    Department = table.Column<string>(maxLength: 150, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Complain", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Complains_ActionBy",
                        column: x => x.ActionBy,
                        principalTable: "CommonUser",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Complains_Employee",
                        column: x => x.EmployeeId,
                        principalTable: "CommonUser",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Complains_Vendor",
                        column: x => x.VendorId,
                        principalTable: "CommonUser",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ContactUs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    LastName = table.Column<string>(unicode: false, maxLength: 100, nullable: false),
                    FirstName = table.Column<string>(unicode: false, maxLength: 100, nullable: false),
                    PhoneNumber = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    Email = table.Column<string>(unicode: false, maxLength: 150, nullable: false),
                    Subject = table.Column<string>(unicode: false, maxLength: 150, nullable: false),
                    Message = table.Column<string>(unicode: false, maxLength: 255, nullable: false),
                    UserID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactUs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContactUs_CommonUser",
                        column: x => x.UserID,
                        principalTable: "CommonUser",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GamerService",
                columns: table => new
                {
                    GamerServiceId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RefId = table.Column<string>(nullable: true),
                    GameId = table.Column<int>(nullable: false),
                    StatusId = table.Column<int>(nullable: false),
                    GamerId = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    UserName = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    Level = table.Column<int>(nullable: false),
                    Rank = table.Column<string>(nullable: true),
                    Price = table.Column<int>(nullable: false),
                    GameServiceType = table.Column<int>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<int>(nullable: false),
                    LastModifiedAt = table.Column<DateTime>(nullable: true),
                    LastModifiedBy = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GamerService", x => x.GamerServiceId);
                    table.ForeignKey(
                        name: "FK_GamerService_Game_GameId",
                        column: x => x.GameId,
                        principalTable: "Game",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GamerService_CommonUser_GamerId",
                        column: x => x.GamerId,
                        principalTable: "CommonUser",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GamerService_Status_StatusId",
                        column: x => x.StatusId,
                        principalTable: "Status",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Notification",
                columns: table => new
                {
                    NotificationID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    SenderID = table.Column<int>(nullable: true),
                    RecipientID = table.Column<int>(nullable: true),
                    Title = table.Column<string>(maxLength: 200, nullable: true),
                    Body = table.Column<string>(nullable: true),
                    Icon = table.Column<string>(maxLength: 50, nullable: true),
                    IsSeen = table.Column<bool>(nullable: true),
                    SeenDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    IsDeleted = table.Column<bool>(nullable: true),
                    DeletionDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    TargetPath = table.Column<string>(nullable: true),
                    RecipientRoleId = table.Column<int>(nullable: true),
                    SeenBy = table.Column<int>(nullable: true),
                    DeletedBy = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notification", x => x.NotificationID);
                    table.ForeignKey(
                        name: "FK_Notification_Notification1",
                        column: x => x.RecipientRoleId,
                        principalTable: "Role",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Notification_Notification",
                        column: x => x.SenderID,
                        principalTable: "CommonUser",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserLocation",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(maxLength: 250, nullable: true),
                    PlaceID = table.Column<string>(maxLength: 250, nullable: false),
                    Address = table.Column<string>(maxLength: 250, nullable: true),
                    Description = table.Column<string>(maxLength: 500, nullable: true),
                    Phone = table.Column<string>(maxLength: 50, nullable: true),
                    Latitude = table.Column<decimal>(type: "decimal(18, 10)", nullable: false),
                    Longitude = table.Column<decimal>(type: "decimal(18, 10)", nullable: false),
                    UserID = table.Column<int>(nullable: false),
                    CreationDateTime = table.Column<DateTime>(nullable: false),
                    SubmitDateTime = table.Column<DateTime>(nullable: false, defaultValueSql: "(getdate())"),
                    BataryStatus = table.Column<int>(nullable: true),
                    NetworkStatus = table.Column<int>(nullable: true),
                    IsOnline = table.Column<bool>(nullable: false, defaultValueSql: "((1))"),
                    DirectionAngle = table.Column<double>(nullable: true, defaultValueSql: "((0))"),
                    AquirityDistance = table.Column<double>(nullable: true, defaultValueSql: "((0))"),
                    ChannelID = table.Column<long>(nullable: true),
                    ApplicationID = table.Column<long>(nullable: false, defaultValueSql: "((1))"),
                    ImagePath = table.Column<string>(maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLocation", x => x.ID);
                    table.ForeignKey(
                        name: "FK_UserLocation_Common_User",
                        column: x => x.UserID,
                        principalTable: "CommonUser",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "VendorProduct",
                columns: table => new
                {
                    VendorProductId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    OriginalPrice = table.Column<double>(nullable: false),
                    VendorId = table.Column<int>(nullable: false),
                    ImageUrl = table.Column<string>(nullable: true),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    IsDeleted = table.Column<bool>(nullable: false),
                    AvailableStock = table.Column<int>(nullable: false),
                    StockPrice = table.Column<double>(nullable: false),
                    Size = table.Column<string>(maxLength: 150, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VendorProduct", x => x.VendorProductId);
                    table.ForeignKey(
                        name: "FK_VendorProduct_CommonUser",
                        column: x => x.VendorId,
                        principalTable: "CommonUser",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Zone",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name_Ar = table.Column<string>(maxLength: 150, nullable: true),
                    Name_En = table.Column<string>(maxLength: 150, nullable: true),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    LastModifiedBy = table.Column<int>(nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    BranchId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Zone", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Branch_Zone",
                        column: x => x.BranchId,
                        principalTable: "Branch",
                        principalColumn: "BranchId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Zone_CommonUser",
                        column: x => x.CreatedBy,
                        principalTable: "CommonUser",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Zone_CommonUser1",
                        column: x => x.LastModifiedBy,
                        principalTable: "CommonUser",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "City",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    City_Name = table.Column<string>(maxLength: 150, nullable: true),
                    City_Name_AR = table.Column<string>(maxLength: 150, nullable: false),
                    State_ID = table.Column<int>(nullable: false, defaultValueSql: "((1))"),
                    CreatedBy = table.Column<string>(maxLength: 100, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    LastModifiedBy = table.Column<string>(maxLength: 100, nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Lat = table.Column<string>(maxLength: 50, nullable: true),
                    Lng = table.Column<string>(maxLength: 50, nullable: true),
                    Address = table.Column<string>(maxLength: 150, nullable: true),
                    ZoneID = table.Column<int>(nullable: true),
                    BranchId = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true, defaultValueSql: "((0))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_City", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Branch_City",
                        column: x => x.BranchId,
                        principalTable: "Branch",
                        principalColumn: "BranchId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_City_Zone",
                        column: x => x.ZoneID,
                        principalTable: "Zone",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ZoneTax",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ZoneID = table.Column<int>(nullable: false),
                    Tax = table.Column<double>(nullable: false),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    LastModifiedBy = table.Column<int>(nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ZoneTax", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ZoneTax_Zone",
                        column: x => x.ZoneID,
                        principalTable: "Zone",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CashTransfer",
                columns: table => new
                {
                    CashTransferId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RefId = table.Column<string>(maxLength: 100, nullable: true),
                    TypeId = table.Column<byte>(nullable: false),
                    ReceiverId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 250, nullable: false),
                    ZoneId = table.Column<int>(nullable: true),
                    AreaId = table.Column<int>(nullable: true),
                    Phone = table.Column<string>(maxLength: 100, nullable: true),
                    Email = table.Column<string>(maxLength: 150, nullable: true),
                    Address = table.Column<string>(nullable: true),
                    Landmark = table.Column<string>(nullable: true),
                    Floor = table.Column<int>(nullable: true),
                    Apartment = table.Column<int>(nullable: true),
                    IsReceived = table.Column<bool>(nullable: false),
                    OTP = table.Column<string>(maxLength: 100, nullable: true),
                    Amount = table.Column<double>(nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    CreatedBy = table.Column<int>(nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    LastModifiedBy = table.Column<int>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CashTransfer", x => x.CashTransferId);
                    table.ForeignKey(
                        name: "FK_CashTransfer_City",
                        column: x => x.AreaId,
                        principalTable: "City",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CashTransfer_CommonUser",
                        column: x => x.CreatedBy,
                        principalTable: "CommonUser",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CashTransfer_CommonUser1",
                        column: x => x.LastModifiedBy,
                        principalTable: "CommonUser",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CashTransfer_Account",
                        column: x => x.ReceiverId,
                        principalTable: "Account",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CashTransfer_Zone",
                        column: x => x.ZoneId,
                        principalTable: "Zone",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PickupRequest",
                columns: table => new
                {
                    PickupRequestId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PickupRequestTypeId = table.Column<int>(nullable: false),
                    PickupFees = table.Column<double>(nullable: false),
                    StatusId = table.Column<int>(nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    CreatedBy = table.Column<int>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    VendorId = table.Column<int>(nullable: false),
                    VendorName = table.Column<string>(nullable: false),
                    VendorPhone = table.Column<string>(maxLength: 250, nullable: false),
                    VendorAddress = table.Column<string>(nullable: true),
                    VendorLocation = table.Column<string>(nullable: true),
                    VendorLandmark = table.Column<string>(nullable: true),
                    VendorFloor = table.Column<int>(nullable: false),
                    VendorApartment = table.Column<int>(nullable: true),
                    ZoneId = table.Column<int>(nullable: false),
                    AreaId = table.Column<int>(nullable: false),
                    PickupDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    TimeFrom = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    TimeTo = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    RefID = table.Column<string>(nullable: true),
                    DeliveryManId = table.Column<int>(nullable: true),
                    Notes = table.Column<string>(nullable: true),
                    BranchId = table.Column<int>(nullable: true),
                    AdvertisementRequestId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PickupRequest", x => x.PickupRequestId);
                    table.ForeignKey(
                        name: "FK_PickupRequest_City",
                        column: x => x.AreaId,
                        principalTable: "City",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Branch_PickupRequest",
                        column: x => x.BranchId,
                        principalTable: "Branch",
                        principalColumn: "BranchId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PickupRequest_CommonUser2",
                        column: x => x.DeliveryManId,
                        principalTable: "CommonUser",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PickupRequestType_PickupRequest",
                        column: x => x.PickupRequestTypeId,
                        principalTable: "PickupRequestType",
                        principalColumn: "PickupRequestTypeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PickupRequest_Status_StatusId",
                        column: x => x.StatusId,
                        principalTable: "Status",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PickupRequest_CommonUser",
                        column: x => x.VendorId,
                        principalTable: "CommonUser",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PickupRequest_Zone",
                        column: x => x.ZoneId,
                        principalTable: "Zone",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AccountTransaction",
                columns: table => new
                {
                    AccountTransactionId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RefId = table.Column<string>(maxLength: 100, nullable: true),
                    TypeId = table.Column<byte>(nullable: false),
                    SenderId = table.Column<int>(nullable: true),
                    ReceiverId = table.Column<int>(nullable: false),
                    VendorId = table.Column<int>(nullable: true),
                    StatusId = table.Column<int>(nullable: true),
                    AdvertisementId = table.Column<int>(nullable: true),
                    PickupRequestId = table.Column<int>(nullable: true),
                    GameFees = table.Column<double>(nullable: false),
                    WeightFees = table.Column<double>(nullable: false),
                    SizeFees = table.Column<double>(nullable: false),
                    PartialDeliveryFees = table.Column<double>(nullable: false),
                    CancelFees = table.Column<double>(nullable: false),
                    PickupFees = table.Column<double>(nullable: false),
                    ShippingFees = table.Column<double>(nullable: false),
                    ShippingFeesPaid = table.Column<double>(nullable: true),
                    VendorCash = table.Column<double>(nullable: false),
                    Total = table.Column<double>(nullable: false),
                    CashTransferId = table.Column<int>(nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    CreatedBy = table.Column<int>(nullable: false),
                    LastModifiedBy = table.Column<int>(nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    IsDeleted = table.Column<bool>(nullable: false),
                    RefundCash = table.Column<double>(nullable: true),
                    RefundFees = table.Column<double>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountTransaction", x => x.AccountTransactionId);
                    table.ForeignKey(
                        name: "FK_AccountTransaction_CashTransfer",
                        column: x => x.CashTransferId,
                        principalTable: "CashTransfer",
                        principalColumn: "CashTransferId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AccountTransaction_CommonUser",
                        column: x => x.CreatedBy,
                        principalTable: "CommonUser",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AccountTransaction_CommonUser1",
                        column: x => x.LastModifiedBy,
                        principalTable: "CommonUser",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AccountTransaction_PickupRequest",
                        column: x => x.PickupRequestId,
                        principalTable: "PickupRequest",
                        principalColumn: "PickupRequestId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AccountTransaction_Account1",
                        column: x => x.ReceiverId,
                        principalTable: "Account",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AccountTransaction_Account",
                        column: x => x.SenderId,
                        principalTable: "Account",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AccountTransaction_Status_StatusId",
                        column: x => x.StatusId,
                        principalTable: "Status",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AccountTransaction_Account2",
                        column: x => x.VendorId,
                        principalTable: "Account",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Advertisement",
                columns: table => new
                {
                    AdvertisementId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RefId = table.Column<string>(maxLength: 255, nullable: true),
                    GameId = table.Column<int>(nullable: false),
                    StatusId = table.Column<int>(nullable: false),
                    GamerId = table.Column<int>(nullable: false),
                    BuyerId = table.Column<int>(nullable: true),
                    AdvertisementRequestId = table.Column<int>(nullable: true),
                    CashTransferId = table.Column<int>(nullable: true),
                    Description = table.Column<string>(maxLength: 500, nullable: true),
                    UserName = table.Column<string>(maxLength: 500, nullable: true),
                    Password = table.Column<string>(maxLength: 250, nullable: true),
                    Level = table.Column<int>(nullable: false),
                    Rank = table.Column<string>(nullable: true),
                    Price = table.Column<int>(nullable: false),
                    IsRefund = table.Column<bool>(nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    CreatedBy = table.Column<int>(nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    LastModifiedBy = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    BranchId = table.Column<int>(nullable: true),
                    CityId = table.Column<int>(nullable: true),
                    CommonUserId = table.Column<int>(nullable: true),
                    GameId1 = table.Column<int>(nullable: true),
                    PickupRequestId = table.Column<int>(nullable: true),
                    ShipmentTypeId = table.Column<int>(nullable: true),
                    ZoneId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Advertisement", x => x.AdvertisementId);
                    table.ForeignKey(
                        name: "FK_Advertisement_Branch_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branch",
                        principalColumn: "BranchId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Advertisement_CommonUser_BuyerId",
                        column: x => x.BuyerId,
                        principalTable: "CommonUser",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Advertisement_CashTransfer_CashTransferId",
                        column: x => x.CashTransferId,
                        principalTable: "CashTransfer",
                        principalColumn: "CashTransferId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Advertisement_City_CityId",
                        column: x => x.CityId,
                        principalTable: "City",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Advertisement_CommonUser_CommonUserId",
                        column: x => x.CommonUserId,
                        principalTable: "CommonUser",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Shipment_Game",
                        column: x => x.GameId,
                        principalTable: "Game",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Advertisement_Game_GameId1",
                        column: x => x.GameId1,
                        principalTable: "Game",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Advertisement_CommonUser_GamerId",
                        column: x => x.GamerId,
                        principalTable: "CommonUser",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Advertisement_PickupRequest_PickupRequestId",
                        column: x => x.PickupRequestId,
                        principalTable: "PickupRequest",
                        principalColumn: "PickupRequestId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Advertisement_ShipmentType_ShipmentTypeId",
                        column: x => x.ShipmentTypeId,
                        principalTable: "ShipmentType",
                        principalColumn: "ShipmentTypeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Advertisement_Status_StatusId",
                        column: x => x.StatusId,
                        principalTable: "Status",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Advertisement_Zone_ZoneId",
                        column: x => x.ZoneId,
                        principalTable: "Zone",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AdvertisementPhotos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AdvertisementId = table.Column<int>(nullable: false),
                    Url = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdvertisementPhotos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdvertisementPhotos_Advertisement_AdvertisementId",
                        column: x => x.AdvertisementId,
                        principalTable: "Advertisement",
                        principalColumn: "AdvertisementId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FollowUp",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AdvertisementId = table.Column<int>(nullable: true),
                    StatusID = table.Column<int>(nullable: true),
                    Comment = table.Column<string>(maxLength: 500, nullable: true),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    LastModifiedBy = table.Column<int>(nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Lat = table.Column<string>(maxLength: 50, nullable: true),
                    Lng = table.Column<string>(maxLength: 50, nullable: true),
                    Title = table.Column<string>(maxLength: 100, nullable: true),
                    GameServiceGamerServiceId = table.Column<int>(nullable: true),
                    FollowUpTypeId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FollowUp", x => x.ID);
                    table.ForeignKey(
                        name: "FK_FollowUp_Shipment",
                        column: x => x.AdvertisementId,
                        principalTable: "Advertisement",
                        principalColumn: "AdvertisementId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Visit_FollowUp_CommonUser",
                        column: x => x.CreatedBy,
                        principalTable: "CommonUser",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FollowUp_FollowUpType_FollowUpTypeId",
                        column: x => x.FollowUpTypeId,
                        principalTable: "FollowUpType",
                        principalColumn: "FollowUpTypeID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FollowUp_GamerService_GameServiceGamerServiceId",
                        column: x => x.GameServiceGamerServiceId,
                        principalTable: "GamerService",
                        principalColumn: "GamerServiceId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Invoices",
                columns: table => new
                {
                    InvoiceId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RefId = table.Column<string>(nullable: true),
                    AdvertisementId = table.Column<int>(nullable: true),
                    GamerServiceId = table.Column<int>(nullable: true),
                    Price = table.Column<int>(nullable: false),
                    IsRefund = table.Column<bool>(nullable: false),
                    InvoiceType = table.Column<int>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<int>(nullable: false),
                    LastModifiedAt = table.Column<DateTime>(nullable: true),
                    LastModifiedBy = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invoices", x => x.InvoiceId);
                    table.ForeignKey(
                        name: "FK_Invoices_Advertisement_AdvertisementId",
                        column: x => x.AdvertisementId,
                        principalTable: "Advertisement",
                        principalColumn: "AdvertisementId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Invoices_GamerService_GamerServiceId",
                        column: x => x.GamerServiceId,
                        principalTable: "GamerService",
                        principalColumn: "GamerServiceId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ShipmentCustomerFollowUp",
                columns: table => new
                {
                    ShipmentCustomerFollowUpId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AdvertisementId = table.Column<int>(nullable: false),
                    Note = table.Column<string>(nullable: true),
                    IsConfirmed = table.Column<bool>(nullable: true),
                    IsCallAnswered = table.Column<bool>(nullable: true),
                    NextCallOn = table.Column<DateTime>(type: "datetime", nullable: true),
                    NextCallTimeFrom = table.Column<DateTime>(type: "datetime", nullable: true),
                    NextCallTimeTo = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    CreatedBy = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShipmentCustomerFollowUp", x => x.ShipmentCustomerFollowUpId);
                    table.ForeignKey(
                        name: "FK_ShipmentCustomerFollowUp_Advertisement_AdvertisementId",
                        column: x => x.AdvertisementId,
                        principalTable: "Advertisement",
                        principalColumn: "AdvertisementId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShipmentCustomerFollowup_CommonUser",
                        column: x => x.CreatedBy,
                        principalTable: "CommonUser",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Account_CreatedBy",
                table: "Account",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Account_LastModifiedBy",
                table: "Account",
                column: "LastModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Account_RoleId",
                table: "Account",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Account_UserId",
                table: "Account",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountTransaction_CashTransferId",
                table: "AccountTransaction",
                column: "CashTransferId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountTransaction_CreatedBy",
                table: "AccountTransaction",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_AccountTransaction_LastModifiedBy",
                table: "AccountTransaction",
                column: "LastModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_AccountTransaction_PickupRequestId",
                table: "AccountTransaction",
                column: "PickupRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountTransaction_ReceiverId",
                table: "AccountTransaction",
                column: "ReceiverId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountTransaction_SenderId",
                table: "AccountTransaction",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountTransaction_StatusId",
                table: "AccountTransaction",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountTransaction_VendorId",
                table: "AccountTransaction",
                column: "VendorId");

            migrationBuilder.CreateIndex(
                name: "IX_Advertisement_BranchId",
                table: "Advertisement",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Advertisement_BuyerId",
                table: "Advertisement",
                column: "BuyerId");

            migrationBuilder.CreateIndex(
                name: "IX_Advertisement_CashTransferId",
                table: "Advertisement",
                column: "CashTransferId");

            migrationBuilder.CreateIndex(
                name: "IX_Advertisement_CityId",
                table: "Advertisement",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_Advertisement_CommonUserId",
                table: "Advertisement",
                column: "CommonUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Advertisement_GameId",
                table: "Advertisement",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_Advertisement_GameId1",
                table: "Advertisement",
                column: "GameId1");

            migrationBuilder.CreateIndex(
                name: "IX_Advertisement_GamerId",
                table: "Advertisement",
                column: "GamerId");

            migrationBuilder.CreateIndex(
                name: "IX_Advertisement_PickupRequestId",
                table: "Advertisement",
                column: "PickupRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_Advertisement_ShipmentTypeId",
                table: "Advertisement",
                column: "ShipmentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Advertisement_StatusId",
                table: "Advertisement",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Advertisement_ZoneId",
                table: "Advertisement",
                column: "ZoneId");

            migrationBuilder.CreateIndex(
                name: "IX_AdvertisementPhotos_AdvertisementId",
                table: "AdvertisementPhotos",
                column: "AdvertisementId");

            migrationBuilder.CreateIndex(
                name: "Service_Name_unique",
                table: "AppService",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CashTransfer_AreaId",
                table: "CashTransfer",
                column: "AreaId");

            migrationBuilder.CreateIndex(
                name: "IX_CashTransfer_CreatedBy",
                table: "CashTransfer",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_CashTransfer_LastModifiedBy",
                table: "CashTransfer",
                column: "LastModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_CashTransfer_ReceiverId",
                table: "CashTransfer",
                column: "ReceiverId");

            migrationBuilder.CreateIndex(
                name: "IX_CashTransfer_ZoneId",
                table: "CashTransfer",
                column: "ZoneId");

            migrationBuilder.CreateIndex(
                name: "IX_City_BranchId",
                table: "City",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_City_ZoneID",
                table: "City",
                column: "ZoneID");

            migrationBuilder.CreateIndex(
                name: "IX_Common_Resource",
                table: "Common_Resource",
                columns: new[] { "ResourceKey", "ResourceLang" },
                unique: true,
                filter: "[ResourceKey] IS NOT NULL AND [ResourceLang] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Common_UserDevice_Common_UserID",
                table: "Common_UserDevice",
                column: "Common_UserID");

            migrationBuilder.CreateIndex(
                name: "IX_CommonUser_BranchId",
                table: "CommonUser",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_CommonUser_CountryId",
                table: "CommonUser",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_CommonUser_RoleID",
                table: "CommonUser",
                column: "RoleID");

            migrationBuilder.CreateIndex(
                name: "IX_Complain_ActionBy",
                table: "Complain",
                column: "ActionBy");

            migrationBuilder.CreateIndex(
                name: "IX_Complain_EmployeeId",
                table: "Complain",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Complain_VendorId",
                table: "Complain",
                column: "VendorId");

            migrationBuilder.CreateIndex(
                name: "IX_ContactUs_UserID",
                table: "ContactUs",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_FollowUp_AdvertisementId",
                table: "FollowUp",
                column: "AdvertisementId");

            migrationBuilder.CreateIndex(
                name: "IX_FollowUp_CreatedBy",
                table: "FollowUp",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_FollowUp_FollowUpTypeId",
                table: "FollowUp",
                column: "FollowUpTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_FollowUp_GameServiceGamerServiceId",
                table: "FollowUp",
                column: "GameServiceGamerServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_Game_CategoryId",
                table: "Game",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_GamerService_GameId",
                table: "GamerService",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_GamerService_GamerId",
                table: "GamerService",
                column: "GamerId");

            migrationBuilder.CreateIndex(
                name: "IX_GamerService_StatusId",
                table: "GamerService",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_AdvertisementId",
                table: "Invoices",
                column: "AdvertisementId",
                unique: true,
                filter: "[AdvertisementId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_GamerServiceId",
                table: "Invoices",
                column: "GamerServiceId",
                unique: true,
                filter: "[GamerServiceId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Notification_RecipientRoleId",
                table: "Notification",
                column: "RecipientRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Notification_SenderID",
                table: "Notification",
                column: "SenderID");

            migrationBuilder.CreateIndex(
                name: "IX_PickupRequest_AreaId",
                table: "PickupRequest",
                column: "AreaId");

            migrationBuilder.CreateIndex(
                name: "IX_PickupRequest_BranchId",
                table: "PickupRequest",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_PickupRequest_DeliveryManId",
                table: "PickupRequest",
                column: "DeliveryManId");

            migrationBuilder.CreateIndex(
                name: "IX_PickupRequest_PickupRequestTypeId",
                table: "PickupRequest",
                column: "PickupRequestTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_PickupRequest_StatusId",
                table: "PickupRequest",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_PickupRequest_VendorId",
                table: "PickupRequest",
                column: "VendorId");

            migrationBuilder.CreateIndex(
                name: "IX_PickupRequest_ZoneId",
                table: "PickupRequest",
                column: "ZoneId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleAppService_RoleID",
                table: "RoleAppService",
                column: "RoleID");

            migrationBuilder.CreateIndex(
                name: "RoleIDServiceIDUniq",
                table: "RoleAppService",
                columns: new[] { "AppServiceID", "RoleID" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ShipmentCustomerFollowUp_AdvertisementId",
                table: "ShipmentCustomerFollowUp",
                column: "AdvertisementId");

            migrationBuilder.CreateIndex(
                name: "IX_ShipmentCustomerFollowUp_CreatedBy",
                table: "ShipmentCustomerFollowUp",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_UserLocation_UserID",
                table: "UserLocation",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_VendorProduct_VendorId",
                table: "VendorProduct",
                column: "VendorId");

            migrationBuilder.CreateIndex(
                name: "IX_Zone_BranchId",
                table: "Zone",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Zone_CreatedBy",
                table: "Zone",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Zone_LastModifiedBy",
                table: "Zone",
                column: "LastModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ZoneTax_ZoneID",
                table: "ZoneTax",
                column: "ZoneID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountTransaction");

            migrationBuilder.DropTable(
                name: "AdvertisementPhotos");

            migrationBuilder.DropTable(
                name: "Common_Resource");

            migrationBuilder.DropTable(
                name: "Common_UserDevice");

            migrationBuilder.DropTable(
                name: "Complain");

            migrationBuilder.DropTable(
                name: "ContactUs");

            migrationBuilder.DropTable(
                name: "FollowUp");

            migrationBuilder.DropTable(
                name: "Invoices");

            migrationBuilder.DropTable(
                name: "Notification");

            migrationBuilder.DropTable(
                name: "ProblemType");

            migrationBuilder.DropTable(
                name: "RoleAppService");

            migrationBuilder.DropTable(
                name: "ShipmentCustomerFollowUp");

            migrationBuilder.DropTable(
                name: "Userhubconnection");

            migrationBuilder.DropTable(
                name: "UserLocation");

            migrationBuilder.DropTable(
                name: "VendorProduct");

            migrationBuilder.DropTable(
                name: "Warehouse");

            migrationBuilder.DropTable(
                name: "ZoneTax");

            migrationBuilder.DropTable(
                name: "FollowUpType");

            migrationBuilder.DropTable(
                name: "GamerService");

            migrationBuilder.DropTable(
                name: "AppService");

            migrationBuilder.DropTable(
                name: "Advertisement");

            migrationBuilder.DropTable(
                name: "CashTransfer");

            migrationBuilder.DropTable(
                name: "Game");

            migrationBuilder.DropTable(
                name: "PickupRequest");

            migrationBuilder.DropTable(
                name: "ShipmentType");

            migrationBuilder.DropTable(
                name: "Account");

            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropTable(
                name: "City");

            migrationBuilder.DropTable(
                name: "PickupRequestType");

            migrationBuilder.DropTable(
                name: "Status");

            migrationBuilder.DropTable(
                name: "Zone");

            migrationBuilder.DropTable(
                name: "CommonUser");

            migrationBuilder.DropTable(
                name: "Branch");

            migrationBuilder.DropTable(
                name: "Countries");

            migrationBuilder.DropTable(
                name: "Role");
        }
    }
}

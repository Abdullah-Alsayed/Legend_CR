using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LegendCR.DAL.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppServiceClass",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppServiceClass", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CommonResource",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ResourceKey = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ResourceValue = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ResourceLang = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    LastUpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    ApplicationId = table.Column<int>(type: "int", nullable: true),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MediaUrl = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommonResource", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FollowUpType",
                columns: table => new
                {
                    FollowUpTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TypeName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FollowUpType", x => x.FollowUpTypeId);
                });

            migrationBuilder.CreateTable(
                name: "ProblemType",
                columns: table => new
                {
                    ProblemTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProblemType", x => x.ProblemTypeId);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Editable = table.Column<bool>(type: "bit", nullable: true),
                    IsInternal = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Status",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Status", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Warehouse",
                columns: table => new
                {
                    WarehouseId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BranchId = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WarehouseName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Warehouse", x => x.WarehouseId);
                });

            migrationBuilder.CreateTable(
                name: "AppServiceGroup",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AppServiceClassId = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppServiceGroup", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppServiceGroup_AppServiceClass_AppServiceClassId",
                        column: x => x.AppServiceClassId,
                        principalTable: "AppServiceClass",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CommonUser",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastLoginDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsLoggedIn = table.Column<bool>(type: "bit", nullable: false),
                    NationalId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AreaId = table.Column<int>(type: "int", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Lat = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Lng = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastLocationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    VerificationCode = table.Column<long>(type: "bigint", nullable: true),
                    IsVerified = table.Column<bool>(type: "bit", nullable: true),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AddressDetails = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Averageorders = table.Column<int>(type: "int", nullable: true),
                    OpenPackage = table.Column<bool>(type: "bit", nullable: true),
                    PartialDelivery = table.Column<bool>(type: "bit", nullable: true),
                    VisaPayment = table.Column<bool>(type: "bit", nullable: true),
                    PageName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LocationUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Landmark = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Floor = table.Column<int>(type: "int", nullable: true),
                    Apartment = table.Column<int>(type: "int", nullable: true),
                    BranchId = table.Column<int>(type: "int", nullable: true),
                    Bank = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AccountName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AccountNumber = table.Column<int>(type: "int", nullable: true),
                    IbanNumber = table.Column<int>(type: "int", nullable: true),
                    VodafoneCashNumber = table.Column<int>(type: "int", nullable: true),
                    InstaPayAccountName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ZoneId = table.Column<int>(type: "int", nullable: true),
                    ImgUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HashedPassword = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommonUser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CommonUser_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppService",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AllowAnonymous = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShowToUser = table.Column<bool>(type: "bit", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ClassName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AllowLog = table.Column<bool>(type: "bit", nullable: false),
                    LogMessage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AppServiceClassId = table.Column<int>(type: "int", nullable: true),
                    AppServiceGroupId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppService", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppService_AppServiceClass_AppServiceClassId",
                        column: x => x.AppServiceClassId,
                        principalTable: "AppServiceClass",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppService_AppServiceGroup_AppServiceGroupId",
                        column: x => x.AppServiceGroupId,
                        principalTable: "AppServiceGroup",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CommonUserDevice",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CommonUserId = table.Column<int>(type: "int", nullable: false),
                    DeviceName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeviceImei = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeviceType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeviceOsversion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeviceToken = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeviceEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EnableNotification = table.Column<bool>(type: "bit", nullable: true),
                    AuthToken = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AuthIp = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AuthCreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AuthExpirationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsLoggedIn = table.Column<bool>(type: "bit", nullable: false),
                    DeviceMobileNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastActiveDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Lang = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    LastUpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsGoogleSupport = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommonUserDevice", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CommonUserDevice_CommonUser_CommonUserId",
                        column: x => x.CommonUserId,
                        principalTable: "CommonUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ContactUs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Subject = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactUs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContactUs_CommonUser_UserId",
                        column: x => x.UserId,
                        principalTable: "CommonUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FollowUp",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ShipmentId = table.Column<int>(type: "int", nullable: true),
                    StatusId = table.Column<int>(type: "int", nullable: true),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<int>(type: "int", nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Lat = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Lng = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FollowUpTypeId = table.Column<int>(type: "int", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedByNavigationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FollowUp", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FollowUp_CommonUser_CreatedByNavigationId",
                        column: x => x.CreatedByNavigationId,
                        principalTable: "CommonUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FollowUp_FollowUpType_FollowUpTypeId",
                        column: x => x.FollowUpTypeId,
                        principalTable: "FollowUpType",
                        principalColumn: "FollowUpTypeId");
                });

            migrationBuilder.CreateTable(
                name: "Notification",
                columns: table => new
                {
                    NotificationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SenderId = table.Column<int>(type: "int", nullable: true),
                    RecipientId = table.Column<int>(type: "int", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Body = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Icon = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsSeen = table.Column<bool>(type: "bit", nullable: true),
                    SeenDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TargetPath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RecipientRoleId = table.Column<int>(type: "int", nullable: true),
                    SeenBy = table.Column<int>(type: "int", nullable: true),
                    DeletedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notification", x => x.NotificationId);
                    table.ForeignKey(
                        name: "FK_Notification_CommonUser_SenderId",
                        column: x => x.SenderId,
                        principalTable: "CommonUser",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Notification_Role_RecipientRoleId",
                        column: x => x.RecipientRoleId,
                        principalTable: "Role",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserLocation",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PlaceId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Latitude = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Longitude = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CreationDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SubmitDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BataryStatus = table.Column<int>(type: "int", nullable: true),
                    NetworkStatus = table.Column<int>(type: "int", nullable: true),
                    IsOnline = table.Column<bool>(type: "bit", nullable: true),
                    DirectionAngle = table.Column<double>(type: "float", nullable: true),
                    AquirityDistance = table.Column<double>(type: "float", nullable: true),
                    ChannelId = table.Column<long>(type: "bigint", nullable: true),
                    ApplicationId = table.Column<long>(type: "bigint", nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLocation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserLocation_CommonUser_UserId",
                        column: x => x.UserId,
                        principalTable: "CommonUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VendorProduct",
                columns: table => new
                {
                    VendorProductId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OriginalPrice = table.Column<double>(type: "float", nullable: false),
                    VendorId = table.Column<int>(type: "int", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    AvailableStock = table.Column<int>(type: "int", nullable: false),
                    StockPrice = table.Column<double>(type: "float", nullable: false),
                    Size = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VendorProduct", x => x.VendorProductId);
                    table.ForeignKey(
                        name: "FK_VendorProduct_CommonUser_VendorId",
                        column: x => x.VendorId,
                        principalTable: "CommonUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Zone",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<int>(type: "int", nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BranchId = table.Column<int>(type: "int", nullable: true),
                    CommonUserId = table.Column<int>(type: "int", nullable: true),
                    CommonUserId1 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Zone", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Zone_CommonUser_CommonUserId",
                        column: x => x.CommonUserId,
                        principalTable: "CommonUser",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Zone_CommonUser_CommonUserId1",
                        column: x => x.CommonUserId1,
                        principalTable: "CommonUser",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RoleAppService",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    AppServiceId = table.Column<int>(type: "int", nullable: false),
                    Enabled = table.Column<bool>(type: "bit", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleAppService", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoleAppService_AppService_AppServiceId",
                        column: x => x.AppServiceId,
                        principalTable: "AppService",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoleAppService_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "City",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CityName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CityNameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StateId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Lat = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Lng = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ZoneId = table.Column<int>(type: "int", nullable: true),
                    BranchId = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_City", x => x.Id);
                    table.ForeignKey(
                        name: "FK_City_Zone_ZoneId",
                        column: x => x.ZoneId,
                        principalTable: "Zone",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ZoneTax",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ZoneId = table.Column<int>(type: "int", nullable: false),
                    Tax = table.Column<double>(type: "float", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<int>(type: "int", nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ZoneTax", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ZoneTax_Zone_ZoneId",
                        column: x => x.ZoneId,
                        principalTable: "Zone",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppService_AppServiceClassId",
                table: "AppService",
                column: "AppServiceClassId");

            migrationBuilder.CreateIndex(
                name: "IX_AppService_AppServiceGroupId",
                table: "AppService",
                column: "AppServiceGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_AppServiceGroup_AppServiceClassId",
                table: "AppServiceGroup",
                column: "AppServiceClassId");

            migrationBuilder.CreateIndex(
                name: "IX_City_ZoneId",
                table: "City",
                column: "ZoneId");

            migrationBuilder.CreateIndex(
                name: "IX_CommonUser_RoleId",
                table: "CommonUser",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_CommonUserDevice_CommonUserId",
                table: "CommonUserDevice",
                column: "CommonUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ContactUs_UserId",
                table: "ContactUs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_FollowUp_CreatedByNavigationId",
                table: "FollowUp",
                column: "CreatedByNavigationId");

            migrationBuilder.CreateIndex(
                name: "IX_FollowUp_FollowUpTypeId",
                table: "FollowUp",
                column: "FollowUpTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Notification_RecipientRoleId",
                table: "Notification",
                column: "RecipientRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Notification_SenderId",
                table: "Notification",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleAppService_AppServiceId",
                table: "RoleAppService",
                column: "AppServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleAppService_RoleId",
                table: "RoleAppService",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLocation_UserId",
                table: "UserLocation",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_VendorProduct_VendorId",
                table: "VendorProduct",
                column: "VendorId");

            migrationBuilder.CreateIndex(
                name: "IX_Zone_CommonUserId",
                table: "Zone",
                column: "CommonUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Zone_CommonUserId1",
                table: "Zone",
                column: "CommonUserId1");

            migrationBuilder.CreateIndex(
                name: "IX_ZoneTax_ZoneId",
                table: "ZoneTax",
                column: "ZoneId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "City");

            migrationBuilder.DropTable(
                name: "CommonResource");

            migrationBuilder.DropTable(
                name: "CommonUserDevice");

            migrationBuilder.DropTable(
                name: "ContactUs");

            migrationBuilder.DropTable(
                name: "FollowUp");

            migrationBuilder.DropTable(
                name: "Notification");

            migrationBuilder.DropTable(
                name: "ProblemType");

            migrationBuilder.DropTable(
                name: "RoleAppService");

            migrationBuilder.DropTable(
                name: "Status");

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
                name: "AppService");

            migrationBuilder.DropTable(
                name: "Zone");

            migrationBuilder.DropTable(
                name: "AppServiceGroup");

            migrationBuilder.DropTable(
                name: "CommonUser");

            migrationBuilder.DropTable(
                name: "AppServiceClass");

            migrationBuilder.DropTable(
                name: "Role");
        }
    }
}

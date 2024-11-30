using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DicomApp.DAL.Migrations
{
    public partial class cleanUpDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Advertisement_Branch_BranchId",
                table: "Advertisement");

            migrationBuilder.DropForeignKey(
                name: "FK_Advertisement_CashTransfer_CashTransferId",
                table: "Advertisement");

            migrationBuilder.DropForeignKey(
                name: "FK_Advertisement_City_CityId",
                table: "Advertisement");

            migrationBuilder.DropForeignKey(
                name: "FK_Advertisement_PickupRequest_PickupRequestId",
                table: "Advertisement");

            migrationBuilder.DropForeignKey(
                name: "FK_Advertisement_ShipmentType_ShipmentTypeId",
                table: "Advertisement");

            migrationBuilder.DropForeignKey(
                name: "FK_Advertisement_Zone_ZoneId",
                table: "Advertisement");

            migrationBuilder.DropForeignKey(
                name: "FK_Branch_CommonUser",
                table: "CommonUser");

            migrationBuilder.DropForeignKey(
                name: "FK_FollowUp_FollowUpType_FollowUpTypeId",
                table: "FollowUp");

            migrationBuilder.DropForeignKey(
                name: "FK_FollowUp_GamerService_GameServiceGamerServiceId",
                table: "FollowUp");

            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_CashTransfer_CashTransferId",
                table: "Transaction");

            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_PickupRequest_PickupRequestId",
                table: "Transaction");

            migrationBuilder.DropTable(
                name: "CashTransfer");

            migrationBuilder.DropTable(
                name: "Complain");

            migrationBuilder.DropTable(
                name: "FollowUpType");

            migrationBuilder.DropTable(
                name: "PickupRequest");

            migrationBuilder.DropTable(
                name: "ProblemType");

            migrationBuilder.DropTable(
                name: "ShipmentCustomerFollowUp");

            migrationBuilder.DropTable(
                name: "ShipmentType");

            migrationBuilder.DropTable(
                name: "VendorProduct");

            migrationBuilder.DropTable(
                name: "Warehouse");

            migrationBuilder.DropTable(
                name: "ZoneTax");

            migrationBuilder.DropTable(
                name: "City");

            migrationBuilder.DropTable(
                name: "PickupRequestType");

            migrationBuilder.DropTable(
                name: "Zone");

            migrationBuilder.DropTable(
                name: "Branch");

            migrationBuilder.DropIndex(
                name: "IX_Transaction_CashTransferId",
                table: "Transaction");

            migrationBuilder.DropIndex(
                name: "IX_Transaction_PickupRequestId",
                table: "Transaction");

            migrationBuilder.DropIndex(
                name: "IX_FollowUp_FollowUpTypeId",
                table: "FollowUp");

            migrationBuilder.DropIndex(
                name: "IX_FollowUp_GameServiceGamerServiceId",
                table: "FollowUp");

            migrationBuilder.DropIndex(
                name: "IX_CommonUser_BranchId",
                table: "CommonUser");

            migrationBuilder.DropIndex(
                name: "IX_Advertisement_BranchId",
                table: "Advertisement");

            migrationBuilder.DropIndex(
                name: "IX_Advertisement_CashTransferId",
                table: "Advertisement");

            migrationBuilder.DropIndex(
                name: "IX_Advertisement_CityId",
                table: "Advertisement");

            migrationBuilder.DropIndex(
                name: "IX_Advertisement_PickupRequestId",
                table: "Advertisement");

            migrationBuilder.DropIndex(
                name: "IX_Advertisement_ShipmentTypeId",
                table: "Advertisement");

            migrationBuilder.DropIndex(
                name: "IX_Advertisement_ZoneId",
                table: "Advertisement");

            migrationBuilder.DropColumn(
                name: "CashTransferId",
                table: "Transaction");

            migrationBuilder.DropColumn(
                name: "PickupRequestId",
                table: "Transaction");

            migrationBuilder.DropColumn(
                name: "Comment",
                table: "FollowUp");

            migrationBuilder.DropColumn(
                name: "FollowUpTypeId",
                table: "FollowUp");

            migrationBuilder.DropColumn(
                name: "GameServiceGamerServiceId",
                table: "FollowUp");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "FollowUp");

            migrationBuilder.DropColumn(
                name: "LastModifiedAt",
                table: "FollowUp");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                table: "FollowUp");

            migrationBuilder.DropColumn(
                name: "Lat",
                table: "FollowUp");

            migrationBuilder.DropColumn(
                name: "Lng",
                table: "FollowUp");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "FollowUp");

            migrationBuilder.DropColumn(
                name: "BranchId",
                table: "Advertisement");

            migrationBuilder.DropColumn(
                name: "CityId",
                table: "Advertisement");

            migrationBuilder.DropColumn(
                name: "PickupRequestId",
                table: "Advertisement");

            migrationBuilder.DropColumn(
                name: "ShipmentTypeId",
                table: "Advertisement");

            migrationBuilder.DropColumn(
                name: "ZoneId",
                table: "Advertisement");

            migrationBuilder.RenameColumn(
                name: "StatusID",
                table: "FollowUp",
                newName: "GamerServiceId");

            migrationBuilder.AddColumn<int>(
                name: "ActionType",
                table: "FollowUp",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EntityType",
                table: "FollowUp",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Message",
                table: "FollowUp",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_FollowUp_GamerServiceId",
                table: "FollowUp",
                column: "GamerServiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_FollowUp_GamerService_GamerServiceId",
                table: "FollowUp",
                column: "GamerServiceId",
                principalTable: "GamerService",
                principalColumn: "GamerServiceId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FollowUp_GamerService_GamerServiceId",
                table: "FollowUp");

            migrationBuilder.DropIndex(
                name: "IX_FollowUp_GamerServiceId",
                table: "FollowUp");

            migrationBuilder.DropColumn(
                name: "ActionType",
                table: "FollowUp");

            migrationBuilder.DropColumn(
                name: "EntityType",
                table: "FollowUp");

            migrationBuilder.DropColumn(
                name: "Message",
                table: "FollowUp");

            migrationBuilder.RenameColumn(
                name: "GamerServiceId",
                table: "FollowUp",
                newName: "StatusID");

            migrationBuilder.AddColumn<int>(
                name: "CashTransferId",
                table: "Transaction",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PickupRequestId",
                table: "Transaction",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Comment",
                table: "FollowUp",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FollowUpTypeId",
                table: "FollowUp",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GameServiceGamerServiceId",
                table: "FollowUp",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "FollowUp",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedAt",
                table: "FollowUp",
                type: "datetime",
                nullable: false,
                defaultValueSql: "(getdate())");

            migrationBuilder.AddColumn<int>(
                name: "LastModifiedBy",
                table: "FollowUp",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Lat",
                table: "FollowUp",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Lng",
                table: "FollowUp",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "FollowUp",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BranchId",
                table: "Advertisement",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CityId",
                table: "Advertisement",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PickupRequestId",
                table: "Advertisement",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ShipmentTypeId",
                table: "Advertisement",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ZoneId",
                table: "Advertisement",
                nullable: true);

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
                    IsDeleted = table.Column<bool>(nullable: true, defaultValueSql: "((0))"),
                    Phone = table.Column<string>(nullable: true),
                    State = table.Column<string>(nullable: true),
                    ZipCode = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Branch", x => x.BranchId);
                });

            migrationBuilder.CreateTable(
                name: "Complain",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ActionBy = table.Column<int>(nullable: false),
                    Comments = table.Column<string>(nullable: true),
                    Date = table.Column<DateTime>(type: "datetime", nullable: false),
                    Department = table.Column<string>(maxLength: 150, nullable: true),
                    Description = table.Column<string>(nullable: true),
                    EmployeeId = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true, defaultValueSql: "((0))"),
                    IsSolved = table.Column<bool>(nullable: true, defaultValueSql: "((0))"),
                    VendorId = table.Column<int>(nullable: true)
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
                    IsDeleted = table.Column<bool>(nullable: true, defaultValueSql: "((0))"),
                    NameAr = table.Column<string>(maxLength: 150, nullable: true),
                    NameEn = table.Column<string>(maxLength: 150, nullable: true),
                    Type = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProblemType", x => x.ProblemTypeId);
                });

            migrationBuilder.CreateTable(
                name: "ShipmentCustomerFollowUp",
                columns: table => new
                {
                    ShipmentCustomerFollowUpId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AdvertisementId = table.Column<int>(nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    CreatedBy = table.Column<int>(nullable: false),
                    IsCallAnswered = table.Column<bool>(nullable: true),
                    IsConfirmed = table.Column<bool>(nullable: true),
                    NextCallOn = table.Column<DateTime>(type: "datetime", nullable: true),
                    NextCallTimeFrom = table.Column<DateTime>(type: "datetime", nullable: true),
                    NextCallTimeTo = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    Note = table.Column<string>(nullable: true)
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
                name: "VendorProduct",
                columns: table => new
                {
                    VendorProductId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AvailableStock = table.Column<int>(nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    CreatedBy = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    ImageUrl = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    OriginalPrice = table.Column<double>(nullable: false),
                    Size = table.Column<string>(maxLength: 150, nullable: true),
                    StockPrice = table.Column<double>(nullable: false),
                    VendorId = table.Column<int>(nullable: false)
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
                name: "Zone",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BranchId = table.Column<int>(nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    CreatedBy = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    LastModifiedBy = table.Column<int>(nullable: false),
                    Name_Ar = table.Column<string>(maxLength: 150, nullable: true),
                    Name_En = table.Column<string>(maxLength: 150, nullable: true)
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
                    Address = table.Column<string>(maxLength: 150, nullable: true),
                    BranchId = table.Column<int>(nullable: true),
                    City_Name = table.Column<string>(maxLength: 150, nullable: true),
                    City_Name_AR = table.Column<string>(maxLength: 150, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    CreatedBy = table.Column<string>(maxLength: 100, nullable: false),
                    IsDeleted = table.Column<bool>(nullable: true, defaultValueSql: "((0))"),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    LastModifiedBy = table.Column<string>(maxLength: 100, nullable: false),
                    Lat = table.Column<string>(maxLength: 50, nullable: true),
                    Lng = table.Column<string>(maxLength: 50, nullable: true),
                    State_ID = table.Column<int>(nullable: false, defaultValueSql: "((1))"),
                    ZoneID = table.Column<int>(nullable: true)
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
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    CreatedBy = table.Column<int>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    LastModifiedBy = table.Column<int>(nullable: false),
                    Tax = table.Column<double>(nullable: false),
                    ZoneID = table.Column<int>(nullable: false)
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
                    Address = table.Column<string>(nullable: true),
                    Amount = table.Column<double>(nullable: false),
                    Apartment = table.Column<int>(nullable: true),
                    AreaId = table.Column<int>(nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    CreatedBy = table.Column<int>(nullable: false),
                    Email = table.Column<string>(maxLength: 150, nullable: true),
                    Floor = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    IsReceived = table.Column<bool>(nullable: false),
                    Landmark = table.Column<string>(nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    LastModifiedBy = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 250, nullable: false),
                    OTP = table.Column<string>(maxLength: 100, nullable: true),
                    Phone = table.Column<string>(maxLength: 100, nullable: true),
                    ReceiverId = table.Column<int>(nullable: false),
                    RefId = table.Column<string>(maxLength: 100, nullable: true),
                    TypeId = table.Column<byte>(nullable: false),
                    ZoneId = table.Column<int>(nullable: true)
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
                    AdvertisementRequestId = table.Column<int>(nullable: true),
                    AreaId = table.Column<int>(nullable: false),
                    BranchId = table.Column<int>(nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    CreatedBy = table.Column<int>(nullable: false),
                    DeliveryManId = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Notes = table.Column<string>(nullable: true),
                    PickupDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    PickupFees = table.Column<double>(nullable: false),
                    PickupRequestTypeId = table.Column<int>(nullable: false),
                    RefID = table.Column<string>(nullable: true),
                    StatusId = table.Column<int>(nullable: false),
                    TimeFrom = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    TimeTo = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    VendorAddress = table.Column<string>(nullable: true),
                    VendorApartment = table.Column<int>(nullable: true),
                    VendorFloor = table.Column<int>(nullable: false),
                    VendorId = table.Column<int>(nullable: false),
                    VendorLandmark = table.Column<string>(nullable: true),
                    VendorLocation = table.Column<string>(nullable: true),
                    VendorName = table.Column<string>(nullable: false),
                    VendorPhone = table.Column<string>(maxLength: 250, nullable: false),
                    ZoneId = table.Column<int>(nullable: false)
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

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_CashTransferId",
                table: "Transaction",
                column: "CashTransferId");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_PickupRequestId",
                table: "Transaction",
                column: "PickupRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_FollowUp_FollowUpTypeId",
                table: "FollowUp",
                column: "FollowUpTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_FollowUp_GameServiceGamerServiceId",
                table: "FollowUp",
                column: "GameServiceGamerServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_CommonUser_BranchId",
                table: "CommonUser",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Advertisement_BranchId",
                table: "Advertisement",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Advertisement_CashTransferId",
                table: "Advertisement",
                column: "CashTransferId");

            migrationBuilder.CreateIndex(
                name: "IX_Advertisement_CityId",
                table: "Advertisement",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_Advertisement_PickupRequestId",
                table: "Advertisement",
                column: "PickupRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_Advertisement_ShipmentTypeId",
                table: "Advertisement",
                column: "ShipmentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Advertisement_ZoneId",
                table: "Advertisement",
                column: "ZoneId");

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
                name: "IX_ShipmentCustomerFollowUp_AdvertisementId",
                table: "ShipmentCustomerFollowUp",
                column: "AdvertisementId");

            migrationBuilder.CreateIndex(
                name: "IX_ShipmentCustomerFollowUp_CreatedBy",
                table: "ShipmentCustomerFollowUp",
                column: "CreatedBy");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Advertisement_Branch_BranchId",
                table: "Advertisement",
                column: "BranchId",
                principalTable: "Branch",
                principalColumn: "BranchId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Advertisement_CashTransfer_CashTransferId",
                table: "Advertisement",
                column: "CashTransferId",
                principalTable: "CashTransfer",
                principalColumn: "CashTransferId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Advertisement_City_CityId",
                table: "Advertisement",
                column: "CityId",
                principalTable: "City",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Advertisement_PickupRequest_PickupRequestId",
                table: "Advertisement",
                column: "PickupRequestId",
                principalTable: "PickupRequest",
                principalColumn: "PickupRequestId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Advertisement_ShipmentType_ShipmentTypeId",
                table: "Advertisement",
                column: "ShipmentTypeId",
                principalTable: "ShipmentType",
                principalColumn: "ShipmentTypeId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Advertisement_Zone_ZoneId",
                table: "Advertisement",
                column: "ZoneId",
                principalTable: "Zone",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Branch_CommonUser",
                table: "CommonUser",
                column: "BranchId",
                principalTable: "Branch",
                principalColumn: "BranchId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FollowUp_FollowUpType_FollowUpTypeId",
                table: "FollowUp",
                column: "FollowUpTypeId",
                principalTable: "FollowUpType",
                principalColumn: "FollowUpTypeID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FollowUp_GamerService_GameServiceGamerServiceId",
                table: "FollowUp",
                column: "GameServiceGamerServiceId",
                principalTable: "GamerService",
                principalColumn: "GamerServiceId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_CashTransfer_CashTransferId",
                table: "Transaction",
                column: "CashTransferId",
                principalTable: "CashTransfer",
                principalColumn: "CashTransferId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_PickupRequest_PickupRequestId",
                table: "Transaction",
                column: "PickupRequestId",
                principalTable: "PickupRequest",
                principalColumn: "PickupRequestId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

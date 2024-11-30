using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DicomApp.DAL.Migrations
{
    public partial class transaction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountTransaction");

            migrationBuilder.CreateTable(
                name: "Transaction",
                columns: table => new
                {
                    TransactionId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RefId = table.Column<string>(maxLength: 100, nullable: true),
                    TypeId = table.Column<byte>(nullable: false),
                    AdvertisementId = table.Column<int>(nullable: true),
                    BuyerId = table.Column<int>(nullable: false),
                    Amount = table.Column<int>(nullable: false),
                    IsSuccess = table.Column<bool>(nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    CreatedBy = table.Column<int>(nullable: false),
                    LastModifiedBy = table.Column<int>(nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    IsDeleted = table.Column<bool>(nullable: false),
                    AccountId = table.Column<int>(nullable: true),
                    AccountId1 = table.Column<int>(nullable: true),
                    AccountId2 = table.Column<int>(nullable: true),
                    CashTransferId = table.Column<int>(nullable: true),
                    PickupRequestId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transaction", x => x.TransactionId);
                    table.ForeignKey(
                        name: "FK_Transaction_Account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Transaction_Account_AccountId1",
                        column: x => x.AccountId1,
                        principalTable: "Account",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Transaction_Account_AccountId2",
                        column: x => x.AccountId2,
                        principalTable: "Account",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Transaction_Advertisement_AdvertisementId",
                        column: x => x.AdvertisementId,
                        principalTable: "Advertisement",
                        principalColumn: "AdvertisementId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Transaction_CommonUser_BuyerId",
                        column: x => x.BuyerId,
                        principalTable: "CommonUser",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Transaction_CashTransfer_CashTransferId",
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
                        name: "FK_Transaction_PickupRequest_PickupRequestId",
                        column: x => x.PickupRequestId,
                        principalTable: "PickupRequest",
                        principalColumn: "PickupRequestId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_AccountId",
                table: "Transaction",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_AccountId1",
                table: "Transaction",
                column: "AccountId1");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_AccountId2",
                table: "Transaction",
                column: "AccountId2");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_AdvertisementId",
                table: "Transaction",
                column: "AdvertisementId");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_BuyerId",
                table: "Transaction",
                column: "BuyerId");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_CashTransferId",
                table: "Transaction",
                column: "CashTransferId");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_CreatedBy",
                table: "Transaction",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_LastModifiedBy",
                table: "Transaction",
                column: "LastModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_PickupRequestId",
                table: "Transaction",
                column: "PickupRequestId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Transaction");

            migrationBuilder.CreateTable(
                name: "AccountTransaction",
                columns: table => new
                {
                    AccountTransactionId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AdvertisementId = table.Column<int>(nullable: true),
                    CancelFees = table.Column<double>(nullable: false),
                    CashTransferId = table.Column<int>(nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    CreatedBy = table.Column<int>(nullable: false),
                    GameFees = table.Column<double>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    LastModifiedBy = table.Column<int>(nullable: false),
                    PartialDeliveryFees = table.Column<double>(nullable: false),
                    PickupFees = table.Column<double>(nullable: false),
                    PickupRequestId = table.Column<int>(nullable: true),
                    ReceiverId = table.Column<int>(nullable: false),
                    RefId = table.Column<string>(maxLength: 100, nullable: true),
                    RefundCash = table.Column<double>(nullable: true),
                    RefundFees = table.Column<double>(nullable: true),
                    SenderId = table.Column<int>(nullable: true),
                    ShippingFees = table.Column<double>(nullable: false),
                    ShippingFeesPaid = table.Column<double>(nullable: true),
                    SizeFees = table.Column<double>(nullable: false),
                    StatusId = table.Column<int>(nullable: true),
                    Total = table.Column<double>(nullable: false),
                    TypeId = table.Column<byte>(nullable: false),
                    VendorCash = table.Column<double>(nullable: false),
                    VendorId = table.Column<int>(nullable: true),
                    WeightFees = table.Column<double>(nullable: false)
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
        }
    }
}

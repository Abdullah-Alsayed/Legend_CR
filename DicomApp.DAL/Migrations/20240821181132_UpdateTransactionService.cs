using Microsoft.EntityFrameworkCore.Migrations;

namespace DicomApp.DAL.Migrations
{
    public partial class UpdateTransactionService : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TypeId",
                table: "Transaction");

            migrationBuilder.AddColumn<int>(
                name: "ServiceId",
                table: "Transaction",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TransactionTypeEnum",
                table: "Transaction",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_ServiceId",
                table: "Transaction",
                column: "ServiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_GamerService_ServiceId",
                table: "Transaction",
                column: "ServiceId",
                principalTable: "GamerService",
                principalColumn: "GamerServiceId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_GamerService_ServiceId",
                table: "Transaction");

            migrationBuilder.DropIndex(
                name: "IX_Transaction_ServiceId",
                table: "Transaction");

            migrationBuilder.DropColumn(
                name: "ServiceId",
                table: "Transaction");

            migrationBuilder.DropColumn(
                name: "TransactionTypeEnum",
                table: "Transaction");

            migrationBuilder.AddColumn<byte>(
                name: "TypeId",
                table: "Transaction",
                nullable: false,
                defaultValue: (byte)0);
        }
    }
}

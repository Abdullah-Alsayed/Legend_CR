using Microsoft.EntityFrameworkCore.Migrations;

namespace DicomApp.DAL.Migrations
{
    public partial class Advertisement4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Advertisement_CashTransfer_CashTransferId",
                table: "Advertisement");

            migrationBuilder.DropForeignKey(
                name: "FK_Advertisement_CommonUser_CommonUserId1",
                table: "Advertisement");

            migrationBuilder.DropForeignKey(
                name: "FK_Advertisement_CommonUser_CommonUserId2",
                table: "Advertisement");

            migrationBuilder.DropForeignKey(
                name: "FK_Advertisement_CommonUser_CommonUserId3",
                table: "Advertisement");

            migrationBuilder.DropForeignKey(
                name: "FK_Shipment_CommonUser1",
                table: "Advertisement");

            migrationBuilder.DropIndex(
                name: "IX_Advertisement_CommonUserId1",
                table: "Advertisement");

            migrationBuilder.DropIndex(
                name: "IX_Advertisement_CommonUserId2",
                table: "Advertisement");

            migrationBuilder.DropIndex(
                name: "IX_Advertisement_CommonUserId3",
                table: "Advertisement");

            migrationBuilder.DropColumn(
                name: "CommonUserId1",
                table: "Advertisement");

            migrationBuilder.DropColumn(
                name: "CommonUserId2",
                table: "Advertisement");

            migrationBuilder.DropColumn(
                name: "CommonUserId3",
                table: "Advertisement");

            migrationBuilder.AlterColumn<int>(
                name: "CashTransferId",
                table: "Advertisement",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_Advertisement_CashTransfer_CashTransferId",
                table: "Advertisement",
                column: "CashTransferId",
                principalTable: "CashTransfer",
                principalColumn: "CashTransferId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Advertisement_CommonUser_GamerId",
                table: "Advertisement",
                column: "GamerId",
                principalTable: "CommonUser",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Advertisement_CashTransfer_CashTransferId",
                table: "Advertisement");

            migrationBuilder.DropForeignKey(
                name: "FK_Advertisement_CommonUser_GamerId",
                table: "Advertisement");

            migrationBuilder.AlterColumn<int>(
                name: "CashTransferId",
                table: "Advertisement",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CommonUserId1",
                table: "Advertisement",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CommonUserId2",
                table: "Advertisement",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CommonUserId3",
                table: "Advertisement",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Advertisement_CommonUserId1",
                table: "Advertisement",
                column: "CommonUserId1");

            migrationBuilder.CreateIndex(
                name: "IX_Advertisement_CommonUserId2",
                table: "Advertisement",
                column: "CommonUserId2");

            migrationBuilder.CreateIndex(
                name: "IX_Advertisement_CommonUserId3",
                table: "Advertisement",
                column: "CommonUserId3");

            migrationBuilder.AddForeignKey(
                name: "FK_Advertisement_CashTransfer_CashTransferId",
                table: "Advertisement",
                column: "CashTransferId",
                principalTable: "CashTransfer",
                principalColumn: "CashTransferId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Advertisement_CommonUser_CommonUserId1",
                table: "Advertisement",
                column: "CommonUserId1",
                principalTable: "CommonUser",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Advertisement_CommonUser_CommonUserId2",
                table: "Advertisement",
                column: "CommonUserId2",
                principalTable: "CommonUser",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Advertisement_CommonUser_CommonUserId3",
                table: "Advertisement",
                column: "CommonUserId3",
                principalTable: "CommonUser",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Shipment_CommonUser1",
                table: "Advertisement",
                column: "GamerId",
                principalTable: "CommonUser",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

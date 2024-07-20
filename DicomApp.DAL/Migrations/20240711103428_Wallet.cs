using Microsoft.EntityFrameworkCore.Migrations;

namespace DicomApp.DAL.Migrations
{
    public partial class Wallet : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "VodafoneCashNumber",
                table: "CommonUser",
                newName: "WalletNumber");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "WalletNumber",
                table: "CommonUser",
                newName: "VodafoneCashNumber");
        }
    }
}

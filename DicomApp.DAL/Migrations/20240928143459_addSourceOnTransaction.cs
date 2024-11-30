using Microsoft.EntityFrameworkCore.Migrations;

namespace DicomApp.DAL.Migrations
{
    public partial class addSourceOnTransaction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TransactionTypeEnum",
                table: "Transaction");

            migrationBuilder.AddColumn<string>(
                name: "Attachment",
                table: "Transaction",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TransactionSource",
                table: "Transaction",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TransactionType",
                table: "Transaction",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Attachment",
                table: "Transaction");

            migrationBuilder.DropColumn(
                name: "TransactionSource",
                table: "Transaction");

            migrationBuilder.DropColumn(
                name: "TransactionType",
                table: "Transaction");

            migrationBuilder.AddColumn<int>(
                name: "TransactionTypeEnum",
                table: "Transaction",
                nullable: false,
                defaultValue: 0);
        }
    }
}

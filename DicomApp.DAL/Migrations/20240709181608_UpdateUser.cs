using Microsoft.EntityFrameworkCore.Migrations;

namespace DicomApp.DAL.Migrations
{
    public partial class UpdateUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Age",
                table: "CommonUser",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CountryId",
                table: "CommonUser",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Gender",
                table: "CommonUser",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "TelegramUserName",
                table: "CommonUser",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Age",
                table: "CommonUser");

            migrationBuilder.DropColumn(
                name: "CountryId",
                table: "CommonUser");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "CommonUser");

            migrationBuilder.DropColumn(
                name: "TelegramUserName",
                table: "CommonUser");
        }
    }
}

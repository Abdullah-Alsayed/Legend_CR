using Microsoft.EntityFrameworkCore.Migrations;

namespace DicomApp.DAL.Migrations
{
    public partial class UpdateServices : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Rank",
                table: "GamerService",
                newName: "CurrentLevel");

            migrationBuilder.AlterColumn<string>(
                name: "Level",
                table: "GamerService",
                nullable: true,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CurrentLevel",
                table: "GamerService",
                newName: "Rank");

            migrationBuilder.AlterColumn<int>(
                name: "Level",
                table: "GamerService",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}

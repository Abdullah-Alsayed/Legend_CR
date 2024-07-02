using Microsoft.EntityFrameworkCore.Migrations;

namespace DicomApp.DAL.Migrations
{
    public partial class addRank : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Level",
                table: "Advertisement",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true
            );

            migrationBuilder.AddColumn<string>(
                name: "Rank",
                table: "Advertisement",
                nullable: true
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(name: "Rank", table: "Advertisement");

            migrationBuilder.AlterColumn<string>(
                name: "Level",
                table: "Advertisement",
                nullable: true,
                oldClrType: typeof(int)
            );
        }
    }
}

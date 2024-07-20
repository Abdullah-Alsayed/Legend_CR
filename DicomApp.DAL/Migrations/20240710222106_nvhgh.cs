using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DicomApp.DAL.Migrations
{
    public partial class nvhgh : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ConfirmEmail",
                table: "CommonUser",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    CountryId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    NameEn = table.Column<string>(nullable: true),
                    NameAr = table.Column<string>(nullable: true),
                    FlagUrl = table.Column<string>(nullable: true),
                    CountryCode = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.CountryId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Countries");

            migrationBuilder.DropColumn(
                name: "ConfirmEmail",
                table: "CommonUser");
        }
    }
}

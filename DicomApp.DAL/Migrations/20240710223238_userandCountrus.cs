using Microsoft.EntityFrameworkCore.Migrations;

namespace DicomApp.DAL.Migrations
{
    public partial class userandCountrus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_CommonUser_CountryId",
                table: "CommonUser",
                column: "CountryId");

            migrationBuilder.AddForeignKey(
                name: "FK_CommonUser_Countries_CountryId",
                table: "CommonUser",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "CountryId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommonUser_Countries_CountryId",
                table: "CommonUser");

            migrationBuilder.DropIndex(
                name: "IX_CommonUser_CountryId",
                table: "CommonUser");
        }
    }
}

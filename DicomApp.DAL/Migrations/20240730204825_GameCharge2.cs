using Microsoft.EntityFrameworkCore.Migrations;

namespace DicomApp.DAL.Migrations
{
    public partial class GameCharge2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameCharge_Game_GameId",
                table: "GameCharge");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GameCharge",
                table: "GameCharge");

            migrationBuilder.RenameTable(
                name: "GameCharge",
                newName: "GameCharges");

            migrationBuilder.RenameIndex(
                name: "IX_GameCharge_GameId",
                table: "GameCharges",
                newName: "IX_GameCharges_GameId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GameCharges",
                table: "GameCharges",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GameCharges_Game_GameId",
                table: "GameCharges",
                column: "GameId",
                principalTable: "Game",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameCharges_Game_GameId",
                table: "GameCharges");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GameCharges",
                table: "GameCharges");

            migrationBuilder.RenameTable(
                name: "GameCharges",
                newName: "GameCharge");

            migrationBuilder.RenameIndex(
                name: "IX_GameCharges_GameId",
                table: "GameCharge",
                newName: "IX_GameCharge_GameId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GameCharge",
                table: "GameCharge",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GameCharge_Game_GameId",
                table: "GameCharge",
                column: "GameId",
                principalTable: "Game",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

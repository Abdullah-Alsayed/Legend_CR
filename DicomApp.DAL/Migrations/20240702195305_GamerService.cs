using Microsoft.EntityFrameworkCore.Migrations;

namespace DicomApp.DAL.Migrations
{
    public partial class GamerService : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FollowUp_GameService_GameServiceGamerServiceId",
                table: "FollowUp");

            migrationBuilder.DropForeignKey(
                name: "FK_GameService_Game_GameId",
                table: "GameService");

            migrationBuilder.DropForeignKey(
                name: "FK_GameService_CommonUser_GamerId",
                table: "GameService");

            migrationBuilder.DropForeignKey(
                name: "FK_GameService_Status_StatusId",
                table: "GameService");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GameService",
                table: "GameService");

            migrationBuilder.RenameTable(
                name: "GameService",
                newName: "GamerService");

            migrationBuilder.RenameIndex(
                name: "IX_GameService_StatusId",
                table: "GamerService",
                newName: "IX_GamerService_StatusId");

            migrationBuilder.RenameIndex(
                name: "IX_GameService_GamerId",
                table: "GamerService",
                newName: "IX_GamerService_GamerId");

            migrationBuilder.RenameIndex(
                name: "IX_GameService_GameId",
                table: "GamerService",
                newName: "IX_GamerService_GameId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GamerService",
                table: "GamerService",
                column: "GamerServiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_FollowUp_GamerService_GameServiceGamerServiceId",
                table: "FollowUp",
                column: "GameServiceGamerServiceId",
                principalTable: "GamerService",
                principalColumn: "GamerServiceId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GamerService_Game_GameId",
                table: "GamerService",
                column: "GameId",
                principalTable: "Game",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GamerService_CommonUser_GamerId",
                table: "GamerService",
                column: "GamerId",
                principalTable: "CommonUser",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GamerService_Status_StatusId",
                table: "GamerService",
                column: "StatusId",
                principalTable: "Status",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FollowUp_GamerService_GameServiceGamerServiceId",
                table: "FollowUp");

            migrationBuilder.DropForeignKey(
                name: "FK_GamerService_Game_GameId",
                table: "GamerService");

            migrationBuilder.DropForeignKey(
                name: "FK_GamerService_CommonUser_GamerId",
                table: "GamerService");

            migrationBuilder.DropForeignKey(
                name: "FK_GamerService_Status_StatusId",
                table: "GamerService");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GamerService",
                table: "GamerService");

            migrationBuilder.RenameTable(
                name: "GamerService",
                newName: "GameService");

            migrationBuilder.RenameIndex(
                name: "IX_GamerService_StatusId",
                table: "GameService",
                newName: "IX_GameService_StatusId");

            migrationBuilder.RenameIndex(
                name: "IX_GamerService_GamerId",
                table: "GameService",
                newName: "IX_GameService_GamerId");

            migrationBuilder.RenameIndex(
                name: "IX_GamerService_GameId",
                table: "GameService",
                newName: "IX_GameService_GameId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GameService",
                table: "GameService",
                column: "GamerServiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_FollowUp_GameService_GameServiceGamerServiceId",
                table: "FollowUp",
                column: "GameServiceGamerServiceId",
                principalTable: "GameService",
                principalColumn: "GamerServiceId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GameService_Game_GameId",
                table: "GameService",
                column: "GameId",
                principalTable: "Game",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GameService_CommonUser_GamerId",
                table: "GameService",
                column: "GamerId",
                principalTable: "CommonUser",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GameService_Status_StatusId",
                table: "GameService",
                column: "StatusId",
                principalTable: "Status",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

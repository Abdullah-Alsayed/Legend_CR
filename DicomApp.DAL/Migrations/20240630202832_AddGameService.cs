using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DicomApp.DAL.Migrations
{
    public partial class AddGameService : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GameServiceGamerServiceId",
                table: "FollowUp",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "GameService",
                columns: table => new
                {
                    GamerServiceId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RefId = table.Column<string>(nullable: true),
                    GameId = table.Column<int>(nullable: false),
                    StatusId = table.Column<int>(nullable: false),
                    GamerId = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    UserName = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    Level = table.Column<int>(nullable: false),
                    Rank = table.Column<string>(nullable: true),
                    Price = table.Column<int>(nullable: false),
                    GameServiceType = table.Column<int>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<int>(nullable: false),
                    LastModifiedAt = table.Column<DateTime>(nullable: true),
                    LastModifiedBy = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameService", x => x.GamerServiceId);
                    table.ForeignKey(
                        name: "FK_GameService_Game_GameId",
                        column: x => x.GameId,
                        principalTable: "Game",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GameService_CommonUser_GamerId",
                        column: x => x.GamerId,
                        principalTable: "CommonUser",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GameService_Status_StatusId",
                        column: x => x.StatusId,
                        principalTable: "Status",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FollowUp_GameServiceGamerServiceId",
                table: "FollowUp",
                column: "GameServiceGamerServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_GameService_GameId",
                table: "GameService",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_GameService_GamerId",
                table: "GameService",
                column: "GamerId");

            migrationBuilder.CreateIndex(
                name: "IX_GameService_StatusId",
                table: "GameService",
                column: "StatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_FollowUp_GameService_GameServiceGamerServiceId",
                table: "FollowUp",
                column: "GameServiceGamerServiceId",
                principalTable: "GameService",
                principalColumn: "GamerServiceId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FollowUp_GameService_GameServiceGamerServiceId",
                table: "FollowUp");

            migrationBuilder.DropTable(
                name: "GameService");

            migrationBuilder.DropIndex(
                name: "IX_FollowUp_GameServiceGamerServiceId",
                table: "FollowUp");

            migrationBuilder.DropColumn(
                name: "GameServiceGamerServiceId",
                table: "FollowUp");
        }
    }
}

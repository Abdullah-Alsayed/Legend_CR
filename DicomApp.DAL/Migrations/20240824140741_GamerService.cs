using Microsoft.EntityFrameworkCore.Migrations;

namespace DicomApp.DAL.Migrations
{
    public partial class UpadteGamerServiceMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GameChargeId",
                table: "GamerService",
                nullable: true
            );

            migrationBuilder.CreateIndex(
                name: "IX_GamerService_GameChargeId",
                table: "GamerService",
                column: "GameChargeId"
            );

            migrationBuilder.AddForeignKey(
                name: "FK_GamerService_GameCharges_GameChargeId",
                table: "GamerService",
                column: "GameChargeId",
                principalTable: "GameCharges",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GamerService_GameCharges_GameChargeId",
                table: "GamerService"
            );

            migrationBuilder.DropIndex(name: "IX_GamerService_GameChargeId", table: "GamerService");

            migrationBuilder.DropColumn(name: "GameChargeId", table: "GamerService");
        }
    }
}

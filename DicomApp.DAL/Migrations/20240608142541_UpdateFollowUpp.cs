using Microsoft.EntityFrameworkCore.Migrations;

namespace DicomApp.DAL.Migrations
{
    public partial class UpdateFollowUpp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FollowUp_FollowUpType",
                table: "FollowUp");

            migrationBuilder.RenameColumn(
                name: "FollowUpTypeID",
                table: "FollowUp",
                newName: "FollowUpTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_FollowUp_FollowUpTypeID",
                table: "FollowUp",
                newName: "IX_FollowUp_FollowUpTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_FollowUp_FollowUpType_FollowUpTypeId",
                table: "FollowUp",
                column: "FollowUpTypeId",
                principalTable: "FollowUpType",
                principalColumn: "FollowUpTypeID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FollowUp_FollowUpType_FollowUpTypeId",
                table: "FollowUp");

            migrationBuilder.RenameColumn(
                name: "FollowUpTypeId",
                table: "FollowUp",
                newName: "FollowUpTypeID");

            migrationBuilder.RenameIndex(
                name: "IX_FollowUp_FollowUpTypeId",
                table: "FollowUp",
                newName: "IX_FollowUp_FollowUpTypeID");

            migrationBuilder.AddForeignKey(
                name: "FK_FollowUp_FollowUpType",
                table: "FollowUp",
                column: "FollowUpTypeID",
                principalTable: "FollowUpType",
                principalColumn: "FollowUpTypeID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DicomApp.DAL.Migrations
{
    public partial class AddDelaeta : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsInternal",
                table: "Role");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeleteAt",
                table: "Role",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "DeleteBy",
                table: "Role",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DeleteBy",
                table: "Game",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "Game",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Game",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "DeleteBy",
                table: "Category",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "Category",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Category",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeleteAt",
                table: "Role");

            migrationBuilder.DropColumn(
                name: "DeleteBy",
                table: "Role");

            migrationBuilder.DropColumn(
                name: "DeleteBy",
                table: "Game");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "Game");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Game");

            migrationBuilder.DropColumn(
                name: "DeleteBy",
                table: "Category");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "Category");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Category");

            migrationBuilder.AddColumn<bool>(
                name: "IsInternal",
                table: "Role",
                nullable: false,
                defaultValue: false);
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DicomApp.DAL.Migrations
{
    public partial class AddInvoices : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Invoices",
                columns: table => new
                {
                    InvoiceId = table
                        .Column<int>(nullable: false)
                        .Annotation(
                            "SqlServer:ValueGenerationStrategy",
                            SqlServerValueGenerationStrategy.IdentityColumn
                        ),
                    RefId = table.Column<string>(nullable: true),
                    AdvertisementId = table.Column<int>(nullable: true),
                    GamerServiceId = table.Column<int>(nullable: true),
                    Price = table.Column<int>(nullable: false),
                    IsRefund = table.Column<bool>(nullable: false),
                    InvoiceType = table.Column<int>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<int>(nullable: false),
                    LastModifiedAt = table.Column<DateTime>(nullable: true),
                    LastModifiedBy = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invoices", x => x.InvoiceId);
                    table.ForeignKey(
                        name: "FK_Invoices_Advertisement_AdvertisementId",
                        column: x => x.AdvertisementId,
                        principalTable: "Advertisement",
                        principalColumn: "AdvertisementId",
                        onDelete: ReferentialAction.Restrict
                    );
                    table.ForeignKey(
                        name: "FK_Invoices_GamerService_GamerServiceId",
                        column: x => x.GamerServiceId,
                        principalTable: "GamerService",
                        principalColumn: "GamerServiceId",
                        onDelete: ReferentialAction.Restrict
                    );
                }
            );

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_AdvertisementId",
                table: "Invoices",
                column: "AdvertisementId",
                unique: true,
                filter: "[AdvertisementId] IS NOT NULL"
            );

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_GamerServiceId",
                table: "Invoices",
                column: "GamerServiceId",
                unique: true,
                filter: "[GamerServiceId] IS NOT NULL"
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "Invoices");
        }
    }
}

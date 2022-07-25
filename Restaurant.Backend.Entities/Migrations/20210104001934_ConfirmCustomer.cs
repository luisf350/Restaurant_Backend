using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Restaurant.Backend.Entities.Migrations
{
    public partial class ConfirmCustomer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ConfirmCustomers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UniqueEmailKey = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ExpirationEmail = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UniquePhoneKey = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    CreationDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ModificationDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfirmCustomers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConfirmCustomers_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConfirmCustomers_CustomerId",
                table: "ConfirmCustomers",
                column: "CustomerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConfirmCustomers");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Restaurant.Backend.Entities.Migrations
{
    public partial class Country : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    CallingCode = table.Column<int>(type: "int", nullable: false),
                    Capital = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Region = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    SubRegion = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Flag = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    CreationDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ModificationDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Countries");
        }
    }
}

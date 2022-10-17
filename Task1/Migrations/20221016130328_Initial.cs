using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Task1.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Task1",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RandomDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RandomLatinString = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RandomCyrillicString = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RandomInteger = table.Column<int>(type: "int", nullable: false),
                    RandomFloat = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Task1", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Task1");
        }
    }
}

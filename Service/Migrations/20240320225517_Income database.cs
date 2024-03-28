using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Service.Migrations
{
    public partial class Incomedatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "IncomesDb",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Income = table.Column<double>(type: "float", nullable: true),
                    Currency = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SourceOfIncome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IncomeTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    User = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IncomesDb", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IncomesDb");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RoutesStation.Migrations
{
    public partial class R23 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Company",
                table: "Buses");

            migrationBuilder.DropColumn(
                name: "Company",
                table: "BusDriverMaps");

            migrationBuilder.AddColumn<Guid>(
                name: "CompanyID",
                table: "Buses",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Company = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Buses_CompanyID",
                table: "Buses",
                column: "CompanyID");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_Company",
                table: "Companies",
                column: "Company",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Buses_Companies_CompanyID",
                table: "Buses",
                column: "CompanyID",
                principalTable: "Companies",
                principalColumn: "id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Buses_Companies_CompanyID",
                table: "Buses");

            migrationBuilder.DropTable(
                name: "Companies");

            migrationBuilder.DropIndex(
                name: "IX_Buses_CompanyID",
                table: "Buses");

            migrationBuilder.DropColumn(
                name: "CompanyID",
                table: "Buses");

            migrationBuilder.AddColumn<string>(
                name: "Company",
                table: "Buses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Company",
                table: "BusDriverMaps",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}

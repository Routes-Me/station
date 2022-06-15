using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RoutesStation.Migrations
{
    public partial class RS33 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RouteRequests",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name_EN = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name_AR = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Area_EN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Area_AR = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Price = table.Column<double>(type: "float", nullable: false),
                    AdminID = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RouteRequests", x => x.id);
                    table.ForeignKey(
                        name: "FK_RouteRequests_AspNetUsers_AdminID",
                        column: x => x.AdminID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RouteRequests_Companies_CompanyID",
                        column: x => x.CompanyID,
                        principalTable: "Companies",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_RouteRequests_AdminID",
                table: "RouteRequests",
                column: "AdminID");

            migrationBuilder.CreateIndex(
                name: "IX_RouteRequests_CompanyID",
                table: "RouteRequests",
                column: "CompanyID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RouteRequests");
        }
    }
}

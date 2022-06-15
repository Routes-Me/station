using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RoutesStation.Migrations
{
    public partial class RS26 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PromoterInstallationMaps",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PromoterID = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    UserID = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Installation_Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PromoterInstallationMaps", x => x.id);
                    table.ForeignKey(
                        name: "FK_PromoterInstallationMaps_AspNetUsers_PromoterID",
                        column: x => x.PromoterID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PromoterInstallationMaps_AspNetUsers_UserID",
                        column: x => x.UserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_PromoterInstallationMaps_PromoterID",
                table: "PromoterInstallationMaps",
                column: "PromoterID");

            migrationBuilder.CreateIndex(
                name: "IX_PromoterInstallationMaps_UserID",
                table: "PromoterInstallationMaps",
                column: "UserID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PromoterInstallationMaps");
        }
    }
}

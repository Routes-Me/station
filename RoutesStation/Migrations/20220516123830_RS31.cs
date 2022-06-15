using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RoutesStation.Migrations
{
    public partial class RS31 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PromoterInstallationMaps_UserID",
                table: "PromoterInstallationMaps");

            migrationBuilder.CreateIndex(
                name: "IX_PromoterInstallationMaps_UserID",
                table: "PromoterInstallationMaps",
                column: "UserID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PromoterInstallationMaps_UserID",
                table: "PromoterInstallationMaps");

            migrationBuilder.CreateIndex(
                name: "IX_PromoterInstallationMaps_UserID",
                table: "PromoterInstallationMaps",
                column: "UserID",
                unique: true,
                filter: "[UserID] IS NOT NULL");
        }
    }
}

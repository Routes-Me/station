using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RoutesStation.Migrations
{
    public partial class RS29 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PromoterInstallationMaps_PromoterID",
                table: "PromoterInstallationMaps");

            migrationBuilder.DropIndex(
                name: "IX_PromoterInstallationMaps_UserID",
                table: "PromoterInstallationMaps");

            migrationBuilder.CreateIndex(
                name: "IX_PromoterInstallationMaps_PromoterID",
                table: "PromoterInstallationMaps",
                column: "PromoterID");

            migrationBuilder.CreateIndex(
                name: "IX_PromoterInstallationMaps_UserID",
                table: "PromoterInstallationMaps",
                column: "UserID",
                unique: true,
                filter: "[UserID] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PromoterInstallationMaps_PromoterID",
                table: "PromoterInstallationMaps");

            migrationBuilder.DropIndex(
                name: "IX_PromoterInstallationMaps_UserID",
                table: "PromoterInstallationMaps");

            migrationBuilder.CreateIndex(
                name: "IX_PromoterInstallationMaps_PromoterID",
                table: "PromoterInstallationMaps",
                column: "PromoterID",
                unique: true,
                filter: "[PromoterID] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_PromoterInstallationMaps_UserID",
                table: "PromoterInstallationMaps",
                column: "UserID");
        }
    }
}

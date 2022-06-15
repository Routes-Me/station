using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RoutesStation.Migrations
{
    public partial class RS27 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PromoterInstallationMaps_PromoterID",
                table: "PromoterInstallationMaps");

            migrationBuilder.CreateIndex(
                name: "IX_PromoterInstallationMaps_PromoterID",
                table: "PromoterInstallationMaps",
                column: "PromoterID",
                unique: true,
                filter: "[PromoterID] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PromoterInstallationMaps_PromoterID",
                table: "PromoterInstallationMaps");

            migrationBuilder.CreateIndex(
                name: "IX_PromoterInstallationMaps_PromoterID",
                table: "PromoterInstallationMaps",
                column: "PromoterID");
        }
    }
}

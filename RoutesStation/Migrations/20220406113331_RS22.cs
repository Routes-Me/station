using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RoutesStation.Migrations
{
    public partial class RS22 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "InspectorID",
                table: "WalletTrips",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WalletTrips_InspectorID",
                table: "WalletTrips",
                column: "InspectorID");

            migrationBuilder.AddForeignKey(
                name: "FK_WalletTrips_AspNetUsers_InspectorID",
                table: "WalletTrips",
                column: "InspectorID",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WalletTrips_AspNetUsers_InspectorID",
                table: "WalletTrips");

            migrationBuilder.DropIndex(
                name: "IX_WalletTrips_InspectorID",
                table: "WalletTrips");

            migrationBuilder.DropColumn(
                name: "InspectorID",
                table: "WalletTrips");
        }
    }
}

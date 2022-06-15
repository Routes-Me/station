using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RoutesStation.Migrations
{
    public partial class RS19 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DriverID",
                table: "Buses",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Buses_DriverID",
                table: "Buses",
                column: "DriverID");

            migrationBuilder.AddForeignKey(
                name: "FK_Buses_AspNetUsers_DriverID",
                table: "Buses",
                column: "DriverID",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Buses_AspNetUsers_DriverID",
                table: "Buses");

            migrationBuilder.DropIndex(
                name: "IX_Buses_DriverID",
                table: "Buses");

            migrationBuilder.DropColumn(
                name: "DriverID",
                table: "Buses");
        }
    }
}

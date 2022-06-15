using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RoutesStation.Migrations
{
    public partial class RS18 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "BusID",
                table: "WalletTrips",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DriverID",
                table: "WalletTrips",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Buses",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Company = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RouteID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PalteNumber = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Kind = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Buses", x => x.id);
                    table.ForeignKey(
                        name: "FK_Buses_Routes_RouteID",
                        column: x => x.RouteID,
                        principalTable: "Routes",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "BusDriverMaps",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Company = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BusID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DriverID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Start_Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    End_Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusDriverMaps", x => x.id);
                    table.ForeignKey(
                        name: "FK_BusDriverMaps_AspNetUsers_DriverID",
                        column: x => x.DriverID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BusDriverMaps_Buses_BusID",
                        column: x => x.BusID,
                        principalTable: "Buses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WalletTrips_BusID",
                table: "WalletTrips",
                column: "BusID");

            migrationBuilder.CreateIndex(
                name: "IX_WalletTrips_DriverID",
                table: "WalletTrips",
                column: "DriverID");

            migrationBuilder.CreateIndex(
                name: "IX_BusDriverMaps_BusID",
                table: "BusDriverMaps",
                column: "BusID");

            migrationBuilder.CreateIndex(
                name: "IX_BusDriverMaps_DriverID",
                table: "BusDriverMaps",
                column: "DriverID");

            migrationBuilder.CreateIndex(
                name: "IX_Buses_PalteNumber",
                table: "Buses",
                column: "PalteNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Buses_RouteID",
                table: "Buses",
                column: "RouteID");

            migrationBuilder.AddForeignKey(
                name: "FK_WalletTrips_AspNetUsers_DriverID",
                table: "WalletTrips",
                column: "DriverID",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WalletTrips_Buses_BusID",
                table: "WalletTrips",
                column: "BusID",
                principalTable: "Buses",
                principalColumn: "id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WalletTrips_AspNetUsers_DriverID",
                table: "WalletTrips");

            migrationBuilder.DropForeignKey(
                name: "FK_WalletTrips_Buses_BusID",
                table: "WalletTrips");

            migrationBuilder.DropTable(
                name: "BusDriverMaps");

            migrationBuilder.DropTable(
                name: "Buses");

            migrationBuilder.DropIndex(
                name: "IX_WalletTrips_BusID",
                table: "WalletTrips");

            migrationBuilder.DropIndex(
                name: "IX_WalletTrips_DriverID",
                table: "WalletTrips");

            migrationBuilder.DropColumn(
                name: "BusID",
                table: "WalletTrips");

            migrationBuilder.DropColumn(
                name: "DriverID",
                table: "WalletTrips");
        }
    }
}

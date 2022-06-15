using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RoutesStation.Migrations
{
    public partial class RS3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Stations_Routes_RouteID",
                table: "Stations");

            migrationBuilder.DropIndex(
                name: "IX_Stations_RouteID",
                table: "Stations");

            migrationBuilder.DropColumn(
                name: "Direction",
                table: "Stations");

            migrationBuilder.DropColumn(
                name: "RouteID",
                table: "Stations");

            migrationBuilder.CreateTable(
                name: "RouteStationMaps",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    Direction = table.Column<int>(type: "int", nullable: false),
                    RouteID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StationID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RouteStationMaps", x => x.id);
                    table.ForeignKey(
                        name: "FK_RouteStationMaps_Routes_RouteID",
                        column: x => x.RouteID,
                        principalTable: "Routes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RouteStationMaps_Stations_StationID",
                        column: x => x.StationID,
                        principalTable: "Stations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RouteStationMaps_RouteID",
                table: "RouteStationMaps",
                column: "RouteID");

            migrationBuilder.CreateIndex(
                name: "IX_RouteStationMaps_StationID",
                table: "RouteStationMaps",
                column: "StationID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RouteStationMaps");

            migrationBuilder.AddColumn<int>(
                name: "Direction",
                table: "Stations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "RouteID",
                table: "Stations",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Stations_RouteID",
                table: "Stations",
                column: "RouteID");

            migrationBuilder.AddForeignKey(
                name: "FK_Stations_Routes_RouteID",
                table: "Stations",
                column: "RouteID",
                principalTable: "Routes",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

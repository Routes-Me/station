using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RoutesStation.Migrations
{
    public partial class RS9 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Trips",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RouteID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    StartStationID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    EndStationID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UserID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Creat_Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trips", x => x.id);
                    table.ForeignKey(
                        name: "FK_Trips_AspNetUsers_UserID",
                        column: x => x.UserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Trips_Routes_RouteID",
                        column: x => x.RouteID,
                        principalTable: "Routes",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Trips_Stations_EndStationID",
                        column: x => x.EndStationID,
                        principalTable: "Stations",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Trips_Stations_StartStationID",
                        column: x => x.StartStationID,
                        principalTable: "Stations",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Trips_EndStationID",
                table: "Trips",
                column: "EndStationID");

            migrationBuilder.CreateIndex(
                name: "IX_Trips_RouteID",
                table: "Trips",
                column: "RouteID");

            migrationBuilder.CreateIndex(
                name: "IX_Trips_StartStationID",
                table: "Trips",
                column: "StartStationID");

            migrationBuilder.CreateIndex(
                name: "IX_Trips_UserID",
                table: "Trips",
                column: "UserID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Trips");
        }
    }
}

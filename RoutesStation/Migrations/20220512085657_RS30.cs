using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RoutesStation.Migrations
{
    public partial class RS30 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InspectionBusMaps",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BusID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    InspectorID = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Creat_Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InspectionBusMaps", x => x.id);
                    table.ForeignKey(
                        name: "FK_InspectionBusMaps_AspNetUsers_InspectorID",
                        column: x => x.InspectorID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_InspectionBusMaps_Buses_BusID",
                        column: x => x.BusID,
                        principalTable: "Buses",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "InspectionUserMaps",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserID = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    InspectorID = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Creat_Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InspectionUserMaps", x => x.id);
                    table.ForeignKey(
                        name: "FK_InspectionUserMaps_AspNetUsers_InspectorID",
                        column: x => x.InspectorID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_InspectionUserMaps_AspNetUsers_UserID",
                        column: x => x.UserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_InspectionBusMaps_BusID",
                table: "InspectionBusMaps",
                column: "BusID");

            migrationBuilder.CreateIndex(
                name: "IX_InspectionBusMaps_InspectorID",
                table: "InspectionBusMaps",
                column: "InspectorID");

            migrationBuilder.CreateIndex(
                name: "IX_InspectionUserMaps_InspectorID",
                table: "InspectionUserMaps",
                column: "InspectorID");

            migrationBuilder.CreateIndex(
                name: "IX_InspectionUserMaps_UserID",
                table: "InspectionUserMaps",
                column: "UserID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InspectionBusMaps");

            migrationBuilder.DropTable(
                name: "InspectionUserMaps");
        }
    }
}

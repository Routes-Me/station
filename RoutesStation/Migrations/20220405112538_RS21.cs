using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RoutesStation.Migrations
{
    public partial class RS21 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "paymentGateway",
                table: "WalletChargings",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "invoiceId",
                table: "WalletChargings",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "InspectorID",
                table: "WalletChargings",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PromoterID",
                table: "WalletChargings",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TypeCharge",
                table: "WalletChargings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_WalletChargings_InspectorID",
                table: "WalletChargings",
                column: "InspectorID");

            migrationBuilder.CreateIndex(
                name: "IX_WalletChargings_PromoterID",
                table: "WalletChargings",
                column: "PromoterID");

            migrationBuilder.AddForeignKey(
                name: "FK_WalletChargings_AspNetUsers_InspectorID",
                table: "WalletChargings",
                column: "InspectorID",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WalletChargings_AspNetUsers_PromoterID",
                table: "WalletChargings",
                column: "PromoterID",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WalletChargings_AspNetUsers_InspectorID",
                table: "WalletChargings");

            migrationBuilder.DropForeignKey(
                name: "FK_WalletChargings_AspNetUsers_PromoterID",
                table: "WalletChargings");

            migrationBuilder.DropIndex(
                name: "IX_WalletChargings_InspectorID",
                table: "WalletChargings");

            migrationBuilder.DropIndex(
                name: "IX_WalletChargings_PromoterID",
                table: "WalletChargings");

            migrationBuilder.DropColumn(
                name: "InspectorID",
                table: "WalletChargings");

            migrationBuilder.DropColumn(
                name: "PromoterID",
                table: "WalletChargings");

            migrationBuilder.DropColumn(
                name: "TypeCharge",
                table: "WalletChargings");

            migrationBuilder.AlterColumn<string>(
                name: "paymentGateway",
                table: "WalletChargings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "invoiceId",
                table: "WalletChargings",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}

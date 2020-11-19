using Microsoft.EntityFrameworkCore.Migrations;

namespace EMenuApplication.Migrations
{
    public partial class Modify_Emenu_MenuItems_2608 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Currency",
                table: "Emenu_MenuItems");

            migrationBuilder.AddColumn<int>(
                name: "CurrencyId",
                table: "Emenu_MenuItems",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrencyId",
                table: "Emenu_MenuItems");

            migrationBuilder.AddColumn<string>(
                name: "Currency",
                table: "Emenu_MenuItems",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}

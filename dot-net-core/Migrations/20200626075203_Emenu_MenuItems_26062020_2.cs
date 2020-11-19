using Microsoft.EntityFrameworkCore.Migrations;

namespace EMenuApplication.Migrations
{
    public partial class Emenu_MenuItems_26062020_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LabelAR",
                table: "Emenu_MenuItems",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LabelEN",
                table: "Emenu_MenuItems",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LabelAR",
                table: "Emenu_MenuItems");

            migrationBuilder.DropColumn(
                name: "LabelEN",
                table: "Emenu_MenuItems");
        }
    }
}

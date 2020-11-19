using Microsoft.EntityFrameworkCore.Migrations;

namespace EMenuApplication.Migrations
{
    public partial class Emenu_MenuItems_26062020_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CommentsAR",
                table: "Emenu_MenuItems",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CommentsEN",
                table: "Emenu_MenuItems",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CommentsAR",
                table: "Emenu_MenuItems");

            migrationBuilder.DropColumn(
                name: "CommentsEN",
                table: "Emenu_MenuItems");
        }
    }
}

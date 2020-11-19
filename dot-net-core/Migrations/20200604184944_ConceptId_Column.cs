using Microsoft.EntityFrameworkCore.Migrations;

namespace EMenuApplication.Migrations
{
    public partial class ConceptId_Column : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ConceptId",
                table: "Emenu_MenuSchedules",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ConceptId",
                table: "Emenu_Menus",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ConceptId",
                table: "Emenu_MenuMItems",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ConceptId",
                table: "Emenu_MenuItemTags",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ConceptId",
                table: "Emenu_MenuItems",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ConceptId",
                table: "Emenu_ItemTags",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConceptId",
                table: "Emenu_MenuSchedules");

            migrationBuilder.DropColumn(
                name: "ConceptId",
                table: "Emenu_Menus");

            migrationBuilder.DropColumn(
                name: "ConceptId",
                table: "Emenu_MenuMItems");

            migrationBuilder.DropColumn(
                name: "ConceptId",
                table: "Emenu_MenuItemTags");

            migrationBuilder.DropColumn(
                name: "ConceptId",
                table: "Emenu_MenuItems");

            migrationBuilder.DropColumn(
                name: "ConceptId",
                table: "Emenu_ItemTags");
        }
    }
}

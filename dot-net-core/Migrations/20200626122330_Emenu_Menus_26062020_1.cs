using Microsoft.EntityFrameworkCore.Migrations;

namespace EMenuApplication.Migrations
{
    public partial class Emenu_Menus_26062020_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConceptId",
                table: "Emenu_MenuMItems");

            migrationBuilder.DropColumn(
                name: "ConceptId",
                table: "Emenu_MenuItemTags");

            migrationBuilder.AlterColumn<string>(
                name: "ConceptId",
                table: "Emenu_Menus",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ConceptId",
                table: "Emenu_Menus",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ConceptId",
                table: "Emenu_MenuMItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ConceptId",
                table: "Emenu_MenuItemTags",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}

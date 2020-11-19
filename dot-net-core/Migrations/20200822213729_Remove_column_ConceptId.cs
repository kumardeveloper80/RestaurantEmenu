using Microsoft.EntityFrameworkCore.Migrations;

namespace EMenuApplication.Migrations
{
    public partial class Remove_column_ConceptId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConceptId",
                table: "Emenu_MenuItems");

            migrationBuilder.DropColumn(
                name: "ConceptId",
                table: "Emenu_ItemTags");

            migrationBuilder.DropColumn(
                name: "ConceptId",
                table: "Emenu_Category");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ConceptId",
                table: "Emenu_MenuItems",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ConceptId",
                table: "Emenu_ItemTags",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ConceptId",
                table: "Emenu_Category",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}

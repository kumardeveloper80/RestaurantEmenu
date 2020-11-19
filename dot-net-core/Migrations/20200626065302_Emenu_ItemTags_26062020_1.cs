using Microsoft.EntityFrameworkCore.Migrations;

namespace EMenuApplication.Migrations
{
    public partial class Emenu_ItemTags_26062020_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IconTagName",
                table: "Emenu_ItemTags",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LabelAR",
                table: "Emenu_ItemTags",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LabelEN",
                table: "Emenu_ItemTags",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IconTagName",
                table: "Emenu_ItemTags");

            migrationBuilder.DropColumn(
                name: "LabelAR",
                table: "Emenu_ItemTags");

            migrationBuilder.DropColumn(
                name: "LabelEN",
                table: "Emenu_ItemTags");
        }
    }
}

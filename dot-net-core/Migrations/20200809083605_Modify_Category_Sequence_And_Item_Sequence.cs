using Microsoft.EntityFrameworkCore.Migrations;

namespace EMenuApplication.Migrations
{
    public partial class Modify_Category_Sequence_And_Item_Sequence : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MenuItemName",
                table: "Item_Sequence",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CategoryName",
                table: "Category_Sequence",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MenuItemName",
                table: "Item_Sequence");

            migrationBuilder.DropColumn(
                name: "CategoryName",
                table: "Category_Sequence");
        }
    }
}

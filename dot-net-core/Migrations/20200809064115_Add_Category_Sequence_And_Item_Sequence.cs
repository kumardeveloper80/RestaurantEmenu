using Microsoft.EntityFrameworkCore.Migrations;

namespace EMenuApplication.Migrations
{
    public partial class Add_Category_Sequence_And_Item_Sequence : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Category_Sequence",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryId = table.Column<int>(nullable: false),
                    Sequence = table.Column<int>(nullable: false),
                    MenuId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category_Sequence", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Item_Sequence",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MenuItemId = table.Column<int>(nullable: false),
                    Sequence = table.Column<int>(nullable: false),
                    MenuId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Item_Sequence", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Category_Sequence");

            migrationBuilder.DropTable(
                name: "Item_Sequence");
        }
    }
}

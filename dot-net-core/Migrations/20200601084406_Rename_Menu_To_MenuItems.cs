using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EMenuApplication.Migrations
{
    public partial class Rename_Menu_To_MenuItems : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Emenu_Menus");

            migrationBuilder.DropColumn(
                name: "EMenuId",
                table: "Emenu_MenuItemTags");

            migrationBuilder.AddColumn<int>(
                name: "EMenuItemId",
                table: "Emenu_MenuItemTags",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Emenu_MenuItems",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PLU = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    CategoryId = table.Column<int>(nullable: false),
                    Currency = table.Column<string>(nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<bool>(nullable: false),
                    DetailsEN = table.Column<string>(nullable: true),
                    DetailsAR = table.Column<string>(nullable: true),
                    ThumbnailImageName = table.Column<string>(nullable: true),
                    LargeImageName = table.Column<string>(nullable: true),
                    LargeDetailsEN = table.Column<string>(nullable: true),
                    LargeDetailsAR = table.Column<string>(nullable: true),
                    OverLayImageName = table.Column<string>(nullable: true),
                    OverlayDetailsEN = table.Column<string>(nullable: true),
                    OverlayDetailsAR = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<int>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    ModifiedBy = table.Column<int>(nullable: false),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<int>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Emenu_MenuItems", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Emenu_MenuItems");

            migrationBuilder.DropColumn(
                name: "EMenuItemId",
                table: "Emenu_MenuItemTags");

            migrationBuilder.AddColumn<int>(
                name: "EMenuId",
                table: "Emenu_MenuItemTags",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Emenu_Menus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Currency = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedBy = table.Column<int>(type: "int", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DetailsAR = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DetailsEN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    LargeDetailsAR = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LargeDetailsEN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LargeImageName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedBy = table.Column<int>(type: "int", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OverLayImageName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OverlayDetailsAR = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OverlayDetailsEN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PLU = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    ThumbnailImageName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Emenu_Menus", x => x.Id);
                });
        }
    }
}

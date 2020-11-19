using Microsoft.EntityFrameworkCore.Migrations;

namespace EMenuApplication.Migrations
{
    public partial class Modify_Sec_Stores_And_Sec_Concepts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Regionid",
                table: "Set_Stores",
                newName: "RegionId");

            migrationBuilder.RenameColumn(
                name: "Conceptid",
                table: "Set_Stores",
                newName: "ConceptId");

            migrationBuilder.AddColumn<int>(
                name: "ClientId",
                table: "Set_Stores",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ClientId",
                table: "Set_Concepts",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "Set_Stores");

            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "Set_Concepts");

            migrationBuilder.RenameColumn(
                name: "RegionId",
                table: "Set_Stores",
                newName: "Regionid");

            migrationBuilder.RenameColumn(
                name: "ConceptId",
                table: "Set_Stores",
                newName: "Conceptid");
        }
    }
}

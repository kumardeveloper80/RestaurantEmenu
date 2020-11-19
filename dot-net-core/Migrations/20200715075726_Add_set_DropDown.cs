using Microsoft.EntityFrameworkCore.Migrations;

namespace EMenuApplication.Migrations
{
    public partial class Add_set_DropDown : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "set_DropDown",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DD_ID = table.Column<string>(nullable: true),
                    DD_Value = table.Column<string>(nullable: true),
                    DD_Display = table.Column<string>(nullable: true),
                    DD_ValueOrder = table.Column<string>(nullable: true),
                    createdby = table.Column<string>(nullable: true),
                    status = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_set_DropDown", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "set_DropDown");
        }
    }
}

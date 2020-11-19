using Microsoft.EntityFrameworkCore.Migrations;

namespace EMenuApplication.Migrations
{
    public partial class Add_Set_CountryCodes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Set_CountryCodes",
                columns: table => new
                {
                    CountryID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Country = table.Column<string>(nullable: true),
                    Code = table.Column<string>(nullable: true),
                    Digits = table.Column<int>(nullable: true),
                    minDigits = table.Column<int>(nullable: true),
                    timezone = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Set_CountryCodes", x => x.CountryID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Set_CountryCodes");
        }
    }
}

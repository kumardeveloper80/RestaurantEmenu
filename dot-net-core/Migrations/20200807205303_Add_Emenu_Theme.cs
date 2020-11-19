using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EMenuApplication.Migrations
{
    public partial class Add_Emenu_Theme : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Emenu_Theme",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ConceptId = table.Column<string>(nullable: true),
                    ColorCode = table.Column<string>(nullable: true),
                    LogoName = table.Column<string>(nullable: true),
                    Status = table.Column<bool>(nullable: false),
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
                    table.PrimaryKey("PK_Emenu_Theme", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Emenu_Theme");
        }
    }
}

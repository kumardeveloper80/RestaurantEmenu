using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EMenuApplication.Migrations
{
    public partial class Add_CRM_CQuestionsGroups : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CRM_CQuestionsGroups",
                columns: table => new
                {
                    cqg_id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    cqg_name = table.Column<string>(nullable: true),
                    cqg_description = table.Column<string>(nullable: true),
                    Createdby = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    cqg_status = table.Column<short>(nullable: true),
                    active = table.Column<bool>(nullable: true),
                    ServerPerformance = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CRM_CQuestionsGroups", x => x.cqg_id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CRM_CQuestionsGroups");
        }
    }
}

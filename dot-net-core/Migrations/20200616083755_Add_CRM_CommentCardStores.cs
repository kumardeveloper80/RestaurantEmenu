using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EMenuApplication.Migrations
{
    public partial class Add_CRM_CommentCardStores : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CRM_CommentCardStores",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Store_ID = table.Column<Guid>(nullable: false),
                    cc_id = table.Column<int>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    Status = table.Column<byte>(nullable: true),
                    active = table.Column<bool>(nullable: true),
                    selected = table.Column<bool>(nullable: true),
                    StartDate = table.Column<DateTime>(nullable: true),
                    EndDate = table.Column<DateTime>(nullable: true),
                    StartTime = table.Column<TimeSpan>(nullable: true),
                    EndTime = table.Column<TimeSpan>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CRM_CommentCardStores", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CRM_CommentCardStores");
        }
    }
}

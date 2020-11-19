using Microsoft.EntityFrameworkCore.Migrations;

namespace EMenuApplication.Migrations
{
    public partial class Add_CRM_CommentCardResults : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CRM_CommentCardResults",
                columns: table => new
                {
                    ctr_id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CCR_ID = table.Column<int>(nullable: true),
                    q_id = table.Column<int>(nullable: true),
                    ctr_answer = table.Column<string>(nullable: true),
                    ctr_AnswerText = table.Column<string>(nullable: true),
                    ctr_score = table.Column<string>(nullable: true),
                    q_type = table.Column<string>(nullable: true),
                    weight = table.Column<string>(nullable: true),
                    Calculated = table.Column<string>(nullable: true),
                    LineBased = table.Column<string>(nullable: true),
                    Required = table.Column<string>(nullable: true),
                    Answered = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CRM_CommentCardResults", x => x.ctr_id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CRM_CommentCardResults");
        }
    }
}

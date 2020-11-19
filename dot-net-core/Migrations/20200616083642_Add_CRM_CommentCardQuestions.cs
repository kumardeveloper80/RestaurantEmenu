using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EMenuApplication.Migrations
{
    public partial class Add_CRM_CommentCardQuestions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CRM_CommentCardQuestions",
                columns: table => new
                {
                    ccq_id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    cc_id = table.Column<int>(nullable: false),
                    q_id = table.Column<int>(nullable: false),
                    q_ordre = table.Column<int>(nullable: true),
                    q_type = table.Column<string>(nullable: true),
                    q_typevalue = table.Column<string>(nullable: true),
                    q_answerDisplay = table.Column<string>(nullable: true),
                    q_weight = table.Column<int>(nullable: true),
                    q_calculated = table.Column<bool>(nullable: true),
                    q_minval = table.Column<int>(nullable: true),
                    q_required = table.Column<bool>(nullable: true),
                    q_linebased = table.Column<bool>(nullable: true),
                    cqg_order = table.Column<int>(nullable: true),
                    cqg_visible = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    Modifiedby = table.Column<string>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    status = table.Column<short>(nullable: true),
                    q_bootstrapSkin = table.Column<bool>(nullable: true),
                    ShowQuestionText = table.Column<bool>(nullable: true),
                    textPlaceholder = table.Column<string>(nullable: true),
                    AnswerFullSpace = table.Column<bool>(nullable: true),
                    Relatedquestion = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CRM_CommentCardQuestions", x => x.ccq_id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CRM_CommentCardQuestions");
        }
    }
}

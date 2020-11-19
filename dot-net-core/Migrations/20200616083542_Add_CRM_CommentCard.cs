using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EMenuApplication.Migrations
{
    public partial class Add_CRM_CommentCard : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CRM_CommentCard",
                columns: table => new
                {
                    cc_id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    cc_name = table.Column<string>(nullable: true),
                    cc_description = table.Column<string>(nullable: true),
                    linebased = table.Column<bool>(nullable: true),
                    AvgScore = table.Column<bool>(nullable: true),
                    cc_minvalue = table.Column<int>(nullable: true),
                    Display = table.Column<string>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    ModifiedBy = table.Column<DateTime>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    ct_status = table.Column<short>(nullable: true),
                    active = table.Column<bool>(nullable: true),
                    Notify_Bymail = table.Column<bool>(nullable: true),
                    Notify_Bysms = table.Column<bool>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    ColorCoded = table.Column<bool>(nullable: true),
                    CCheaderIMG = table.Column<string>(nullable: true),
                    CCfooterIMG = table.Column<string>(nullable: true),
                    PRheaderIMG = table.Column<string>(nullable: true),
                    PRfooterIMG = table.Column<string>(nullable: true),
                    CCheaderHeight = table.Column<string>(nullable: true),
                    CCfooterHeight = table.Column<string>(nullable: true),
                    PRheaderHeight = table.Column<string>(nullable: true),
                    PRfooterHeight = table.Column<string>(nullable: true),
                    CCControlsBorderColor = table.Column<string>(nullable: true),
                    PRControlsBorderColor = table.Column<string>(nullable: true),
                    CCFontFamily = table.Column<string>(nullable: true),
                    PRFontFamily = table.Column<string>(nullable: true),
                    CCFontColor = table.Column<string>(nullable: true),
                    PRFontColor = table.Column<string>(nullable: true),
                    CCFontSize = table.Column<string>(nullable: true),
                    PRFontSize = table.Column<string>(nullable: true),
                    PRBorderWidth = table.Column<string>(nullable: true),
                    CCBorderWidth = table.Column<string>(nullable: true),
                    PRFontStyle = table.Column<string>(nullable: true),
                    CCFontStyle = table.Column<string>(nullable: true),
                    PRFontWeight = table.Column<string>(nullable: true),
                    CCFontWeight = table.Column<string>(nullable: true),
                    CCCustomFont = table.Column<bool>(nullable: true),
                    CCCustomFontFamily = table.Column<string>(nullable: true),
                    CCCustomFontLocal = table.Column<string>(nullable: true),
                    CCCustomFontFile = table.Column<string>(nullable: true),
                    CCCustomFontFormat = table.Column<string>(nullable: true),
                    ThankYouImg = table.Column<string>(nullable: true),
                    CCType = table.Column<string>(nullable: true),
                    ProfileLeftImg = table.Column<string>(nullable: true),
                    ProfileRightImg = table.Column<string>(nullable: true),
                    CCLeftImg = table.Column<string>(nullable: true),
                    CCRightImg = table.Column<string>(nullable: true),
                    CCBackground = table.Column<string>(nullable: true),
                    ProfileTitle = table.Column<string>(nullable: true),
                    ProfileTitle1 = table.Column<string>(nullable: true),
                    SubmitbtnBackColor = table.Column<string>(nullable: true),
                    SubmitbtnForeColor = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CRM_CommentCard", x => x.cc_id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CRM_CommentCard");
        }
    }
}

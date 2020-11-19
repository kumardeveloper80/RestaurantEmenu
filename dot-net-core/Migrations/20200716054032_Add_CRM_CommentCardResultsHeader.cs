using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EMenuApplication.Migrations
{
    public partial class Add_CRM_CommentCardResultsHeader : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CRM_CommentCardResultsHeader",
                columns: table => new
                {
                    CCR_ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    cc_id = table.Column<int>(nullable: true),
                    Customer_ID = table.Column<Guid>(nullable: true),
                    AccountID = table.Column<Guid>(nullable: true),
                    Store_ID = table.Column<Guid>(nullable: true),
                    Storeno = table.Column<int>(nullable: true),
                    Checkno = table.Column<int>(nullable: true),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    LastCall = table.Column<DateTime>(nullable: true),
                    NoCall = table.Column<int>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    company = table.Column<string>(nullable: true),
                    Mobile = table.Column<string>(nullable: true),
                    SecondaryMobile = table.Column<string>(nullable: true),
                    Birthday = table.Column<DateTime>(nullable: true),
                    Gender = table.Column<string>(nullable: true),
                    Synchronized = table.Column<string>(nullable: true),
                    OperationalReporting = table.Column<string>(nullable: true),
                    ImmediateAction = table.Column<string>(nullable: true),
                    CallAgentNote = table.Column<string>(nullable: true),
                    Score = table.Column<double>(nullable: true),
                    Avgscore = table.Column<decimal>(nullable: true),
                    TotalScore = table.Column<decimal>(nullable: true),
                    Waiter = table.Column<string>(nullable: true),
                    Tableno = table.Column<string>(nullable: true),
                    BadgeNo = table.Column<string>(nullable: true),
                    Fields = table.Column<string>(nullable: true),
                    Params = table.Column<string>(nullable: true),
                    ParamValues = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),
                    active = table.Column<bool>(nullable: true),
                    status = table.Column<byte>(nullable: true),
                    CaseCreated = table.Column<bool>(nullable: true),
                    ActNbreCall = table.Column<int>(nullable: true),
                    ActNote = table.Column<string>(nullable: true),
                    Actactive = table.Column<bool>(nullable: true),
                    Act_lastStatus = table.Column<string>(nullable: true),
                    Act_lastcalled = table.Column<DateTime>(nullable: true),
                    Act_lastUser = table.Column<string>(nullable: true),
                    Act_Status = table.Column<byte>(nullable: true),
                    salutation_id = table.Column<int>(nullable: true),
                    CountryID = table.Column<int>(nullable: true),
                    Region = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    Street = table.Column<string>(nullable: true),
                    Fax = table.Column<string>(nullable: true),
                    BusinessCard = table.Column<string>(nullable: true),
                    MaritalStatus_ID = table.Column<int>(nullable: true),
                    Nationality = table.Column<int>(nullable: true),
                    Twitter = table.Column<string>(nullable: true),
                    Facebook = table.Column<string>(nullable: true),
                    Instagram = table.Column<string>(nullable: true),
                    LinkedIn = table.Column<string>(nullable: true),
                    Website = table.Column<string>(nullable: true),
                    OtherSM = table.Column<string>(nullable: true),
                    TimeCreatedOn = table.Column<DateTime>(nullable: true),
                    COR = table.Column<int>(nullable: true),
                    QSA = table.Column<float>(nullable: false),
                    Promo = table.Column<float>(nullable: false),
                    Amount = table.Column<float>(nullable: false),
                    visible = table.Column<bool>(nullable: false),
                    Concept = table.Column<string>(nullable: true),
                    StoreTimezone = table.Column<string>(nullable: true),
                    Language = table.Column<string>(nullable: true),
                    ServiceAVGscore = table.Column<float>(nullable: false),
                    ImmediateUserDate = table.Column<DateTime>(nullable: false),
                    ImmediateUser = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CRM_CommentCardResultsHeader", x => x.CCR_ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CRM_CommentCardResultsHeader");
        }
    }
}

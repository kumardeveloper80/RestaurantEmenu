using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EMenuApplication.Migrations
{
    public partial class Add_Set_Stores : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Set_Stores",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Store_ID = table.Column<Guid>(nullable: false),
                    Store_name = table.Column<string>(nullable: true),
                    IP = table.Column<string>(nullable: true),
                    URL = table.Column<string>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    Status = table.Column<bool>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    SquirrelID = table.Column<int>(nullable: false),
                    StoreCode = table.Column<string>(nullable: true),
                    CountryCode = table.Column<int>(nullable: false),
                    Currency_ID = table.Column<int>(nullable: false),
                    SquirrelCurrency = table.Column<int>(nullable: false),
                    CCEmailRecipient = table.Column<string>(nullable: true),
                    ListOrder = table.Column<int>(nullable: false),
                    CCSMSReceipient = table.Column<string>(nullable: true),
                    CommentCardAutoFill = table.Column<bool>(nullable: false),
                    TimeZone = table.Column<string>(nullable: true),
                    Regionid = table.Column<int>(nullable: false),
                    Conceptid = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Set_Stores", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Set_Stores");
        }
    }
}

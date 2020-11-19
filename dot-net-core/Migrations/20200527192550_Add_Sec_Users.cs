using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EMenuApplication.Migrations
{
    public partial class Add_Sec_Users : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Sec_Users",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    FullName = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: true),
                    ProfilePicture = table.Column<string>(nullable: true),
                    Lock = table.Column<bool>(nullable: true),
                    DefaultModule = table.Column<int>(nullable: true),
                    DefaultPage = table.Column<int>(nullable: true),
                    LockProfile = table.Column<bool>(nullable: true),
                    UnlockProfile = table.Column<bool>(nullable: true),
                    Status = table.Column<byte>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<int>(nullable: true),
                    ModifiedBy = table.Column<int>(nullable: true),
                    LastLoggedIn = table.Column<DateTime>(nullable: true),
                    Active = table.Column<bool>(nullable: true),
                    CanExportProfiles = table.Column<bool>(nullable: true),
                    IsAdmin = table.Column<bool>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sec_Users", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Sec_Users");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace EMenuApplication.Migrations
{
    public partial class Modify_Sec_Users_11082020_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAllowVoucherApprovalPermission",
                table: "Sec_Users",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsAllowVoucherIssuancePermission",
                table: "Sec_Users",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAllowVoucherApprovalPermission",
                table: "Sec_Users");

            migrationBuilder.DropColumn(
                name: "IsAllowVoucherIssuancePermission",
                table: "Sec_Users");
        }
    }
}

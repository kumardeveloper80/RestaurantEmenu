using Microsoft.EntityFrameworkCore.Migrations;

namespace EMenuApplication.Migrations
{
    public partial class modify_voucherIssunace_13112020 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsApproved",
                table: "VoucherIssuance");

            migrationBuilder.AddColumn<string>(
                name: "ApprovedStatus",
                table: "VoucherIssuance",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApprovedStatus",
                table: "VoucherIssuance");

            migrationBuilder.AddColumn<bool>(
                name: "IsApproved",
                table: "VoucherIssuance",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace EMenuApplication.Migrations
{
    public partial class modify_VOucherIssuance : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Reason",
                table: "VoucherIssuance");

            migrationBuilder.AddColumn<int>(
                name: "ReasonCategoryId",
                table: "VoucherIssuance",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ReasonSubCategoryId",
                table: "VoucherIssuance",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReasonCategoryId",
                table: "VoucherIssuance");

            migrationBuilder.DropColumn(
                name: "ReasonSubCategoryId",
                table: "VoucherIssuance");

            migrationBuilder.AddColumn<string>(
                name: "Reason",
                table: "VoucherIssuance",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}

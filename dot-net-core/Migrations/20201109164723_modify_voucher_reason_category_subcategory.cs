using Microsoft.EntityFrameworkCore.Migrations;

namespace EMenuApplication.Migrations
{
    public partial class modify_voucher_reason_category_subcategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ClientId",
                table: "VoucherReasonSubCategoryMaster",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ClientId",
                table: "VoucherReasonCategoryMaster",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "VoucherReasonSubCategoryMaster");

            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "VoucherReasonCategoryMaster");
        }
    }
}

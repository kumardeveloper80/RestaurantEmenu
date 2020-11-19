using Microsoft.EntityFrameworkCore.Migrations;

namespace EMenuApplication.Migrations
{
    public partial class Add_Set_VoucherStore : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StoreId",
                table: "VoucherSetup");

            migrationBuilder.AlterColumn<bool>(
                name: "IsMultipleTimeUsage",
                table: "VoucherSetup",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Set_VoucherStore",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VoucherId = table.Column<int>(nullable: false),
                    StoreId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Set_VoucherStore", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Set_VoucherStore");

            migrationBuilder.AlterColumn<string>(
                name: "IsMultipleTimeUsage",
                table: "VoucherSetup",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(bool));

            migrationBuilder.AddColumn<int>(
                name: "StoreId",
                table: "VoucherSetup",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}

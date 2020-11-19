using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EMenuApplication.Migrations
{
    public partial class Modify_Sec_UserStores : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StoreId",
                table: "Sec_UserStores");

            migrationBuilder.AlterColumn<byte>(
                name: "Status",
                table: "Set_Stores",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AddColumn<Guid>(
                name: "Store_ID",
                table: "Sec_UserStores",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Store_ID",
                table: "Sec_UserStores");

            migrationBuilder.AlterColumn<bool>(
                name: "Status",
                table: "Set_Stores",
                type: "bit",
                nullable: false,
                oldClrType: typeof(byte),
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "StoreId",
                table: "Sec_UserStores",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }
    }
}

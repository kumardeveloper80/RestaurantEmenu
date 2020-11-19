using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EMenuApplication.Migrations
{
    public partial class Modify_Set_UserStores : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Active",
                table: "Sec_UserStores");

            migrationBuilder.DropColumn(
                name: "Username",
                table: "Sec_UserStores");

            migrationBuilder.AlterColumn<bool>(
                name: "Status",
                table: "Sec_UserStores",
                nullable: false,
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ModifiedBy",
                table: "Sec_UserStores",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CreatedBy",
                table: "Sec_UserStores",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeletedBy",
                table: "Sec_UserStores",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "Sec_UserStores",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "Sec_UserStores");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "Sec_UserStores");

            migrationBuilder.AlterColumn<byte>(
                name: "Status",
                table: "Sec_UserStores",
                type: "tinyint",
                nullable: true,
                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<string>(
                name: "ModifiedBy",
                table: "Sec_UserStores",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "Sec_UserStores",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "Sec_UserStores",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "Sec_UserStores",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}

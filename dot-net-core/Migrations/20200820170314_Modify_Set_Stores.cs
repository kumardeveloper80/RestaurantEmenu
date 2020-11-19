using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EMenuApplication.Migrations
{
    public partial class Modify_Set_Stores : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Active",
                table: "Set_Stores");

            migrationBuilder.AlterColumn<bool>(
                name: "Status",
                table: "Set_Stores",
                nullable: true,
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ModifiedBy",
                table: "Set_Stores",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CreatedBy",
                table: "Set_Stores",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeletedBy",
                table: "Set_Stores",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "Set_Stores",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Set_Stores",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "Set_Stores");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "Set_Stores");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Set_Stores");

            migrationBuilder.AlterColumn<byte>(
                name: "Status",
                table: "Set_Stores",
                type: "tinyint",
                nullable: true,
                oldClrType: typeof(bool),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ModifiedBy",
                table: "Set_Stores",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "Set_Stores",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "Set_Stores",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
